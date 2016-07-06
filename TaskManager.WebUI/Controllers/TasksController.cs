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

                    default: break; // All tasks.
                }
            }

            var taskGroups = (from task in tasks
                             orderby task.DueTime
                             group task by task.DueTime.Date into tasksGroup
                             select new TasksIndexModel.TasksGroup
                                 {
                                     Date = tasksGroup.Key,
                                     Tasks = tasksGroup.ToList<ToDoTask>()
                                 }).OrderBy(taskGroup => taskGroup.Date);

            return View(new TasksIndexModel
                {
                    IndexMode = indexMode,
                    GroupsByDate = taskGroups
                });
        }

        public ActionResult Edit(int taskId, string returnUrl)
        {
            ToDoTask task = null;

            using (var db = new TaskManagerContext())
            {
                task = db.ToDoTasks
                    .FirstOrDefault(t => t.Id == taskId);
            }

            return View(new ToDoTaskModel
                {
                    ReturnUrl = returnUrl,
                    TheTask = task
                });
        }

        [HttpPost]
        public ActionResult Edit(ToDoTaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TaskManagerContext())
                {
                    if (taskModel.TheTask.Id == 0)
                    {
                        db.ToDoTasks.Add(taskModel.TheTask);
                    }
                    else
                    {
                        ToDoTask dbEntry = db.ToDoTasks.Find(taskModel.TheTask.Id);
                        
                        if (dbEntry != null)
                        {
                            dbEntry.Title = taskModel.TheTask.Title;
                            dbEntry.Comment = taskModel.TheTask.Comment;
                            dbEntry.IsComplete = taskModel.TheTask.IsComplete;
                            dbEntry.TaskPriority = taskModel.TheTask.TaskPriority;
                            dbEntry.DueTime = taskModel.TheTask.DueTime;
                        }
                    }

                    db.SaveChanges();
                }

                TempData["message"] = string.Format(
                    "'{0}' has been saved", taskModel.TheTask.Title);

                return Redirect(taskModel.ReturnUrl);
            }
            else
            {
                return View(taskModel);
            }
        }

        public ViewResult Create(string returnUrl)
        {
            return View("Edit", new ToDoTaskModel
                {
                    ReturnUrl = returnUrl,
                    TheTask = new ToDoTask
                        {
                            DueTime = DateTime.Now,
                            TaskPriority = ToDoTask.Priority.Normal,
                            IsComplete = false,
                            OwnerId = User.Identity.GetUserId()
                        }
                });
        }

        [HttpPost]
        public ActionResult Delete(int taskId, string returnUrl)
        {
            using (var db = new TaskManagerContext())
            {
                var entry = db.ToDoTasks.FirstOrDefault(t => t.Id == taskId);

                if (entry != null)
                {
                    db.ToDoTasks.Remove(entry);
                    db.SaveChanges();

                    TempData["message"] = string.Format(
                        "'{0}' has been successfully deleted", entry.Title);
                }
            }
            
            return Redirect(returnUrl);
        }

        [HttpPost]
        public ActionResult MarkAsComplete(int taskId, string returnUrl)
        {
            using (var db = new TaskManagerContext())
            {
                var entry = db.ToDoTasks.FirstOrDefault(t => t.Id == taskId);

                if (entry != null)
                {
                    entry.IsComplete = true;
                    db.SaveChanges();

                    TempData["message"] = string.Format(
                        "'{0}' has been marked as complete", entry.Title);
                }
            }
            
            return Redirect(returnUrl);
        }
    }
}