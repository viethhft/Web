using Microsoft.AspNetCore.Mvc;
using Data.Dto.User;
using Application.Services.IServices;
using Microsoft.AspNetCore.Http;
using Data.Common;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<ResponseData<string>> UpdateUser()
        {
            return await _userService.UpdateUser();
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
    }
}