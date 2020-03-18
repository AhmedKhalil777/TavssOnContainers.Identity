using Identity.Api.Data;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using Identity.Api.Domain;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Identity.Api.Contracts.V1.Responses;
using Identity.Api.Services;
using Microsoft.AspNetCore.Identity;
using System;

namespace Identity.Api.DataSeeding
{
    public static class DataSeeder
    {

        public static bool AllMigrationsApplied(this ApplicationDbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }
        public  static void SeedUsers(this UserManager<ApplicationUser> context)
        {
           
            using (var reader = new StreamReader("conseeder.json"))
                if (!context.Users.Any())
                {
                    var AUsers = new List<ApplicationUser>();
                    var users = JsonConvert.DeserializeObject<List<UserViewModel>>(reader.ReadToEnd());

                    foreach (var user in users)
                    {
                        var s = new ApplicationUser()
                        {

                            PicPath = user.PicPath,
                            Email =user.Email,
                            Department = user.Department,
                            StudyYear = user.StudyYear,
                            UserName =user.Username,
                            SecurityStamp = Guid.NewGuid().ToString()
                        };
                        context.CreateAsync(s, s.UserName + "@123").Wait();
                        context.AddToRolesAsync(s, user.Roles).Wait();
                    }
                    
                }

                    //using (var reader = new StreamReader("seedUsers.json"))

                    //if (!context.Users.Any())
                    //{
                    //    var types = JsonConvert.DeserializeObject<List<ApplicationUser>>(reader.ReadToEnd());
                    //    context.AddRange(types);
                    //    context.SaveChanges();
                    //}

                    //using (var reader = new StreamReader("seedRoles.json"))
                    //if (!context.Users.Any())
                    //{
                    //    var types = JsonConvert.DeserializeObject<List<ApplicationUser>>(reader.ReadToEnd());
                    //    context.AddRange(types);
                    //    context.SaveChanges();
                    //}

        }
    }
}
