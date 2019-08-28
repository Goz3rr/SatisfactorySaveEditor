using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SatisfactorySaveEditor.Message;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveEditor.ViewModel
{
    public class CoordinateListViewModel : ViewModelBase
    {
        
        private Vector3 selectedPoint;
        private ObservableCollection<Vector3> points;

        public ObservableCollection<Vector3> Points
        {
            get => points ?? (points = new ObservableCollection<Vector3>());
            set { Set(() => Points, ref points, value); }
        }

        public Vector3 SelectedPoint
        {
            get => selectedPoint;
            set { Set(() => SelectedPoint, ref selectedPoint, value); }
        }

        public float MinZ { get; set; }
        public float MaxZ { get; set; }
        public RelayCommand AddCommand => new RelayCommand(GoToAdd);
        public RelayCommand<object> DeleteCommand => new RelayCommand<object>(Delete);

        public CoordinateListViewModel()
        {
            Messenger.Default.Register<CoordinateAddedMessage>(this,AddCoordinate);
        }

        private void AddCoordinate(CoordinateAddedMessage message)
        {
            Points.Add(message.Point);
        }

        private void GoToAdd()
        {
            Messenger.Default.Send(new DismantleNavigateMessage(DismantleSlides.CoordinateForm, true));
        }
        private void Delete(object obj)
        {
            var point = (Vector3) obj;
            Points.Remove(point);
        }

    }
}
