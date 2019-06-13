using SatisfactorySaveParser;
using SatisfactorySaveParser.Structures;
using System.Numerics;

namespace SatisfactorySaveEditor.Model
{
    public class SaveEntityModel : SaveObjectModel
    {
        private bool needTransform;
        private bool wasPlacedInLevel;
        private Vector4 rotation;
        private Vector3 position;
        private Vector3 scale;
        private string parentObjectRoot;
        private string parentObjectName;

        public bool NeedTransform
        { 
            get => needTransform;
            set { Set(() => NeedTransform, ref needTransform, value); }
        }

        public bool WasPlacedInLevel
        {
            get => wasPlacedInLevel;
            set { Set(() => WasPlacedInLevel, ref wasPlacedInLevel, value); }
        }

        public Vector4 Rotation
        {
            get => rotation;
            set { Set(() => Rotation, ref rotation, value); }
        }
        public Vector3 Position
        {
            get => position;
            set { Set(() => Position, ref position, value); }
        }

        public Vector3 Scale
        {
            get => scale;
            set { Set(() => Scale, ref scale, value); }
        }

        public string ParentObjectRoot
        {
            get => parentObjectRoot;
            set { Set(() => ParentObjectRoot, ref parentObjectRoot, value); }
        }

        public string ParentObjectName
        {
            get => parentObjectName;
            set { Set(() => ParentObjectName, ref parentObjectName, value); }
        }

        public SaveEntityModel(SaveEntity ent) : base(ent)
        {
            NeedTransform = ent.NeedTransform;
            WasPlacedInLevel = ent.WasPlacedInLevel;
            ParentObjectRoot = ent.ParentObjectRoot;
            ParentObjectName = ent.ParentObjectName;

            Rotation = ent.Rotation;
            Position = ent.Position;
            Scale = ent.Scale;
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();

            var model = (SaveEntity) Model;

            model.NeedTransform = NeedTransform;
            model.Rotation = Rotation;
            model.Position = Position;
            model.Scale = Scale;
            model.WasPlacedInLevel = WasPlacedInLevel;
            model.ParentObjectRoot = ParentObjectRoot;
            model.ParentObjectName = ParentObjectName;
        }
    }
}
