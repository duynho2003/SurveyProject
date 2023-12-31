﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BE.Controllers
{
    public class UsersController : Controller
    {
        private readonly SurveyProjectContext _context;

        public UsersController(SurveyProjectContext context)
        {
            _context = context;
        }

        // GET: Users //Da duyet hien staff/student active = 1
        public async Task<IActionResult> Index(string? usearch)
        {
            var model = await _context.Users.Where(u => u.Active == 1 && u.Role == "Staff" || u.Active == 1 && u.Role == "Student").ToListAsync();
            if (usearch == null)
            {
                return View(model);
            }
            else
            {
                var model1 = model!.Where(m => m.UserName!.Contains(usearch!) || m.Email!.Contains(usearch!) || m.NumberCode!.Contains(usearch!));
                return View(model1);
            }
        }
        
        // GET: Users //Da duyet hien admin active = 1
        public async Task<IActionResult> Admin(string? asearch)
        {
            var model = await _context.Users.Where(u => u.Active == 1 && u.Role == "Admin").ToListAsync();
            if (asearch == null)
            {
                return View(model);
            }
            else
            {
                var model1 = model!.Where(m => m.UserName!.Contains(asearch!) || m.Email!.Contains(asearch!) || m.NumberCode!.Contains(asearch!));
                return View(model1);
            }
            //return _context.Users != null ? 
            //    View(await _context.Users.Where(u => u.Active == 1 && u.Role == "Admin").ToListAsync()) :
            //      Problem("Entity set 'SurveyProjectContext.Users' is null.");
        }

        //Chua duyet hien user active = 0
        public async Task<IActionResult> Pending()
        {
            var list = await _context.Users.Where(x => x.Active == 0).ToListAsync();
            ViewBag.Count = list.Count;
            if (list.Count == 0)
            {
                ViewBag.Msg = "Nothing";
            }
            return View(list);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,Email,NumberCode,Class,Specification,Section,JoinDate,Role,Active")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Password,Email,NumberCode,Class,Specification,Section,JoinDate,Role,Active")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'SurveyProjectContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //duyet user
        public async Task<IActionResult> ApproveActive(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Active = 1;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Pending));
            }
            return View(user);
        }
        //tu choi user
        public async Task<IActionResult> DenyActive(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Active = -1;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Pending));
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
