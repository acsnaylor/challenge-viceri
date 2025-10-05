using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeViceri.Domain.Entities
{
    public abstract class EntityBase
    {
        [Column("Id")]
        public int Id { get; set; }
    }
}
