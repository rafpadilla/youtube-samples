using System;

namespace SQLiteSample.Domain
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
