using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMTrackerDev.Data;
using EMTrackerDev.Models;

namespace EMTrackerDev.Controllers
{
    public class SamplesController : Controller
    {
        private readonly EMTrackerDevContext _context;

        public SamplesController(EMTrackerDevContext context)
        {
            _context = context;
        }

        // GET: Samples
        public async Task<IActionResult> Index()
        {
            var eMTrackerDevContext = _context.Samples.Include(t=>t.Test).Include(s => s.AnalysisResults).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status);
            return View(await eMTrackerDevContext.ToListAsync());
        }

        // GET: Samples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sample = await _context.Samples
                .Include(s => s.AnalysisResults)
                .Include(s => s.ApprovedBy)
                .Include(s => s.CollectedBy)
                .Include(s => s.Status)
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
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId");
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            return View();
        }

        // POST: Samples/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SampleID,StatusId,CollectedById,ApprovedById,LocatedCodeId,AnalysisResultId,CollectedDate,ApprovedDate,amount,latitude,longitude")] Sample sample)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sample);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId", sample.AnalysisResultId);
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.StatusId);
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
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId", sample.AnalysisResultId);
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.StatusId);
            return View(sample);
        }

        // POST: Samples/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SampleID,StatusId,CollectedById,ApprovedById,LocatedCodeId,AnalysisResultId,CollectedDate,ApprovedDate,amount,latitude,longitude")] Sample sample)
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
                catch (DbUpdateConcurrencyException)
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
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId", sample.AnalysisResultId);
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.StatusId);
            return View(sample);
        }

        // GET: Samples/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sample = await _context.Samples
                .Include(s => s.AnalysisResults)
                .Include(s => s.ApprovedBy)
                .Include(s => s.CollectedBy)
                .Include(s => s.Status)
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
            var sample = await _context.Samples.FindAsync(id);
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
