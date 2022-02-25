using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMTrackerDev.Data;
using EMTrackerDev.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace EMTrackerDev.Controllers
{
    public class SamplesController : Controller
    {
        private readonly EMTrackerDevContext _context;
        private Sample sam;

        public SamplesController(EMTrackerDevContext context)
        {
            _context = context;
        }

        // GET: Samples
        public async Task<IActionResult> Index()
        {
            var eMTrackerDevContext = _context.Samples.Where(s => s.StatusId == 1).Include(s=>s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status);
            return View(await eMTrackerDevContext.ToListAsync());
        }
        public async Task<IActionResult> Collected()
        {
            var eMTrackerDevContext = _context.Samples.Where(s=> s.StatusId == 2).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status);
            return View(await eMTrackerDevContext.ToListAsync());
        }
        public async Task<IActionResult> InProcess()
        {
            var eMTrackerDevContext = _context.Samples.Where(s => s.StatusId == 3).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status);
            return View(await eMTrackerDevContext.ToListAsync());
        }
        public async Task<IActionResult> Approved()
        {
            var eMTrackerDevContext = _context.TestResults.Where(s => s.EnteredById != null);
            List<int> Ids = eMTrackerDevContext.Select(o => o.TestId).ToList();
            var samples = _context.Samples.Where(c => Ids.Contains(c.SampleID)).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status);
         //   var eMTrackerDevContext2 = _context.Samples.Where(s => s.StatusId == 4).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status);
            return View(await samples.ToListAsync());
        }
        // GET: Samples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sample = await _context.Samples
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
            //ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId");
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["AnalysisId"] = new SelectList(_context.Analyses, "AnalysisId", "AnalysisId");
            return View();
        }

        // POST: Samples/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SampleID,AnalysisId,LocationCodeId")] Sample sample)
        {
            if (ModelState.IsValid)
            {
                int analysisId = (int)sample.AnalysisId;
          
                sample.StatusId = 1;
                _context.Add(sample);
                await _context.SaveChangesAsync();
                int sampleId = (int)sample.SampleID;
               // Console.WriteLine("WTFFFF " + sampleId + " " + analysisId);
                var ars = _context.AnalysisResults.Where(s => s.AnalysisId == analysisId).OrderBy(s=> s.AnalysisId);
                List<AnalysisResult> analysisResults = ars.ToList();
                List<Test> tests = new List<Test>();
                for (int i = 0; i < analysisResults.Count(); i++)
                {
                    Console.WriteLine("ARRRR "+analysisResults[i].AnalysisResultId+" ");
                    var test = new Test { AnalysisResultId = analysisResults[i].AnalysisResultId, SampleId = sampleId };
                    tests.Add(test);
                    _context.Add(test);
                }
                await _context.SaveChangesAsync();

                for (int i = 0; i < tests.Count(); i++)
                {
                    Console.WriteLine("TTTTTT " + tests[i].TestID + " ");
                    Console.WriteLine("tttttttttt " + tests[i].AnalysisResultId + " ");

                    var testResult = new TestResult { AnalysisResultId =(int) analysisResults[i].AnalysisResultId, TestId = tests[i].TestID };
                    _context.Add(testResult);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          //  ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId", sample.AnalysisResultId);
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            ViewData["AnalysisId"] = new SelectList(_context.Analyses, "AnalysisId", "AnalysisId", sample.AnalysisId);
            populateStatusDropList();
            return View(sample);
        }
    
        // GET: Samples/Tests/5
        public async Task<IActionResult> Tests(int? id)
        {

            /*   var eMTrackerDevContext = await (from b in _context.Samples
                                  orderby b.SampleID
                                  select b).ToListAsync();
               return View(eMTrackerDevContext.ToList());*/
            /*
               if (id == null)
               {
                   return NotFound();
               }

               var sample = await _context.Tests.Include(test=>test.Sample).Include(test=> test.AnalysisResult)
                   .FirstOrDefaultAsync(m => m.SampleId == id);
               if (sample == null)
               {
                   return NotFound();
               }
               return View(sample);*/
            //test result,  to edit
            var eMTrackerDevContext = _context.Tests.Where(s => s.SampleId == id).Include(s => s.AnalysisResult).Include(s=>s.Sample);
            List<int> Ids = eMTrackerDevContext.Select(o => o.TestID).ToList();
            var testResults = _context.TestResults.Where(c => Ids.Contains(c.TestId)).Include(c=> c.AnalysisResult).Include(c=>c.Test).Include(c=>c.EnteredBy);
            ViewBag.Id = id ;
            return View(await testResults.ToListAsync());

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
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            //ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.Status.StatusId);
            populateStatusDropList();
            return View(sample);
        }
        // POST: Samples/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int analid, int loc, [Bind("SampleID,AnalysisId,StatusId,CollectedById,ApprovedById,LocationCodeId,AnalysisResultId,CollectedDate,ApprovedDate,amount,latitude,longitude")] Sample sample)
        {
            if (id != sample.SampleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  //  sample.LocationCodeId = loc;
                  //  sample.AnalysisId = analid;
                    sample.StatusId = 2;
                    Console.WriteLine("AMT " + sample.amount);
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
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.StatusId);
            return View(sample);
        }
        public async Task<IActionResult> Edit_Collected(int? id)
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
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            //ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.Status.StatusId);
            populateStatusDropList();
            return View(sample);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_Collected(int id, [Bind("SampleID,StatusId,AnalysisId,CollectedById,ApprovedById,LocationCodeId,AnalysisResultId,CollectedDate,ApprovedDate,amount,latitude,longitude")] Sample sample)
        {
            if (id != sample.SampleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sample.StatusId = 3;
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
                return RedirectToAction(nameof(Collected));
            }
            ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.StatusId);
            return View(sample);
        }
        public async Task<IActionResult> Edit_TestResults(int? id)
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
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId");

            ViewData["EnteredById"] = new SelectList(_context.Users, "UserId", "UserId", testResult.EnteredById);
            ViewData["TestId"] = new SelectList(_context.Tests, "TestID", "TestID", testResult.TestId);
            return View(testResult);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_TestResults(int id,[Bind("TestResultId,AnalysisResultId,EnteredById,StartDate,EndDate,TestId")] TestResult testResult)
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
                    var test = _context.Tests.Find((int)testResult.TestId);
                    var sample = _context.Samples.Find((int)test.SampleId);
                    var eMTrackerDevContext = _context.TestResults.Where(s => s.EnteredById != null);
                    List<int> Ids = eMTrackerDevContext.Select(o => o.TestId).ToList();
                    List<Sample> samples = _context.Samples.Where(c => Ids.Contains(c.SampleID)).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status).ToList();
                    for(int i=0; i<samples.Count; i++)
                    {
                        if (sample.SampleID == samples[i].SampleID)
                        {
                            sample.StatusId = 4;
                        }
                    }

                    _context.Update(sample);

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
                return RedirectToAction(nameof(InProcess));
            }
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults, "AnalysisResultId", "AnalysisResultId");
            ViewData["EnteredById"] = new SelectList(_context.Users, "UserId", "UserId", testResult.EnteredById);
            ViewData["TestId"] = new SelectList(_context.Tests, "TestID", "TestID", testResult.TestId);

            return View(testResult);
        }
        

        // GET: Samples/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sample = await _context.Samples
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
        private bool TestResultExists(int id)
        {
            return _context.TestResults.Any(e => e.TestResultId == id);
        }
        public void populateStatusDropList(object selectedStatus = null)
        {
            var statusQuery = from q in _context.Statuses
                              orderby q.StatusId
                              select q;
            ViewBag.StatusId = new SelectList(statusQuery, "StatusId", "StatusName", selectedStatus);
        }
    }
}
