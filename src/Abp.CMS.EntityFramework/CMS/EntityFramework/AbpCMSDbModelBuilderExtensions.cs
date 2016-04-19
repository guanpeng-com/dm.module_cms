using Abp.Apps;
using Abp.Channels;
using System.Data.Entity;

namespace Abp.CMS.EntityFramework
{
    /// <summary>
    /// Extension methods for <see cref="DbModelBuilder"/>.
    /// </summary>
    public static class AbpCMSDbModelBuilderExtensions
    {
        /// <summary>
        /// Changes prefix for ABP tables (which is "Abp" by default).
        /// Can be null/empty string to clear the prefix.
        /// </summary>
        /// <param name="modelBuilder">Model builder.</param>
        /// <param name="prefix">Table prefix, or null to clear prefix.</param>
        public static void ChangeAbpTablePrefix(this DbModelBuilder modelBuilder, string prefix)
        {
            prefix = prefix ?? "";

            modelBuilder.Entity<Channel>().ToTable(prefix + "Channels");
            modelBuilder.Entity<App>().ToTable(prefix + "Apps");
        }
    }
}