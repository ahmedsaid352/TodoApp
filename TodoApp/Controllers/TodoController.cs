using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApp.Data;
using TodoApp.Dtos;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Todos = _context.Todos.Where(t => t.UserName == userId).ToList();


            return View(Todos);
        }
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(TodoDto todoDto)
        {
            if (ModelState.IsValid)
            {
                Todo todo = todoDto.ToEntity(); 
                todo.UserName = User.FindFirstValue(ClaimTypes.NameIdentifier); // Set current user ID for the new todo
                _context.Todos.Add(todo);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(todoDto);
        }

        // GET: Todo/Edit/5
        public IActionResult Edit(int id)
        {
            var todo = _context.Todos.Find(id);
            if (todo == null || todo.UserName != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound(); // Handle case where todo not found or doesn't belong to user
            }
            return View(todo);
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TodoDto todoDto)
        {
            var existingTodo = _context.Todos.Find(id);
            if (existingTodo is null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Detach the existing entity to avoid tracking multiple instances
                _context.Entry(existingTodo).State = EntityState.Detached;

                // Create the updated entity and set the state to Modified
                Todo updatedTodo = todoDto.ToEntity(id);
                updatedTodo.UserName = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Entry(updatedTodo).State = EntityState.Modified;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(todoDto);
        }


        // GET: Todo/Delete/5
        public IActionResult Delete(int id)
        {
            var todo = _context.Todos.Find(id);
            if (todo == null || todo.UserName != User.FindFirstValue(ClaimTypes.NameIdentifier))
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
