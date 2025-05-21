namespace Data.Dto.User
{
    public class UpdateRoleUserDto
    {
        public Guid IdUser { get; set; }
        public List<Guid> ListIdRole { get; set; }
    }
}