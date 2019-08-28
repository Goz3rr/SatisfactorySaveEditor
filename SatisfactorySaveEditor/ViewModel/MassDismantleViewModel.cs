using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using SatisfactorySaveEditor.Message;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.View.Control;
using SatisfactorySaveEditor.View.Dialogs;

namespace SatisfactorySaveEditor.ViewModel
{
    public enum DismantleSlides
    {
        CoordinateList,
        CoordinateForm
    }

    public class MassDismantleViewModel : ViewModelBase, ISlideNavigationSubject
    {

        private int activeSlideIndex;
        private ISnackbarMessageQueue snackbar;
        private string buttonText;
        private bool lastStep;
        private bool formMode;

        public bool FormMode
        {
            get => formMode;
            set { Set(() => FormMode, ref formMode, value); }
        }

        public bool CanProceed => CoordinateList.Points.Count >= 3;

        public int ActiveSlideIndex
        {
            get => activeSlideIndex;
            set => Set(() => ActiveSlideIndex, ref activeSlideIndex, value);
        }

        public string ButtonText
        {
            get => buttonText;
            set { Set(() => ButtonText, ref buttonText, value); }
        }
        public object[] Slides { get; }
        public SlideNavigator SlideNavigator { get; }

        public CoordinateFormViewModel CoordinateForm { get; }
        public CoordinateListViewModel CoordinateList { get; }
        public RelayCommand HelpCommand => new RelayCommand(Help);
        public RelayCommand NextCommand => new RelayCommand(Next);
        public MassDismantleViewModel(ISnackbarMessageQueue snackbar)
        {
            this.snackbar = snackbar;
            CoordinateList = new CoordinateListViewModel();
            CoordinateForm = new CoordinateFormViewModel(snackbar);

            Slides = new object[] { CoordinateList, CoordinateForm };
            SlideNavigator = new SlideNavigator(this, Slides);
            SlideNavigator.GoTo((int)DismantleSlides.CoordinateList);
            Messenger.Default.Register<DismantleNavigateMessage>(this, Navigate);
        }

        private void Navigate(DismantleNavigateMessage message)
        {
            SlideNavigator.GoTo((int)message.Slide);
            RaisePropertyChanged(nameof(CanProceed));
            FormMode = message.FormMode;
        }

        private void Next()
        {
            if (lastStep)
            {
                try
                {
                    if (!string.IsNullOrEmpty(CoordinateForm.FirstCoordinate) && !string.IsNullOrEmpty(CoordinateForm.SecondCoordinate))
                    {
                        CoordinateList.MinZ = float.Parse(CoordinateForm.FirstCoordinate);
                        CoordinateList.MaxZ = float.Parse(CoordinateForm.SecondCoordinate);
                    }
                    else
                    {
                        CoordinateList.MinZ = float.NegativeInfinity;
                        CoordinateList.MaxZ = float.PositiveInfinity;
                    }
                    DialogHost.CloseDialogCommand.Execute(CoordinateList, null);
                }
                catch (FormatException)
                {
                    snackbar.Enqueue("Error! Coordinate format: 123456", "Ok", () => { });
                }
            }
            ButtonText = "Done";
            lastStep = true;
            SlideNavigator.GoTo((int)DismantleSlides.CoordinateForm, () =>
            {
                CoordinateForm.IsZDialog = true;
                CoordinateForm.FirstCoordinateHint = "Min Z";
                CoordinateForm.SecondCoordinateHint = "Max Z";
            });
        }

        private void Help()
        {
            System.Diagnostics.Process.Start("https://ficsit.app/guide/1Nk4oKqhpMhgN");
        }
    }
}
