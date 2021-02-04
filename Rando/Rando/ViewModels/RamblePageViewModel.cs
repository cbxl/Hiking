using Rando.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.ViewModels
{
    public class RamblePageViewModel
    {
        public IEnumerable<Opinion> Opinions { get; set; }
        public Opinion PostOpinion { get; set; }
        public Ramble Ramble { get; set; }
        public User User { get; set; }
    }
}
