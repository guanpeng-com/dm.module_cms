using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.CMS.Configuration;
using System;

namespace Abp.Channels
{
    /// <summary>
    /// Implements <see cref="IChannelSettings"/> to get settings from <see cref="ISettingManager"/>.
    /// </summary>
    public class ChannelSettings : IChannelSettings, ITransientDependency
    {
        /// <summary>
        ///  栏目的允许最大内容数量
        /// </summary>
        public int MaxContentCount
        {
            get
            {
                return _settingManager.GetSettingValue<int>(AbpCMSSettingNames.Channel.MaxContentCount);
            }
        }

        private readonly ISettingManager _settingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelSettings"/> class.
        /// </summary>
        public ChannelSettings(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        /// <summary>
        /// 获取栏目的允许最大内容数量
        /// </summary>
        /// <param name="publishmentSystemId">站点ID</param>
        /// <returns></returns>
        public Task<int> GetMaxContentCountAsync(int publishmentSystemId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置栏目的允许最大内容数量
        /// </summary>
        /// <param name="publishmentSystemId">站点ID</param>
        /// <param name="value">设置值</param>
        /// <returns></returns>
        public Task SetMaxContentCountAsync(int publishmentSystemId, int value)
        {
            throw new NotImplementedException();
        }
    }
}