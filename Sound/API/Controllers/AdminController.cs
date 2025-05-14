using Microsoft.AspNetCore.Mvc;
using Data.Dto;
using Application.Services.IServices;
using Microsoft.AspNetCore.Http;
using Data.Common;

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
        public async Task<ResponseData<Pagination<SoundDto>>> GetSound(int PageSize = 10, int PageNumber = 1)
        {
            var repsonse = await _soundService.GetSound(PageSize, PageNumber);
            if (repsonse.IsSuccess)
            {
                return new ResponseData<Pagination<SoundDto>>
                {
                    IsSuccess = true,
                    Data = new Pagination<SoundDto>
                    {
                        TotalPage = repsonse.Data.TotalPage,
                        CurrentPage = repsonse.Data.CurrentPage,
                        Data = repsonse.Data.Data,
                    },
                };
            }
            else
            {
                return new ResponseData<Pagination<SoundDto>>
                {
                    IsSuccess = false,
                    Message = "Không có dữ liệu.",
                };
            }
        }

        [HttpPost("AddSound")]
        public async Task<ResponseData<string>> AddSound(AddSound sound)
        {
            if ((sound.File.Length > 0 && sound.File.ContentType == "audio/mpeg") && (sound.Image.Length > 0 && sound.Image.ContentType.StartsWith("image/")))
            {
                byte[] content;
                string image = "";
                using (var ms = new MemoryStream())
                {
                    await sound.File.CopyToAsync(ms);
                    content = ms.ToArray();
                    await sound.Image.CopyToAsync(ms);
                    var tempImageData = ms.ToArray();
                    image = Convert.ToBase64String(tempImageData);
                }

                var file = new FileSound()
                {
                    Content = content,
                    ContentType = sound.File.ContentType,
                    FileName = sound.File.FileName
                };

                var temp = new AddSoundDto()
                {
                    Image = image,
                    Name = sound.Name,
                };
                return await _soundService.AddSound(temp, file);
            }
            else
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Message = "Vui lòng chọn file để thêm hoặc chọn đúng file là định dạng âm thanh."
                };
            }
        }

        [HttpPut("UpdateSound")]
        public async Task<ResponseData<string>> UpdateSound(EditSound sound)
        {
            byte[] content;
            string image = "";
            FileSound file = new FileSound();

            if (sound.File.Length > 0 && sound.File.ContentType == "audio/mpeg")
            {
                using (var ms = new MemoryStream())
                {
                    await sound.File.CopyToAsync(ms);
                    content = ms.ToArray();
                }

                file = new FileSound()
                {
                    Content = content,
                    ContentType = sound.File.ContentType,
                    FileName = sound.File.FileName
                };
            }

            if (sound.Image.Length > 0 && sound.Image.ContentType.StartsWith("image/"))
            {
                using (var ms = new MemoryStream())
                {
                    await sound.Image.CopyToAsync(ms);
                    image = Convert.ToBase64String(ms.ToArray());
                }

            }

            var temp = new EditSoundDto()
            {
                Image = image,
                Name = sound.Name,
                Id = sound.Id,
            };
            return await _soundService.UpdateSound(temp, file);
        }

        [HttpDelete("DeleteSound")]
        public async Task<ResponseData<string>> DeleteSound(long id)
        {
            return await _soundService.DeleteSound(id);
        }
    }
}