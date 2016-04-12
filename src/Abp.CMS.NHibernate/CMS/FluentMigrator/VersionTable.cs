using FluentMigrator.VersionTableInfo;

namespace Abp.CMS.FluentMigrator
{
    [VersionTableMetaData]
    public class VersionTable : DefaultVersionTableMetaData
    {
        public override string TableName
        {
            get
            {
                return "AbpVersionInfo";
            }
        }
    }
}
