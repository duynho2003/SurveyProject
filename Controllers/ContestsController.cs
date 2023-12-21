using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    public class ContestsController : Controller
    {
        private readonly SurveyProjectContext _context;

        public ContestsController(SurveyProjectContext context)
        {
            _context = context;
        }

        // GET: Contests
        public async Task<IActionResult> Index()
        {
            return _context.Contests != null ?
                        View(await _context.Contests.ToListAsync()) :
                        Problem("Entity set 'SurveyProjectContext.Contests'  is null.");
        }

        // GET: Contests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contests == null)
            {
                return NotFound();
            }

            var contest = await _context.Contests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        // GET: Contests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,StartTime,EndTime")] Contest contest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contest);
        }

        // GET: Contests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contests == null)
            {
                return NotFound();
            }

            var contest = await _context.Contests.FindAsync(id);
            if (contest == null)
            {
                return NotFound();
            }
            return View(contest);
        }

        // POST: Contests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StartTime,EndTime")] Contest contest)
        {
            if (id != contest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContestExists(contest.Id))
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
            return View(contest);
        }

        // GET: Contests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contests == null)
            {
                return NotFound();
            }

            var contest = await _context.Contests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contest == null)
            {
                return NotFound();
            }

            return View(contest);
        }

        // POST: Contests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contests == null)
            {
                return Problem("Entity set 'SurveyProjectContext.Contests'  is null.");
            }
            var contest = await _context.Contests.FindAsync(id);
            if (contest != null)
            {
                _context.Contests.Remove(contest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult TakeContest(int id)
        {
            var survey = _context.Contests.Include(s => s.QuestionContests).ThenInclude(q => q.AnswerOptions).FirstOrDefault(s => s.Id == id);
            if (survey == null)
            {
                return NotFound();
            }
            // Check if survey is closed (kiểm tra nếu khảo sát hết giờ => Đóng)
            if (survey.EndTime < DateTime.Now)
            {
                return View("Closed", survey); // Use a dedicated "Closed" view to inform users
            }
            return View(survey);
        }

        [HttpPost]
        public IActionResult TakeContest(int id, List<int> answerIds)
        {
            var survey = _context.Contests.Find(id);
            if (survey == null)
            {
                return NotFound();
            }

            // Check if survey is closed (kiểm tra nếu khảo sát hết giờ => Đóng)
            if (survey.EndTime < DateTime.Now)
            {
                return View("Closed", survey); // Alternatively, redirect to an error or information page (tạo trang thông báo lỗi)
            }

            // Process answers and save results

            return RedirectToAction(nameof(Index));
        }

        private bool ContestExists(int id)
        {
            return (_context.Contests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
