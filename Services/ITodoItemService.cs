using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ITodoItemService
    {
        Task<TodoItem[]> GetIncompleteItemsAsync();
        Task<bool> AddItemAsync(TodoItem item,ApplicationUser user);
        Task<bool> MarkItemAsDoneAsync(Guid id, ApplicationUser user);
        Task<TodoItem[]> GetItemsAsync(ApplicationUser user);
    }
}
