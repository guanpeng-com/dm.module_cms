using Abp.NHibernate.EntityMappings;
using Abp.Channels;

namespace Abp.CMS.NHibernate.EntityMappings
{
    public class ChannelMap : EntityMap<Channel, long>
    {
        public ChannelMap()
            : base("Channel")
        {
            Map(x => x.AppId);
            References(x => x.Parent).Column("ParentId").Nullable();
            //Map(x => x.ParentId);
            Map(x => x.Code);
            Map(x => x.DisplayName);

            this.MapFullAudited();
        }
    }
}