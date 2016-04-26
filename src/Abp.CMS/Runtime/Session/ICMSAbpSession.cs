using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Runtime.Session
{
    public interface ICMSAbpSession : IAbpSession
    {
        /// <summary>
        /// Gets current AppId or null.
        /// </summary>
        long? AppId { get; }
    }
}
