using NHibernate;

namespace Abp.CMS.SampleApp.NHibernate.TestDatas
{
    public class InitialTestDataBuilder
    {
        private readonly ISession _session;

        public InitialTestDataBuilder(ISession session)
        {
            _session = session;
        }

        public void Build()
        {
            //_session.DisableAllFilters(); //TODO: Needs?
            
            new InitialTestChannelsBuilder(_session).Build();
        }
    }
}