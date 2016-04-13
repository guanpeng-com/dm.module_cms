using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using Abp.CMS;

namespace Abp.Channels
{
    /// <summary>
    /// Performs domain logic for Organization Units.
    /// </summary>
    public class ChannelManager : DomainService
    {
        protected IRepository<Channel, long> channelRepository { get; private set; }

        public const string DefaultChannelName = "";
        public const int DefaultParentId = 0;

        public ChannelManager(IRepository<Channel, long> ChannelRepository)
        {
            channelRepository = ChannelRepository;

            LocalizationSourceName = AbpCMSConsts.LocalizationSourceName;
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(Channel Channel)
        {
            Channel.Code = await GetNextChildCodeAsync(Channel.ParentId);
            await ValidateChannelAsync(Channel);
            await channelRepository.InsertAsync(Channel);
        }

        public virtual async Task UpdateAsync(Channel Channel)
        {
            await ValidateChannelAsync(Channel);
            await channelRepository.UpdateAsync(Channel);
        }

        public virtual async Task<string> GetNextChildCodeAsync(long? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild == null)
            {
                var parentCode = parentId != null ? await GetCodeAsync(parentId.Value) : null;
                return Channel.AppendCode(parentCode, Channel.CreateCode(1));
            }

            return Channel.CalculateNextCode(lastChild.Code);
        }

        public virtual async Task<Channel> GetLastChildOrNullAsync(long? parentId)
        {
            var children = await channelRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        public virtual string GetCode(long id)
        {
            //TODO: Move to an extension class
            return AsyncHelper.RunSync(() => GetCodeAsync(id));
        }

        public virtual async Task<string> GetCodeAsync(long id)
        {
            return (await channelRepository.GetAsync(id)).Code;
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(long id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await channelRepository.DeleteAsync(child);
            }

            await channelRepository.DeleteAsync(id);
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(long id, long? parentId)
        {
            var Channel = await channelRepository.GetAsync(id);
            if (Channel.ParentId == parentId)
            {
                return;
            }

            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);

            //Store old code of OU
            var oldCode = Channel.Code;

            //Move OU
            Channel.Code = await GetNextChildCodeAsync(parentId);
            Channel.ParentId = parentId;

            await ValidateChannelAsync(Channel);

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = Channel.AppendCode(Channel.Code, Channel.GetRelativeCode(child.Code, oldCode));
            }
        }

        public async Task<List<Channel>> FindChildrenAsync(long? parentId, bool recursive = false)
        {
            if (recursive)
            {
                if (!parentId.HasValue)
                {
                    return await channelRepository.GetAllListAsync();
                }

                var code = await GetCodeAsync(parentId.Value);
                return await channelRepository.GetAllListAsync(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value);
            }
            else
            {
                return await channelRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            }
        }

        protected virtual async Task ValidateChannelAsync(Channel Channel)
        {
            var siblings = (await FindChildrenAsync(Channel.ParentId))
                .Where(ou => ou.Id != Channel.Id)
                .ToList();

            if (siblings.Any(ou => ou.DisplayName == Channel.DisplayName))
            {
                throw new UserFriendlyException(L("ChannelDuplicateDisplayNameWarning", Channel.DisplayName));
            }
        }
    }
}