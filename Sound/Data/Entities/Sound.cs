namespace Data.Entities
{
    public class Sound
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public AudioData AudioData { get; set; }
    }
}