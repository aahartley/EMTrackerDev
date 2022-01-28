using System.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMTrackerDev.Data;
using EMTrackerDev.Models;
//using System.Data.Entity.Infrastructure;

namespace EMTrackerDev.Controllers
{
    public class SamplesController : Controller
    {
        private readonly EMTrackerDevContext _context;

        public void populateStatusDropList(object selectedStatus =null)
        {
            var statusQuery = from q in _context.Status
                                   orderby q.StatusId
                                   select q;
            ViewBag.StatusId = new SelectList(statusQuery, "StatusId", "StatusName", selectedStatus);
        }

        public SamplesController(EMTrackerDevContext context)
        {
            _context = context;
        }

        // GET: Samples
        public async Task<IActionResult> Index()
        {
            return View(await _context.Samples.Include(s=>s.Result).Include(s=>s.Status).ToListAsync());
        }

        // GET: Samples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sample = await _context.Samples
                .FirstOrDefaultAsync(m => m.SampleID == id);
            if (sample == null)
            {
                return NotFound();
            }

            return View(sample);
        }
        // GET: Samples/Retrieve/5
        public async Task<IActionResult> Retrieve(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sample = await _context.Samples.Include(sample => sample.Result)
                .FirstOrDefaultAsync(m => m.SampleID == id);
            if (sample == null)
            {
                return NotFound();
            }

            return View(sample);
        }

        // GET: Samples/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Samples/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SampleID,SampleName,Amount,UOM,Notes,SampleDate,StatusId")] Sample sample)
        {
            if (ModelState.IsValid)
            {
                sample.StatusId = 1;
                _context.Add(sample);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sample);
        }

        // GET: Samples/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sample = await _context.Samples.FindAsync(id);
            if (sample == null)
            {
                return NotFound();
            }
            populateStatusDropList(sample.StatusId);

            return View(sample);
        }

        // POST: Samples/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SampleID,SampleName,Amount,UOM,Notes,SampleDate,StatusId")] Sample sample)
        {
            if (id != sample.SampleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sample);
                    await _context.SaveChangesAsync();
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
                {
                    if (!SampleExists(sample.SampleID))
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
            populateStatusDropList(sample.StatusId); 
            return View(sample);
        }

        // GET: Samples/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sample = await _context.Samples.Include(s=>s.Result).Include(s => s.Test)
                .FirstOrDefaultAsync(m => m.SampleID == id);
            if (sample == null)
            {
                return NotFound();
            }

            return View(sample);
        }

        // POST: Samples/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sample = await _context.Samples.Include(s => s.Result).Include(s=>s.Test)
                          .FirstOrDefaultAsync(m => m.SampleID == id);
            _context.Samples.Remove(sample);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SampleExists(int id)
        {
            return _context.Samples.Any(e => e.SampleID == id);
        }
    }
}
