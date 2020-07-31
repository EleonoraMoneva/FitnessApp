using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace FitnessApp.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            IdentityResult roleResult;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("Trainer");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Trainer"));
            }
            roleCheck = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("User"));
            }

            AppUser user = await UserManager.FindByEmailAsync("admin@fitness.com");
            if (user == null)
            {
                var User = new AppUser();
                User.Email = "admin@fitness.com";
                User.UserName = "admin@fitness.com";
                User.Role = "Admin";
                string userPWD = "Admin123.";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);

                
                if (chkUser.Succeeded)
                {
                    var result1 = await UserManager.AddToRoleAsync(User, "Admin");
                }

            }
        }



        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FitnessAppContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<FitnessAppContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                // Look for any movies.
                if (context.DietPlan.Any() || context.Programme.Any() || context.Recipe.Any())
                {
                    return;   // DB has been seeded
                }

                context.DietPlan.AddRange(
                    new DietPlan
                    {/*Id = 1*/
                        Breakfast = "2 Boiled  Eggs/2 Oranges/Ceaser Salad/Milk with oats",
                        Lunch = "Beef meat/Pork meat/Chicken breasts",
                        Dinner = "2 Oranges/2 Apples/1 Banana",
                        Snacks = "Nuts/Smoothie",
                        

                    },
                    new DietPlan
                    {/*Id = 2*/
                        Breakfast = "2 Boiled  Eggs/2 Oranges/Ceaser Salad/Milk with oats",
                        Lunch = "Beef meat/Pork meat/Chicken breasts",
                        Dinner = "2 Oranges/2 Apples/1 Banana",
                        Snacks = "Nuts/Smoothie",
                    },
                    new DietPlan
                    {/*Id = 3*/
                        Breakfast = "2 Boiled  Eggs/2 Oranges/Ceaser Salad/Milk with oats",
                        Lunch = "Beef meat/Pork meat/Chicken breasts",
                        Dinner = "2 Oranges/2 Apples/1 Banana",
                        Snacks = "Nuts/Smoothie",
                    }
                );
                context.SaveChanges();
                context.Programme.AddRange(
                    new Programme {/*Id = 3*/ Name = "PROGRAMA 1", Equipment = "NO EQUIPMENT", Duration = 50, Description = "For begginers....", Price = 150 },
                    new Programme {/*Id = 2*/ Name = "PROGRAMA 2", Equipment = "WEIGHTS", Duration = 21, Description = "gaining muscules..", Price = 500 },
                    new Programme {/*Id = 1*/ Name = "PROGRAMA 3", Equipment = "STRENGTH EQUIPMENT", Duration = 30, Description = "Getting more energy", Price = 100 }

                    );
                context.SaveChanges();
                context.Recipe.AddRange(
                    new Recipe {/*Id = 1*/ Name = "Boiled eggs salad", Ingredients = "2 eggs,100g spinach,50g chicken breast", Directions=" First you boil the eggs,boil the chicken breast then mix " },
                    new Recipe {/*Id = 2*/ Name = "Baked beef", Ingredients = "250g Beef meat,1 Orange,Little oil", Directions = " Put in oven 250C" },
                    new Recipe {/*Id = 3*/Name = "Green smoothie", Ingredients = "150g Spinach,1 Banana, 1 Avocado", Directions = " Blend everything" }
                    );
                context.SaveChanges();
            }


        }

    }
}
