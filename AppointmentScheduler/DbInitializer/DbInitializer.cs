using AppointmentScheduler.Models;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                //check if there are migrations that have note been push to the DB and push them
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            //Check if there is a Role called Admin in Roles table, if not create all Role
            if (_db.Roles.Any(x => x.Name == Utility.Helper.Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Doctor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Patient)).GetAwaiter().GetResult();

            //Create a default admin user
            _userManager.CreateAsync( new ApplicationUser
            {
                UserName="jeff.ict@gmail.com",
                Email = "jeff.ict@gmail.com",
                Name = "Jeff O",
                EmailConfirmed = true,
            }, "Admin123$" ).GetAwaiter().GetResult();

            //Assign default user admin role
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "jeff.ict@gmail.com");
            _userManager.AddToRoleAsync(user, Helper.Admin).GetAwaiter().GetResult();

        }
    }
}
