﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Episode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EpisodeId { get; set; }
        public short SeasonId { get; set; }
        public virtual Season Season { get; set; }
        public DateOnly? RealeaseDate { get; set; }
        public string TitleResource { get; set; }
        public string SynopsisResource { get; set; }
        public byte? Time { get; set; }
        public byte Number { get; set; }
        public float? StardateFrom { get; set; }
        public float? StardateTo { get; set; }
        [MaxLength(11)]
        public string ImdbId { get; set; }
    }
}
