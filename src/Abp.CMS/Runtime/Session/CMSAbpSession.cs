using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Runtime.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.Runtime.Session
{
    /// <summary>
    ///  cms abp session
    /// </summary>
    public class CMSAbpSession : ClaimsAbpSession, ICMSAbpSession, ISingletonDependency
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="multiTenancy"></param>
        public CMSAbpSession(IMultiTenancyConfig multiTenancy)
            : base(multiTenancy)
        {

        }

        /// <summary>
        /// 应用ID
        /// </summary>
        public virtual long? AppId
        {
            get
            {
                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    return null;
                }

                var appIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == CMSAbpClaimTypes.AppId);
                if (appIdClaim == null || string.IsNullOrEmpty(appIdClaim.Value))
                {
                    return null;
                }

                return Convert.ToInt32(appIdClaim.Value);
            }
        }
    }
}
