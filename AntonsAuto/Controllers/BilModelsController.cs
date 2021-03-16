using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AntonsAuto.Data;
using AntonsAuto.Models;
using Microsoft.AspNetCore.Authorization;

namespace AntonsAuto.Controllers
{
    [Authorize]
    public class BilModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BilModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BilModels
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            
            ViewData["CurrentFilter"] = searchString;

            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nameDesc" : "";
            ViewData["FabrikantSortParam"] = sortOrder == "fabrik" ? "fabrikDesc" : "fabrik";
            ViewData["KoertKmSortParam"] = sortOrder == "koertKm" ? "koertKmDesc" : "koertKm";

            var bilModels = from s in _context.BilModels.Include(b => b.Bilfabrikant) select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                bilModels = bilModels.Where(
                    s => s.Navn.Contains(searchString) || 
                    s.Bilfabrikant.Navn.Contains(searchString) ||
                    s.AntalKilometer.ToString().Contains(searchString)

                    );
            }

            switch (sortOrder)
            {
                case "nameDesc":
                    bilModels = bilModels.OrderByDescending(s => s.Navn);
                    break;

                case "fabrik":
                    bilModels = bilModels.OrderBy(s => s.Bilfabrikant);
                    break;

                case "fabrikDesc":
                    bilModels = bilModels.OrderByDescending(s => s.Bilfabrikant);
                    break;

                case "koertKm":
                    bilModels = bilModels.OrderBy(s => s.AntalKilometer);
                    break;

                case "koertKmDesc":
                    bilModels = bilModels.OrderByDescending(s => s.AntalKilometer);
                    break;

                default:
                    bilModels = bilModels.OrderBy(s => s.Navn);
                    break;
            }


            return View(await bilModels.AsNoTracking().ToListAsync());
        }

        // GET: BilModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilModel = await _context.BilModels
                .Include(b => b.Bilfabrikant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bilModel == null)
            {
                return NotFound();
            }

            return View(bilModel);
        }

        // GET: BilModels/Create
        public IActionResult Create()
        {
            ViewData["BilfabrikantID"] = new SelectList(_context.Bilfabritants, "ID", "Navn");
            return View();
        }

        // POST: BilModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Navn,AntalKilometer,BilfabrikantID")] BilModel bilModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bilModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BilfabrikantID"] = new SelectList(_context.Bilfabritants, "ID", "ID", bilModel.BilfabrikantID);
            return View(bilModel);
        }

        // GET: BilModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilModel = await _context.BilModels.FindAsync(id);
            if (bilModel == null)
            {
                return NotFound();
            }
            ViewData["BilfabrikantID"] = new SelectList(_context.Bilfabritants, "ID", "Navn", bilModel.BilfabrikantID);
            return View(bilModel);
        }

        // POST: BilModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Navn,AntalKilometer,BilfabrikantID")] BilModel bilModel)
        {
            if (id != bilModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bilModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BilModelExists(bilModel.ID))
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
            ViewData["BilfabrikantID"] = new SelectList(_context.Bilfabritants, "ID", "ID", bilModel.BilfabrikantID);
            return View(bilModel);
        }

        // GET: BilModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilModel = await _context.BilModels
                .Include(b => b.Bilfabrikant)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bilModel == null)
            {
                return NotFound();
            }

            return View(bilModel);
        }

        // POST: BilModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bilModel = await _context.BilModels.FindAsync(id);
            _context.BilModels.Remove(bilModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BilModelExists(int id)
        {
            return _context.BilModels.Any(e => e.ID == id);
        }
    }
}
