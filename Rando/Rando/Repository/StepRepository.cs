using Rando.Data;
using Rando.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rando.Repository
{
    public class StepRepository
    {
        private readonly ApplicationDbContext _context;

        public StepRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Step> FindAllByRamble(Ramble ramble)
        {
            IQueryable<Step> steps = _context.Steps.Where(o => o.Ramble.Id == ramble.Id);
            IQueryable<Step> allSteps = steps.Select(i => new Step
            {
                Latitude = i.Latitude,
                Longitude = i.Longitude,
                Name = i.Name
            });
            
            return allSteps;
        }

        public Double OpinionAverage(IEnumerable<Opinion> opinions)
        {
            Int32 opinionSum = 0;
            Double opinionAverage = 0;
            foreach (Opinion o in opinions)
            {
                opinionSum = opinionSum + o.Score;
            }
            if (opinionSum != 0)
            {
                opinionAverage = opinionSum / opinions.Count();
                return opinionAverage;
            }
            else
            {
                return 0;
            }
        }

         

    }
}