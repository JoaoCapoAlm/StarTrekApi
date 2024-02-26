namespace Domain.Model
{
    public class CrewRole
    {
        public int CrewRoleId { get; set; }
        public int CrewId { get; set; }
        public Crew Crew { get; set; }
        public byte RoleId { get; set; }
        public Role Role { get; set; }
    }
}
