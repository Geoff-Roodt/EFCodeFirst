
using System;

namespace ToDoApp.Tables
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}