using Microsoft.AspNetCore.Mvc.Rendering;
using Rando.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.ViewModels
{
    public class RamblesListDifficultyViewModel
    {
        public List<Ramble> Rambles { get; set; }
        public SelectList Difficulties { get; set; }
        public Int32 RambleDifficulty { get; set; }
        public SelectList Regions { get; set; }
        public String RambleRegion { get; set; }        
        public Int32 RambleDuration { get; set; }
        public String SearchString { get; set; }
        
    }
}
