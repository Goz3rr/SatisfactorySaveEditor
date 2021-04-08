using System;
using System.Collections.Generic;
using System.Linq;

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
        None = 255,
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

    public enum ETextFlag
    {
        Transient = (1 << 0),
        CultureInvariant = (1 << 1),
        ConvertedProperty = (1 << 2),
        Immutable = (1 << 3),
        InitializedFromString = (1 << 4),  // this ftext was initialized using FromString
    }

    public abstract class TextEntry
    {
        public abstract ETextHistoryType HistoryType { get; }
        public abstract int SerializedLength { get; }

        public ETextFlag Flags { get; set; }

        public TextEntry(ETextFlag flags)
        {
            Flags = flags;
        }
    }

    public class BaseTextEntry : TextEntry, IEquatable<BaseTextEntry>
    {
        public override ETextHistoryType HistoryType => ETextHistoryType.Base;
        public override int SerializedLength => 5 + Namespace.GetSerializedLength() + Key.GetSerializedLength() + Value.GetSerializedLength();

        public string Namespace { get; set; }

        /// <summary>
        ///     Unknown string value, possibly relating to string table
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     Main text value
        /// </summary>
        public string Value { get; set; }

        public BaseTextEntry(ETextFlag flags) : base(flags)
        {
        }

        public bool Equals(BaseTextEntry other)
        {
            if (other == null) return false;

            return Flags == other.Flags &&
                Namespace == other.Namespace &&
                Key == other.Key &&
                Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseTextEntry);
        }

        public override int GetHashCode()
        {
            return (int)Flags + Namespace.GetHashCode(StringComparison.InvariantCulture) + 17 * Key.GetHashCode(StringComparison.InvariantCulture) + 23 * Value.GetHashCode(StringComparison.InvariantCulture);
        }
    }

    public class ArgumentFormat : IEquatable<ArgumentFormat>
    {
        public int SerializedLength => Name.GetSerializedLength() + 1 + Value.SerializedLength;

        public string Name { get; set; }
        public EFormatArgumentType ValueType { get; set; }
        public TextEntry Value { get; set; }

        public bool Equals(ArgumentFormat other)
        {
            if (other == null) return false;

            return Name == other.Name &&
                Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ArgumentFormat);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode(StringComparison.InvariantCulture) + 17 * Value.GetHashCode();
        }
    }

    public class ArgumentFormatTextEntry : TextEntry
    {
        public override ETextHistoryType HistoryType => ETextHistoryType.ArgumentFormat;
        public override int SerializedLength => 5 + SourceFormat.SerializedLength + 4 + Arguments.Sum(x => x.SerializedLength);

        public BaseTextEntry SourceFormat { get; set; }

        public List<ArgumentFormat> Arguments { get; } = new List<ArgumentFormat>();

        public ArgumentFormatTextEntry(ETextFlag flags) : base(flags)
        {
        }
    }

    public class NoneTextEntry : TextEntry
    {
        public override ETextHistoryType HistoryType => ETextHistoryType.None;

        public override int SerializedLength
        {
            get
            {
                // TODO: this breaks if the value is set but we're saving to an older version

                // Old format
                if (!HasCultureInvariantString.HasValue || CultureInvariantString == null)
                    return 5;

                return 9 + CultureInvariantString.GetSerializedLength();
            }
        }

        /// <summary>
        ///     Nullable in case we're using the old format, where this does not exist
        ///     When null it should not be (de)serialized at all
        /// </summary>
        public bool? HasCultureInvariantString { get; set; }
        public string CultureInvariantString { get; set; }

        public NoneTextEntry(ETextFlag flags) : base(flags)
        {
        }
    }
}
