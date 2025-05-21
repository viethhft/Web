using Application.Repositories.IRepositories;
using Application.Services.IServices;
using Data.Dto;
using Data.Dto.Mix;
using Data.Common;
using Data.Dto.User;
using Data.Dto.Role;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepo _userRepo;
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<ResponseData<Pagination<UserDto>>> GetListUser(int PageSize = 10, int PageNumber = 1)
        {
            return await _userRepo.GetListUser(PageSize, PageNumber);
        }
        public async Task<ResponseData<List<RoleDto>>> GetListRole()
        {
            return await _userRepo.GetListRole();
        }
        public async Task<ResponseData<string>> CreateUser(CreateUserDto user)
        {
            return await _userRepo.CreateUser(user);
        }
        public async Task<ResponseData<string>> UpdateUser(UpdateInfoDto updateInfo)
        {
            return await _userRepo.UpdateUser(updateInfo);
        }
        public async Task<ResponseData<string>> DeleteUser(ActionDto action)
        {
            return await _userRepo.DeleteUser(action);
        }
        public async Task<ResponseData<string>> ActiveUser(ActionDto action)
        {
            return await _userRepo.ActiveUser(action);
        }
        public async Task<ResponseData<string>> Login(LoginDto dataLogin)
        {
            return await _userRepo.Login(dataLogin);
        }
        public async Task<ResponseData<string>> Admin(CreateAdminDto user)
        {
            return await _userRepo.Admin(user);
        }
        public async Task<ResponseData<string>> UpdateRole(UpdateRoleUserDto updateRole)
        {
            return await _userRepo.UpdateRole(updateRole);
        }

        public async Task<ResponseData<string>> ChangePassword(ChangePasswordDto changePassword)
        {
            return await _userRepo.ChangePassword(changePassword);
        }
        public async Task<ResponseData<string>> ForgotPassword(string email)
        {
            return await _userRepo.ForgotPassword(email);
        }
        public async Task<ResponseData<string>> ChangeForgotPassword(ForgotPasswordDto forgotPassword)
        {
            return await _userRepo.ChangeForgotPassword(forgotPassword);
        }
    }
}