using GalaSoft.MvvmLight;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveEditor.Model
{
    public class IOProgressModel : ObservableObject
    {
        private bool _isLoading;
        private bool _bottomIndeterminate;

        private string _stageName;

        private long _topCurrent;
        private long _topTotal;
        private long _bottomCurrent;
        private long _bottomTotal;

        private float _topProgress;
        private float _bottomProgress;

        private string OperationName => _isLoading ? "Loading" : "Saving";

        public string TopText => $"{OperationName} ({_topCurrent}/{_topTotal})";
        public string BottomText => _bottomIndeterminate? _stageName : $"{_stageName} ({_bottomCurrent}/{_bottomTotal})";

        public float TopProgress
        {
            get => _topProgress;
            set => Set(() => TopProgress, ref _topProgress, value);
        }
        public float BottomProgress
        {
            get => _bottomProgress;
            set => Set(() => BottomProgress, ref _bottomProgress, value);
        }

        public bool BottomIndeterminate
        {
            get => _bottomIndeterminate;
            set => Set(() => BottomIndeterminate, ref _bottomIndeterminate, value);
        }

        public void UpdateStatusLoad(object sender, StageChangedEventArgs args)
        {
            _isLoading = true;

            _topCurrent = args.Current + 1;
            _topTotal = args.Total;
            TopProgress = (args.Current + 1) / (float)args.Total * 100f;
            RaisePropertyChanged(() => TopText);

            _stageName = GetReadableStageName(args.Stage);
            RaisePropertyChanged(() => BottomText);
        }

        public void UpdateStatusLoad(object sender, StageProgressedEventArgs args)
        {
            _isLoading = true;

            _bottomCurrent = args.Current;
            _bottomTotal = args.Total;

            BottomProgress = args.Progress;
            RaisePropertyChanged(() => BottomText);

            BottomIndeterminate = args.Total == -1;
        }

        private string GetReadableStageName(SerializerStage stage)
        {
            return stage switch
            {
                SerializerStage.FileOpen => "Opening file",
                SerializerStage.ParseHeader => "Parsing header",
                SerializerStage.Decompressing => "Decompressing",
                SerializerStage.ReadObjects => "Reading objects",
                SerializerStage.ReadObjectData => "Reading object data",
                SerializerStage.ReadDestroyedObjects => "Reading destroyed objects",
                SerializerStage.BuildReferences => "Building references",
                SerializerStage.WriteHeader => "Writing headers",
                SerializerStage.WriteObjects => "Writing objects",
                SerializerStage.WriteObjectData => "Writing object data",
                SerializerStage.WriteDestroyedObjects => "Writing destroyed objects",
                SerializerStage.Compressing => "Compressing",
                SerializerStage.FileWrite => "Writing file",
                SerializerStage.Done => "Finalizing",
                _ => "Processing"
            };
        }
    }
}
