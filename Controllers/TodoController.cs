using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace TodoApp.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private ITodoItemService _service;
        private UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoItemService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            var items = await _service.GetItemsAsync(currentUser);
            var model = new TodoViewModel()
            {
                Items = items
            };
            return View(model);
        }
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem item)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var success = await _service.AddItemAsync(item,currentUser);
            if (!success)
            {
                return BadRequest("Could not add item.");
            }
            return RedirectToAction("Index");

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkItemAsDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            var result = await _service.MarkItemAsDoneAsync(id,currentUser);
            if (!result)
            {
                return BadRequest("Can not mark item as done.");
            }
            return RedirectToAction("Index");
        }

    }
}