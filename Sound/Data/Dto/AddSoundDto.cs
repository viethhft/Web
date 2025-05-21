using Microsoft.AspNetCore.Http;

namespace Data.Dto
{
    public class AddSoundDto
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
    }
    public class AddSound
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile File { get; set; }
        public string Token { get; set; }
    }
}