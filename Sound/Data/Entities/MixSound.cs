namespace Data.Entities
{
    public class MixSound
    {
        public long IdMix { get; set; }
        public long IdSound { get; set; }
        public Mix Mix { get; set; }
        public Sound Sound { get; set; }

    }
}