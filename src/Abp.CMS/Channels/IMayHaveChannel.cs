namespace Abp.Channels
{
    /// <summary>
    /// This interface is implemented entities those may have an <see cref="Channel"/>.
    /// </summary>
    public interface IMayHaveChannel
    {
        /// <summary>
        /// <see cref="Channel"/>'s Id which this entity belongs to.
        /// Can be null if this entity is not related to any <see cref="Channel"/>.
        /// </summary>
        long? ChannelId { get; set; }
    }
}