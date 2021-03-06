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
using ClosedXML.Excel;
using System.Data;
using System.IO;

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
        public async Task<IActionResult> Index(int userId)
        {
            ViewBag.UserId = userId;
            Console.WriteLine("LOOKHERE6 "+userId);
            var eMTrackerDevContext = _context.Samples.Where(s => s.StatusId == 1).Include(s=>s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status).Include(s=>s.LocationCode);
            return View(await eMTrackerDevContext.ToListAsync());
        }
        public async Task<IActionResult> Collected()
        {
            var eMTrackerDevContext = _context.Samples.Where(s=> s.StatusId == 2).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status).Include(s => s.LocationCode);
            return View(await eMTrackerDevContext.ToListAsync());
        }
        public async Task<IActionResult> InProcess(int userId)
        {
            ViewBag.UserId = userId;

            var eMTrackerDevContext = _context.Samples.Where(s => s.StatusId == 3).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status).Include(s => s.LocationCode);
            return View(await eMTrackerDevContext.ToListAsync());
        }
        public async Task<IActionResult> Completed(int userId)
        {
            ViewBag.UserId = userId;

            //  List<TestResult> testResults = _context.TestResults.Where(s => s.EnteredById != null).ToList();
            //  List<int> testIds = testResults.Select(o => o.TestId).ToList();

            //   List<Test> tests = new List<Test>();
            //   for(int i = 0; i < testIds.Count(); i++)
            //   {
            //       tests.Add(_context.Tests.Find(testIds[i]));
            //   }
            //   List<int> Ids = new List<int>();
            //   for(int i=0; i < tests.Count(); i++)
            //   {
            //      Ids.Add((int)tests[i].SampleId);
            //  }
            //  var samples = _context.Samples.Where(c => Ids.Contains(c.SampleID)).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status);
            var eMTrackerDevContext2 = _context.Samples.Where(s => s.StatusId == 4).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status).Include(s => s.LocationCode);
            return View(await eMTrackerDevContext2.ToListAsync());
        }
        public async Task<IActionResult> Approved()
        {
            var eMTrackerDevContext = _context.Samples.Where(s => s.StatusId == 5).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status).Include(s => s.LocationCode);
            return View(await eMTrackerDevContext.ToListAsync());
        }
        public async Task<IActionResult> Rejected()
        {
            var eMTrackerDevContext = _context.Samples.Where(s => s.StatusId == 6).Include(s => s.Analysis).Include(s => s.Test).Include(s => s.ApprovedBy).Include(s => s.CollectedBy).Include(s => s.Status).Include(s => s.LocationCode);
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
         //   ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId");
          //  ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["AnalysisId"] = new SelectList(_context.Analyses, "AnalysisId", "Name");
            ViewData["LocationCodeId"] = new SelectList(_context.Locationcodes, "LocationCodeId", "LocationId");

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
          //  ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
          //  ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
             ViewData["AnalysisId"] = new SelectList(_context.Analyses, "AnalysisId", "Name", sample.AnalysisId);
            ViewData["LocationCodeId"] = new SelectList(_context.Locationcodes, "LocationCodeId", "LocationId",sample.LocationCodeId);

            //populateAnalysis();
            // populateStatusDropList();
            return View(sample);
        }
    
        // GET: Samples/Tests/5
        public async Task<IActionResult> Tests(int? id, int userId)
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
            ViewBag.UserId = userId;
            return View(await testResults.ToListAsync());

        }


        // GET: Samples/Edit/5                         //add validation
        public async Task<IActionResult> Edit(int? id, int userId)
        {
            Console.WriteLine("EDIT5 " + userId);
            if (id == null)
            {
                return NotFound();
            }

            var sample = await _context.Samples.FindAsync(id);
            sample.CollectedById = userId;
            _context.Update(sample);
            if (sample == null)
            {
                return NotFound();
            }
         //   ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
        //    ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            ViewData["AnalysisId"] = new SelectList(_context.Analyses.Where(s => s.AnalysisId == sample.AnalysisId), "AnalysisId", "Name");
            ViewData["LocationCodeId"] = new SelectList(_context.Locationcodes.Where(s => s.LocationCodeId == sample.LocationCodeId), "LocationCodeId", "LocationId");
            //ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.Status.StatusId);
          //  populateStatusDropList();
            return View(sample);
        }
        // POST: Samples/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,int userId, [Bind("SampleID,AnalysisId,StatusId,CollectedById,ApprovedById,LocationCodeId,AnalysisResultId,CollectedDate,ApprovedDate,amount,latitude,longitude")] Sample sample)
        {
            if (id != sample.SampleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("POSTEDIT5 " + userId);

                    sample.StatusId = 2;
                    Console.WriteLine("AMT " + sample.amount);
                    sample.CollectedById = 1;
                    sample.CollectedDate = DateTime.Now;
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
            // ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            // ViewData["CollectedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.CollectedById);
            // ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.StatusId);
            ViewData["AnalysisId"] = new SelectList(_context.Analyses.Where(s=>s.AnalysisId==sample.AnalysisId), "AnalysisId", "Name",sample.AnalysisId);
            ViewData["LocationCodeId"] = new SelectList(_context.Locationcodes.Where(s => s.LocationCodeId == sample.LocationCodeId), "LocationCodeId", "LocationId",sample.LocationCodeId);
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
          //  ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["CollectedById"] = new SelectList(_context.Users.Where(s => s.UserId == sample.CollectedById), "UserId", "FirstName", sample.CollectedById);
            ViewData["AnalysisId"] = new SelectList(_context.Analyses.Where(s => s.AnalysisId == sample.AnalysisId), "AnalysisId", "Name", sample.AnalysisId);
            ViewData["LocationCodeId"] = new SelectList(_context.Locationcodes.Where(s => s.LocationCodeId == sample.LocationCodeId), "LocationCodeId", "LocationId", sample.LocationCodeId);
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
                    List<Test> tests = _context.Tests.Where(s => s.SampleId==id).ToList();
                    List<int> Ids = tests.Select(o => o.TestID).ToList();

                    var testResults = _context.TestResults.Where(c => Ids.Contains(c.TestId)).Include(c => c.AnalysisResult).Include(c => c.Test).Include(c => c.EnteredBy).ToList();
                    for (int i=0; i<testResults.Count(); i++)
                    {
                        testResults[i].StartDate = DateTime.Now;
                        _context.Update(testResults[i]);
                    }

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
            //  ViewData["ApprovedById"] = new SelectList(_context.Users, "UserId", "UserId", sample.ApprovedById);
            ViewData["AnalysisId"] = new SelectList(_context.Analyses.Where(s => s.AnalysisId == sample.AnalysisId), "AnalysisId", "Name", sample.AnalysisId);
            ViewData["LocationCodeId"] = new SelectList(_context.Locationcodes.Where(s => s.LocationCodeId == sample.LocationCodeId), "LocationCodeId", "LocationId", sample.LocationCodeId);
            ViewData["CollectedById"] = new SelectList(_context.Users.Where(s => s.UserId == sample.CollectedById), "UserId", "FirstName", sample.CollectedById);
            // ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", sample.StatusId);
            return View(sample);
        }
        public async Task<IActionResult> Edit_TestResults(int? id, int userId)
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
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults.Where(s => s.AnalysisResultId == testResult.AnalysisResultId), "AnalysisResultId", "AnalysisResultId");

            ViewData["EnteredById"] = new SelectList(_context.Users, "UserId", "FirstName", testResult.EnteredById);
            ViewData["TestId"] = new SelectList(_context.Tests.Where(s => s.TestID == testResult.TestId), "TestID", "TestID", testResult.TestId);
            return View(testResult);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_TestResults(int id, int userId,[Bind("TestResultId,AnalysisResultId,EnteredById,StartDate,amount,EndDate,TestId")] TestResult testResult)
        {
            if (id != testResult.TestResultId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    testResult.EnteredById = 1;
                    testResult.EndDate = DateTime.Now;
                    _context.Update(testResult);

                    List<TestResult> testResults = _context.TestResults.Where(s => s.amount >0).ToList();
                    List<int> testIds = testResults.Select(o => o.TestId).ToList();

                    List<Test> tests = new List<Test>();
                    for (int i = 0; i < testIds.Count(); i++)
                    {
                        tests.Add(_context.Tests.Find(testIds[i]));
                    }
                    List<int> Ids = new List<int>();
                    for (int i = 0; i < tests.Count(); i++)
                    {
                        Ids.Add((int)tests[i].SampleId);
                    }
                    List<Sample> samples = new List<Sample>();
                    for (int i = 0; i < testIds.Count(); i++)
                    {
                        samples.Add(_context.Samples.Find(Ids[i]));
                    }
                    for (int i = 0; i < samples.Count(); i++)
                    {
                        samples[i].StatusId = 4;
                        _context.Update(samples[i]);
                    }
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
            ViewData["AnalysisResultId"] = new SelectList(_context.AnalysisResults.Where(s => s.AnalysisResultId == testResult.AnalysisResultId), "AnalysisResultId", "AnalysisResultId");
            ViewData["EnteredById"] = new SelectList(_context.Users, "UserId", "FirstName", testResult.EnteredById);
            ViewData["TestId"] = new SelectList(_context.Tests.Where(s => s.TestID == testResult.TestId), "TestID", "TestID", testResult.TestId);

            return View(testResult);
        }
        // GET: Samples/Edit/5
        public async Task<IActionResult> Edit_Completed(int? id, int userId)
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
            ViewData["AnalysisId"] = new SelectList(_context.Analyses.Where(s => s.AnalysisId == sample.AnalysisId), "AnalysisId", "Name", sample.AnalysisId);
            ViewData["LocationCodeId"] = new SelectList(_context.Locationcodes.Where(s => s.LocationCodeId == sample.LocationCodeId), "LocationCodeId", "LocationId", sample.LocationCodeId);
            ViewData["CollectedById"] = new SelectList(_context.Users.Where(s => s.UserId == sample.CollectedById), "UserId", "FirstName", sample.CollectedById);
            ViewData["StatusId"] = new SelectList(_context.Statuses.Where(s => s.StatusId > 4), "StatusId", "StatusName", sample.StatusId);
            //populateStatusDropList();
            return View(sample);
        }
        // POST: Samples/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit_Completed(int id, int userId, [Bind("SampleID,AnalysisId,StatusId,CollectedById,ApprovedById,LocationCodeId,AnalysisResultId,CollectedDate,ApprovedDate,amount,latitude,longitude")] Sample sample)
        {
            if (id != sample.SampleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sample.ApprovedById = 1;
                    sample.ApprovedDate = DateTime.Now;
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
                return RedirectToAction(nameof(Completed));
            }
            ViewData["AnalysisId"] = new SelectList(_context.Analyses.Where(s => s.AnalysisId == sample.AnalysisId), "AnalysisId", "Name", sample.AnalysisId);
            ViewData["LocationCodeId"] = new SelectList(_context.Locationcodes.Where(s => s.LocationCodeId == sample.LocationCodeId), "LocationCodeId", "LocationId", sample.LocationCodeId);
            ViewData["CollectedById"] = new SelectList(_context.Users.Where(s => s.UserId == sample.CollectedById), "UserId", "FirstName", sample.CollectedById);
            ViewData["StatusId"] = new SelectList(_context.Statuses.Where(s=>s.StatusId>4), "StatusId", "StatusName", sample.StatusId);
           // populateStatusDropList();
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
            List<Test> tests =  _context.Tests.Where(t => t.SampleId == sample.SampleID).ToList();
            for(int i = 0; i < tests.Count; i++)
            {
                _context.Tests.Remove(tests[i]);
            }
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
                              where q.StatusId >=5
                              select q;
            ViewBag.StatusId = new SelectList(statusQuery, "StatusId", "StatusName", selectedStatus);
        }
        public void populateAnalysis(object analysis = null)
        {
            var analysisQuery = from q in _context.Analyses
                              
                              select q;
            ViewBag.AnalysisId = new SelectList(analysisQuery, "AnalysisId", "Name", analysis);
        }

    }
}

