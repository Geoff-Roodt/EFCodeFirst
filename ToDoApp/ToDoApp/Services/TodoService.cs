using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public async Task<List<TodoItem>> Get()
        {
            var items = await RestService.RefreshDataAsync();
            items = items.OrderBy(x => x.Completed).ThenBy(x => x.Description).ToList();
            return items;
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