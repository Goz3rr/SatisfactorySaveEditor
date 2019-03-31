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
                default:
                    throw new ArgumentOutOfRangeException(nameof(property), property, null);
            }
        }
    }
}
