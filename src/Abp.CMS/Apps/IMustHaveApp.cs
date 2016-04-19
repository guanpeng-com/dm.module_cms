using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Apps
{
    /// <summary>
    ///  实体对象实现此接口，表示实体对象属于某个app
    ///  <see cref="App"/>
    /// </summary>
    public interface IMustHaveApp
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        long AppId { get; set; }
    }
}
