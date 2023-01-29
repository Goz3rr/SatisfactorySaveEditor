using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;

using SatisfactorySaveParser;

using System;
using System.Windows;
using static System.Math;

namespace SatisfactorySaveEditor.Cheats
{
    public class CouponChangerCheat : ICheat
    {
        public string Name => "Set AWESOME sink coupons accumulated...";

        private long pointsRequiredFromTicketCount(int tickets)
        {
            // see ticket equations https://satisfactory.gamepedia.com/AWESOME_Sink
            // ticket cost pre-0.3.3 was Pow(Ceiling(tickets / 3.0), 2) * 1000
            // ticket cost U7 is (Pow(Ceiling(tickets / 3.0)-1, 2) * 500) + 1000
            // TODO: keep the cost function up to date.
            if (tickets < 4)
                return 1000;
            else
                return (long) (Pow(Ceiling(tickets / 3.0)-1, 2) * 500) + 1000;
        }

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            var sinkSubsystem = rootItem.FindChild("Persistent_Level:PersistentLevel.ResourceSinkSubsystem", false);
            if (sinkSubsystem == null)
            {
                MessageBox.Show("This save does not contain a ResourceSinkSubsystem.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find ResourceSinkSubsystem", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var pointsTowardsCurrentTicket = sinkSubsystem.FindOrCreateField<Int64PropertyViewModel>("mTotalResourceSinkPoints");
            //var mCurrentPointLevel = sinkSubsystem.FindOrCreateField<IntPropertyViewModel>("mCurrentPointLevel");
            
            // how many tickets are currently printable from sink.
            var numResourceSinkCoupons = sinkSubsystem.FindOrCreateField<IntPropertyViewModel>("mNumResourceSinkCoupons");
            
            // which ticket point level (modulo 3) the sink currently is (points, alien DNA).
            var pointLevels = sinkSubsystem.FindOrCreateField<ArrayPropertyViewModel>("mCurrentPointLevels");
            
            // the accumulated points the sink currently has (points, alien DNA).
            var accumulatedPoints = sinkSubsystem.FindOrCreateField<ArrayPropertyViewModel>("mTotalPoints");
            

            var dialog = new StringPromptWindow
            {
                Owner = Application.Current.MainWindow
            };
            var cvm = (StringPromptViewModel)dialog.DataContext;
            cvm.WindowTitle = "Enter earned ticket count";
            cvm.PromptMessage = "Tickets";
            cvm.ValueChosen = "0";
            //mCurrentPointLevel.Value
            cvm.OldValueMessage = $"Sets the AWESOME Sink ticket prices as if you had earned N tickets.\nFor example, entering 0 sets the price for the next ticket back to 1,000\nCurrent tickets earned: {((IntPropertyViewModel)pointLevels.Elements[0]).Value}\nMore info on AWESOME Sink wiki page";
            dialog.ShowDialog();

            int requestedTicketCount = 0;

            try
            {
                requestedTicketCount = int.Parse(cvm.ValueChosen);

                if (requestedTicketCount < 0)
                {
                    MessageBox.Show("You must enter a number greater than 0.", "Ticket count unchanged.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (requestedTicketCount > 288100000)
                {
                    MessageBox.Show("The highest ticket count supported is about 288,100,000, since the highest supported points per ticket is around 9,223,372,036,854,775,807 (int64 max value)", "That's a lot of tickets!");
                    return false;
                }

                //mCurrentPointLevel.Value = requestedTicketCount; //"point level" is 0 if no tickets have been earned, 1 if one ticket has, etc.

                ((IntPropertyViewModel)pointLevels.Elements[0]).Value = requestedTicketCount;
                
                pointsTowardsCurrentTicket.Value = 0; //reset progress towards the current ticket so the game GUI doesn't get confused

                long calculatedPointsCountNextTicket = pointsRequiredFromTicketCount(requestedTicketCount+1);

                MessageBox.Show($"Earned ticket count set to {requestedTicketCount}. The next ticket will take {calculatedPointsCountNextTicket} points to earn.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception)
            {
                if (!(cvm.ValueChosen == "cancel"))
                {
                    MessageBox.Show($"Could not parse: {cvm.ValueChosen}");
                }
                return false;
            }
        }
    }
}
