using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMTrackerDev.Data;
using EMTrackerDev.Models;
using ClosedXML.Excel;
using System.Data;
using System.IO;

namespace EMTrackerDev.Controllers
{
    public class ResultsController : Controller
    {
        private readonly EMTrackerDevContext _context;

        public ResultsController(EMTrackerDevContext context)
        {
            _context = context;
        }

        // GET: Results
        public async Task<IActionResult> Index()
        {
            return View(await _context.Results.ToListAsync());
        }

        // GET: Results/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .FirstOrDefaultAsync(m => m.ResultID == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // GET: Results/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResultID,file")] Result result)
        {
            if (ModelState.IsValid)
            {
                _context.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        // GET: Results/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResultID,file")] Result result)
        {
            if (id != result.ResultID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultExists(result.ResultID))
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
            return View(result);
        }

        // GET: Results/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .FirstOrDefaultAsync(m => m.ResultID == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.Results.FindAsync(id);
            _context.Results.Remove(result);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultExists(int id)
        {
            return _context.Results.Any(e => e.ResultID == id);
        }
        private void fillRow(DataRow dr, TestResult r)
        {
       

         
                dr.SetField("SampleID", r.Test.SampleId);
                dr.SetField("Analysis", r.AnalysisResult.AnalysisId);
                dr.SetField("Collected Date", r.Test.Sample.CollectedDate);
                dr.SetField("Collected By", r.Test.Sample.CollectedBy.FirstName);
                dr.SetField("Location Code", r.Test.Sample.LocationCode.LocationId);
                dr.SetField("Location Description", r.Test.Sample.LocationCode.Description);
                dr.SetField("Approved Date", r.Test.Sample.ApprovedDate);
                dr.SetField("Approved By", r.Test.Sample.ApprovedBy.FirstName);
                dr.SetField("Component", r.AnalysisResult.Component);

                dr.SetField("Amount", r.amount);
                dr.SetField("UOM", r.AnalysisResult.UOM);

            

        }
        [HttpPost]
        public FileResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("SampleID"),
                                            new DataColumn("Analysis"),
                                            new DataColumn("Collected Date"),
                                            new DataColumn("Collected By"),
                                            new DataColumn("Location Code"),
                                            new DataColumn("Location Description"),
                                            new DataColumn("Approved Date"),
                                            new DataColumn("Approved By"),
                                            new DataColumn("Component"),
                                            new DataColumn("Amount"),
                                            new DataColumn("UOM")});

          /*  var report = from customer in _context.Samples.Include(s=>s.Analysis).Include(s=>s.CollectedBy)
                         .Include(s=>s.LocationCode).Include(s=>s.ApprovedBy)
                            select customer;*/
            var report2 = from customer in _context.TestResults.Include(t => t.AnalysisResult).Include(t=>t.Test).Include(t=>t.Test.Sample).Include(t => t.Test.Sample.CollectedBy)
                          .Include(t => t.Test.Sample.LocationCode).Include(t => t.Test.Sample.ApprovedBy)
                          where customer.EndDate != null
                          select customer;
            foreach (TestResult r in report2)
            {
                DataRow dr = dt.NewRow();
                fillRow(dr,r);
                dt.Rows.Add(dr);
            }
     

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }
    }
}
