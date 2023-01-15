using System.IO;

namespace SatisfactorySaveParser.Metadata
{
    public class ConveyorItemMetaData : SaveObjectMetaData
    {

        public string ItemPathName;
        public float Position;
        
        public override void ParseData(BinaryReader reader)
        {
            base.ParseData(reader);
            
            reader.ReadInt32();
            ItemPathName = reader.ReadLengthPrefixedString();
            reader.ReadLengthPrefixedString();
            reader.ReadLengthPrefixedString();
            Position = reader.ReadSingle();
        }
    }
}