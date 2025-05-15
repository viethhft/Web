using Microsoft.AspNetCore.Http;

namespace Data.Dto.Mix
{
    public class UpdateMixSoundDto
    {
        public long IdMix { get; set; }
        public List<long> IdSounds { get; set; }
    }
}