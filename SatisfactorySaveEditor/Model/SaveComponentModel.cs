using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Model
{
    public class SaveComponentModel : SaveObjectModel
    {
        private string parentEntityName;

        public string ParentEntityName
        {
            get => parentEntityName;
            set { Set(() => ParentEntityName, ref parentEntityName, value); }
        }

        public SaveComponentModel(SaveComponent sc) : base(sc)
        {
            ParentEntityName = sc.ParentEntityName;
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();

            var model = (SaveComponent) Model;

            model.ParentEntityName = ParentEntityName;
        }
    }
}
