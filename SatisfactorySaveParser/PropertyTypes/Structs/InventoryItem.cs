using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class InventoryItem : IStructData, INotifyPropertyChanged
    {
        private string itemType;
        public int Unknown1 { get; set; }

        public string ItemType
        {
            get => itemType;
            set
            {
                itemType = value;
                OnPropertyChanged(nameof(ItemType));
            }
        }

        public string Unknown2 { get; set; }
        public string Unknown3 { get; set; }

        public int SerializedLength => 4 + ItemType.GetSerializedLength() + Unknown2.GetSerializedLength() + Unknown3.GetSerializedLength();
        public string Type => "InventoryItem";

        public InventoryItem(BinaryReader reader)
        {
            Unknown1 = reader.ReadInt32();
            ItemType = reader.ReadLengthPrefixedString();
            Unknown2 = reader.ReadLengthPrefixedString();
            Unknown3 = reader.ReadLengthPrefixedString();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(Unknown1);
            writer.WriteLengthPrefixedString(ItemType);
            writer.WriteLengthPrefixedString(Unknown2);
            writer.WriteLengthPrefixedString(Unknown3);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}