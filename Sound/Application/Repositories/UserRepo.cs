using Application.Repositories.IRepositories;
using System.Data.SqlClient;
using Data.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Data.Common;
using System.Data;
using Data.Dto.User;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;
using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace Application.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly IConfiguration _configuration;
        string _connectionString = "";
        public UserRepo(IConfiguration configuration)
        {
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

                        cmd.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 255).Value = user.Email;
                        cmd.Parameters.Add("@displayName", System.Data.SqlDbType.NVarChar, 50).Value = user.DisplayName;
                        cmd.Parameters.Add("@gender", System.Data.SqlDbType.Bit).Value = user.Gender;
                        cmd.Parameters.Add("@idBoss", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(GetIdForToken(user.Token));
                        cmd.Parameters.Add("@password", System.Data.SqlDbType.VarChar, -1).Value = HashPassword(GeneratePassword());
                        cmd.Parameters.Add("@codeConfirm", System.Data.SqlDbType.VarChar, 50).Value = GenerateCodeConfirm();
                        cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar, 50).Value = GenerateUserNameByName(user.DisplayName);

                        var response = await cmd.ExecuteNonQueryAsync();

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
                        cmd.Parameters.Add("@pass", System.Data.SqlDbType.VarChar, -1).Value = HashPassword(user.Password);

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
        public async Task<ResponseData<string>> UpdateUser()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    // using (SqlCommand cmd = new SqlCommand("CreateStaff", conn))
                    // {
                    //     cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //     cmd.Parameters.Add("@email", System.Data.SqlDbType.NVarchar, 255).Value = user.Email;
                    //     cmd.Parameters.Add("@displayName", System.Data.SqlDbType.NVarchar, 50).Value = user.DisplayName;
                    //     cmd.Parameters.Add("@gender", System.Data.SqlDbType.Bit).Value = user.Gender;
                    //     cmd.Parameters.Add("@idBoss", System.Data.SqlDbType.UniqueIdentifier).Value = user.IdBoss;
                    //     cmd.Parameters.Add("@password", System.Data.SqlDbType.VarChar, -1).Value = user.Password;
                    //     cmd.Parameters.Add("@codeConfirm", System.Data.SqlDbType.VarChar, 50).Value = user.CodeConfirm;

                    //     var response = await cmd.ExecuteNonQueryAsync();

                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Message = "Chưa có api!",
                    };
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
                        cmd.Parameters.Add("@idBoss", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(GetIdForToken(action.Token));

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
                        cmd.Parameters.Add("@idBoss", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(GetIdForToken(action.Token));

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
                                data.TotalRow = Convert.ToInt32(reader["TotalRow"]);
                                if (data.TotalRow == 1)
                                {
                                    if (await reader.NextResultAsync())
                                    {
                                        if (await reader.ReadAsync())
                                        {
                                            data.Password = reader["PasswordHash"].ToString();
                                            data.Id = reader["Id"].ToString();
                                            data.Roles = reader["Roles"].ToString();
                                        }
                                        if (VerifyPassword(data.Password, dataLogin.Password))
                                        {
                                            return new ResponseData<string>
                                            {
                                                IsSuccess = true,
                                                Message = "Đăng nhập thành công!",
                                                Data = GenerateTokenString(data),
                                            };
                                        }
                                        else
                                        {
                                            return new ResponseData<string>
                                            {
                                                IsSuccess = false,
                                                Message = "Tài khoản hoặc mật khẩu không chính xác!",
                                            };
                                        }
                                    }
                                    else
                                    {
                                        return new ResponseData<string>
                                        {
                                            IsSuccess = false,
                                            Message = "Tài khoản không tồn tại!",
                                        };
                                    }

                                }
                                else
                                {
                                    return new ResponseData<string>
                                    {
                                        IsSuccess = false,
                                        Message = "Tài khoản không tồn tại!",
                                    };
                                }

                            }
                            else
                            {
                                return new ResponseData<string>
                                {
                                    IsSuccess = false,
                                    Message = "Tài khoản không tồn tại!",
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
        public string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8, // Số luồng xử lý
                Iterations = 4,          // Số lần lặp
                MemorySize = 1024 * 64   // Dung lượng bộ nhớ (64 MB)
            };

            byte[] hashBytes = argon2.GetBytes(32); // Kích thước hash 32 bytes

            // Kết hợp salt + hash thành một chuỗi để lưu (Base64)
            byte[] result = new byte[salt.Length + hashBytes.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(hashBytes, 0, result, salt.Length, hashBytes.Length);

            return Convert.ToBase64String(result);
        }
        public bool VerifyPassword(string hashedPassword, string password)
        {
            byte[] decoded = Convert.FromBase64String(hashedPassword);

            // Tách salt và hash
            byte[] salt = new byte[16];
            byte[] hash = new byte[32];

            Buffer.BlockCopy(decoded, 0, salt, 0, 16);
            Buffer.BlockCopy(decoded, 16, hash, 0, 32);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 1024 * 64
            };

            byte[] computedHash = argon2.GetBytes(32);

            // So sánh hash
            return CryptographicOperations.FixedTimeEquals(hash, computedHash);
        }
        public string GenerateTokenString(LoginDataDto data)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, data.Id.ToString()),
            };
            foreach (var item in data.Roles.Split(","))
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678901234567890123456789012"));
            SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            SecurityToken securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                issuer: "",
                audience: "",
                signingCredentials: signingCred
                );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
        public string GenerateCodeConfirm()
        {
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                Random random = new Random();
                if (random.Next(0, 10) % 2 == 0)
                {
                    code += random.Next(0, 9).ToString();
                }
                else code += Convert.ToChar(random.Next(65, 90)).ToString();
            }
            return code;
        }
        public string GetIdForToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                return claim.Value;
            }
            return null;
        }
        public string GeneratePassword()
        {
            var random = new Random();
            var passwordChars = new List<char>();

            // 1 ký tự viết hoa
            passwordChars.Add((char)random.Next(65, 91)); // A-Z

            // 6 ký tự: số hoặc chữ hoa ngẫu nhiên
            for (int i = 0; i < 6; i++)
            {
                if (random.Next(2) == 0)
                    passwordChars.Add((char)random.Next(48, 58)); // 0–9
                else
                    passwordChars.Add((char)random.Next(65, 91)); // A–Z
            }

            // 1 ký tự đặc biệt
            var ranges = new[] { (33, 48), (58, 65) };
            var selected = ranges[random.Next(ranges.Length)];
            passwordChars.Add((char)random.Next(selected.Item1, selected.Item2));

            return new string(passwordChars.OrderBy(_ => random.Next()).ToArray());
        }
        public string GenerateUserNameByName(string name)
        {
            var normalizedString = name.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            var result = stringBuilder.ToString().ToLower().Normalize(NormalizationForm.FormC);
            string[] values = result.Split(' ');
            var username = values[values.Length - 1];
            for (int i = 0; i < values.Length - 1; i++)
            {
                username += values[i][0].ToString();
            }
            return username;
        }
    }
}