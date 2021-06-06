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

        public RamblesController(ApplicationDbContext context, OpinionRepository opinionRepository, RambleRepository rambleRepository, StepRepository stepRepository, UserManager<User> userManager)
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
            Ramble existingRamble = _context.Rambles.SingleOrDefault(ramble => ramble.Id == givenRamble.Id);
            if (existingRamble != null)
            {
                if (existingRamble.User.Id == connected.Id)
                {
                    _context.Update(givenRamble);
                }
                else
                {
                    return Unauthorized("Vous n'avez pas les droits pour modifier cette rando");
                }
            }
            else
            {
                givenRamble.User = connected;
                _context.Add(givenRamble);
            }

            _context.SaveChanges();
            IEnumerable<Ramble> userRambles = _context.FindRamblesByUser(connected);
            return View("Remerciements");
        }


        [Authorize]
        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> PutRamble(Guid id, [Bind("Id,Title,Description,HikingPhotoUrl,Region,City,DepartLatitude,DepartLongitude,Distance,HeightDifferencePositive,HeightDifferenceNegative,Duration,Difficulty")] Ramble ramble)
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
                return RedirectToAction(nameof(Index));
            }
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
    
           
