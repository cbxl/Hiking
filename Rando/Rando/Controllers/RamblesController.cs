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
        private readonly UserManager<User> _userManager;

        public RamblesController(ApplicationDbContext context, OpinionRepository opinionRepository, UserManager<User> userManager)
        {
            _context = context;
            _opinionRepository = opinionRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Rambles.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            var model = new RamblePageViewModel();            
            model.Ramble = await _context.Rambles.FirstOrDefaultAsync(r => r.Id == id);
            model.Opinions = _opinionRepository.FindAllByRamble(model.Ramble);

            return View(model);
        }

        public IActionResult Opinions(Ramble ramble)
        {
            IQueryable<Opinion> opinions = _opinionRepository.FindAllByRamble(ramble);
            return View(opinions);
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
            return View("Index", userRambles);
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
            return View("OpinionAccepted");
        }
    }
}
    
           
