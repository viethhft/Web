using Microsoft.AspNetCore.Mvc;
using Data.Dto.User;
using Application.Services.IServices;
using Microsoft.AspNetCore.Http;
using Data.Common;
using Microsoft.AspNetCore.Authorization;
using Data.Dto.Role;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetListUser")]
        public async Task<ResponseData<Pagination<UserDto>>> GetListUser(int PageSize = 10, int PageNumber = 1)
        {
            return await _userService.GetListUser(PageSize, PageNumber);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("FilterUserByRole")]
        public async Task<ResponseData<Pagination<UserDto>>> FilterUserByRole(int PageSize = 10, int PageNumber = 1, string Role = "")
        {
            return await _userService.FilterUserByRole(PageSize, PageNumber, Role);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("FilterUserByStatus")]
        public async Task<ResponseData<Pagination<UserDto>>> FilterUserByStatus(int PageSize = 10, int PageNumber = 1, bool Status = true)
        {
            return await _userService.FilterUserByStatus(PageSize, PageNumber, Status);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("SearchUser")]
        public async Task<ResponseData<Pagination<UserDto>>> SearchUser(int PageSize = 10, int PageNumber = 1, string Key = "")
        {
            return await _userService.SearchUser(PageSize, PageNumber, Key);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("CreateUser")]
        public async Task<ResponseData<string>> CreateUser(CreateUserDto user)
        {
            return await _userService.CreateUser(user);
        }

        [Authorize(Roles = "ADMIN, STAFF")]
        [HttpPut("UpdateUser")]
        public async Task<ResponseData<string>> UpdateUser(UpdateInfoDto updateInfo)
        {
            return await _userService.UpdateUser(updateInfo);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("DeleteUser")]
        public async Task<ResponseData<string>> DeleteUser(ActionDto action)
        {
            return await _userService.DeleteUser(action);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("ActiveUser")]
        public async Task<ResponseData<string>> ActiveUser(ActionDto action)
        {
            return await _userService.ActiveUser(action);
        }


        [HttpPost("Login")]
        public async Task<ResponseData<string>> Login(LoginDto dataLogin)
        {
            return await _userService.Login(dataLogin);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("CreateAccountAdmin")]
        public async Task<ResponseData<string>> Admin(CreateAdminDto user)
        {
            return await _userService.Admin(user);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("UpdateRole")]
        public async Task<ResponseData<string>> UpdateRole(UpdateRoleUserDto updateRole)
        {
            return await _userService.UpdateRole(updateRole);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetListRole")]
        public async Task<ResponseData<List<RoleDto>>> GetListRole()
        {
            return await _userService.GetListRole();
        }

        [Authorize(Roles = "ADMIN, STAFF")]
        [HttpPatch("ChangePassword")]
        public async Task<ResponseData<string>> ChangePassword(ChangePasswordDto changePassword)
        {
            return await _userService.ChangePassword(changePassword);
        }

        [HttpGet("ForgotPassword")]
        public async Task<ResponseData<string>> ForgotPassword(string email)
        {
            return await _userService.ForgotPassword(email);
        }

        [HttpPatch("ChangeForgotPassword")]
        public async Task<ResponseData<string>> ChangeForgotPassword(ForgotPasswordDto forgotPassword)
        {
            return await _userService.ChangeForgotPassword(forgotPassword);
        }

        [Authorize(Roles = "ADMIN, STAFF")]
        [HttpGet("GetProfileUser")]
        public async Task<ResponseData<ProfileUserDto>> GetUserInfo(string token)
        {
            return await _userService.GetUserInfo(token);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("VerifyFirstLogIn")]
        public async Task<ResponseData<string>> VerifyFirstLogIn(FirstLogInDto firstLogInDto)
        {
            return await _userService.VerifyFirstLogIn(firstLogInDto);
        }
    }
}