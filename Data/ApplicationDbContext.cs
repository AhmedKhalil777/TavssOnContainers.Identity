using System;
using System.Collections.Generic;
using System.Text;
using Identity.Api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new { Id=  "1", Name = "Admin", NormalizedName = "ADMIN"},
                new { Id = "2", Name = "Student", NormalizedName = "STUDENT" },
                new { Id = "3", Name = "TeachingAssistant", NormalizedName = "TEACHINGASSISTANT" },
                new { Id = "4", Name = "Doctor", NormalizedName = "DOCTOR" },
                new { Id = "5", Name = "Developer", NormalizedName = "DEVELOPER" },
                new { Id = "6", Name = "Instructor", NormalizedName = "INSTRUCTOR" },
                new { Id = "7", Name = "Company", NormalizedName = "COMPANY" },
                new { Id = "8", Name = "User", NormalizedName = "USER" },
                new { Id = "9", Name = "Supervisor", NormalizedName = "SUPERVISOR" }
                );

           
        }




    }
}
