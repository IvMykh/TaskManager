﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using TaskManager.Domain;

namespace TaskManager.WebUI
{
    public class TaskManagerContext
        : IdentityDbContext<AppUser>
    {
        public TaskManagerContext()
            : base("TaskManagerContext", throwIfV1Schema: false)
        {
        }

        public DbSet<ToDoTask> ToDoTasks { get; set; }

        public static TaskManagerContext Create()
        {
            return new TaskManagerContext();
        }
    }
}