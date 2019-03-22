using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Model
{
    public class SaveComponentModel : SaveObjectModel
    {
        private string parentEntityName;
        private readonly string typePath, rootObject, instanceName;

        public string ParentEntityName
        {
            get => parentEntityName;
            set { Set(() => ParentEntityName, ref parentEntityName, value); }
        }

        public SaveComponentModel(SaveComponent sc) : base(sc)
        {
            typePath = sc.TypePath;
            rootObject = sc.RootObject;
            instanceName = sc.InstanceName;

            ParentEntityName = sc.ParentEntityName;
        }

        public override SaveObject ToSaveObject()
        {
            var component = new SaveComponent(typePath, rootObject, instanceName)
            {
                ParentEntityName = ParentEntityName,
                DataFields = GetSerializedFields()
            };

            return component;
        }
    }
}
