using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeViceri.Domain.Entities
{
    public class Hero : EntityBase
    {
        [Column("Nome", TypeName = "character varying(120)")]
        public required string Name { get; set; }

        [Column("NomeHeroi", TypeName = "character varying(120)")]
        public required string HeroName { get; set; }

        [Column("DataNascimento", TypeName = "timestamp without time zone")]
        public DateTime? BirthDate { get; set; }

        [Column("Altura", TypeName = "double precision")]
        public double Height { get; set; }

        [Column("Peso", TypeName = "double precision")]
        public double Weight { get; set; }

        public ICollection<HeroSuperpower> HeroSuperpowers { get; set; } = new List<HeroSuperpower>();
    }
}

