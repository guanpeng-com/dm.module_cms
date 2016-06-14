using Abp.CMS.SampleApp.DMUsers;
using Abp.CMS.SampleApp.EntityFramework;
using Abp.DMUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.CMS.SampleApp.Tests.DMUsers
{
    public class DMUserLoginHelper : SampleAppTestBase
    {
        public static void CreateLoginUsers(AppDbContext context)
        {
            var defaultTenant = context.Tenants.FirstOrDefault(t => t.TenancyName == "Default");

            context.DMUsers.Add(new DMUser
            {
                UserName = "userOwner",
                Name = "Owner",
                Surname = "One",
                EmailAddress = "owner@aspnetboilerplate.com",
                IsEmailConfirmed = true,
                Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
            });

            context.DMUsers.Add(new DMUser
            {
                TenantId = defaultTenant.Id, //A user of tenant1
                UserName = "user1",
                Name = "User",
                Surname = "One",
                EmailAddress = "user-one@aspnetboilerplate.com",
                IsEmailConfirmed = false,
                Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
            });
        }
    }
}
