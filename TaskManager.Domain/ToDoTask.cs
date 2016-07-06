using System;
using System.ComponentModel.DataAnnotations;

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

        public int Id { get; set; }

        [Required(ErrorMessage = "Please, enter the title")]
        public string Title { get; set; }
        
        public string Comment { get; set; }

        [Required(ErrorMessage = "Please, select the priority")]
        public Priority TaskPriority { get; set; }

        [Required(ErrorMessage = "Please enter the due time")]
        public DateTime DueTime { get; set; }

        public bool IsComplete { get; set; }

        public string OwnerId { get; set; }
    }
}
