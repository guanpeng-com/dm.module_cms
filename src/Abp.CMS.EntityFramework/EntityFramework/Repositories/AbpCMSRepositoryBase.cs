using Abp.CMS.EntityFramework;
using Abp.Domain.Entities;

namespace Abp.EntityFramework.Repositories
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public abstract class AbpCMSRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<AbpCMSDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected AbpCMSRepositoryBase(IDbContextProvider<AbpCMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add your common methods for all repositories
    }

    /// <summary>
    /// Base class for custom repositories of the application.
    /// This is a shortcut of <see cref="AbpCMSRepositoryBase{TEntity,TPrimaryKey}"/> for <see cref="int"/> primary key.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class AbpCMSRepositoryBase<TEntity> : AbpCMSRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected AbpCMSRepositoryBase(IDbContextProvider<AbpCMSDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}
