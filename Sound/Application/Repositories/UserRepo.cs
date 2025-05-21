using Application.Repositories.IRepositories;
using System.Data.SqlClient;
using Data.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Data.Common;
using System.Data;
using Data.Dto.User;
using Data.Dto.Role;
using Sound.Application.Extentions;

namespace Application.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly IConfiguration _configuration;
        private readonly Extentions _extentions;
        string _connectionString = "";
        public UserRepo(IConfiguration configuration, Extentions extentions)
        {
            _extentions = extentions;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SoundDB");
        }
        public async Task<ResponseData<Pagination<UserDto>>> GetListUser(int PageSize = 10, int PageNumber = 1)
        {
            PageNumber = PageNumber < 0 ? PageNumber = 1 : PageNumber;
            List<UserDto> lst = new List<UserDto>();
            int totalPage = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("GetListStaff", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pageSize", System.Data.SqlDbType.Int).Value = PageSize;
                        cmd.Parameters.Add("@pageNumber", System.Data.SqlDbType.Int).Value = PageNumber;
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var temp = new UserDto
                                {
                                    Name = reader["Name"].ToString(),
                                    DisplayName = reader["DisplayName"].ToString(),
                                    IsConfirm = (bool)reader["IsConfirm"],
                                    IsDeleted = (bool)reader["IsDeleted"],
                                    Gender = reader["Gender"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Roles = reader["Roles"].ToString(),
                                    Id = Guid.Parse(reader["Id"].ToString()),
                                };
                                lst.Add(temp);
                            }
                            if (await reader.NextResultAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    totalPage = Convert.ToInt32(reader["TotalPage"]);
                                }
                            }
                        }
                    }
                    if (lst.Count > 0)
                    {
                        return new ResponseData<Pagination<UserDto>>
                        {
                            IsSuccess = true,
                            Data = new Pagination<UserDto>
                            {
                                TotalPage = totalPage,
                                CurrentPage = PageNumber,
                                Data = lst,
                            }
                        };
                    }
                    else
                    {
                        return new ResponseData<Pagination<UserDto>>
                        {
                            IsSuccess = false,
                            Data = new Pagination<UserDto>
                            {
                                TotalPage = totalPage,
                                CurrentPage = PageNumber,
                                Data = new List<UserDto>(),
                            }
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<Pagination<UserDto>>
                    {
                        IsSuccess = false,
                        Message = "Lỗi : " + ex.Message
                    };
                }
            }
        }
        public async Task<ResponseData<string>> CreateUser(CreateUserDto user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("CreateStaff", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        var infoUserDto = new InfoUserDto
                        {
                            Name = _extentions.GenerateUserNameByName(user.DisplayName),
                            Password = _extentions.GeneratePassword(),
                            Email = user.Email,
                            Code = _extentions.GenerateCodeConfirm(),
                        };
                        cmd.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 255).Value = user.Email;
                        cmd.Parameters.Add("@displayName", System.Data.SqlDbType.NVarChar, 50).Value = user.DisplayName;
                        cmd.Parameters.Add("@gender", System.Data.SqlDbType.Bit).Value = user.Gender;
                        cmd.Parameters.Add("@idBoss", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(_extentions.GetIdForToken(user.Token));
                        cmd.Parameters.Add("@password", System.Data.SqlDbType.VarChar, -1).Value = _extentions.HashPassword(infoUserDto.Password);
                        cmd.Parameters.Add("@codeConfirm", System.Data.SqlDbType.VarChar, 50).Value = infoUserDto.Code;
                        cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar, 50).Value = infoUserDto.Name;

                        var response = await cmd.ExecuteNonQueryAsync();

                        await _extentions.SendEmailAccount(infoUserDto);

                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Tạo nhân viên thành công!",
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Message = "Lỗi : " + ex.Message,
                    };
                }
            }
        }
        public async Task<ResponseData<string>> Admin(CreateAdminDto user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("CreateAdmin", conn))
                    {

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 255).Value = user.Email;
                        cmd.Parameters.Add("@displayName", System.Data.SqlDbType.NVarChar, 50).Value = user.DisplayName;
                        cmd.Parameters.Add("@gender", System.Data.SqlDbType.Bit).Value = user.Gender;
                        cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = user.Name;
                        cmd.Parameters.Add("@pass", System.Data.SqlDbType.VarChar, -1).Value = _extentions.HashPassword(user.Password);

                        var response = await cmd.ExecuteNonQueryAsync();

                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Tạo thành công!",
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Message = "Lỗi : " + ex.Message,
                    };
                }
            }
        }
        public async Task<ResponseData<string>> UpdateUser(UpdateInfoDto updateInfo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("UpdateInfo", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(_extentions.GetIdForToken(updateInfo.Token));
                        cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar, 50).Value = updateInfo.Name;
                        cmd.Parameters.Add("@displayName", System.Data.SqlDbType.NVarChar, 100).Value = updateInfo.DisplayName;
                        cmd.Parameters.Add("@phoneNumber", System.Data.SqlDbType.VarChar, 20).Value = updateInfo.PhoneNumber;
                        cmd.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 255).Value = updateInfo.Email;

                        var response = await cmd.ExecuteNonQueryAsync();

                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Cập nhật thông tin thành công!",
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Message = "Lỗi : " + ex.Message,
                    };
                }
            }
        }
        public async Task<ResponseData<string>> DeleteUser(ActionDto action)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("DeleteStaff", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", System.Data.SqlDbType.UniqueIdentifier).Value = action.IdUser;
                        cmd.Parameters.Add("@idBoss", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(_extentions.GetIdForToken(action.Token));

                        var response = await cmd.ExecuteNonQueryAsync();

                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Xoá nhân viên thành công!",
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Message = "Lỗi : " + ex.Message,
                    };
                }
            }
        }
        public async Task<ResponseData<string>> ActiveUser(ActionDto action)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("ActiveStaff", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", System.Data.SqlDbType.UniqueIdentifier).Value = action.IdUser;
                        cmd.Parameters.Add("@idBossUpdate", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(_extentions.GetIdForToken(action.Token));

                        var response = await cmd.ExecuteNonQueryAsync();

                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Kích hoạt nhân viên thành công!",
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Message = "Lỗi : " + ex.Message,
                    };
                }
            }
        }
        public async Task<ResponseData<string>> Login(LoginDto dataLogin)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("Login", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@userName", System.Data.SqlDbType.VarChar, 50).Value = dataLogin.Name;
                        var data = new LoginDataDto();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                data.Password = reader["PasswordHash"].ToString();
                                data.Id = reader["Id"].ToString();
                                data.Roles = reader["Roles"].ToString();
                                data.IsConfirm = (bool)reader["IsConfirm"];
                                if (_extentions.VerifyPassword(data.Password, dataLogin.Password))
                                {
                                    return new ResponseData<string>
                                    {
                                        IsSuccess = true,
                                        Message = "Đăng nhập thành công!",
                                        Data = _extentions.GenerateTokenString(data),
                                    };
                                }
                                else
                                {
                                    return new ResponseData<string>
                                    {
                                        IsSuccess = false,
                                        Message = "Tài khoản hoặc mật khẩu không chính xác hoặc tài khoản đã bị khoá!!",
                                    };
                                }
                            }
                            else
                            {
                                return new ResponseData<string>
                                {
                                    IsSuccess = false,
                                    Message = "Tài khoản hoặc mật khẩu không chính xác hoặc tài khoản đã bị khoá!!",
                                };
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Message = "Lỗi : " + ex.Message,
                    };
                }
            }
        }
        public async Task<ResponseData<string>> UpdateRole(UpdateRoleUserDto updateRole)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("UpdateRoleUser", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdUser", System.Data.SqlDbType.UniqueIdentifier).Value = updateRole.IdUser;

                        DataTable idRole = new DataTable();
                        idRole.Columns.Add("IdRole", typeof(Guid));
                        foreach (var item in updateRole.ListIdRole)
                        {
                            idRole.Rows.Add(item);
                        }
                        var param = cmd.Parameters.AddWithValue("@Ids", idRole);
                        param.SqlDbType = SqlDbType.Structured;
                        param.TypeName = "ListIdRole";
                        var response = await cmd.ExecuteNonQueryAsync();

                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Thay đổi danh sách quyền thành công!"
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Message = "Lỗi : " + ex.Message,
                    };
                }
            }
        }
        public async Task<ResponseData<List<RoleDto>>> GetListRole()
        {
            List<RoleDto> lst = new List<RoleDto>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("GetListRole", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var temp = new RoleDto
                                {
                                    Id = Guid.Parse(reader["Id"].ToString()),
                                    Name = reader["DisplayName"].ToString(),
                                };
                                lst.Add(temp);
                            }
                        }
                    }
                    if (lst.Count > 0)
                    {
                        return new ResponseData<List<RoleDto>>
                        {
                            IsSuccess = true,
                            Data = lst,
                        };
                    }
                    else
                    {
                        return new ResponseData<List<RoleDto>>
                        {
                            IsSuccess = false,
                            Data = new List<RoleDto>()
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<List<RoleDto>>
                    {
                        IsSuccess = false,
                        Message = "Lỗi : " + ex.Message
                    };
                }
            }
        }
        public async Task<ResponseData<string>> ChangePassword(ChangePasswordDto changePassword)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    if (changePassword.NewPassword != changePassword.ConfirmPassword)
                    {
                        return new ResponseData<string>
                        {
                            IsSuccess = false,
                            Message = "Mật khẩu không khớp!"
                        };
                    }
                    else
                    {
                        await conn.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand("GetHashPasswordUser", conn))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@id", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(_extentions.GetIdForToken(changePassword.Token));

                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    if (_extentions.VerifyPassword(reader["PasswordHash"].ToString(), changePassword.OldPassword))
                                    {
                                        using (SqlCommand cmd1 = new SqlCommand("ChangePassword", conn))
                                        {
                                            cmd1.CommandType = System.Data.CommandType.StoredProcedure;

                                            cmd1.Parameters.Add("@id", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(_extentions.GetIdForToken(changePassword.Token));
                                            cmd1.Parameters.Add("@newPass", System.Data.SqlDbType.VarChar, -1).Value = _extentions.HashPassword(changePassword.NewPassword);

                                            var response = await cmd1.ExecuteNonQueryAsync();
                                        }
                                        return new ResponseData<string>
                                        {
                                            IsSuccess = true,
                                            Message = "Đổi mật khẩu thành công!",
                                        };
                                    }
                                    else
                                    {
                                        return new ResponseData<string>
                                        {
                                            IsSuccess = false,
                                            Message = "Mật khẩu cũ không chính xác!"
                                        };
                                    }
                                }
                                else
                                {
                                    return new ResponseData<string>
                                    {
                                        IsSuccess = false,
                                        Message = "Không tìm thấy tài khoản!"
                                    };
                                }
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Message = "Lỗi : " + ex.Message
                    };
                }
            }
        }
        public async Task<ResponseData<string>> ForgotPassword(string email)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("ForgotPassword", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 255).Value = email;

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var code = _extentions.GenerateCodeConfirm();
                                using (SqlCommand cmd1 = new SqlCommand("UpdateCodeForgotPassword", conn))
                                {
                                    cmd1.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd1.Parameters.Add("@id", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(reader["Id"].ToString());
                                    cmd1.Parameters.Add("@code", System.Data.SqlDbType.VarChar, 50).Value = code;

                                    var response = await cmd1.ExecuteNonQueryAsync();
                                    await _extentions.SendEmailForgotPassword(reader["Email"].ToString(), code);
                                    return new ResponseData<string>
                                    {
                                        IsSuccess = true,
                                        Message = "Đã gửi mã xác nhận đến email của bạn!",
                                    };
                                }
                            }
                            else
                            {
                                return new ResponseData<string>
                                {
                                    IsSuccess = false,
                                    Message = "Tài khoản không tồn tại!"
                                };
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Message = "Lỗi : " + ex.Message
                    };
                }
            }
        }
        public async Task<ResponseData<string>> ChangeForgotPassword(ForgotPasswordDto forgotPassword)
        {
            try
            {
                if (forgotPassword.NewPassword != forgotPassword.ConfirmPassword)
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Message = "Mật khẩu không khớp!"
                    };
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        await conn.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand("CheckCodeForgot", conn))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 50).Value = forgotPassword.Email;
                            cmd.Parameters.Add("@code", System.Data.SqlDbType.VarChar, -1).Value = forgotPassword.Code;

                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    if (reader["CodeConfirm"].ToString() == forgotPassword.Code)
                                    {
                                        using (SqlCommand cmd1 = new SqlCommand("ChangeForgotPassword", conn))
                                        {
                                            cmd1.CommandType = System.Data.CommandType.StoredProcedure;

                                            cmd1.Parameters.Add("@id", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(reader["Id"].ToString());
                                            cmd1.Parameters.Add("@newPass", System.Data.SqlDbType.VarChar, -1).Value = _extentions.HashPassword(forgotPassword.NewPassword);

                                            var response = await cmd1.ExecuteNonQueryAsync();
                                        }
                                        return new ResponseData<string>
                                        {
                                            IsSuccess = true,
                                            Message = "Đổi mật khẩu thành công!",
                                        };
                                    }
                                    else
                                    {
                                        return new ResponseData<string>
                                        {
                                            IsSuccess = false,
                                            Message = "Mã xác nhận không chính xác!"
                                        };
                                    }
                                }
                                else
                                {
                                    return new ResponseData<string>
                                    {
                                        IsSuccess = false,
                                        Message = "Không tìm thấy tài khoản!"
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Lỗi SQL: " + ex.Message);
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Message = "Lỗi : " + ex.Message
                };
            }
        }
    }
}