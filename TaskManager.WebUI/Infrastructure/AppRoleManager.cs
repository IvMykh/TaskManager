﻿using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TaskManager.Domain;

namespace TaskManager.WebUI.Infrastructure
{
    public class AppRoleManager
        : RoleManager<AppRole>,
          IDisposable
    {
        public AppRoleManager(RoleStore<AppRole> store)
            : base(store)
        {
        }

        public static AppRoleManager Create(
                IdentityFactoryOptions<AppRoleManager> options,
                IOwinContext context)
        {
            return new AppRoleManager(new
                RoleStore<AppRole>(context.Get<TaskManagerContext>()));
        }
    }
}