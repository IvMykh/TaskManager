using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain
{
    public class ToDoTask
    {
        public enum Priority
        {
            Low     = 1,
            Normal  = 2,
            High    = 3
        }

        public int      Id              { get; set; }
        public string   Title           { get; set; }
        public string   Comment         { get; set; }
        public Priority TaskPriority    { get; set; }
        public DateTime DueTime         { get; set; }
        public bool     IsComplete      { get; set; }
        public string   OwnerId         { get; set; }
    }
}
