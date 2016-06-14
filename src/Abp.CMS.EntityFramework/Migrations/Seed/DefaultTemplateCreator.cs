using Abp.Templates;
using Abp.Channels;
using System.Linq;
using Abp.Apps;
using Abp.DMUsers;
using Abp.CMS.SampleApp.MultiTenancy;
using Abp.CMS.SampleApp.Authorization.Roles;
using Abp.CMS.SampleApp.Authorization.Users;
using Abp.CMS.SampleApp.DMUsers;

namespace Abp.CMS.EntityFramework.Migrations.Seed
{
    public class DefaultTemplateCreator
    {
        private readonly AbpCMSDbContext<Tenant, Role, User, DMUser> _context;

        public DefaultTemplateCreator(AbpCMSDbContext<Tenant, Role, User, DMUser> context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateTemplates();
        }

        private void CreateTemplates()
        {
            var defaultApp = _context.Apps.FirstOrDefault(a => a.AppName == App.DefaultName);
            var defaultTemplate = _context.Templates.FirstOrDefault(e => e.Name == Template.IndexDefaultName);
            if (defaultTemplate == null)
            {
                defaultTemplate = new Template();
                defaultTemplate.InitDefaultIndexTemplate(defaultApp.Id);
                _context.Templates.Add(defaultTemplate);

                var defaultChannelTemplate = new Template();
                defaultChannelTemplate.InitDefaultChannelTemplate(defaultApp.Id);
                _context.Templates.Add(defaultChannelTemplate);

                var defaultContentTemplate = new Template();
                defaultContentTemplate.InitDefaultContentTemplate(defaultApp.Id);
                _context.Templates.Add(defaultContentTemplate);

                _context.SaveChanges();

                //TODO: Add desired features to the standard Channel, if wanted!
            }
        }
    }
}