using System.Linq;
using System.Web.Http;
using Todo.Context;
using Todo.Web.Service.Interfaces;
using Todo.Web.Service.Services;

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


    }
}