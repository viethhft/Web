using Microsoft.AspNetCore.Mvc;
using Data.Dto;
using Application.Services.IServices;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoundController : Controller
    {
        private readonly ISoundService _soundService;
        public SoundController(ISoundService soundService)
        {
            _soundService = soundService;
        }
        [HttpGet("GetSoundData")]
        public async Task<List<GetSoundDto>> GetSound(int PageSize = 10, int PageNumber = 1)
        {
            List<GetSoundDto> lstSound = new List<GetSoundDto>();
            var lst = await _soundService.GetSound(PageSize, PageNumber);
            foreach (var item in lst)
            {
                var content = new MemoryStream(item.Content as byte[]);
                var file = new FormFile(content, 0, content.Length, "file", item.FileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = item.ContentType
                };
                var sound = new GetSoundDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Image = item.Image,
                    SoundFile = file,
                };
                lstSound.Add(sound);
            }
            return lstSound;
        }

        [HttpPost("AddSound")]
        public async Task<string> AddSound(AddSoundDto sound, IFormFile fileSound)
        {
            byte[] content;
            using (var ms = new MemoryStream())
            {
                await fileSound.CopyToAsync(ms);
                content = ms.ToArray();
            }

            var file = new FileSound()
            {
                Content = content,
                ContentType = fileSound.ContentType,
                FileName = fileSound.FileName
            };

            return await _soundService.AddSound(sound, file);
        }

        [HttpPut("UpdateSound")]
        public async Task<string> UpdateSound(EditSoundDto sound, IFormFile fileSound)
        {
            byte[] content;
            using (var ms = new MemoryStream())
            {
                await fileSound.CopyToAsync(ms);
                content = ms.ToArray();
            }

            var file = new FileSound()
            {
                Content = content,
                ContentType = fileSound.ContentType,
                FileName = fileSound.FileName
            };
            return await _soundService.UpdateSound(sound, file);
        }

        [HttpDelete("DeleteSound")]
        public async Task<string> DeleteSound(long id)
        {
            return await _soundService.DeleteSound(id);
        }
    }
}