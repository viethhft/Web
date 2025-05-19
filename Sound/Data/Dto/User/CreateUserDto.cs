namespace Data.Dto.User
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool Gender { get; set; }
        public string Token { get; set; }
    }
    public class CreateAdminDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Password { get; set; }
    }
}