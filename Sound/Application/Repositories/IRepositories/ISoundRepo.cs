using Data.Dto;

namespace Application.Repositories.IRepositories
{
    public interface ISoundRepo
    {
        Task<List<SoundDto>> GetSound(int PageSize = 10, int PageNumber = 1);
        Task<string> AddSound(AddSoundDto sound, FileSound file);
        Task<string> UpdateSound(EditSoundDto sound, FileSound file);
        Task<string> DeleteSound(long id);
    }
}
