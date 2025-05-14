using Microsoft.AspNetCore.Http;

namespace Data.Dto
{
    public class SoundDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "audio/mpeg";
    }
}