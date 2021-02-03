using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.Models
{
    public class Opinion
    {
        public Guid Id { get; set; }
        public Int32 Score { get; set; }
        public String Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
        public Ramble Ramble { get; set; }
    }
}
