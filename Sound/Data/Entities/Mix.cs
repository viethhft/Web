namespace Data.Entities
{
    public class Mix
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<MixSound> MixSound { get; set; }
    }
}