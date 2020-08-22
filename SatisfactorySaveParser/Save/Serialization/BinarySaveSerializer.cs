using System;
using System.IO;

namespace SatisfactorySaveParser.Save.Serialization
{
    public class BinarySaveSerializer : ISaveSerializer
    {
        public event EventHandler<StageChangedEventArgs> SerializationStageChanged;
        public event EventHandler<StageProgressedEventArgs> SerializationStageProgressed;
        public event EventHandler<StageChangedEventArgs> DeserializationStageChanged;
        public event EventHandler<StageProgressedEventArgs> DeserializationStageProgressed;

        public FGSaveSession Deserialize(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(FGSaveSession save, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}