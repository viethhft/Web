using Data.Dto;

namespace Application.Services.IServices
{
    public interface ISoundService
    {
        public Task<List<SoundDto>> GetSound(int PageSize = 10, int PageNumber = 1);
        public Task<string> AddSound(AddSoundDto sound, FileSound file);
        public Task<string> UpdateSound(EditSoundDto sound, FileSound file);
        public Task<string> DeleteSound(long id);
    }
}