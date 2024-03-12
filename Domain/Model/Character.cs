using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Model
{
    public class Character
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public DateOnly? DateBirth { get; set; }
        public DateOnly? DateDeath { get; set; }
        public short SpeciesId { get; set; }
        public virtual Species Species { get; set; }
    }
}
