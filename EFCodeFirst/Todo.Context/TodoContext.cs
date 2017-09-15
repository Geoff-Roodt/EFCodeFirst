using System.Data.Entity;

namespace Todo.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext() : base("DefaultConnection")
        {
            
        }

        public virtual IDbSet<TodoItem> TodoItems { get; set; }
    }
}
