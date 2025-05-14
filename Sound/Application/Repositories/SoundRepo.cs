using Application.Repositories.IRepositories;
using System.Data.SqlClient;
using Data.Dto;
using Data.Dto.Mix;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Data.Common;
using System.Data;

namespace Application.Repositories
{
    public class SoundRepo : ISoundRepo
    {
        private readonly IConfiguration _configuration;
        string _connectionString = "";
        public SoundRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SoundDB");
        }

        public async Task<ResponseData<Pagination<SoundDto>>> GetSound(int PageSize = 10, int PageNumber = 1)
        {
            PageNumber = PageNumber < 0 ? PageNumber = 1 : PageNumber;
            List<SoundDto> lst = new List<SoundDto>();
            int totalPage = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("GetSoundData", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pageSize", System.Data.SqlDbType.Int).Value = PageSize;
                        cmd.Parameters.Add("@pageNumber", System.Data.SqlDbType.Int).Value = PageNumber;
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var temp = new SoundDto
                                {
                                    Name = reader["Name"].ToString(),
                                    Image = reader["Image"].ToString(),
                                    Id = (long)reader["Id"],
                                    FileName = reader["FileName"].ToString(),
                                    Content = reader["Content"] as byte[],
                                    ContentType = reader["ContentType"].ToString()
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
                    return new ResponseData<Pagination<SoundDto>>
                    {
                        IsSuccess = true,
                        Data = new Pagination<SoundDto>
                        {
                            TotalPage = totalPage,
                            CurrentPage = PageNumber,
                            Data = lst,
                        }
                    };
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<Pagination<SoundDto>>
                    {
                        IsSuccess = false,
                        Message = "Lỗi : " + ex.Message
                    };
                }
            }
        }

        public async Task<ResponseData<string>> AddSound(AddSoundDto sound, FileSound file)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("AddSound", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 50).Value = sound.Name;
                        cmd.Parameters.Add("@Image", System.Data.SqlDbType.VarChar, 50).Value = sound.Image;

                        cmd.Parameters.Add("@FileName", System.Data.SqlDbType.NVarChar, 50).Value = file.FileName;
                        cmd.Parameters.Add("@Content", System.Data.SqlDbType.VarBinary, -1).Value = file.Content;
                        cmd.Parameters.Add("@ContentType", System.Data.SqlDbType.VarChar, 50).Value = file.ContentType;
                        var response = await cmd.ExecuteNonQueryAsync();
                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Thêm sound thành công!",
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

        public async Task<ResponseData<string>> UpdateSound(EditSoundDto sound, FileSound file)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("UpdateSound", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.BigInt).Value = sound.Id;
                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 50).Value = sound.Name;
                        cmd.Parameters.Add("@Image", System.Data.SqlDbType.VarChar, 50).Value = sound.Image;
                        if (file.Content != null && file.Content.Length > 0)
                        {
                            cmd.Parameters.Add("@FileName", System.Data.SqlDbType.NVarChar, 50).Value = file.FileName;
                            cmd.Parameters.Add("@Content", System.Data.SqlDbType.VarBinary, -1).Value = file.Content;
                            cmd.Parameters.Add("@ContentType", System.Data.SqlDbType.VarChar, 50).Value = file.ContentType;
                        }
                        var response = await cmd.ExecuteNonQueryAsync();
                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Sửa sound thành công!",
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

        public async Task<ResponseData<string>> DeleteSound(long id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("DeleteSound", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.BigInt).Value = id;

                        var response = await cmd.ExecuteNonQueryAsync();
                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Xoá sound thành công!",
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
        public async Task<ResponseData<List<GetMixSoundDto>>> GetSound(int idMix)
        {
            List<GetMixSoundDto> lst = new List<GetMixSoundDto>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("GetMix", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdMix", System.Data.SqlDbType.BigInt).Value = idMix;

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            var temp = new GetMixSoundDto
                            {
                                SoundName = reader["Name"].ToString(),
                                Image = reader["Image"].ToString(),
                                FileName = reader["FileName"].ToString(),
                                Content = reader["Content"] as byte[],
                                ContentType = reader["ContentType"].ToString(),
                            };
                            lst.Add(temp);
                        }
                    }
                    return new ResponseData<List<GetMixSoundDto>>
                    {
                        IsSuccess = true,
                        Data = lst,
                    };
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<List<GetMixSoundDto>>
                    {
                        IsSuccess = true,
                        Message = "Lỗi : " + ex.Message,
                    };
                }
            }
        }
        public async Task<ResponseData<string>> CreateMix(CreateMixSoundDto mix)
        {
            List<GetMixSoundDto> lst = new List<GetMixSoundDto>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("CreateMix", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.BigInt).Value = mix.Name;

                        DataTable idSound = new DataTable();
                        idSound.Columns.Add("IdSound", typeof(long));
                        foreach (var item in mix.IdSounds)
                        {
                            idSound.Rows.Add(item);
                        }
                        var param = cmd.Parameters.AddWithValue("@Ids", idSound);
                        param.SqlDbType = SqlDbType.Structured;
                        param.TypeName = "IdListType";
                        var response = await cmd.ExecuteNonQueryAsync();
                    }

                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Message = "Thêm danh sách nhạc mix thành công!"
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
        public async Task<ResponseData<string>> SaveMix()
        {
            await Task.Delay(1000);
            return new ResponseData<string>
            {
                IsSuccess = true,
                Message = "Chưa có Api",
            };
        }
    }
}