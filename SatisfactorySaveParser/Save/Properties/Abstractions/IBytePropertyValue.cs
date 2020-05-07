namespace SatisfactorySaveParser.Save.Properties.Abstractions
{
    public interface IBytePropertyValue
    {
        string EnumType { get; set; }
        string EnumValue { get; set; }

        byte ByteValue { get; set; }

        bool IsEnum { get; }
    }
}
