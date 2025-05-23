using Microsoft.AspNetCore.Mvc;
using Data.Dto;
using Application.Services.IServices;
using Microsoft.AspNetCore.Http;
using Data.Common;
using Data.Dto.Mix;
using Microsoft.AspNetCore.Authorization;
using Sound.Application.Extentions;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoundController : Controller
    {
        private readonly ISoundService _soundService;
        private readonly Extentions _extentions;
        public SoundController(ISoundService soundService, Extentions ext)
        {
            _extentions = ext;
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

        [HttpGet("GetSoundByAdmin")]
        public async Task<ResponseData<Pagination<AdminSoundDto>>> GetSoundByAdmin(int PageSize = 10, int PageNumber = 1)
        {
            var repsonse = await _soundService.GetSoundByAdmin(PageSize, PageNumber);
            if (repsonse.IsSuccess)
            {
                return new ResponseData<Pagination<AdminSoundDto>>
                {
                    IsSuccess = true,
                    Data = new Pagination<AdminSoundDto>
                    {
                        TotalPage = repsonse.Data.TotalPage,
                        CurrentPage = repsonse.Data.CurrentPage,
                        Data = repsonse.Data.Data,
                    },
                };
            }
            else
            {
                return new ResponseData<Pagination<AdminSoundDto>>
                {
                    IsSuccess = false,
                    Message = "Không có dữ liệu.",
                };
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost("AddSound")]
        public async Task<ResponseData<string>> AddSound(AddSound sound)
        {
            if ((sound.File != null && sound.File.Length > 0 && sound.File.ContentType == "audio/mpeg") && (sound.Image != null && sound.Image.Length > 0 && sound.Image.ContentType.StartsWith("image/")))
            {
                byte[] content;
                string image = "";

                content = await _extentions.CompressMp3Async(sound.File);
    
                using (var ms = new MemoryStream())
                {
                    await sound.Image.CopyToAsync(ms);
                    var data = ms.ToArray();
                    image += $"data:{sound.Image.ContentType};base64,";
                    image += Convert.ToBase64String(data);
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
                    Token = sound.Token
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

        // [Authorize(Roles = "Admin")]
        [HttpPut("UpdateSound")]
        public async Task<ResponseData<string>> UpdateSound(EditSound sound)
        {
            byte[] content;
            string image = "";
            FileSound file = new FileSound();

            if ((sound.File != null && sound.File.Length > 0 && sound.File.ContentType == "audio/mpeg"))
            {
                content = await _extentions.CompressMp3Async(sound.File);

                file = new FileSound()
                {
                    Content = content,
                    ContentType = sound.File.ContentType,
                    FileName = sound.File.FileName
                };
            }

            if ((sound.Image != null && sound.Image.Length > 0 && sound.Image.ContentType.StartsWith("image/")))
            {
                using (var ms = new MemoryStream())
                {
                    await sound.Image.CopyToAsync(ms);
                    var data = ms.ToArray();
                    image += $"data:{sound.Image.ContentType};base64,";
                    image += Convert.ToBase64String(data);
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

        // [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteSound")]
        public async Task<ResponseData<string>> DeleteSound(long id)
        {
            return await _soundService.DeleteSound(id);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPatch("ActiveSound")]
        public async Task<ResponseData<string>> ActiveSound(long id)
        {
            return await _soundService.ActiveSound(id);
        }

        [HttpGet("GetSoundMix")]
        public async Task<ResponseData<List<GetMixSoundDto>>> GetSound(int idMix)
        {
            return await _soundService.GetSound(idMix);
        }

        [HttpPost("CreateMixSound")]
        public async Task<ResponseData<string>> CreateMix(CreateMixSoundDto mix)
        {
            return await _soundService.CreateMix(mix);
        }

        [HttpPut("SaveMixSound")]
        public async Task<ResponseData<string>> SaveMix(UpdateMixSoundDto update)
        {
            return await _soundService.SaveMix(update);
        }
    }
}