using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Jeuci.WeChatApp.EntityFramework.Repositories
{
    public abstract class WeChatAppRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<WeChatAppDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected WeChatAppRepositoryBase(IDbContextProvider<WeChatAppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class WeChatAppRepositoryBase<TEntity> : WeChatAppRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected WeChatAppRepositoryBase(IDbContextProvider<WeChatAppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
