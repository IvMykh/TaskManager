using TaskManager.Domain;

namespace TaskManager.WebUI.Models
{
    public class ToDoTaskModel
    {
        public ToDoTask TheTask { get; set; }
        public string ReturnUrl { get; set; }
    }
}