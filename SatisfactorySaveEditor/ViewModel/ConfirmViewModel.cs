using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace SatisfactorySaveEditor.ViewModel
{
    public class ConfirmViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string IconKind { get; set; }
        public string ConfirmText { get; set; }
        public string CancelText { get; set; }
        public bool ShowConfirm { get; set; }

    }
}
