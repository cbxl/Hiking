using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.Models
{
    public class User : IdentityUser
    {
        public String Pseudo { get; set; }
        public String Description { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String ProfilImageUrl { get; set; }
        public ICollection<Ramble> Rambles { get; set; }
    }
}
