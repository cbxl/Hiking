using Rando.Data;
using Rando.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.Repository
{
    public class OpinionRepository
    {
        private readonly ApplicationDbContext _context;

        public OpinionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Opinion> FindAllByRamble(Ramble ramble)
        {
            IQueryable<Opinion> opinions = _context.Opinions.Where(o => o.Ramble.Id == ramble.Id);
            IQueryable<Opinion> allOpinions = opinions.Select(i => new Opinion
            {
                Score = i.Score,
                Comment = i.Comment,
                CreatedAt = DateTime.Now,
                User = i.User
            });

            return allOpinions;
        }

    }
}