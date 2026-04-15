using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio.Data;
using portfolio.Models;
using System.Linq;
using System.Threading.Tasks;

namespace portfolio.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // SHOW ACTIVE TASKS ONLY
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.Tasks
                .Where(t => !t.IsDeleted)
                .ToListAsync();

            return View(tasks);
        }

        // CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }

        // EDIT
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null || task.IsDeleted)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItem task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null || existingTask.IsDeleted)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.IsCompleted = task.IsCompleted;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }

        // SOFT DELETE → MOVE TO TRASH
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // TRASH PAGE
        public async Task<IActionResult> Trash()
        {
            var deletedTasks = await _context.Tasks
                .Where(t => t.IsDeleted)
                .ToListAsync();

            return View(deletedTasks);
        }

        // RESTORE TASK
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.IsDeleted = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Trash));
        }

        // PERMANENT DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PermanentDelete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Trash));
        }

        // TOGGLE COMPLETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null || task.IsDeleted)
            {
                return NotFound();
            }

            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}