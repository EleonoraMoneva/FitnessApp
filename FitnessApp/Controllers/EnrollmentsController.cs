using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;


namespace FitnessApp.Controllers
{
    [Authorize(Roles = "Trainer, Admin")]

    public class EnrollmentsController : Controller
    {
        private readonly FitnessAppContext _context;

        public EnrollmentsController(FitnessAppContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var fitnessAppContext = _context.Enrollment.Include(e => e.DietPlan).Include(e => e.Trainer).Include(e => e.User);
            return View(await fitnessAppContext.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.DietPlan)
                .Include(e => e.Trainer)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["DietPlanId"] = new SelectList(_context.Set<DietPlan>(), "Id", "Id");
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "FirstName");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "FName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TrainerId,DietPlanId,StartDate,EndDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DietPlanId"] = new SelectList(_context.Set<DietPlan>(), "Id", "Id", enrollment.DietPlanId);
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "FirstName", enrollment.TrainerId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "FName", enrollment.UserId);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["DietPlanId"] = new SelectList(_context.Set<DietPlan>(), "Id", "Id", enrollment.DietPlanId);
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "FirstName", enrollment.TrainerId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "FName", enrollment.UserId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TrainerId,DietPlanId,StartDate,EndDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
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
            ViewData["DietPlanId"] = new SelectList(_context.Set<DietPlan>(), "Id", "Id", enrollment.DietPlanId);
            ViewData["TrainerId"] = new SelectList(_context.Trainer, "Id", "FirstName", enrollment.TrainerId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "FName", enrollment.UserId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.DietPlan)
                .Include(e => e.Trainer)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }
    }
}
