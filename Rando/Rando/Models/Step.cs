using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.Models
{
    public class Step
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public Ramble Ramble { get; set; }
    }
}
