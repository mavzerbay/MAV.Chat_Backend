using MAV.Chat.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAV.Chat.Infrastructure.Context
{
    public class MavDbContextSeed
    {
        public static async Task SeedAsync(MavDbContext context, ILoggerFactory loggerFactory, RoleManager<MavRole> roleManager)
        {
            try
            {
                var roles = new List<MavRole>
                {
                    new MavRole{Name="Member"},
                    new MavRole{Name="Admin"},
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
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
    }
}
