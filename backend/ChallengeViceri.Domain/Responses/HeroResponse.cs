using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeViceri.Domain.Responses
{
    public class HeroResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string HeroName { get; set; }
        public DateTime? BirthDate { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<SuperpowerResponse> Superpowers { get; set; } = new();
    }
}

