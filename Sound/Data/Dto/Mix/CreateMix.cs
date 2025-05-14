using Microsoft.AspNetCore.Http;

namespace Data.Dto.Mix
{
    public class CreateMixSoundDto
    {
        public string Name { get; set; }
        public List<long> IdSounds { get; set; }
    }
}