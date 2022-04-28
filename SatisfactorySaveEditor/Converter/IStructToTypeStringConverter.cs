using System;
using System.Globalization;
using System.Windows.Data;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveEditor.ViewModel.Struct;
using SatisfactorySaveParser.PropertyTypes.Structs;

namespace SatisfactorySaveEditor.Converter
{
    public class IStructToTypeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Box b:
                    return "Box";
                case Color c:
                    return "Color (0 - 255)";
                case DynamicStructDataViewModel dsd:
                    return "Dynamic struct data";
                case InventoryItem ii:
                    return "Inventory item";
                case LinearColor lc:
                    return "Linear color (0f - 1f)";
                case Quat q:
                    return "Quaternion";
                case RailroadTrackPosition rtp:
                    return "Railroad track position";
                case Rotator r:
                    return "Rotator";
                case Vector v:
                    return "Vector";
                case Vector2D v2:
                    return "Vector2D";
                case Vector4D v4:
                    return "Vector4";
                case GuidStruct g:
                    return "Guid";
                case FluidBox fb:
                    return "FluidBox";
                case FINNetworkTrace nt:
                    return "FINNetworkTrace";
                case SatisfactorySaveParser.PropertyTypes.Structs.DateTime dt:
                    return "DateTime";
                case SerializedPropertyViewModel spvm: // TODO: This seems like a bad idea, but it works for now
                    return new SerializablePropertyToTypeStringConverter().Convert(spvm, targetType, parameter, culture);
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
