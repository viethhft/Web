using Data.Dto;
using Data.Common;
using Data.Dto.Mix;
using Data.Dto.User;

namespace Application.Services.IServices
{
    public interface IUserService
    {
        Task<ResponseData<Pagination<UserDto>>> GetListUser(int PageSize = 10, int PageNumber = 1);
        Task<ResponseData<string>> CreateUser(CreateUserDto user);
        Task<ResponseData<string>> UpdateUser();
        Task<ResponseData<string>> Admin(CreateAdminDto user);
        Task<ResponseData<string>> DeleteUser(ActionDto action);
        Task<ResponseData<string>> ActiveUser(ActionDto action);
        Task<ResponseData<string>> Login(LoginDto dataLogin);
    }
}