using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.Models
{
    public class Ramble
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String HikingPhotoUrl { get; set; }
        public String Region { get; set; }
        public String City { get; set; }
        public Double DepartLatitude { get; set; }
        public Double DepartLongitude { get; set; }
        public Double Distance { get; set; }
        public Int32 HeightDifferencePositive { get; set; }
        public Int32 HeightDifferenceNegative { get; set; }
        public Double Duration { get; set; }
        public Int32 Difficulty { get; set; }
        public User User { get; set; }
        public ICollection<Opinion> Opinions { get; set; }
    }
}
