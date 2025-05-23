using Application.Repositories.IRepositories;
using System.Data.SqlClient;
using Data.Dto;
using Data.Dto.Mix;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Data.Common;
using System.Data;
using Sound.Application.Extentions;

namespace Application.Repositories
{
    public class SoundRepo : ISoundRepo
    {
        private readonly IConfiguration _configuration;
        private readonly Extentions _extentions;
        string _connectionString = "";
        public SoundRepo(IConfiguration configuration, Extentions extentions)
        {
            _extentions = extentions;
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
                    if (lst.Count > 0)
                    {
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
                    else
                    {
                        return new ResponseData<Pagination<SoundDto>>
                        {
                            IsSuccess = false,
                            Data = new Pagination<SoundDto>
                            {
                                TotalPage = totalPage,
                                CurrentPage = PageNumber,
                                Data = new List<SoundDto>(),
                            }
                        };
                    }
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
        public async Task<ResponseData<Pagination<AdminSoundDto>>> GetSoundByAdmin(int PageSize = 10, int PageNumber = 1)
        {
            PageNumber = PageNumber < 0 ? PageNumber = 1 : PageNumber;
            List<AdminSoundDto> lst = new List<AdminSoundDto>();
            int totalPage = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("GetSoundDataByAdmin", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@pageSize", System.Data.SqlDbType.Int).Value = PageSize;
                        cmd.Parameters.Add("@pageNumber", System.Data.SqlDbType.Int).Value = PageNumber;
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var temp = new AdminSoundDto
                                {
                                    Name = reader["Name"].ToString(),
                                    Image = reader["Image"].ToString(),
                                    Id = (long)reader["Id"],
                                    FileName = reader["FileName"].ToString(),
                                    Content = Convert.ToBase64String(reader["Content"] as byte[]),
                                    ContentType = reader["ContentType"].ToString(),
                                    NameUserAdd = reader["DisplayName"].ToString(),
                                    DateCreate = (DateTime)reader["CreateDate"],
                                    DateUpdate = (DateTime)reader["UpdateDate"],
                                    IsDeleted = (bool)reader["IsDeleted"],
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
                            if (lst.Count > 0)
                            {
                                return new ResponseData<Pagination<AdminSoundDto>>
                                {
                                    IsSuccess = true,
                                    Data = new Pagination<AdminSoundDto>
                                    {
                                        TotalPage = totalPage,
                                        CurrentPage = PageNumber,
                                        Data = lst,
                                    }
                                };
                            }
                            else
                            {
                                return new ResponseData<Pagination<AdminSoundDto>>
                                {
                                    IsSuccess = false,
                                    Data = new Pagination<AdminSoundDto>
                                    {
                                        TotalPage = totalPage,
                                        CurrentPage = PageNumber,
                                        Data = new List<AdminSoundDto>(),
                                    }
                                };
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi SQL: " + ex.Message);
                    return new ResponseData<Pagination<AdminSoundDto>>
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
                        cmd.Parameters.Add("@Image", System.Data.SqlDbType.VarChar, -1).Value = sound.Image;
                        cmd.Parameters.Add("@IdUserAdd", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.Parse(_extentions.GetIdForToken(sound.Token));

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
                        cmd.Parameters.Add("@Image", System.Data.SqlDbType.VarChar, -1).Value = sound.Image;
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
        public async Task<ResponseData<string>> ActiveSound(long id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("ActiveSound", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdSound", System.Data.SqlDbType.BigInt).Value = id;

                        var response = await cmd.ExecuteNonQueryAsync();
                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Message = "Active sound thành công!",
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
                            while (await reader.ReadAsync())
                            {
                                var temp = new GetMixSoundDto
                                {
                                    SoundName = reader["Name"].ToString(),
                                    Image = reader["Image"].ToString(),
                                    FileName = reader["FileName"].ToString(),
                                    Content = reader["Content"] as byte[],
                                    ContentType = reader["ContentType"].ToString(),
                                    IdMix = idMix,
                                    IdSound = (long)reader["IdSound"]
                                };
                                lst.Add(temp);
                            }
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
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("CreateMix", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 50).Value = mix.Name;

                        DataTable idSound = new DataTable();
                        idSound.Columns.Add("IdSound", typeof(long));
                        foreach (var item in mix.IdSounds)
                        {
                            idSound.Rows.Add(item);
                        }
                        var param = cmd.Parameters.AddWithValue("@Ids", idSound);
                        param.SqlDbType = SqlDbType.Structured;
                        param.TypeName = "IdListType";
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new ResponseData<string>
                                {
                                    IsSuccess = true,
                                    Message = "Thêm danh sách nhạc mix thành công!",
                                    Data = reader["IdMix"].ToString()
                                };
                            }
                            else
                            {
                                return new ResponseData<string>
                                {
                                    IsSuccess = false,
                                    Message = "Thêm danh sách nhạc mix thất bại!",
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
                        IsSuccess = true,
                        Message = "Lỗi : " + ex.Message,
                    };
                }
            }
        }
        public async Task<ResponseData<string>> SaveMix(UpdateMixSoundDto update)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("UpdateMix", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdMix", System.Data.SqlDbType.BigInt).Value = update.IdMix;

                        DataTable idSound = new DataTable();
                        idSound.Columns.Add("IdSound", typeof(long));
                        foreach (var item in update.IdSounds)
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
                        Message = "Thay đổi danh sách nhạc mix thành công!"
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
    }
}