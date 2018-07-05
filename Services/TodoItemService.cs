using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class TodoItemService:ITodoItemService
    {
        private ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            return _context.Items.Where(x => x.IsDone == false).ToArrayAsync();
        }

        public async Task<bool> AddItemAsync(TodoItem item, ApplicationUser user)
        {
            item.Id = Guid.NewGuid();
            item.IsDone = false;
            item.DueAt = DateTimeOffset.Now.AddDays(3);
            item.UserId = user.Id;
            _context.Items.Add(item);
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }

        public async Task<bool> MarkItemAsDoneAsync(Guid id,ApplicationUser user)
        {
            var item = await _context.Items.Where(x => x.Id == id && x.UserId == user.Id).SingleOrDefaultAsync();
            if (item == null)
            {
                return false;
            }
            item.IsDone = true;


            var result = await _context.SaveChangesAsync();
            return result == 1;
        }

        public async Task<TodoItem[]> GetItemsAsync(ApplicationUser user)
        {
            return await _context.Items.Where(x => x.UserId == user.Id).ToArrayAsync();
        }
    }
}
