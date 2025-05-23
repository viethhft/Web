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
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Sound.Application.Extentions
{
    public class Extentions
    {
        private readonly string ffmpegPath = "/Users/macminia2/Web/Sound/Application/Extentions/AppZip/ffmpeg";
        public Extentions()
        {
        }

        public async Task<byte[]> CompressMp3Async(IFormFile inputFile, int bitrateKbps = 96)
        {
            using var ffmpeg = new Process();

            ffmpeg.StartInfo.FileName = ffmpegPath;
            // Đọc input từ stdin (-i pipe:0), xuất ra stdout (-f mp3 pipe:1)
            ffmpeg.StartInfo.Arguments = $"-i pipe:0 -b:a {bitrateKbps}k -f mp3 pipe:1";

            ffmpeg.StartInfo.UseShellExecute = false;
            ffmpeg.StartInfo.RedirectStandardInput = true;
            ffmpeg.StartInfo.RedirectStandardOutput = true;
            ffmpeg.StartInfo.RedirectStandardError = true;  // Có thể đọc lỗi debug

            ffmpeg.StartInfo.CreateNoWindow = true;

            ffmpeg.Start();

            // Task ghi dữ liệu file upload vào stdin của ffmpeg
            var copyInputTask = inputFile.CopyToAsync(ffmpeg.StandardInput.BaseStream)
                .ContinueWith(t => ffmpeg.StandardInput.Close());

            // Đọc toàn bộ output nén từ stdout của ffmpeg
            using var msOutput = new MemoryStream();
            var copyOutputTask = ffmpeg.StandardOutput.BaseStream.CopyToAsync(msOutput);

            // (Optional) Đọc log lỗi FFmpeg (nếu cần debug)
            var errorLog = await ffmpeg.StandardError.ReadToEndAsync();

            await Task.WhenAll(copyInputTask, copyOutputTask);

            ffmpeg.WaitForExit();

            if (ffmpeg.ExitCode != 0)
            {
                throw new Exception($"FFmpeg exited with code {ffmpeg.ExitCode}. Error: {errorLog}");
            }
            return msOutput.ToArray();
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
                new Claim("IsConfirm", data.IsConfirm.ToString()),
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
        public string AddValidateForCodeConfirm(string code)
        {
            DateTime now = DateTime.Now;
            now = now.AddMinutes(30);
            code += "." + now.ToString("dd/MM/yyyy HH:mm:ss");
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
        public async Task<string> SendEmailAccount(InfoUserDto infoUserDto)
        {
            try
            {
                MailAddress mailFrom = new MailAddress("shoppet79@gmail.com", "SoundSleep");
                MailAddress mailTo = new MailAddress(infoUserDto.Email);
                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = "Tài khoản đăng nhập của bạn";
                message.Body = $"<body style=\"font-family: Arial, sans-serif;\">\r\n\r\n    <h2>Tài khoản đăng nhập cá nhân vui lòng không để lộ!!</h2>\r\n\r\n    <p>Cuộc sống vốn có rất nhiều lựa chọn, cảm ơn bạn vì đã chọn làm việc với chúng tôi.<br> Chúc bạn đồng hành vui vẻ cùng MewShop.\n Dưới đây là thông tin tài khoản của bạn:</p>\r\n\r\n Tài khoản:  <p style=\"font-size: 20px; font-weight: bold; color: #007BFF;\">[{infoUserDto.Name}]</p>\r\n\r\n Mật khẩu: <p style=\"font-size: 20px; font-weight: bold; color: #007BFF;\">[{infoUserDto.Password}]</p>\r\n\r\n  Code verify lần đầu đăng nhập: <p style=\"font-size: 20px; font-weight: bold; color: #007BFF;\">[{infoUserDto.Code}]</p>\r\n\r\n   <b>Vui lòng không chia sẻ thông tin này với người khác.\n Sau khi đăng nhập thành công để bảo mật thông tin và tài khoản cá nhân của bạn, vui lòng thay đổi mật khẩu cho tài khoản của bạn.</b>\r\n    <p>Nếu bạn có bất kỳ thắc mắc hãy liên hệ qua: <b>shoppet79@gmail.com</b>, <br>\r\n    Hoặc liên hệ qua fanpage của chúng tôi [Địa chỉ fanpage]</p>\r\n    <p>Trân trọng,<br>\r\n    MewShop</p>\r\n</body>";
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("shoppet79@gmail.com", "tznx twfq hclm ysok");
                await client.SendMailAsync(message);
                return $"Mã xác thực đã gửi tới email của bạn!!";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public async Task<string> SendEmailForgotPassword(string email, string code)
        {
            try
            {
                MailAddress mailFrom = new MailAddress("shoppet79@gmail.com", "SoundSleep");
                MailAddress mailTo = new MailAddress(email);
                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = "Mã xác nhận quên mật khẩu";
                message.Body = $@"<body style='font-family: Arial, sans-serif;'>

                                <h2>Mã xác nhận quên mật khẩu - Vui lòng không chia sẻ!</h2>

                                <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu từ bạn.<br>
                                Nếu bạn không yêu cầu điều này, vui lòng bỏ qua email này hoặc liên hệ với chúng tôi.<br>
                                Dưới đây là mã xác nhận để bạn tiếp tục quá trình khôi phục mật khẩu:</p>

                                Mã xác nhận:
                                <p style='font-size: 20px; font-weight: bold; color: #007BFF;'>
                                    {code}
                                </p>

                                <b>Vui lòng không chia sẻ mã xác nhận này với bất kỳ ai.<br>
                                Mã có hiệu lực trong thời gian giới hạn. Sau khi xác nhận thành công, bạn có thể đặt lại mật khẩu mới.</b>

                                <p>Nếu bạn cần hỗ trợ thêm, hãy liên hệ qua: <b>shoppet79@gmail.com</b>,<br>
                                Hoặc nhắn tin trực tiếp cho fanpage của chúng tôi: [Địa chỉ fanpage]</p>

                                <p>Trân trọng,<br>
                                MewShop</p>

                            </body>";
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("shoppet79@gmail.com", "tznx twfq hclm ysok");
                await client.SendMailAsync(message);
                return $"Mã xác thực đã gửi tới email của bạn!!";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public bool IsValidCodeConfirm(string code)
        {
            try
            {
                string[] values = code.Split('.');
                DateTime dateTime = DateTime.ParseExact(values[1], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                if (DateTime.Now > dateTime.AddMinutes(30))
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}