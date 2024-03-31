using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte RoleId { get; set; }
        public string RoleResource { get; set; }
        public virtual ICollection<CrewRole> CrewRoles { get; set; }
    }
}
