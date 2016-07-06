using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TaskManager.Domain;
using TaskManager.WebUI.Models;

namespace TaskManager.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            List<ToDoTask> model = null;

            using (var db = new TaskManagerContext())
            {
                string userId = User.Identity.GetUserId();

                var todayStart = DateTime.Today;
                var todayEnd = DateTime.Today.AddDays(1);

                var tasks = db.ToDoTasks.ToList<ToDoTask>();

                model = tasks.Where(
                    t => t.OwnerId.CompareTo(userId) == 0 && 
                         t.DueTime.Ticks > todayStart.Ticks && t.DueTime.Ticks < todayEnd.Ticks)
                    .OrderBy(t => t.DueTime)
                    .ToList<ToDoTask>();
            }

            return View(model);
        }

        [Authorize]
        [ChildActionOnly]
        public ActionResult GetDaySummary()
        {
            string userId = User.Identity.GetUserId();

            var todayStart = DateTime.Today;
            var todayEnd = DateTime.Today.AddDays(1);

            var model = new DaySummaryModel { UserName = User.Identity.Name };

            using (var db = new TaskManagerContext())
            {
                var tasks = db.ToDoTasks.ToList<ToDoTask>();

                model.CompleteTasksQuantity = tasks.Count(
                    t => t.OwnerId.CompareTo(userId) == 0 &&
                         t.DueTime.Ticks > todayStart.Ticks && t.DueTime.Ticks < todayEnd.Ticks &&
                         t.IsComplete);

                model.OverdueTasksQuantity  = tasks.Count(
                    t => t.OwnerId.CompareTo(userId) == 0 && 
                         t.DueTime.Ticks > todayStart.Ticks && t.DueTime.Ticks < DateTime.Now.Ticks &&
                         !t.IsComplete);

                model.PendingTasksQuantity  = tasks.Count(
                    t => t.OwnerId.CompareTo(userId) == 0 &&
                         t.DueTime.Ticks > DateTime.Now.Ticks && t.DueTime.Ticks < todayEnd.Ticks &&
                         !t.IsComplete);

                var m = db.ToDoTasks.Where(
                    t => t.OwnerId.CompareTo(userId) == 0 &&
                         DateTime.Now <= t.DueTime && t.DueTime < todayEnd &&
                         !t.IsComplete).ToList<ToDoTask>();
            }

            return PartialView("_DaySummaryPartial", model);
        }
	}
}