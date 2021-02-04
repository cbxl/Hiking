using Rando.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.ViewModels
{
    public class RamblesListPageViewModel
    {
        public IEnumerable<Ramble> Rambles { get; set; }
        
    }
}
