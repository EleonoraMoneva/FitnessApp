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

    public class DietPlansController : Controller
    {
        private readonly FitnessAppContext _context;

        public DietPlansController(FitnessAppContext context)
        {
            _context = context;
        }

        // GET: DietPlans
        public async Task<IActionResult> Index()
        {
            return View(await _context.DietPlan.ToListAsync());
        }

        // GET: DietPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dietPlan = await _context.DietPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dietPlan == null)
            {
                return NotFound();
            }

            return View(dietPlan);
        }

        // GET: DietPlans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DietPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Breakfast,Lunch,Dinner,Snacks")] DietPlan dietPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dietPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dietPlan);
        }

        // GET: DietPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dietPlan = await _context.DietPlan.FindAsync(id);
            if (dietPlan == null)
            {
                return NotFound();
            }
            return View(dietPlan);
        }

        // POST: DietPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Breakfast,Lunch,Dinner,Snacks")] DietPlan dietPlan)
        {
            if (id != dietPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dietPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DietPlanExists(dietPlan.Id))
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
            return View(dietPlan);
        }

        // GET: DietPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dietPlan = await _context.DietPlan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dietPlan == null)
            {
                return NotFound();
            }

            return View(dietPlan);
        }

        // POST: DietPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dietPlan = await _context.DietPlan.FindAsync(id);
            _context.DietPlan.Remove(dietPlan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DietPlanExists(int id)
        {
            return _context.DietPlan.Any(e => e.Id == id);
        }
    }
}
