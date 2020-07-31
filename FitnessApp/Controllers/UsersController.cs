
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Models;
using FitnessApp.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FitnessApp.Controllers
{
    [Authorize(Roles = "User")]

    public class UsersController : Controller
    {
        private readonly FitnessAppContext _context;
        private readonly FitnessAppContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppUser> userManager;

        public UsersController(FitnessAppContext context, IWebHostEnvironment hostEnvironment, UserManager<AppUser> userMgr)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            dbContext = context;
            userManager = userMgr;
        }
        /*
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        */

        // GET: Users/Create
        [AllowAnonymous]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FName,LName,Birthdate,MainGoal,PhoneNumber,Password,Email,Career,Height,Weight,Adress")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        /*

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FName,LName,Birthdate,MainGoal,PhoneNumber,Email,Career,Height,Weight,Adress")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        public async Task<IActionResult> UserHomePage(int? id)
        {
            if (id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if (curruser.UserId != null)
                    return RedirectToAction(nameof(User), new { id = curruser.UserId });
                else
                    return NotFound();
            }

            var user = await _context.User.Include(e => e.Enrollments).ThenInclude(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            AppUser user1 = await userManager.GetUserAsync(User);
            if (id != user1.UserId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }
            return View(user1);
        }
        [AllowAnonymous]

        public async Task<IActionResult> UserSeeProfile(int? id)
        {
            if (id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if (curruser.UserId != null)
                    return RedirectToAction(nameof(User), new { id = curruser.UserId });
                else
                    return NotFound();
            }

            var user = await _context.User.Include(e => e.Enrollments).ThenInclude(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            AppUser user1 = await userManager.GetUserAsync(User);
            if (id != user1.UserId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }
            return View(user1);
        }



        public async Task<IActionResult> EditProfile(int? id)
        {
            if (id == null)
            {
                AppUser curruser = await userManager.GetUserAsync(User);
                if (curruser.UserId != null)
                    return RedirectToAction(nameof(User), new { id = curruser.UserId });
                else
                    return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            AppUser user1 = await userManager.GetUserAsync(User);
            if (id != user1.UserId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }
            return View(user1);
        }


        public async Task<IActionResult> EditProfile(int id, [Bind("Id,FName,LName,Birthdate,MainGoal,Password,PhoneNumber,Email,Career,Height,Weight,Adress")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("");
            }
            AppUser user1 = await userManager.GetUserAsync(User);
            if (id != user1.UserId)
            {
                return RedirectToAction("AccessDenied", "Account", null);
            }
            return View(user1);
        }
    }
}
