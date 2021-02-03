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

namespace Rando.Controllers
{
    public class RamblesController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly OpinionRepository _opinionRepository;
        private readonly UserManager<User> _userManager;

        public RamblesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            //_opinionRepository = opinionRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Rambles.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            Ramble ramble = await _context.Rambles.FirstOrDefaultAsync(r => r.Id == id);            
            if (ramble == null)
            {
                return NotFound();
            }
            return View(ramble);
        }

        //public IActionResult Opinions(Ramble ramble)
        //{
        //    IQueryable<Opinion> opinions = _opinionRepository.FindAllByRamble(ramble);
        //    return View(opinions);
        //}

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
        [HttpGet]
        public IActionResult AddOpinion(Ramble givenRamble)
        {
            Ramble ramble = _context.Rambles.SingleOrDefault(r => r.Id == givenRamble.Id);
            return View("AddOpinion", ramble);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostOpinion(Opinion opinion, Ramble ramble)
        {
            User connected = await _userManager.GetUserAsync(this.User);
            Ramble actualRamble = _context.Rambles.SingleOrDefault(r => r.Id == ramble.Id);
            opinion.User = connected;
            opinion.Ramble = actualRamble;
            _context.Add(opinion);
            _context.SaveChanges();
            return View("AddOpinion");
        }
    }
}
