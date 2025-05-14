using Application.Repositories.IRepositories;
using Application.Services.IServices;
using Data.Dto;

namespace Application.Services
{
    public class SoundService : ISoundService
    {
        private ISoundRepo _soundRepo;
        public SoundService(ISoundRepo soundRepo)
        {
            _soundRepo = soundRepo;
        }
        public async Task<List<SoundDto>> GetSound(int PageSize = 10, int PageNumber = 1)
        {
            return await _soundRepo.GetSound(PageSize, PageNumber);
        }
        public async Task<string> AddSound(AddSoundDto sound, FileSound file)
        {
            return await _soundRepo.AddSound(sound, file);
        }
        public async Task<string> UpdateSound(EditSoundDto sound, FileSound file)
        {
            return await _soundRepo.UpdateSound(sound, file);
        }
        public async Task<string> DeleteSound(long id)
        {
            return await _soundRepo.DeleteSound(id);
        }
    }
}