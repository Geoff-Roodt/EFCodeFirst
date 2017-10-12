using System;
using System.Data.Entity.Migrations;

namespace Todo.Context.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<TodoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TodoContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.TodoItems.AddOrUpdate(i => i.Description,
                new TodoItem {Description = "Item 1", Created = DateTime.Now, Modified = DateTime.Now},
                new TodoItem {Description = "Item 2", Created = DateTime.Now, Modified = DateTime.Now},
                new TodoItem {Description = "Item 3", Created = DateTime.Now, Modified = DateTime.Now},
                new TodoItem {Description = "Item 4", Created = DateTime.Now, Modified = DateTime.Now }
            );

        }
    }
}
