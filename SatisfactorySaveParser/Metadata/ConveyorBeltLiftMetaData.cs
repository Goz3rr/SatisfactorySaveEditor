using System.IO;

namespace SatisfactorySaveParser.Metadata
{
    public class ConveyorBeltLiftMetaData : SaveObjectMetaData
    {
        public ConveyorItemMetaData[] Items;
        
        public override void ParseData(BinaryReader reader)
        {
            base.ParseData(reader);
            
            int numItems = reader.ReadInt32();
            Items = new ConveyorItemMetaData[numItems];
            for (int i = 0; i < numItems; i++)
            {
                Items[i] = new ConveyorItemMetaData();
                Items[i].ParseData(reader);
            }
        }
    }
}