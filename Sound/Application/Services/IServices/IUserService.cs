using Data.Dto;
using Data.Common;
using Data.Dto.Mix;
using Data.Dto.User;
using Data.Dto.Role;

namespace Application.Services.IServices
{
    public interface IUserService
    {
        Task<ResponseData<Pagination<UserDto>>> GetListUser(int PageSize = 10, int PageNumber = 1);
        Task<ResponseData<List<RoleDto>>> GetListRole();
        Task<ResponseData<string>> CreateUser(CreateUserDto user);
        Task<ResponseData<string>> UpdateUser(UpdateInfoDto updateInfo);
        Task<ResponseData<string>> Admin(CreateAdminDto user);
        Task<ResponseData<string>> DeleteUser(ActionDto action);
        Task<ResponseData<string>> ActiveUser(ActionDto action);
        Task<ResponseData<string>> Login(LoginDto dataLogin);
        Task<ResponseData<string>> UpdateRole(UpdateRoleUserDto updateRole);
        Task<ResponseData<string>> ChangePassword(ChangePasswordDto changePassword);
        Task<ResponseData<string>> ForgotPassword(string email);
        Task<ResponseData<string>> ChangeForgotPassword(ForgotPasswordDto forgotPassword);
    }
}