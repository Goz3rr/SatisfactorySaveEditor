using System;
using System.Drawing;

namespace SatisfactorySaveEditor.Service.Toast
{
    public class ToastViewModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public Icon Icon { get; set; } = SystemIcons.Information;
        public TimeSpan Lifespan { get; set; } = TimeSpan.FromSeconds(10);
    }
}
