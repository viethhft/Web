using Microsoft.AspNetCore.Http;

namespace Data.Dto.Mix
{
    public class GetMixSoundDto
    {
        public string SoundName { get; set; }
        public string Image { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public long IdMix { get; set; }
        public long IdSound { get; set; }
    }
}