using SatisfactorySaveEditor.ViewModel;

namespace SatisfactorySaveEditor.Message
{
    public class DismantleNavigateMessage
    {
        public DismantleSlides Slide { get; }
        public bool FormMode { get; }

        public DismantleNavigateMessage(DismantleSlides slide, bool formMode)
        {
            Slide = slide;
            FormMode = formMode;
        }
    }
}
