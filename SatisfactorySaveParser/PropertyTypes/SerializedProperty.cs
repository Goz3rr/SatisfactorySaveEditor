namespace SatisfactorySaveParser.PropertyTypes
{
    public abstract class SerializedProperty
    {
        public string PropertyName { get; }

        public SerializedProperty(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}