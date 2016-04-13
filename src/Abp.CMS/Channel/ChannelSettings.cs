using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.CMS.Configuration;

namespace Abp.Channels
{
    /// <summary>
    /// Implements <see cref="IChannelSettings"/> to get settings from <see cref="ISettingManager"/>.
    /// </summary>
    public class ChannelSettings : IChannelSettings, ITransientDependency
    {
        /// <summary>
        /// Maximum allowed organization unit membership count for a user.
        /// Returns value for current tenant.
        /// </summary>
        public int MaxUserMembershipCount
        {
            get
            {
                return _settingManager.GetSettingValue<int>(AbpCMSSettingNames.Channel.MaxUserMembershipCount);
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
        /// Maximum allowed organization unit membership count for a user.
        /// Returns value for given tenant.
        /// </summary>
        public async Task<int> GetMaxUserMembershipCountAsync(int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return await _settingManager.GetSettingValueForTenantAsync<int>(AbpCMSSettingNames.Channel.MaxUserMembershipCount, tenantId.Value);
            }
            else
            {
                return await _settingManager.GetSettingValueForApplicationAsync<int>(AbpCMSSettingNames.Channel.MaxUserMembershipCount);
            }
        }

        public async Task SetMaxUserMembershipCountAsync(int? tenantId, int value)
        {
            if (tenantId.HasValue)
            {
                await _settingManager.ChangeSettingForTenantAsync(tenantId.Value, AbpCMSSettingNames.Channel.MaxUserMembershipCount, value.ToString());
            }
            else
            {
                await _settingManager.ChangeSettingForApplicationAsync(AbpCMSSettingNames.Channel.MaxUserMembershipCount, value.ToString());
            }
        }
    }
}