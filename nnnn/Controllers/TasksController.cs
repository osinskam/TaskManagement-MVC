﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using nnnn.Data;
using nnnn.Models;

namespace nnnn.Controllers
{

    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks

        public async Task<IActionResult> Index()
        {
            if (_context.Task == null)
            {
                return NotFound("Entity set 'ApplicationDbContext.Task' is null.");
            }
            var tasks = await _context.Task.OrderByDescending(t => t.Status != "Done").ThenBy(t => t.Status == "Done").ThenByDescending(t => t.DueDate).ToListAsync();

            return View(tasks);
        }

        //Get:

        //Get: Tasks/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        //Post: Tasks/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchCriteria, string SearchPhrase, string Description, string Category, string Status)
        {
            if (SearchCriteria == "Title")
            {
                return View("Index", await _context.Task.Where(t => t.Title.Contains(SearchPhrase)
                                                                    && (string.IsNullOrEmpty(Category) || t.Category == Category)
                                                                    && (string.IsNullOrEmpty(Description) || t.Description == Description)
                                                                    && (string.IsNullOrEmpty(Status) || t.Status == Status))
                                                                    .ToListAsync());
            }
            else if (SearchCriteria == "Description")
            {
                return View("Index", await _context.Task.Where(t => t.Description.Contains(SearchPhrase)
                                                                    && (string.IsNullOrEmpty(Category) || t.Category == Category)
                                                                    && (string.IsNullOrEmpty(Status) || t.Status == Status))
                                                                    .ToListAsync());
            }
            else if (SearchCriteria == "Category")
            {
                return View("Index", await _context.Task.Where(t => t.Category.Contains(SearchPhrase)
                                                                    && (string.IsNullOrEmpty(Description) || t.Description == Description)
                                                                    && (string.IsNullOrEmpty(Status) || t.Status == Status))
                                                                    .ToListAsync());
                }
            else if (SearchCriteria == "Status")
                {
                    return View("Index", await _context.Task.Where(t => t.Status.Contains(SearchPhrase)
                                                                        && (string.IsNullOrEmpty(Description) || t.Description == Description)
                                                                        && (string.IsNullOrEmpty(Category) || t.Category == Category))
                                                                        .ToListAsync());
                }
            else
            {
                return View("Index", await _context.Task.ToListAsync());
            }
        }


        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Category,Status,Assignee,DueDate")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(task.Title))
                {
                    ModelState.AddModelError("Title", "Title is required.");
                    return View(task);
                }
                DateTime dueDate;
                if (!DateTime.TryParse(task.DueDate.ToString(), out dueDate))
                {
                    ModelState.AddModelError("DueDate", "Due date is not in a correct format.");
                    return View(task);
                }

                task.DueDate = dueDate;


                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
                return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }


    // POST: Tasks/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Category,Status,Assignee,DueDate")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Task == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Task'  is null.");
            }
            var task = await _context.Task.FindAsync(id);
            if (task != null)
            {
                _context.Task.Remove(task);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
          return (_context.Task?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
