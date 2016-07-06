using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TaskManager.Domain;
using TaskManager.WebUI.Models;

namespace TaskManager.WebUI.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        //
        // GET: /Tasks/
        public ActionResult Index(TasksIndexMode indexMode = TasksIndexMode.All)
        {
            List<ToDoTask> tasks = null;

            using (var db = new TaskManagerContext())
            {
                string userId = User.Identity.GetUserId();

                tasks = db.ToDoTasks
                    .Where(t => t.OwnerId.CompareTo(userId) == 0)
                    .ToList<ToDoTask>();

                switch (indexMode)
                {
                    case TasksIndexMode.Complete:
                        {
                            tasks = tasks
                                .Where(t => t.IsComplete)
                                .ToList<ToDoTask>();
                        } break;
                    case TasksIndexMode.Pending:
                        {
                            tasks = tasks
                                .Where(t => !t.IsComplete && t.DueTime.Ticks > DateTime.Now.Ticks)
                                .ToList<ToDoTask>();
                        } break;
                    case TasksIndexMode.Overdue:
                        {
                            tasks = tasks
                                .Where(t => !t.IsComplete && t.DueTime.Ticks < DateTime.Now.Ticks)
                                .ToList<ToDoTask>();
                        } break;
                    default:
                        throw new ArgumentException("Unexpected index mode");
                }
            }

            var taskGroups = from task in tasks
                             group task by task.DueTime.Date into tasksGroup
                             select new TasksIndexModel.TasksGroup
                                 {
                                     Date = tasksGroup.Key,
                                     Tasks = tasksGroup.ToList<ToDoTask>()
                                 };

            return View(new TasksIndexModel
            {
                IndexMode = indexMode,
                GroupsByDate = taskGroups
            });
        }
    }
}