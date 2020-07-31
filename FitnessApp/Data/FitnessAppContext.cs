using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Models;

namespace FitnessApp.Models
{
    public class FitnessAppContext : IdentityDbContext<AppUser>
    {
        public FitnessAppContext (DbContextOptions<FitnessAppContext> options)
            : base(options)
        {
        }

        public DbSet<FitnessApp.Models.User> User { get; set; }

        public DbSet<FitnessApp.Models.Trainer> Trainer { get; set; }

        public DbSet<FitnessApp.Models.Recipe> Recipe { get; set; }

        public DbSet<FitnessApp.Models.Programme> Programme { get; set; }

        public DbSet<FitnessApp.Models.Exrecise> Exrecise { get; set; }

        public DbSet<FitnessApp.Models.Enrollment> Enrollment { get; set; }

        public DbSet<FitnessApp.Models.DietPlan> DietPlan { get; set; }

     

    }
}
