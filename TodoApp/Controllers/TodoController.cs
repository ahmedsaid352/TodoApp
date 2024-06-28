using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TodoController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var userId = User.Identity.Name;
            Console.WriteLine(User.Identity.Name);
            Console.WriteLine("Printed the fucken userName");
            var Todos = _context.Todos.Where(t => t.UserName == userId).ToList();


            return View(Todos);
        }
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(Todo todo)
        {
            if (ModelState.IsValid)
            {
                todo.UserName = User.Identity.Name; // Set current user ID for the new todo
                _context.Todos.Add(todo);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todo/Edit/5
        public IActionResult Edit(int id)
        {
            var todo = _context.Todos.Find(id);
            if (todo == null || todo.UserName != User.Identity.Name)
            {
                return NotFound(); // Handle case where todo not found or doesn't belong to user
            }
            return View(todo);
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(todo);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todo/Delete/5
        public IActionResult Delete(int id)
        {
            var todo = _context.Todos.Find(id);
            if (todo == null || todo.UserName != User.Identity.Name)
            {
                return NotFound(); // Handle case where todo not found or doesn't belong to user
            }
            return View(todo);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var todo = _context.Todos.Find(id);
            _context.Todos.Remove(todo);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
