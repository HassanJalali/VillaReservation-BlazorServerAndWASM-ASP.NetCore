using DataAccess.Data;
using HiddenVilla_Server.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HiddenVilla_Server.Service
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_context.Roles.Any(x => x.Name == Common.StaticDetail.Role_Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(Common.StaticDetail.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Common.StaticDetail.Role_Customer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Common.StaticDetail.Role_Employee)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new IdentityUser
            {
                UserName = "misterDeveloper@yahoo.com",
                Email = "misterDeveloper@yahoo.com",
                EmailConfirmed = true
            }, "Hassan12@").GetAwaiter().GetResult();

            IdentityUser user = _context.Users.FirstOrDefault(u => u.Email == "misterDeveloper@yahoo.com");
            _userManager.AddToRoleAsync(user, Common.StaticDetail.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
