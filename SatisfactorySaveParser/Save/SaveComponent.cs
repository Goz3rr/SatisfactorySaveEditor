namespace SatisfactorySaveParser.Save
{
    public class SaveComponent : SaveObject
    {
        public override SaveObjectKind ObjectKind => SaveObjectKind.Component;

        /// <summary>
        ///     Instance name of the parent entity object
        /// </summary>
        public string ParentEntityName { get; set; }
    }
}
