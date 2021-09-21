using MAV.Chat.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAV.Chat.Infrastructure.Context
{
    public class MavDbContextSeed
    {
        public static async Task SeedAsync(MavDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {

                //if (!context.ProductBrands.Any())
                //{
                //    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                //    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                //    foreach (var item in brands)
                //    {
                //        context.ProductBrands.Add(item);
                //    }
                //    await context.SaveChangesAsync();
                //}

                //if (!context.ProductTypes.Any())
                //{
                //    var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                //    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                //    foreach (var item in types)
                //    {
                //        context.ProductTypes.Add(item);
                //    }
                //    await context.SaveChangesAsync();
                //}

                //if (!context.Products.Any())
                //{
                //    var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                //    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                //    foreach (var item in products)
                //    {
                //        context.Products.Add(item);
                //    }
                //    await context.SaveChangesAsync();
                //}

                //if (!context.DeliveryMethods.Any())
                //{
                //    var dmData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                //    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

                //    foreach (var item in methods)
                //    {
                //        context.DeliveryMethods.Add(item);
                //    }
                //    await context.SaveChangesAsync();
                //}
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<MavDbContextSeed>();
                logger.LogError(ex.Message);
            }
        }
        public static async Task SeedUsers(UserManager<MavUser> userManager, RoleManager<MavRole> roleManager)
        {
           if (await userManager.Users.AnyAsync()) return;


            string path = Directory.GetCurrentDirectory();
            var userData = await System.IO.File.ReadAllTextAsync("C:\\Users\\erbay\\source\\repos\\MAV.Chat\\MAV.Chat.Infrastructure\\Seed\\User.json");
            var users = JsonSerializer.Deserialize<List<MavUser>>(userData);
            if (users == null) return;

            var roles = new List<MavRole>
            {
                new MavRole{Name="Member"},
                new MavRole{Name="Admin"},
                new MavRole{Name="Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            WebClient webClient = new WebClient();
            foreach (var user in users)
            {

                user.ProfilePhoto = webClient.DownloadData(user.PhotoUrl);
                user.Email = user.UserName.ToLower();
                user.UserName = user.Email.Split("@")[0];
                user.CreatedDate = DateTime.Now;
                user.PhoneNumberConfirmed = true;
                user.EmailConfirmed = true;
                var result = await userManager.CreateAsync(user, "Password123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new MavUser
            {
                UserName = "mavzerbay",
                PhoneNumber = "(553) 283-0310",
                Email = "mavzerbay@gmail.com",
                CreatedDate = DateTime.Now,
                Name = "Erbay",
                Surname = "Mavzer",
                ProfilePhoto = webClient.DownloadData("https://avatars.githubusercontent.com/u/45784521?s=400&u=51c15d611a25065401b1097b48684d08f784fcb5&v=4"),
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(admin, "Password123*");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }
    }
}
