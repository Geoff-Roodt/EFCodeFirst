using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Runtime;
using ToDoApp.Tables;

namespace ToDoApp.Interfaces
{
    public interface IRestService
    {
        Task<JavaList<TodoItem>> RefreshDataAsync();
        Task<int> Update(TodoItem item);
        Task<int> Add(TodoItem item);
        Task<int> Delete(int id);
    }
}