using Application.Repositories.IRepositories;
using Application.Services.IServices;
using Data.Dto;
using Data.Dto.Mix;
using Data.Common;
using Data.Dto.User;

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
        public async Task<ResponseData<string>> CreateUser(CreateUserDto user)
        {
            return await _userRepo.CreateUser(user);
        }
        public async Task<ResponseData<string>> UpdateUser()
        {
            return await _userRepo.UpdateUser();
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
    }
}