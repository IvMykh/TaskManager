
namespace TaskManager.WebUI.Models
{
    public class DaySummaryModel
    {
        public string UserName { get; set; }
        public int CompleteTasksQuantity { get; set; }
        public int PendingTasksQuantity { get; set; }
        public int OverdueTasksQuantity { get; set; }

        public int Total 
        { 
            get 
            {
                return CompleteTasksQuantity + 
                    OverdueTasksQuantity + 
                    PendingTasksQuantity;
            } 
        }
    }
}