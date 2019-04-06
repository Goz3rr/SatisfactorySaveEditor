using SatisfactorySaveParser;
using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveEditor.Model
{
    public class SaveEntityModel : SaveObjectModel
    {
        private int int4;
        private int int6;
        private Vector4 rotation;
        private Vector3 position;
        private Vector3 scale;
        private string parentObjectRoot;
        private string parentObjectName;

        public int Int4
        { 
            get => int4;
            set { Set(() => Int4, ref int4, value); }
        }

        public int Int6
        {
            get => int6;
            set { Set(() => Int6, ref int6, value); }
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
            Int4 = ent.Int4;
            Int6 = ent.Int6;
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

            model.Int4 = Int4;
            model.Rotation = Rotation;
            model.Position = Position;
            model.Scale = Scale;
            model.Int6 = Int6;
            model.ParentObjectRoot = ParentObjectRoot;
            model.ParentObjectName = ParentObjectName;
        }
    }
}
