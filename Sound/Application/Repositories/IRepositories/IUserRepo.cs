using Data.Dto;
using Data.Common;
using Data.Dto.User;

namespace Application.Repositories.IRepositories
{
    public interface IUserRepo
    {
        Task<ResponseData<Pagination<UserDto>>> GetListUser(int PageSize = 10, int PageNumber = 1);
        Task<ResponseData<string>> CreateUser(CreateUserDto user);
        Task<ResponseData<string>> Admin(CreateAdminDto user);
        Task<ResponseData<string>> UpdateUser();
        Task<ResponseData<string>> DeleteUser(ActionDto action);
        Task<ResponseData<string>> ActiveUser(ActionDto action);
        Task<ResponseData<string>> Login(LoginDto dataLogin);
    }
}