using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Net;

namespace KaracadanWebApp.Data
{
    public class Seed
    {
        public Seed() { }
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(new List<Category>()
                    {
                         new Category()
                         {
                             Name = "Electronies",
                         },

                         new Category()
                         {
                             Name = "Fashion",
                         },
                         new Category()
                         {
                              Name = "Books",
                         }
                    });
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>()
                    {
                         new Product()
                         {
                             Name = "Samsung Telefon",
                             Description="black color, 16GB ",
                             Image ="https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                             Price =600,
                             ProductsStatus=ProductsStatus.stock,
                             CategoryId =1

                         },
                         new Product()
                         {
                             Name = "leather Jacket",
                             Description="blue color ",
                             Image ="https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                             Price =400,
                             ProductsStatus=ProductsStatus.stock,
                             CategoryId =2
                         },
                         new Product()
                         {
                             Name = "storirs for kids",
                             Description="have four story in and too many helpful things ",
                             Image ="https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                             Price =150,
                             ProductsStatus=ProductsStatus.Available,
                             CategoryId =3
                         }
                    });
                    context.SaveChanges();
                }

                if (!context.Orders.Any())
                {
                    context.Orders.AddRange(new List<Order>()
                    {
                             new Order()
                             {
                                 No = 1789,
                                 Date=DateTime.Now,
                                 OrderStatus=OrderStatus.Shipping,
                                 StatusId=(int) OrderStatus.Shipping,
                                 ApplicationUserId = "4c9a6734-df6b-4171-ad53-84abf63dc91b"

                             },

                            new Order()
                            {
                                 No = 1212,
                                 Date=DateTime.Now,
                                 OrderStatus=OrderStatus.Submitted,
                                 StatusId=(int)OrderStatus.Submitted,
                                 ApplicationUserId = "4c9a6734-df6b-4171-ad53-84abf63dc91b"

                            },
                            new Order()
                            {
                                 No = 1345,
                                 Date=DateTime.Now,
                                 OrderStatus=OrderStatus.Delivered,
                                 StatusId=(int) OrderStatus.Delivered,
                                 ApplicationUserId ="4c9a6734-df6b-4171-ad53-84abf63dc91b"

                            }
                    });
                    context.SaveChanges();
                }

                if (!context.OrderDetails.Any())
                {
                    context.OrderDetails.AddRange(new List<OrderDetail>()
                    {
                         new OrderDetail()
                         {
                             Quantity = 2,
                             PriceProductOrderDetail=1200,
                             ProductId=1,
                             OrderId=5
                         },
                         new OrderDetail()
                         {
                            Quantity = 3,
                            PriceProductOrderDetail=750 ,
                            ProductId=2,
                            OrderId=6
                         },

                          new OrderDetail()
                         {
                             Quantity = 4,
                             PriceProductOrderDetail=1000 ,
                             ProductId=3,
                             OrderId=7

                         }

                    });
                    context.SaveChanges();
                }


            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()

                    {
                        Email = adminUserEmail,
                        UserName = "AdminNameHusam",         
                        EmailConfirmed = true ,
                        NormalizedEmail = "AdminNameHusam"
                    };

                     await userManager.CreateAsync(newAdminUser, "Admin@1234");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        
                        Email = appUserEmail,
                        UserName = "UserNameHusam",
                        EmailConfirmed = true,
                        NormalizedEmail = "AdminNameHusam"


                    };
                     await userManager.CreateAsync(newAppUser, "User@1234");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }

                string appManagerEmail = "manager@gmail.com";

                var appManager = await userManager.FindByEmailAsync(appManagerEmail);
                if (appManager == null)
                {
                    var newMangerUser = new ApplicationUser()
                    {      
                        Email =appManagerEmail,
                        UserName = "ManagerNameHusam",
                        EmailConfirmed = true,
                        NormalizedEmail = "AdminNameHusam"

                    };
                    await userManager.CreateAsync(newMangerUser, "Manager@1234");
                    await userManager.AddToRoleAsync(newMangerUser, UserRoles.Manager);
                }

            }




        }
    }
}


