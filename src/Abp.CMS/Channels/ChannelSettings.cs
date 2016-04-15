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
        ///  ��Ŀ�����������������
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
        /// ��ȡ��Ŀ�����������������
        /// </summary>
        /// <param name="publishmentSystemId">վ��ID</param>
        /// <returns></returns>
        public Task<int> GetMaxContentCountAsync(int publishmentSystemId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ������Ŀ�����������������
        /// </summary>
        /// <param name="publishmentSystemId">վ��ID</param>
        /// <param name="value">����ֵ</param>
        /// <returns></returns>
        public Task SetMaxContentCountAsync(int publishmentSystemId, int value)
        {
            throw new NotImplementedException();
        }
    }
}