using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Apps
{
    /// <summary>
    ///  实体对象实现此接口，表示实体对象属于某个app
    ///  可以为null，表示不属于任何app
    ///  <see cref="App"/>
    /// </summary>
    public interface IMayHaveApp
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        long? AppId { get; set; }
    }
}
