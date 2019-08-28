using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using SatisfactorySaveEditor.Message;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveEditor.ViewModel
{
    public class CoordinateFormViewModel : ViewModelBase
    {
        private string firstCoordinate;
        private string secondCoordinate;
        private string firstCoordinateHint;
        private string secondCoordinateHint;
        private readonly ISnackbarMessageQueue snackbar;
        private bool isZDialog;

        public bool IsZDialog
        {
            get => isZDialog;
            set { Set(() => IsZDialog, ref isZDialog, value); }
        }

        public string FirstCoordinate
        {
            get => firstCoordinate;
            set { Set(() => FirstCoordinate, ref firstCoordinate, value); }
        }

        public string SecondCoordinate
        {
            get => secondCoordinate;
            set { Set(() => SecondCoordinate, ref secondCoordinate, value); }
        }

        public string FirstCoordinateHint
        {
            get => firstCoordinateHint ?? (firstCoordinate = "X");
            set
            {
                Set(() => FirstCoordinateHint, ref firstCoordinateHint, value);
            }
        }

        public string SecondCoordinateHint
        {
            get => secondCoordinateHint ?? (secondCoordinate = "Y");
            set
            {
                Set(() => SecondCoordinateHint, ref secondCoordinateHint, value);
            }
        }

        public RelayCommand AddCommand => new RelayCommand(Add);
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

        public CoordinateFormViewModel(ISnackbarMessageQueue snackbar)
        {
            this.snackbar = snackbar;
        }

        private void Add()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(firstCoordinate) && !string.IsNullOrWhiteSpace(secondCoordinate))
                {
                    Messenger.Default.Send(new CoordinateAddedMessage(new Vector3()
                    {
                        X = float.Parse(firstCoordinate),
                        Y = float.Parse(secondCoordinate)
                    }));
                    Messenger.Default.Send(new DismantleNavigateMessage(DismantleSlides.CoordinateList, false));
                    FirstCoordinate = string.Empty;
                    SecondCoordinate = string.Empty;
                }
                else
                    snackbar.Enqueue("Enter coordinates", "Ok", () => { });
            }
            catch (FormatException)
            {
                snackbar.Enqueue("Error! Coordinate format: 123456", "Ok", () => { });
            }
        }

        private void Cancel()
        {
            Messenger.Default.Send(new DismantleNavigateMessage(DismantleSlides.CoordinateList, false));
        }
    }
}