using System;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using Todo.Context;
using Todo.Web.Service.Services;
using Todo.Web.Service.Interfaces;

namespace Todo.Web.Controllers
{
    public class TodoItemsController : ApiController
    {
        private IService<TodoItem> Service { get; set; }

        public TodoItemsController()
        {
            TodoContext context = new TodoContext();
            Service = new Service<TodoItem>(context);
        }


        [HttpGet]
        [Route("todoitems")]
        public IQueryable<TodoItem> Get()
        {
            return Service.GetAll().AsQueryable();
        }

        [HttpGet]
        [Route("todoitems/{id}")]
        public TodoItem GetSingle(int id)
        {
            return Service.GetSingle(id);
        }

        [HttpPost]
        [Route("todoitems")]
        public async Task<TodoItem> Post(TodoItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (item.Created.Equals(DateTime.MinValue)) item.Created = DateTime.Now;
            if (item.Modified.Equals(DateTime.MinValue)) item.Modified = DateTime.Now;

            return await Service.Add(item);
        }

        [HttpPut]
        [Route("todoitems")]
        public async Task<bool> Put(TodoItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            item.Modified = DateTime.Now;
            return await Service.Update(item) > 0;
        }

        [HttpDelete]
        [Route("todoitems/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await Service.Remove(id);
        }

    }
}