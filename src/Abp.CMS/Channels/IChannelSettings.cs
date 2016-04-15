using System.Threading.Tasks;

namespace Abp.Channels
{
    /// <summary>
    /// Used to get settings related to Channels.
    /// </summary>
    public interface IChannelSettings
    {
        #region ��Ŀ����ҳ���·����ʽ
        /// <summary>
        /// ��Ŀ����ҳ���·����ʽ
        /// </summary>
        int MaxContentCount { get; }

        /// <summary>
        /// ��ȡ ��Ŀ����ҳ���·����ʽ
        /// </summary>
        /// <param name="publishmentSystemId">վ��ID</param>
        Task<int> GetMaxContentCountAsync(int publishmentSystemId);

        /// <summary>
        /// ���� ��Ŀ����ҳ���·����ʽ
        /// </summary>
        /// <param name="publishmentSystemId">վ��ID</param>
        /// <param name="value">����ֵ</param>
        /// <returns></returns>
        Task SetMaxContentCountAsync(int publishmentSystemId, int value); 
        #endregion
    }
}