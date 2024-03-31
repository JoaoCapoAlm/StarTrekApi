using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class CrewRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CrewRoleId { get; set; }
        public int CrewId { get; set; }
        public virtual Crew Crew { get; set; }
        public byte RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
