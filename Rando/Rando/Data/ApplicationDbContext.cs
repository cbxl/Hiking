using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rando.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rando.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<Ramble> Rambles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public IQueryable<Ramble> FindRamblesByUser(User connected)
        {
            return Rambles.Where(r => r.User.Id == connected.Id);
        }
    }
}
