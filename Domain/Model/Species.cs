﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Species
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short SpeciesId { get; set; }
        public string ResourceName { get; set; }
        public short PlanetId { get; set; }
        public virtual Place Planet { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
    }
}
