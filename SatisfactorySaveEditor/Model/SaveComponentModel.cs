using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Model
{
    public class SaveComponentModel : SaveObjectModel
    {
        private string parentEntityName;

        public SaveComponentModel(string title, SerializedFields fields, string rootObject, string parentEntityName) : base(title, fields, rootObject)
        {
            ParentEntityName = parentEntityName;
        }

        public string ParentEntityName
        {
            get => parentEntityName;
            set { Set(() => ParentEntityName, ref parentEntityName, value); }
        }
    }
}
