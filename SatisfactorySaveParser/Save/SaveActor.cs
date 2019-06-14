using System.Collections.Generic;
using System.Numerics;

namespace SatisfactorySaveParser.Save
{
    public class SaveActor : SaveObject
    {
        public override SaveObjectKind ObjectKind => SaveObjectKind.Actor;

        /// <summary>
        ///     If returning true, then when loading, we are returned to the location that was stored when saving
        /// </summary>
        public bool NeedTransform { get; set; }

        /// <summary>
        ///     Rotation in the world
        /// </summary>
        public Vector4 Rotation { get; set; }

        /// <summary>
        ///     Position in the world
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        ///     Scale in the world
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        ///     Unknown use
        /// </summary>
        public bool WasPlacedInLevel { get; set; }

        /// <summary>
        ///     Unknown related (parent?) object
        /// </summary>
        public ObjectReference ParentObject { get; set; }

        /// <summary>
        ///     List of SaveComponents belonging to this object
        /// </summary>
        public List<ObjectReference> Components { get; } = new List<ObjectReference>();
    }
}
