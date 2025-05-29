using Data.Dto;
using Data.Common;
using Data.Dto.Mix;

namespace Application.Repositories.IRepositories
{
    public interface ISoundRepo
    {
        Task<ResponseData<Pagination<SoundDto>>> GetSound(int PageSize = 10, int PageNumber = 1);
        Task<ResponseData<Pagination<AdminSoundDto>>> GetSoundByAdmin(int PageSize = 10, int PageNumber = 1);
        Task<ResponseData<Pagination<AdminSoundDto>>> SearchSound(int PageSize = 10, int PageNumber = 1,string Key="");
        Task<ResponseData<Pagination<AdminSoundDto>>> FilterSoundByStatus(int PageSize = 10, int PageNumber = 1,bool Status=false);
        Task<ResponseData<List<GetMixSoundDto>>> GetSound(int idMix);
        Task<ResponseData<string>> CreateMix(CreateMixSoundDto mix);
        Task<ResponseData<string>> SaveMix(UpdateMixSoundDto update);
        Task<ResponseData<string>> AddSound(AddSoundDto sound, FileSound file);
        Task<ResponseData<string>> UpdateSound(EditSoundDto sound, FileSound file);
        Task<ResponseData<string>> DeleteSound(long id);
        Task<ResponseData<string>> ActiveSound(long id);
    }
}
