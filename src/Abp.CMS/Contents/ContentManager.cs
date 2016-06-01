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
    public class ContentManager : DomainService
    {
        /// <summary>
        /// 内容仓储
        /// </summary>
        public IRepository<Content, long> ContentRepository { get; private set; }

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
        /// <param name="contentRepository"></param>
        /// <param name="channelManager"></param>
        public ContentManager(
            IRepository<Content, long> contentRepository,
            ChannelManager channelManager,
            AppManager appManager)
        {
            ContentRepository = contentRepository;
            ChannelManager = channelManager;
            LocalizationSourceName = AbpCMSConsts.LocalizationSourceName;
            _appManager = appManager;
        }

        /// <summary>
        /// 创建内容
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task CreateAsync(Content Content)
        {
            await ValidateContentAsync(Content);
            await ContentRepository.InsertAsync(Content);
        }

        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Content Content)
        {
            await ValidateContentAsync(Content);
            await ContentRepository.UpdateAsync(Content);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteAsync(long id)
        {
            await ContentRepository.DeleteAsync(id);
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
            var Content = await ContentRepository.GetAsync(id);
            if (Content.ChannelId == channelId)
            {
                return;
            }

            Content.ChannelId = channelId;

            await ValidateContentAsync(Content);
        }

        /// <summary>
        /// 获取栏目下的内容
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public async Task<List<Content>> FindContentsByChannelIdAsync(long? channelId, bool recursive = false)
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
                    var query = from c in ContentRepository.GetAll()
                                join ch in channels on c.ChannelId equals ch.Id
                                where ch.Id == channelId.Value
                                select c;
                    return await Task.FromResult(query.ToList<Content>());
                }

                return await Task.FromResult(new List<Content>());
            }
            else
            {
                //单独栏目下的内容
                var query = from c in ContentRepository.GetAll()
                            join ch in ChannelManager.ChannelRepository.GetAll() on c.ChannelId equals ch.Id
                            where ch.Id == channelId.Value
                            select c;
                return await Task.FromResult(query.ToList<Content>());
            }
        }

        protected virtual async Task ValidateContentAsync(Content content)
        {

        }

        #region ImageUrl
        /// <summary>
        /// 根据内容的ImageUrl获取图片显示地址的相对路径，包含AppDir
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public virtual string GetImageUrl(long contentId)
        {
            var content = ContentRepository.Get(contentId);
            var app = _appManager.AppRepository.Get(content.AppId);

            return GetImageUrl(app, content);
        }

        /// <summary>
        /// 根据内容的ImageUrl获取图片显示地址的相对路径，包含AppDir
        /// </summary>
        /// <param name="app"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual string GetImageUrl(App app, Content content)
        {
            return PageUtils.GetShowUrlByApp(app, content.ImageUrl);
        }

        /// <summary>
        /// 根据图片的文件名设置内容的ImageUrl，不包含AppDir
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public virtual void SetImageUrl(Content content, string fileName)
        {
            var app = _appManager.AppRepository.Get(content.AppId);
            var filePath = PageUtils.Combine(ImageFolder, fileName);
            content.ImageUrl = filePath;
        }
        #endregion

        #region VideoUrl
        /// <summary>
        /// 根据内容的VideoUrl获取图片显示地址的相对路径，包含AppDir
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public virtual string GetVideoUrl(long contentId)
        {
            var content = ContentRepository.Get(contentId);
            var app = _appManager.AppRepository.Get(content.AppId);

            return GetVideoUrl(app, content);
        }

        /// <summary>
        /// 根据内容的VideoUrl获取图片显示地址的相对路径，包含AppDir
        /// </summary>
        /// <param name="app"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual string GetVideoUrl(App app, Content content)
        {
            return PageUtils.GetShowUrlByApp(app, content.VideoUrl);
        }

        /// <summary>
        /// 根据图片的文件名设置内容的VideoUrl，不包含AppDir
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public virtual void SetVideoUrl(Content content, string fileName)
        {
            var app = _appManager.AppRepository.Get(content.AppId);
            var filePath = PageUtils.Combine(VideoFolder, fileName);
            content.VideoUrl = filePath;
        }
        #endregion

        #region FileUrl
        /// <summary>
        /// 根据内容的FileUrl获取图片显示地址的相对路径，包含AppDir
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public virtual string GetFileUrl(long contentId)
        {
            var content = ContentRepository.Get(contentId);
            var app = _appManager.AppRepository.Get(content.AppId);

            return GetFileUrl(app, content);
        }

        /// <summary>
        /// 根据内容的FileUrl获取图片显示地址的相对路径，包含AppDir
        /// </summary>
        /// <param name="app"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual string GetFileUrl(App app, Content content)
        {
            return PageUtils.GetShowUrlByApp(app, content.FileUrl);
        }

        /// <summary>
        /// 根据图片的文件名设置内容的FileUrl，不包含AppDir
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public virtual void SetFileUrl(Content content, string fileName)
        {
            var app = _appManager.AppRepository.Get(content.AppId);
            var filePath = PageUtils.Combine(FileFolder, fileName);
            content.FileUrl = filePath;
        }
        #endregion
    }
}