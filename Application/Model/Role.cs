namespace Application.Model
{
    public class Role
    {
        public byte RoleId { get; set; }
        public string RoleResource { get; set; }
        public ICollection<CrewRole> CrewRoles { get; set; }
    }
}
