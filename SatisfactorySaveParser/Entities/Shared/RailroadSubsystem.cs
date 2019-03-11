using System;
using System.IO;

namespace SatisfactorySaveParser.Entities.Shared
{
    [SaveEntity("/Game/FactoryGame/-Shared/Blueprint/BP_RailroadSubsystem.BP_RailroadSubsystem_C")]
    public class RailroadSubsystem : SaveEntity
    {
        public int DataInt6 { get; set; }

        public override void ParseData(int length, BinaryReader reader)
        {
            base.ParseData(length, reader);

            //DataInt6 = reader.ReadInt32();
        }
    }
}
