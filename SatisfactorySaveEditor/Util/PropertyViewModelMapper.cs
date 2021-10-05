using System;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.Util
{
    public static class PropertyViewModelMapper
    {
        public static SerializedPropertyViewModel Convert(SerializedProperty property)
        {
            switch (property)
            {
                case ArrayProperty arp:
                    return new ArrayPropertyViewModel(arp);
                case BoolProperty bop:
                    return new BoolPropertyViewModel(bop);
                case ByteProperty byp:
                    return new BytePropertyViewModel(byp);
                case EnumProperty enp:
                    return new EnumPropertyViewModel(enp);
                case FloatProperty flp:
                    return new FloatPropertyViewModel(flp);
                case DoubleProperty dlp:
                    return new DoublePropertyViewModel(dlp);
                case IntProperty inp:
                    return new IntPropertyViewModel(inp);
                case MapProperty map:
                    return new MapPropertyViewModel(map);
                case NameProperty nap:
                    return new NamePropertyViewModel(nap);
                case ObjectProperty obp:
                    return new ObjectPropertyViewModel(obp);
                case StrProperty strip:
                    return new StrPropertyViewModel(strip);
                case StructProperty strup:
                    return new StructPropertyViewModel(strup);
                case TextProperty tep:
                    return new TextPropertyViewModel(tep);
                case SetProperty sep:
                    return new SetPropertyViewModel(sep);
                case InterfaceProperty ifp:
                    return new InterfacePropertyViewModel(ifp);
                case Int64Property i64p:
                    return new Int64PropertyViewModel(i64p);
                case UInt64Property ui64p:
                    return new UInt64PropertyViewModel(ui64p);
                case UInt32Property ui32p:
                    return new UInt32PropertyViewModel(ui32p);
                case Int8Property i8p:
                    return new Int8PropertyViewModel(i8p);
                default:
                    throw new ArgumentOutOfRangeException(nameof(property), property, null);
            }
        }

        public static SerializedPropertyViewModel Create<T>(string propertyName) where T : SerializedPropertyViewModel
        {
            if (typeof(T) == typeof(ArrayPropertyViewModel))
                return new ArrayPropertyViewModel(new ArrayProperty(propertyName));

            if (typeof(T) == typeof(BoolPropertyViewModel))
                return new BoolPropertyViewModel(new BoolProperty(propertyName));

            if (typeof(T) == typeof(BytePropertyViewModel))
                return new BytePropertyViewModel(new ByteProperty(propertyName));

            if (typeof(T) == typeof(EnumPropertyViewModel))
                return new EnumPropertyViewModel(new EnumProperty(propertyName));

            if (typeof(T) == typeof(FloatPropertyViewModel))
                return new FloatPropertyViewModel(new FloatProperty(propertyName));

            if (typeof(T) == typeof(DoublePropertyViewModel))
                return new DoublePropertyViewModel(new DoubleProperty(propertyName));

            if (typeof(T) == typeof(IntPropertyViewModel))
                return new IntPropertyViewModel(new IntProperty(propertyName));

            if (typeof(T) == typeof(MapPropertyViewModel))
                return new MapPropertyViewModel(new MapProperty(propertyName));

            if (typeof(T) == typeof(NamePropertyViewModel))
                return new NamePropertyViewModel(new NameProperty(propertyName));

            if (typeof(T) == typeof(ObjectPropertyViewModel))
                return new ObjectPropertyViewModel(new ObjectProperty(propertyName));

            if (typeof(T) == typeof(StrPropertyViewModel))
                return new StrPropertyViewModel(new StrProperty(propertyName));

            if (typeof(T) == typeof(StructPropertyViewModel))
                return new StructPropertyViewModel(new StructProperty(propertyName));

            if (typeof(T) == typeof(TextPropertyViewModel))
                return new TextPropertyViewModel(new TextProperty(propertyName));

            if (typeof(T) == typeof(SetPropertyViewModel))
                return new SetPropertyViewModel(new SetProperty(propertyName));

            if (typeof(T) == typeof(InterfacePropertyViewModel))
                return new InterfacePropertyViewModel(new InterfaceProperty(propertyName));

            if (typeof(T) == typeof(Int64PropertyViewModel))
                return new Int64PropertyViewModel(new Int64Property(propertyName));

            if (typeof(T) == typeof(UInt64PropertyViewModel))
                return new UInt64PropertyViewModel(new UInt64Property(propertyName));

            if (typeof(T) == typeof(Int8PropertyViewModel))
                return new Int8PropertyViewModel(new Int8Property(propertyName));

            throw new NotImplementedException($"Can't instantiate unknown property type {typeof(T)}");
        }
    }
}
