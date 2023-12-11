using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BE.Models;

namespace BE.Controllers
{
    public class SurveysController : Controller
    {
        private readonly SurveyProjectContext _context;

        public SurveysController(SurveyProjectContext context)
        {
            _context = context;
        }

        // GET: Surveys
        public async Task<IActionResult> Index()
        {
            return _context.Surveys != null ? 
                        View(await _context.Surveys.ToListAsync()) :
                        Problem("Entity set 'SurveyProjectContext.Surveys'  is null.");
        }

        // GET: Surveys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Surveys == null)
            {
                return NotFound();
            }

            var survey = await _context.Surveys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        // GET: Surveys/ViewQuestions/5
        public async Task<IActionResult> Question(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var question = await _context.Questions.SingleOrDefaultAsync(q => q.SurveyId == id);
            //var options = await _context.Options.SingleOrDefaultAsync(o => o.QuestionId.Equals(question!.Id));

            var questions = await _context.Questions.Where(q => q.SurveyId == id).ToListAsync();
            var options = await _context.Options.Where(o => questions.Any(q => q.Id == o.QuestionId)).ToListAsync();

            if (questions == null)
            {
                return NotFound();
            }

            //var viewModels = new QuestionOptionsViewModel();

            //viewModels.Question = question;
            //viewModels.Options = options;

            var viewModels = new List<QuestionOptionsViewModel>();

            foreach (var question in questions)
            {
                var viewModel = new QuestionOptionsViewModel();
                viewModel.Question = question;
                viewModel.Options = options;
                viewModels.Add(viewModel);
            }

            // Trả về view với danh sách các view model
            return View(viewModels);
        }

        // GET: Surveys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Surveys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,UserType,Form,UserPost,CreatedAt,EndAt")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                _context.Add(survey);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(survey);
        }

        // GET: Surveys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Surveys == null)
            {
                return NotFound();
            }

            var survey = await _context.Surveys.FindAsync(id);
            if (survey == null)
            {
                return NotFound();
            }
            return View(survey);
        }

        // POST: Surveys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,UserType,Form,UserPost,CreatedAt,EndAt")] Survey survey)
        {
            if (id != survey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(survey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveyExists(survey.Id))
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
            return View(survey);
        }

        // GET: Surveys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Surveys == null)
            {
                return NotFound();
            }

            var survey = await _context.Surveys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        // POST: Surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Surveys == null)
            {
                return Problem("Entity set 'SurveyProjectContext.Surveys'  is null.");
            }
            var survey = await _context.Surveys.FindAsync(id);
            if (survey != null)
            {
                _context.Surveys.Remove(survey);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SurveyExists(int id)
        {
          return (_context.Surveys?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
