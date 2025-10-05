using ChallengeViceri.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeViceri.Domain.Requests
{
    public class HeroRequest
    {
        [System.ComponentModel.DataAnnotations.Required]
        public required string Name { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public required string HeroName { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public DateTime? BirthDate { get; set; }
        [System.ComponentModel.DataAnnotations.Range(0.01, double.MaxValue)]
        public double Height { get; set; }
        [System.ComponentModel.DataAnnotations.Range(0.01, double.MaxValue)]
        public double Weight { get; set; }
        [System.ComponentModel.DataAnnotations.MinLength(1)]
        public List<int> SuperpowerIds { get; set; } = new List<int>();
    }
}

