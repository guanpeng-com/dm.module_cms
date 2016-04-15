using System.Threading.Tasks;

namespace Abp.Channels
{
    /// <summary>
    /// Used to get settings related to Channels.
    /// </summary>
    public interface IChannelSettings
    {
        #region 栏目生成页面的路径格式
        /// <summary>
        /// 栏目生成页面的路径格式
        /// </summary>
        int MaxContentCount { get; }

        /// <summary>
        /// 获取 栏目生成页面的路径格式
        /// </summary>
        /// <param name="publishmentSystemId">站点ID</param>
        Task<int> GetMaxContentCountAsync(int publishmentSystemId);

        /// <summary>
        /// 设置 栏目生成页面的路径格式
        /// </summary>
        /// <param name="publishmentSystemId">站点ID</param>
        /// <param name="value">设置值</param>
        /// <returns></returns>
        Task SetMaxContentCountAsync(int publishmentSystemId, int value); 
        #endregion
    }
}