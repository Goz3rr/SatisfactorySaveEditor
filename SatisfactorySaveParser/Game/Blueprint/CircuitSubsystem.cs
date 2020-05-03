using System.Collections.Generic;
using System.IO;

using SatisfactorySaveParser.Save;

// FGCircuitSubsystem.h

namespace SatisfactorySaveParser.Game.Blueprint
{
    /// <summary>
    ///     Subsystem to handle all circuits, connects, disconnects.
    /// </summary>
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_CircuitSubsystem.BP_CircuitSubsystem_C")]
    public class CircuitSubsystem : SaveActor
    {
        public List<(int CircuitID, ObjectReference Reference)> Circuits { get; } = new List<(int CircuitID, ObjectReference Reference)>();

        public override void DeserializeNativeData(BinaryReader reader, int length)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                Circuits.Add((reader.ReadInt32(), reader.ReadObjectReference()));
            }
        }
    }
}
