using System;
using System.Collections.Generic;
using System.IO;

using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class FINLuaProcessorStateStorage : IStructData
    {
        public List<FINNetworkTrace> Traces { get; set; } = new List<FINNetworkTrace>();
        public List<ObjectReference> References { get; set; } = new List<ObjectReference>();
        //public List<IStructData> Structs { get; set; } = new List<IStructData>();
        public byte[] StructData { get; set; }

        public string Thread { get; set; }
        public string Globals { get; set; }

        public int SerializedLength => 0;
        public string Type => "FINLuaProcessorStateStorage";

        public FINLuaProcessorStateStorage(BinaryReader reader, int size)
        {
            var start = reader.BaseStream.Position;

            var traceCount = reader.ReadInt32();
            for(var i = 0; i < traceCount; i++)
                Traces.Add(new FINNetworkTrace(reader));

            var referenceCount = reader.ReadInt32();
            for (var i = 0; i < referenceCount; i++)
                References.Add(new ObjectReference(reader));

            Thread = reader.ReadLengthPrefixedString();
            Globals = reader.ReadLengthPrefixedString();

            var remaining = size - (int)(reader.BaseStream.Position - start);
            StructData = reader.ReadBytes(remaining);

            /*
            var structCount = reader.ReadInt32();
            for (var i = 0; i < structCount; i++)
                Structs.Add(null);
            */
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(Traces.Count);
            foreach (var trace in Traces)
                trace.Serialize(writer, buildVersion);

            writer.Write(References.Count);
            foreach(var reference in References)
            {
                writer.WriteLengthPrefixedString(reference.LevelName);
                writer.WriteLengthPrefixedString(reference.PathName);
            }

            writer.WriteLengthPrefixedString(Thread);
            writer.WriteLengthPrefixedString(Globals);

            writer.Write(StructData);
        }
    }
}
