using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeViceri.Domain.Entities
{
    public class Superpower : EntityBase
    {
        [Column("Superpoder", TypeName = "character varying(50)")]
        public required string Name { get; set; }

        [Column("Descricao", TypeName = "character varying(250)")]
        public string? Description { get; set; }

        public ICollection<HeroSuperpower> HeroSuperpowers { get; set; } = new List<HeroSuperpower>();
    }
}

