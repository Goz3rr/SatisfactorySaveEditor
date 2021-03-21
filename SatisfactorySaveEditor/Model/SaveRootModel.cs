using SatisfactorySaveParser.Save;

namespace SatisfactorySaveEditor.Model
{
    public class SaveRootModel : SaveObjectModel
    {
        private readonly FSaveHeader model;

        private int buildVersion;
        private string mapName;
        private string mapOptions;
        private string sessionName;
        private int playDuration;
        private long saveDateTime;
        private ESessionVisibility sessionVisibility;

        public SaveHeaderVersion HeaderVersion => model.HeaderVersion;

        public FSaveCustomVersion SaveVersion => model.SaveVersion;

        public int BuildVersion
        {
            get => buildVersion;
            set { Set(() => BuildVersion, ref buildVersion, value); }
        }

        public string MapName
        {
            get => mapName;
            set { Set(() => MapName, ref mapName, value); }
        }

        public string MapOptions
        {
            get => mapOptions;
            set { Set(() => MapOptions, ref mapOptions, value); }
        }

        public string SessionName
        {
            get => sessionName;
            set { Set(() => SessionName, ref sessionName, value); }
        }

        public int PlayDuration
        {
            get => playDuration;
            set { Set(() => PlayDuration, ref playDuration, value); }
        }

        public bool HasSessionVisibility => HeaderVersion >= SaveHeaderVersion.AddedSessionVisibility;

        public ESessionVisibility SessionVisibility
        {
            get => sessionVisibility;
            set { Set(() => SessionVisibility, ref sessionVisibility, value); }
        }

        public long SaveDateTime
        {
            get => saveDateTime;
            set { Set(() => SaveDateTime, ref saveDateTime, value); }
        }

        public SaveRootModel(FSaveHeader header) : base(header.SessionName)
        {
            model = header;
            Type = "Root";

            buildVersion = model.BuildVersion;
            mapName = model.MapName;
            mapOptions = model.MapOptions;
            sessionName = model.SessionName;
            playDuration = model.PlayDuration;
            sessionVisibility = model.SessionVisibility;
            saveDateTime = model.SaveDateTime;
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();

            model.BuildVersion = buildVersion;
            model.MapName = mapName;
            model.MapOptions = mapOptions;
            model.SessionName = sessionName;
            model.PlayDuration = playDuration;
            model.SessionVisibility = sessionVisibility;
            model.SaveDateTime = saveDateTime;
        }
    }
}
