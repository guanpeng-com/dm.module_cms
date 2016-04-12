using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.CMS.Configuration;

namespace Abp.Organizations
{
    /// <summary>
    /// Implements <see cref="IOrganizationUnitSettings"/> to get settings from <see cref="ISettingManager"/>.
    /// </summary>
    public class OrganizationUnitSettings : IOrganizationUnitSettings, ITransientDependency
    {
        /// <summary>
        /// Maximum allowed organization unit membership count for a user.
        /// Returns value for current tenant.
        /// </summary>
        public int MaxUserMembershipCount
        {
            get
            {
                return _settingManager.GetSettingValue<int>(AbpCMSSettingNames.OrganizationUnits.MaxUserMembershipCount);
            }
        }

        private readonly ISettingManager _settingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUnitSettings"/> class.
        /// </summary>
        public OrganizationUnitSettings(ISettingManager settingManager)
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
                return await _settingManager.GetSettingValueForTenantAsync<int>(AbpCMSSettingNames.OrganizationUnits.MaxUserMembershipCount, tenantId.Value);
            }
            else
            {
                return await _settingManager.GetSettingValueForApplicationAsync<int>(AbpCMSSettingNames.OrganizationUnits.MaxUserMembershipCount);
            }
        }

        public async Task SetMaxUserMembershipCountAsync(int? tenantId, int value)
        {
            if (tenantId.HasValue)
            {
                await _settingManager.ChangeSettingForTenantAsync(tenantId.Value, AbpCMSSettingNames.OrganizationUnits.MaxUserMembershipCount, value.ToString());
            }
            else
            {
                await _settingManager.ChangeSettingForApplicationAsync(AbpCMSSettingNames.OrganizationUnits.MaxUserMembershipCount, value.ToString());
            }
        }
    }
}