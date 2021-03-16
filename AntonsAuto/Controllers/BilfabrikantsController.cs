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
    public class BilfabrikantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BilfabrikantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bilfabrikants
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nameDesc" : "";

            var farbrikanter = from s in _context.Bilfabritants select s;

            if(searchString != null )
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                farbrikanter = farbrikanter.Where(s => s.Navn.Contains(searchString));
            }


            switch(sortOrder)
            {
                case "nameDesc":
                    farbrikanter = farbrikanter.OrderByDescending(s => s.Navn);
                    break;

                default:
                    farbrikanter = farbrikanter.OrderBy(s => s.Navn);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Bilfabrikant>.CreateAsync(farbrikanter.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Bilfabrikants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
             var bilfabrikant = await _context.Bilfabritants
            .Include(s => s.BilModels)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

            //var bilfabrikant = await _context.Bilfabritants
            //    .FirstOrDefaultAsync(m => m.ID == id);
            if (bilfabrikant == null)
            {
                return NotFound();
            }

            return View(bilfabrikant);
        }

        // GET: Bilfabrikants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bilfabrikants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Navn")] Bilfabrikant bilfabrikant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bilfabrikant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bilfabrikant);
        }

        // GET: Bilfabrikants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilfabrikant = await _context.Bilfabritants.FindAsync(id);
            if (bilfabrikant == null)
            {
                return NotFound();
            }
            return View(bilfabrikant);
        }

        // POST: Bilfabrikants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Navn")] Bilfabrikant bilfabrikant)
        {
            if (id != bilfabrikant.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bilfabrikant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BilfabrikantExists(bilfabrikant.ID))
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
            return View(bilfabrikant);
        }

        // GET: Bilfabrikants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilfabrikant = await _context.Bilfabritants
                .FirstOrDefaultAsync(m => m.ID == id);
            if (bilfabrikant == null)
            {
                return NotFound();
            }

            return View(bilfabrikant);
        }

        // POST: Bilfabrikants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bilfabrikant = await _context.Bilfabritants.FindAsync(id);
            _context.Bilfabritants.Remove(bilfabrikant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BilfabrikantExists(int id)
        {
            return _context.Bilfabritants.Any(e => e.ID == id);
        }
    }
}
