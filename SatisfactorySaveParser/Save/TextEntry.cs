using System.Collections.Generic;

namespace SatisfactorySaveParser.Save
{
#pragma warning disable CA1720 // Identifier contains type name
    public enum EFormatArgumentType
    {
        Int,
        UInt,
        Float,
        Double,
        Text,
        Gender,
    }
#pragma warning restore CA1720 // Identifier contains type name

    public enum ETextHistoryType
    {
        None = -1,
        Base = 0,
        NamedFormat,
        OrderedFormat,
        ArgumentFormat,
        AsNumber,
        AsPercent,
        AsCurrency,
        AsDate,
        AsTime,
        AsDateTime,
        Transform,
        StringTableEntry,
        TextGenerator,
    }

    public abstract class TextEntry
    {
        public abstract ETextHistoryType HistoryType { get; }
        public abstract int SerializedLength { get; }

        public int Flags { get; set; }

        public TextEntry(int flags)
        {
            Flags = flags;
        }
    }

    public class BaseTextEntry : TextEntry
    {
        public override ETextHistoryType HistoryType => ETextHistoryType.Base;
        public override int SerializedLength => Namespace.GetSerializedLength() + Key.GetSerializedLength() + Value.GetSerializedLength();

        public string Namespace { get; set; }

        /// <summary>
        ///     Unknown string value, possibly relating to string table
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Main text value
        /// </summary>
        public string Value { get; set; }

        public BaseTextEntry(int flags) : base(flags)
        {
        }
    }

    public class ArgumentFormat
    {
        public string Name { get; set; }
        public EFormatArgumentType ValueType { get; set; }
        public TextEntry Value { get; set; }
    }

    public class ArgumentFormatTextEntry : TextEntry
    {
        public override ETextHistoryType HistoryType => ETextHistoryType.ArgumentFormat;
        public override int SerializedLength => 0;

        public BaseTextEntry SourceFormat { get; set; }

        public List<ArgumentFormat> Arguments { get; } = new List<ArgumentFormat>();

        public ArgumentFormatTextEntry(int flags) : base(flags)
        {
        }
    }
}
