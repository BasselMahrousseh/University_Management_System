using EMS.SqlServer.Seed.IdentitySeed.Interfaces;
using Microsoft.EntityFrameworkCore;
using Univ_Manage.Infrastructure.SqlServer;
using Univ_Manage.SharedKernal.Enums;

namespace Univ_Manage.SqlServer.Seed.IdentitySeed.Services
{
    public class UserCreationService : IUserCreationService
    {
        private readonly UserManager<UserSet> userManager;
        private readonly Univ_ManageDBContext context;


        public UserCreationService(UserManager<UserSet> userManager, Univ_ManageDBContext context)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task CreateUsersAsync()
        {
            var adminRoleEntity = await context.Roles.FirstOrDefaultAsync(e => e.Name == nameof(RoleEnum.Admin));

            var adminUser = new UserSet
            {
                UserName = "admin",
                Email = "admin@admin.com",
                FirstName = "Wajih",
                LastName = "Alsayed",
                PhoneNumber = "+963955555555",
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(adminUser, "Aaa@123");

            if (result.Succeeded)
            {
                //result = await userManager.AddToRoleAsync(user1, RoleEnum.Admin.ToString());
                var userRoleEntity = new UserRoleSet()
                {
                    RoleId = adminRoleEntity != null ? adminRoleEntity.Id : 0,
                    UserId = adminUser.Id
                };
                await context.UserRoles.AddAsync(userRoleEntity);
                await context.SaveChangesAsync();
            }
        }
    }
}