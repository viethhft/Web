namespace Data.Dto.User
{
    public class ChangePasswordDto
    {
        public string Token { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}