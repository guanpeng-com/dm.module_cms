using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using Abp.CMS;
using Abp.Apps;
using Abp.Core.Utils;

namespace Abp.Channels
{
    /// <summary>
    /// 领域逻辑：栏目
    /// </summary>
    public class ChannelManager : DomainService
    {
        private readonly AppManager _appManager;

        /// <summary>
        /// 栏目仓储
        /// </summary>
        public IRepository<Channel, long> ChannelRepository { get; private set; }

        /// <summary>
        /// 默认栏目名称
        /// </summary>
        public const string DefaultChannelName = "首页";

        /// <summary>
        ///  默认栏目父级ID
        /// </summary>
        public const int DefaultParentId = 0;

        public const string ImageFolder = "Upload/Images";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="channelRepository"></param>
        /// <param name="appManager"></param>
        public ChannelManager(
            IRepository<Channel, long> channelRepository,
            AppManager appManager)
        {
            ChannelRepository = channelRepository;
            LocalizationSourceName = AbpCMSConsts.LocalizationSourceName;
            _appManager = appManager;
        }

        /// <summary>
        /// 创建栏目
        /// </summary>
        /// <param name="Channel"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task CreateAsync(Channel Channel)
        {
            var app = _appManager.GetById(Channel.AppId);
            Channel.Code = await GetNextChildCodeAsync(Channel.ParentId);
            Channel.TenantId = app.TenantId;
            await ValidateChannelAsync(Channel);
            await ChannelRepository.InsertAsync(Channel);
        }

        /// <summary>
        /// 创建默认的栏目
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <returns></returns>
        public virtual async Task CreateDefaultChannel(long appId)
        {
            var app = _appManager.GetById(appId);
            Channel channel = new Channel();
            channel.Code = await GetNextChildCodeAsync(null);
            channel.AppId = appId;
            channel.DisplayName = DefaultChannelName;
            channel.ParentId = null;
            channel.TenantId = app.TenantId;
            await ChannelRepository.InsertAsync(channel);
        }

        /// <summary>
        /// 更新栏目
        /// </summary>
        /// <param name="Channel"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Channel Channel)
        {
            await ValidateChannelAsync(Channel);
            await ChannelRepository.UpdateAsync(Channel);
        }

        /// <summary>
        /// 获取下一级子栏目的编码
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取子集的最后一个栏目
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public virtual async Task<Channel> GetLastChildOrNullAsync(long? parentId)
        {
            var children = await ChannelRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        /// <summary>
        /// 获取栏目编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual string GetCode(long id)
        {
            //TODO: Move to an extension class
            return AsyncHelper.RunSync(() => GetCodeAsync(id));
        }

        /// <summary>
        /// 获取栏目编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<string> GetCodeAsync(long id)
        {
            return (await ChannelRepository.GetAsync(id)).Code;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteAsync(long id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await ChannelRepository.DeleteAsync(child);
            }

            await ChannelRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 移动栏目
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task MoveAsync(long id, long? parentId)
        {
            var Channel = await ChannelRepository.GetAsync(id);
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

        /// <summary>
        /// 获取栏目子集
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public async Task<List<Channel>> FindChildrenAsync(long? parentId, bool recursive = false)
        {
            if (recursive)
            {
                if (!parentId.HasValue)
                {
                    return await ChannelRepository.GetAllListAsync();
                }

                var code = await GetCodeAsync(parentId.Value);
                return await ChannelRepository.GetAllListAsync(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value);
            }
            else
            {
                return await ChannelRepository.GetAllListAsync(ou => ou.ParentId == parentId);
            }
        }

        /// <summary>
        /// 获取栏目子集
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public async Task<Channel> FindDefaultAsync()
        {
            return await ChannelRepository.FirstOrDefaultAsync(c => c.DisplayName == DefaultChannelName);
        }

        /// <summary>
        /// 校验栏目
        /// </summary>
        /// <param name="Channel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据图片的文件名获取栏目的ImageUrl，不包含AppDir
        /// </summary>
        /// <param name="fileName"></param>
        public virtual string GetImageUrl(string fileName)
        {
            var filePath = PageUtils.Combine(ImageFolder, fileName);
            return filePath;
        }

        /// <summary>
        /// 根据图片的文件名获取栏目的ImageUrl，包含AppDir
        /// </summary>
        /// <param name="fileName"></param>
        public virtual string GetImageUrlWithAppDir(App app, string fileName)
        {
            if (app == null)
                return string.Empty;
            var filePath = PageUtils.Combine(ImageFolder, fileName);
            filePath = PageUtils.GetUrlWithAppDir(app, filePath);
            return filePath;
        }
    }
}