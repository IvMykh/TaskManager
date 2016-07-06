using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.Domain;

namespace TaskManager.WebUI.Models
{
    public class TasksIndexModel
    {
        public class TasksGroup
        {
            public DateTime Date { get; set; }        
            public IEnumerable<ToDoTask> Tasks { get; set; }
        }

        public TasksIndexMode IndexMode { get; set; }
        public IEnumerable<TasksGroup> GroupsByDate { get; set; }
    }
}