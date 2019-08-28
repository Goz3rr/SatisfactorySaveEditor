using System;

namespace SatisfactorySaveEditor.View.Dialogs
{
    internal class SlideNavigatorFrame
    {
        public SlideNavigatorFrame(int slideIndex, Action setupSlide)
        {
            SlideIndex = slideIndex;
            SetupSlide = setupSlide;
        }

        public int SlideIndex { get; }

        public Action SetupSlide { get; }
    }
}
