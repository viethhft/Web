namespace Data.Dto.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsDeleted { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }
    public class LoginDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
    public class LoginDataDto
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
        public int TotalRow { get; set; }

    }
}