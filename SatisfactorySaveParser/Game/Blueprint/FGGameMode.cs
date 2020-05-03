#pragma warning disable CA1707 // Identifiers should not contain underscores

using System;
using System.Collections.Generic;
using System.IO;

using SatisfactorySaveParser.Save;

// FGGameMode.h

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_GameMode.BP_GameMode_C")]
    public class FGGameMode : SaveActor
    {
        /// <summary>
        ///     Last autosave was this id
        /// </summary>
        [SaveProperty("mLastAutosaveId")]
        public byte LastAutosaveId { get; set; }

        [SaveProperty("mSessionId"), Obsolete("Marked as deprecated in Satisfactory headers")]
        public int SessionId_DEPRECATED { get; set; }

        [SaveProperty("mSessionIDString"), Obsolete("Marked as deprecated in Satisfactory headers")]
        public string SessionIDString_DEPRECATED { get; set; }

        /// <summary>
        ///     The name of the session we are playing
        /// </summary>
        [SaveProperty("mSaveSessionName")]
        public string SaveSessionName { get; set; }

        /// <summary>
        ///     Selected starting point
        /// </summary>
        [SaveProperty("mStartingPointTagName")]
        public string StartingPointTagName { get; set; }

        public List<ObjectReference> PlayerStates { get; } = new List<ObjectReference>();

        public override void DeserializeNativeData(BinaryReader reader, int length)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                PlayerStates.Add(reader.ReadObjectReference());
            }
        }
    }
}
