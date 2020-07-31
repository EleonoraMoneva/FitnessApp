using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Models;
using Microsoft.AspNetCore.Hosting;
using FitnessApp.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace FitnessApp.Controllers
{
    [Authorize(Roles = "Trainer, Admin")]

    public class ExrecisesController : Controller
    {
        private readonly FitnessAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ExrecisesController(FitnessAppContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;


        }

        // GET: Exrecises
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exrecise.ToListAsync());
        }

        // GET: Exrecises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exrecise = await _context.Exrecise
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exrecise == null)
            {
                return NotFound();
            }

            return View(exrecise);
        }

        // GET: Exrecises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exrecises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExreciseViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Exrecise exrecise = new Exrecise
                {
                    EName = model.EName,
                    EImage = uniqueFileName,
                };
                _context.Add(exrecise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public string UploadedFile(ExreciseViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfileImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Exrecises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exrecise = await _context.Exrecise.FindAsync(id);
            if (exrecise == null)
            {
                return NotFound();
            }
            return View(exrecise);
        }

        // POST: Exrecises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EName,EImage")] Exrecise exrecise)
        {
            if (id != exrecise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exrecise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExreciseExists(exrecise.Id))
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
            return View(exrecise);
        }

        // GET: Exrecises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exrecise = await _context.Exrecise
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exrecise == null)
            {
                return NotFound();
            }

            return View(exrecise);
        }

        // POST: Exrecises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exrecise = await _context.Exrecise.FindAsync(id);
            _context.Exrecise.Remove(exrecise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExreciseExists(int id)
        {
            return _context.Exrecise.Any(e => e.Id == id);
        }
    }
}
