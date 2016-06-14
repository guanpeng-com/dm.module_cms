using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.CMS;
using System;
using Abp.Channels;
using Abp.Core.Utils;
using Abp.Apps;

namespace Abp.Contents
{
    /// <summary>
    /// 领域逻辑：内容
    /// </summary>
    public class GoodManager : DomainService
    {
        /// <summary>
        /// 内容仓储
        /// </summary>
        public IRepository<Good, long> GoodRepository { get; private set; }

        /// <summary>
        /// 栏目仓储
        /// </summary>
        protected ChannelManager ChannelManager { get; private set; }

        private readonly AppManager _appManager;

        public const string ImageFolder = "Upload/Images";
        public const string VideoFolder = "Upload/Videos";
        public const string FileFolder = "Upload/Files";


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="goodRepository"></param>
        /// <param name="channelManager"></param>
        public GoodManager(
            IRepository<Good, long> goodRepository,
            ChannelManager channelManager,
            AppManager appManager)
        {
            GoodRepository = goodRepository;
            ChannelManager = channelManager;
            LocalizationSourceName = AbpCMSConsts.LocalizationSourceName;
            _appManager = appManager;
        }

        /// <summary>
        /// 创建内容
        /// </summary>
        /// <param name="Good"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task CreateAsync(Good Good)
        {
            await ValidateGoodAsync(Good);
            await GoodRepository.InsertAsync(Good);
        }

        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="Good"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Good Good)
        {
            await ValidateGoodAsync(Good);
            await GoodRepository.UpdateAsync(Good);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteAsync(long id)
        {
            await GoodRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 移动内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task MoveAsync(long id, long channelId)
        {
            var Good = await GoodRepository.GetAsync(id);
            if (Good.ChannelId == channelId)
            {
                return;
            }

            Good.ChannelId = channelId;

            await ValidateGoodAsync(Good);
        }

        /// <summary>
        /// 获取栏目下的内容
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public async Task<List<Good>> FindGoodsByChannelIdAsync(long? channelId, bool recursive = false)
        {
            if (recursive)
            {
                if (!channelId.HasValue)
                {
                    channelId = (await ChannelManager.FindDefaultAsync()).Id;
                }

                //获取栏目子集
                var channels = await ChannelManager.FindChildrenAsync(channelId.Value, recursive);
                //添加自己
                channels.Insert(0, await ChannelManager.ChannelRepository.GetAsync(channelId.Value));

                if (channels != null)
                {
                    var query = from c in GoodRepository.GetAll()
                                join ch in channels on c.ChannelId equals ch.Id
                                where ch.Id == channelId.Value
                                select c;
                    return await Task.FromResult(query.ToList<Good>());
                }

                return await Task.FromResult(new List<Good>());
            }
            else
            {
                //单独栏目下的内容
                var query = from c in GoodRepository.GetAll()
                            join ch in ChannelManager.ChannelRepository.GetAll() on c.ChannelId equals ch.Id
                            where ch.Id == channelId.Value
                            select c;
                return await Task.FromResult(query.ToList<Good>());
            }
        }

        protected virtual async Task ValidateGoodAsync(Good good)
        {

        }

        #region ImageUrl
        /// <summary>
        /// 根据图片的文件名获取内容的ImageUrl，不包含AppDir
        /// </summary>
        /// <param name="fileName"></param>
        public virtual string GetImageUrl(string fileName)
        {
            var filePath = PageUtils.Combine(ImageFolder, fileName);
            return filePath;
        }

        /// <summary>
        /// 根据图片的文件名获取内容的ImageUrl，包含AppDir
        /// </summary>
        /// <param name="app"></param>
        /// <param name="fileName"></param>
        public virtual string GetImageUrlWithAppDir(App app, string fileName)
        {
            if (app == null)
                return string.Empty;
            var filePath = PageUtils.Combine(ImageFolder, fileName);
            filePath = PageUtils.GetUrlWithAppDir(app, filePath);
            return filePath;
        }
        #endregion

        #region VideoUrl
        /// <summary>
        /// 根据文件名获取内容的VideoUrl，不包含AppDir
        /// </summary>
        /// <param name="fileName"></param>
        public virtual string GetVideoUrl(string fileName)
        {
            var filePath = PageUtils.Combine(VideoFolder, fileName);
            return filePath;
        }

        /// <summary>
        /// 根据文件名获取内容的VideoUrl，包含AppDir
        /// </summary>
        /// <param name="app"></param>
        /// <param name="fileName"></param>
        public virtual string GetVideoUrlWithAppDir(App app, string fileName)
        {
            if (app == null)
                return string.Empty;
            var filePath = PageUtils.Combine(VideoFolder, fileName);
            filePath = PageUtils.GetUrlWithAppDir(app, filePath);
            return filePath;
        }
        #endregion

        #region FileUrl
        /// <summary>
        /// 根据文件名获取内容的FileUrl，不包含AppDir
        /// </summary>
        /// <param name="fileName"></param>
        public virtual string GetFileUrl(string fileName)
        {
            var filePath = PageUtils.Combine(FileFolder, fileName);
            return filePath;
        }

        /// <summary>
        /// 根据文件名获取内容的FileUrl，包含AppDir
        /// </summary>
        /// <param name="app"></param>
        /// <param name="fileName"></param>
        public virtual string GetFileUrlWithAppDir(App app, string fileName)
        {
            if (app == null)
                return string.Empty;
            var filePath = PageUtils.Combine(FileFolder, fileName);
            filePath = PageUtils.GetUrlWithAppDir(app, filePath);
            return filePath;
        }
        #endregion
    }
}