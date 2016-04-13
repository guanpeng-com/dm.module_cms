namespace Abp.Channels
{
    /// <summary>
    /// This interface is implemented entities those must have an <see cref="Channel"/>.
    /// </summary>
    public interface IMustHaveChannel
    {
        /// <summary>
        /// <see cref="Channel"/>'s Id which this entity belongs to.
        /// </summary>
        long ChannelId { get; set; }
    }
}