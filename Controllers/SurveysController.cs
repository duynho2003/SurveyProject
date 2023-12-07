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
            if (id == null || _context.Surveys == null)
            {
                return NotFound();
            }

            var question = await _context.Surveys
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
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
