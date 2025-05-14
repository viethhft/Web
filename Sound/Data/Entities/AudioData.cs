namespace Data.Entities
{
    public class AudioData
    {
        public long Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "audio/mpeg";
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}