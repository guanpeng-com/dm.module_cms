using Abp.Apps;
using Abp.CMS.EntityFramework;
using Abp.Domain.Repositories;

namespace Abp.EntityFramework.Repositories
{
    public interface IAppRepository : IRepository<App, long>
    {

    }

    public class AppRepository : AbpCMSRepositoryBase<App, long>, IAppRepository
    {
        public AppRepository(IDbContextProvider<AbpCMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }
    }
}
