using System;
using System.Data.Common;
using Abp.Authorization;
using Abp.Collections;
using Abp.Modules;
using Abp.TestBase;
using Abp.CMS.SampleApp.EntityFramework;
using Castle.MicroKernel.Registration;
using EntityFramework.DynamicFilters;
using Abp.CMS.SampleApp.Tests.TestDatas;

namespace Abp.CMS.SampleApp.Tests
{
    public abstract class SampleAppTestBase : AbpIntegratedTestBase
    {
        protected SampleAppTestBase()
        {
            CreateInitialData();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            //Fake DbConnection using Effort!
            LocalIocManager.IocContainer.Register(
                Component.For<DbConnection>()
                    .UsingFactoryMethod(Effort.DbConnectionFactory.CreateTransient)
                    .LifestyleSingleton()
                );
        }

        private void CreateInitialData()
        {
            UsingDbContext(context => new InitialTestDataBuilder(context).Build());
        }

        protected override void AddModules(ITypeList<AbpModule> modules)
        {
            base.AddModules(modules);

            modules.Add<SampleAppEntityFrameworkModule>();
        }

        public void UsingDbContext(Action<AppDbContext> action)
        {
            using (var context = LocalIocManager.Resolve<AppDbContext>())
            {
                context.DisableAllFilters();
                action(context);
                context.SaveChanges();
            }
        }

        public T UsingDbContext<T>(Func<AppDbContext, T> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<AppDbContext>())
            {
                context.DisableAllFilters();
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}