using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using NuGet.Versioning;
using Microsoft.Extensions.Options;

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

            var questions = await _context.Questions
                .Include(q => q.Options)
                .Where(q => q.SurveyId == id)
                .ToListAsync();

            if (questions == null || !questions.Any())
            {
                return NotFound();
            }

            var options = await _context.Options.ToListAsync();

            // Create and populate the view model
            var viewModel = new QuestionOptionsViewModel(questions, options);

            // Return the view with the view model
            return View(viewModel);
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

        public async Task<IActionResult> Edit(int surveyId)
        {
            // Lấy thông tin survey
            var survey = await _context.Surveys.FindAsync(surveyId);

            // Lấy thông tin các câu hỏi
            var questions = await _context.Questions
                .Where(q => q.SurveyId == surveyId)
                .Include(q => q.Options)
                .ToListAsync();

            // Hiển thị thông tin survey và các câu hỏi
            ViewData["Survey"] = survey;
            ViewData["Questions"] = questions;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int surveyId, [Bind("title,description,questions")] Survey survey)
        {
            // Xử lý yêu cầu chỉnh sửa
            survey.Id = surveyId;

            // Kiểm tra xem danh sách câu hỏi có chứa bất kỳ mục nào hay không
            if (survey.Questions == null || survey.Questions.Count == 0)
            {
                // Trả về mã phản hồi 400 Bad Request
                return BadRequest();
            }

            // Lưu thông tin survey
            await _context.Surveys.UpdateAsync(survey);

            // Lưu thông tin các câu hỏi
            foreach (var question in survey.Questions)
            {
                // Lưu thông tin câu hỏi
                question.Id = question.Id;
                question.SurveyId = surveyId;

                // Lưu thông tin các tùy chọn
                var options = question.Options.ToList();

                // Xóa các tùy chọn bị xóa
                foreach (var optionId in question.DeletedOptions)
                {
                    options.Remove(options.Find(x => x.Id == optionId));
                }

                // Thêm các tùy chọn mới
                foreach (var option in question.NewOptions)
                {
                    options.Add(new Option { Id = Guid.NewGuid(), QuestionId = question.Id, Text = option.Text });
                }

                // Cập nhật các tùy chọn đã chỉnh sửa
                foreach (var option in question.UpdatedOptions)
                {
                    options.FirstOrDefault(x => x.Id == option.Id).Text = option.Text;
                }

                // Lưu thông tin tùy chọn
                await _context.Options.ReplaceRangeAsync(options);
            }

            // Lưu cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Chuyển hướng về trang survey
            return RedirectToRoute("surveys", new { surveyId = surveyId });
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
