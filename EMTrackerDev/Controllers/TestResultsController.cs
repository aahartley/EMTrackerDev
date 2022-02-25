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
    public class TestResultsController : Controller
    {
        private readonly EMTrackerDevContext _context;

        public TestResultsController(EMTrackerDevContext context)
        {
            _context = context;
        }

        // GET: TestResults
        public async Task<IActionResult> Index()
        {
            var eMTrackerDevContext = _context.TestResults.Include(t => t.AnalysisResult).Include(t => t.EnteredBy).Include(t => t.Test);
            return View(await eMTrackerDevContext.ToListAsync());
        }

        // GET: TestResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testResult = await _context.TestResults
                .Include(t => t.AnalysisResult)
                .Include(t => t.EnteredBy)
                .Include(t => t.Test)
                .FirstOrDefaultAsync(m => m.TestResultId == id);
            if (testResult == null)
            {
                return NotFound();
            }

            return View(testResult);
        }

        // GET: TestResults/Create
        public IActionResult Create()
        {
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId");
            ViewData["EnteredById"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["TestId"] = new SelectList(_context.Tests, "TestID", "TestID");
            return View();
        }

        // POST: TestResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestResultId,AnalysisResultId,EnteredById,StartDate,EndDate,TestId")] TestResult testResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId", testResult.AnalysisResultId);
            ViewData["EnteredById"] = new SelectList(_context.Users, "UserId", "UserId", testResult.EnteredById);
            ViewData["TestId"] = new SelectList(_context.Tests, "TestID", "TestID", testResult.TestId);
            return View(testResult);
        }

        // GET: TestResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testResult = await _context.TestResults.FindAsync(id);
            if (testResult == null)
            {
                return NotFound();
            }
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId", testResult.AnalysisResultId);
            ViewData["EnteredById"] = new SelectList(_context.Users, "UserId", "UserId", testResult.EnteredById);
            ViewData["TestId"] = new SelectList(_context.Tests, "TestID", "TestID", testResult.TestId);
            return View(testResult);
        }

        // POST: TestResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestResultId,AnalysisResultId,EnteredById,StartDate,EndDate,TestId")] TestResult testResult)
        {
            if (id != testResult.TestResultId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestResultExists(testResult.TestResultId))
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
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId", testResult.AnalysisResultId);
            ViewData["EnteredById"] = new SelectList(_context.Users, "UserId", "UserId", testResult.EnteredById);
            ViewData["TestId"] = new SelectList(_context.Tests, "TestID", "TestID", testResult.TestId);
            return View(testResult);
        }

        // GET: TestResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testResult = await _context.TestResults
                .Include(t => t.AnalysisResult)
                .Include(t => t.EnteredBy)
                .Include(t => t.Test)
                .FirstOrDefaultAsync(m => m.TestResultId == id);
            if (testResult == null)
            {
                return NotFound();
            }

            return View(testResult);
        }

        // POST: TestResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testResult = await _context.TestResults.FindAsync(id);
            _context.TestResults.Remove(testResult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestResultExists(int id)
        {
            return _context.TestResults.Any(e => e.TestResultId == id);
        }
    }
}
