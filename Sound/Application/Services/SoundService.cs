using Application.Repositories.IRepositories;
using Application.Services.IServices;
using Data.Dto;
using Data.Dto.Mix;
using Data.Common;

namespace Application.Services
{
    public class SoundService : ISoundService
    {
        private ISoundRepo _soundRepo;
        public SoundService(ISoundRepo soundRepo)
        {
            _soundRepo = soundRepo;
        }
        public async Task<ResponseData<Pagination<SoundDto>>> GetSound(int PageSize = 10, int PageNumber = 1)
        {
            return await _soundRepo.GetSound(PageSize, PageNumber);
        }
        public async Task<ResponseData<string>> AddSound(AddSoundDto sound, FileSound file)
        {
            return await _soundRepo.AddSound(sound, file);
        }
        public async Task<ResponseData<string>> UpdateSound(EditSoundDto sound, FileSound file)
        {
            return await _soundRepo.UpdateSound(sound, file);
        }
        public async Task<ResponseData<string>> DeleteSound(long id)
        {
            return await _soundRepo.DeleteSound(id);
        }
        public async Task<ResponseData<List<GetMixSoundDto>>> GetSound(int idMix)
        {
            return await _soundRepo.GetSound(idMix);
        }
        public async Task<ResponseData<string>> CreateMix(CreateMixSoundDto mix)
        {
            return await _soundRepo.CreateMix(mix);
        }
        public async Task<ResponseData<string>> SaveMix(UpdateMixSoundDto update)
        {
            return await _soundRepo.SaveMix(update);
        }
        public async Task<ResponseData<string>> ActiveSound(long id)
        {
            return await _soundRepo.ActiveSound(id);
        }
    }
}