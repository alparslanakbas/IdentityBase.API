namespace Identity.Api.Context.IdentitySeeds
{
    public static class UserSeed
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var roles = new[]
            {
                    new Role
                    {
                        Name = "Admin",
                        Description = "Sistem Yöneticisi",
                        IsActive = true,
                        IsDefault = false,
                        Level = 1
                    },

                    new Role
                    {
                        Name = "User",
                        Description = "Standart kullanıcı",
                        IsActive = true,
                        IsDefault = true,
                        Level = 3
                    }
                };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name!))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            var adminUser = new User
            {
                UserName = "Admin",
                Email = "alpa37374@gmail.com",
                IsActive = true,
                EmailConfirmed = true,
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) is null)
            {
                var result = await userManager.CreateAsync(adminUser, "A.lparslan123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
