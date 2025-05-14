using Microsoft.AspNetCore.Http;

namespace Data.Dto
{
    public class EditSoundDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}