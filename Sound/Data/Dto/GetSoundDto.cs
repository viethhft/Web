using Microsoft.AspNetCore.Http;

namespace Data.Dto
{
    public class GetSoundDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public IFormFile SoundFile { get; set; }
    }
}