using Rando.Data;
using Rando.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.Repository
{
    public class RambleRepository
    {
        private readonly ApplicationDbContext _context;

        public RambleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Ramble> FindAllRamblesByDifficultyOne()
        {
            IQueryable<Ramble> rambles = _context.Rambles.Where(r => r.Difficulty == 1);
            return rambles;
        }

    }
}