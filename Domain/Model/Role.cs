namespace Domain.Model
{
    public class Role
    {
        public byte RoleId { get; set; }
        public string RoleResource { get; set; }
        public virtual ICollection<CrewRole> CrewRoles { get; set; }
    }
}
