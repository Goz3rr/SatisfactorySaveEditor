using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    [SaveObjectClass("/Game/FactoryGame/Equipment/Decoration/BP_Decoration.BP_Decoration_C")]
    public class Decoration : SaveActor
    {
        /// <summary>
        ///     The descriptor of this decoration
        /// </summary>
        [SaveProperty("mDecorationDescriptor")]
        public ObjectReference DecorationDescriptor { get; set; }
    }
}
