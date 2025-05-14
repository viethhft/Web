
namespace Data.Dto
{
    public class FileSound
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "audio/mpeg";
    }
}