using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rando.Data;
using Rando.Models;
using Rando.Repository;
using Rando.ViewModels;

namespace Rando.Controllers
{
    public class RamblesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OpinionRepository _opinionRepository;
        private readonly RambleRepository _rambleRepository;
        private readonly StepRepository _stepRepository;
        private readonly UserManager<User> _userManager;

        public RamblesController(ApplicationDbContext context, 
                                OpinionRepository opinionRepository, 
                                RambleRepository rambleRepository, 
                                StepRepository stepRepository, 
                                UserManager<User> userManager)
        {
            _context = context;
            _opinionRepository = opinionRepository;
            _rambleRepository = rambleRepository;
            _stepRepository = stepRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(String searchString, Int32 rambleDifficulty, String rambleRegion)
        {
            IQueryable<Int32> difficultyQuery = from r in _context.Rambles
                                               orderby r.Difficulty
                                               select r.Difficulty;
            
            IQueryable<String> regionQuery = from r in _context.Rambles
                                                orderby r.Region
                                                select r.Region;            

            var rambles = from r in _context.Rambles
                          select r;
            if (!String.IsNullOrEmpty(searchString))
            {
                rambles = rambles.Where(search => search.Title.Contains(searchString));
            }
            if (rambleDifficulty > 0)
            {
                rambles = rambles.Where(diff => diff.Difficulty == rambleDifficulty);
            }            
            if (!string.IsNullOrEmpty(rambleRegion))
            {
                rambles = rambles.Where(r => r.Region == rambleRegion);
            }
            var rambleFilterView = new RamblesListDifficultyViewModel
            {
                Difficulties = new SelectList(await difficultyQuery.Distinct().ToListAsync()),
                Regions = new SelectList(await regionQuery.Distinct().ToListAsync()),
                Rambles = await rambles.ToListAsync()
            };
            return View(rambleFilterView);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var model = new RamblePageViewModel();            
            model.Ramble = await _context.Rambles.FirstOrDefaultAsync(r => r.Id == id);
            model.Opinions = _opinionRepository.FindAllByRamble(model.Ramble);
            model.Steps = _stepRepository.FindAllByRamble(model.Ramble);
            model.OpinionAverage = _opinionRepository.OpinionAverage(model.Opinions);
            model.OpinionsNumber = model.Opinions.Count();

            return View(model);
        }


        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRamble(Ramble givenRamble)
        {
            User connected = await _userManager.GetUserAsync(this.User);
            givenRamble.User = connected;
            _context.Add(givenRamble);
            _context.SaveChanges();
            return View("Remerciements");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ramble = await _context.Rambles.FindAsync(id);
            if (ramble == null)
            {
                return NotFound();
            }
            return View(ramble);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,HikingPhotoUrl,Region,City,DepartLatitude,DepartLongitude,Distance,HeightDifferencePositive,HeightDifferenceNegative,Duration,Difficulty")] Ramble ramble)
        {
            if (id != ramble.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ramble);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RambleExists(ramble.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("Remerciements");
            }
            return View(ramble);
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ramble = await _context.Rambles
                .FirstOrDefaultAsync(r => r.Id == id);
            if (ramble == null)
            {
                return NotFound();
            }

            return View(ramble);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Ramble ramble = await _context.Rambles.FindAsync(id);
            if (ramble == null)
            {
                return NotFound();
            }
            _context.Rambles.Remove(ramble);
            await _context.SaveChangesAsync();
            return View("Remerciements");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostOpinion([FromForm] Opinion opinion)
        {
            User connected = await _userManager.GetUserAsync(this.User);            
            opinion.User = connected;
            opinion.CreatedAt = DateTime.Now;
            _context.Update(opinion);       
            _context.SaveChanges();
            return View("Remerciements");
        }

        private bool RambleExists(Guid id)
        {
            return _context.Rambles.Any(e => e.Id == id);
        }

    }
}
    
           
