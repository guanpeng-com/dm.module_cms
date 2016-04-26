using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Runtime.Session
{
    /// <summary>
    ///  扩展方法<see cref="CMSAbpSession"/>
    /// </summary>
    public static class CMSAbpSessionExtensions
    {
        /// <summary>
        /// 获取应用ID
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static long GetAppId(this ICMSAbpSession session)
        {
            if (!session.AppId.HasValue)
            {
                throw new AbpException("Session.AppId is null! Probably, user is not logged in.");
            }

            return session.AppId.Value;
        }
    }
}
