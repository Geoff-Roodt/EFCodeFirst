using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Android.Runtime;
using ToDoApp.Tables;
using ToDoApp.Interfaces;

namespace ToDoApp.Services
{
    public class TodoService
    {
        private readonly IRestService RestService;

        public TodoService(IRestService service)
        {
            RestService = service;
        }

        public async Task<JavaList<TodoItem>> Get()
        {
            var items = await RestService.RefreshDataAsync();
            return (JavaList<TodoItem>) items;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await RestService.Delete(1);
            return response == 1;
        }

        public async Task<bool> Insert(TodoItem item)
        {
            var response = await RestService.Add(item);
            return response == 1;
        }

        public async Task<bool> Edit(TodoItem item)
        {
            var response = await RestService.Update(item);
            return response == 1;
        }
    }
}