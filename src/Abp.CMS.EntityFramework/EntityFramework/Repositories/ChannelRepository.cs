using Abp.Channels;
using Abp.CMS.EntityFramework;
using Abp.Domain.Repositories;

namespace Abp.EntityFramework.Repositories
{
    public interface IChannelRepository : IRepository<Channel, long>
    {

    }

    public class ChannelRepository : AbpCMSRepositoryBase<Channel, long>, IChannelRepository
    {
        public ChannelRepository(IDbContextProvider<AbpCMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        { }
    }
}
