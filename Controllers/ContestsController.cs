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

        // GET: /Contest/TakeContest/1
        public IActionResult TakeContest(int id)
        {
            var contest = _context.Contests
                .Include(c => c.QuestionContests)
                .FirstOrDefault(c => c.Id == id);

            if (contest == null)
            {
                return NotFound();
            }

            // Check if contest is closed
            if (contest.EndTime < DateTime.Now)
            {
                return View("Closed", contest);
            }

            return View(contest);
        }

        // POST: /Contest/TakeContest/1
        //[HttpPost]
        //public IActionResult TakeContest(int id, Dictionary<int, int> selectedOptions)
        //{
        //    var contest = _context.Contests.Find(id);

        //    if (contest == null)
        //    {
        //        return NotFound();
        //    }

        //    // Check if contest is closed
        //    if (contest.EndTime < DateTime.Now)
        //    {
        //        return View("Closed", contest);
        //    }

        //    // Process selected options and save results
        //    // The selectedOptions dictionary contains questionId as the key and selected optionId as the value

        //    // Example of processing and saving results
        //    foreach (var kvp in selectedOptions)
        //    {
        //        var questionId = kvp.Key;
        //        var selectedOptionId = kvp.Value;

        //        // Perform your logic to save the selected option for each question
        //        // This is just a placeholder, you need to implement your own saving logic
        //        // For example, you might have a Result model to store user responses
        //        // and you can save the user's response for each question in the database
        //    }
        //    return RedirectToAction(nameof(Index)); // Redirect to the contest index or another appropriate action
        //}

        [HttpPost]
        public IActionResult TakeContest(int id, Dictionary<int, string[]> selectedOptions)
        {
            var contest = _context.Contests
                .Include(c => c.QuestionContests)
                .FirstOrDefault(c => c.Id == id);

            if (contest == null)
            {
                return NotFound();
            }

            // Check if contest is closed
            if (contest.EndTime < DateTime.Now)
            {
                return View("Closed", contest);
            }

            var results = new List<ResultViewModel>(); // Create a list to store results

            foreach (var question in contest.QuestionContests)
            {
                string correctAnswer = question.CorrectAnswer; // assuming CorrectAnswer is a string
                string[] selectedOptionsForQuestion;

                // Kiểm tra xem người dùng đã chọn câu trả lời cho câu hỏi này hay không
                if (selectedOptions.TryGetValue(question.Id, out selectedOptionsForQuestion))
                {
                    // So sánh câu trả lời đã chọn với câu trả lời đúng của câu hỏi
                    bool isCorrect = selectedOptionsForQuestion != null && selectedOptionsForQuestion.Contains(correctAnswer);

                    // Add result to the list
                    results.Add(new ResultViewModel
                    {
                        QuestionText = question.QuestionText,
                        UserAnswer = selectedOptionsForQuestion != null ? string.Join(", ", selectedOptionsForQuestion) : "No answer",
                        IsCorrect = isCorrect
                    });
                }
            }
            // Nếu không có câu trả lời nào được chọn hoặc không có câu trả lời nào đúng
            //return View(contest);
            // Pass the results to the Result view
            return View("Result", results);
        }


        private bool ContestExists(int id)
        {
            return (_context.Contests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
