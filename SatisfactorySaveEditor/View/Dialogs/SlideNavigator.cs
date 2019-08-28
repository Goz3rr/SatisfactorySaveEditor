using System;
using System.Collections.Generic;

namespace SatisfactorySaveEditor.View.Dialogs
{
    public class SlideNavigator
    {
        private readonly ISlideNavigationSubject slideNavigationSubject;
        private readonly object[] slides;
        private readonly LinkedList<SlideNavigatorFrame> historyLinkedList = new LinkedList<SlideNavigatorFrame>();
        private LinkedListNode<SlideNavigatorFrame> currentPositionNode = null;

        public SlideNavigator(ISlideNavigationSubject slideNavigationSubject, object[] slides)
        {
            this.slideNavigationSubject = slideNavigationSubject ?? throw new ArgumentNullException(nameof(slideNavigationSubject));
            this.slides = slides ?? throw new ArgumentNullException(nameof(slides));
        }

        public void GoTo(int slideIndex)
        {
            GoTo(slideIndex, () => { });
        }

        public void GoTo(int slideIndex, Action setupSlide)
        {
            if (currentPositionNode == null)
            {
                currentPositionNode = new LinkedListNode<SlideNavigatorFrame>(new SlideNavigatorFrame(slideIndex, setupSlide));
                historyLinkedList.AddLast(currentPositionNode);
            }
            else
            {
                var newNode = new LinkedListNode<SlideNavigatorFrame>(new SlideNavigatorFrame(slideIndex, setupSlide));
                historyLinkedList.AddAfter(currentPositionNode, newNode);
                currentPositionNode = newNode;
                var tail = newNode.Next;
                while (tail != null)
                {
                    historyLinkedList.Remove(tail);
                    tail = tail.Next;
                }
            }

            var tidyable = slides[currentPositionNode.Value.SlideIndex] as ITidyable;
            tidyable?.Tidy();
            setupSlide();
            GoTo(currentPositionNode);
        }

        public void GoBack()
        {
            if (currentPositionNode?.Previous == null) return;

            currentPositionNode = currentPositionNode.Previous;
            GoTo(currentPositionNode);
        }

        public void GoForward()
        {
            if (currentPositionNode?.Next == null) return;

            currentPositionNode = currentPositionNode.Next;
            GoTo(currentPositionNode);
        }

        public void GoNext()
        {
            if (slideNavigationSubject.ActiveSlideIndex == slides.Length - 1)
            {
                slideNavigationSubject.ActiveSlideIndex = 0;
            }
            else
            {
                slideNavigationSubject.ActiveSlideIndex++;
            }
        }

        public void GoPrevious()
        {
            if (slideNavigationSubject.ActiveSlideIndex == 0)
            {
                slideNavigationSubject.ActiveSlideIndex = slides.Length - 1;
            }
            else
            {
                slideNavigationSubject.ActiveSlideIndex--;
            }
        }

        private void GoTo(LinkedListNode<SlideNavigatorFrame> node)
        {
            slideNavigationSubject.ActiveSlideIndex = node.Value.SlideIndex;
        }

    }
}
