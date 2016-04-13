using Abp.NHibernate.EntityMappings;
using Abp.Channels;

namespace Abp.CMS.NHibernate.EntityMappings
{
    public class ChannelMap : EntityMap<Channels.Channel, long>
    {
        public ChannelMap()
            : base("Channel")
        {
            Map(x => x.TenantId);
            References(x => x.Parent).Column("ParentId").Nullable();
            //Map(x => x.ParentId);
            Map(x => x.Code);
            Map(x => x.DisplayName);
            
            this.MapFullAudited();
        }
    }
}