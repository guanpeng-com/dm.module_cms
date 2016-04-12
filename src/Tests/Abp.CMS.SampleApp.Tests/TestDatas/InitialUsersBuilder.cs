using System.Linq;
using Abp.CMS.SampleApp.EntityFramework;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.CMS.SampleApp.Users;
using Microsoft.AspNet.Identity;

namespace Abp.CMS.SampleApp.Tests.TestDatas
{
    public class InitialUsersBuilder
    {
        private readonly AppDbContext _context;

        public InitialUsersBuilder(AppDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            CreateUsers();
        }

        private void CreateUsers()
        {
            var defaultTenant = _context.Tenants.Single(t => t.TenancyName == Tenant.DefaultTenantName);

            var admin = _context.Users.Add(
                new User
                {
                   TenantId = defaultTenant.Id,
                   Name = "System",
                   Surname = "Administrator",
                   UserName = User.AdminUserName,
                   Password = new PasswordHasher().HashPassword("123qwe"),
                   EmailAddress = "admin@aspnetboilerplate.com"
                });
        }
    }
}