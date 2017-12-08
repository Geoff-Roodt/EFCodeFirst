using System.Threading.Tasks;
using System.Collections.Generic;
using ToDoApp.Tables;

namespace ToDoApp.Interfaces
{
    public interface IRestService
    {
        Task<List<TodoItem>> RefreshDataAsync();
        Task<int> Update(TodoItem item);
        Task<int> Add(TodoItem item);
        Task<int> Delete(int id);
        Task<int> Update(List<TodoItem> items);
    }
}