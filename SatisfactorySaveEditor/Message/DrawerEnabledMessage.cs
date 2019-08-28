namespace SatisfactorySaveEditor.Message
{
    public class DrawerEnabledMessage
    {
        public bool DrawerEnabled { get; }

        public DrawerEnabledMessage(bool drawerEnabled)
        {
            DrawerEnabled = drawerEnabled;
        }
    }
}
