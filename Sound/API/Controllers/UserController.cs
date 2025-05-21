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

        [HttpGet("GetListUser")]
        public async Task<ResponseData<Pagination<UserDto>>> GetListUser(int PageSize = 10, int PageNumber = 1)
        {
            return await _userService.GetListUser(PageSize, PageNumber);
        }

        [HttpPost("CreateUser")]
        public async Task<ResponseData<string>> CreateUser(CreateUserDto user)
        {
            return await _userService.CreateUser(user);
        }

        [HttpPut("UpdateUser")]
        public async Task<ResponseData<string>> UpdateUser(UpdateInfoDto updateInfo)
        {
            return await _userService.UpdateUser(updateInfo);
        }

        [HttpDelete("DeleteUser")]
        public async Task<ResponseData<string>> DeleteUser(ActionDto action)
        {
            return await _userService.DeleteUser(action);
        }

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
        [HttpPost("CreateAccountAdmin")]
        public async Task<ResponseData<string>> Admin(CreateAdminDto user)
        {
            return await _userService.Admin(user);
        }
        [HttpPut("UpdateRole")]
        public async Task<ResponseData<string>> UpdateRole(UpdateRoleUserDto updateRole)
        {
            return await _userService.UpdateRole(updateRole);
        }
        [HttpGet("GetListRole")]
        public async Task<ResponseData<List<RoleDto>>> GetListRole()
        {
            return await _userService.GetListRole();
        }
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
    }
}