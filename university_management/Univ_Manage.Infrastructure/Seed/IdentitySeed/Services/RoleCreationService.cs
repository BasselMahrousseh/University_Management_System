using EMS.SqlServer.Seed.IdentitySeed.Interfaces;
using Univ_Manage.SharedKernal.Enums;

namespace Univ_Manage.SqlServer.Seed.IdentitySeed.Services
{
    public class RoleCreationService : IRoleCreationService
    {
        private readonly RoleManager<RoleSet> RoleManager;

        public RoleCreationService(RoleManager<RoleSet> roleManager)
        {
            RoleManager = roleManager;
        }

        public async Task CreateRolesAsync()
        {
            var Roles = Enum.GetNames(typeof(RoleEnum));
            foreach (var role in Roles)
            {
                var Role = await RoleManager.FindByNameAsync(role);
                if (Role is null)
                {
                    await RoleManager.CreateAsync(new RoleSet()
                    {
                        Name = role
                    });
                }
            }
        }
    }
}