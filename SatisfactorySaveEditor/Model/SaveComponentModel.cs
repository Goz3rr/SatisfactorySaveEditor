using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Model
{
    public class SaveComponentModel : SaveObjectModel
    {
        private string parentEntityName;

        public SaveComponentModel(SaveComponent sc) : base(sc)
        {
            ParentEntityName = sc.ParentEntityName;
        }

        public string ParentEntityName
        {
            get => parentEntityName;
            set { Set(() => ParentEntityName, ref parentEntityName, value); }
        }
    }
}
