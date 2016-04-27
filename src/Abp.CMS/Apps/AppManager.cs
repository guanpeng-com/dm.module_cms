using Abp.CMS;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.UI;

namespace Abp.Apps
{
    /// <summary>
    ///  领域逻辑：应用
    /// </summary>
    public class AppManager : DomainService
    {
        /// <summary>
        /// 应用仓储
        /// </summary>
        public IRepository<App, long> AppRepository { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appRepository"></param>
        public AppManager(IRepository<App, long> appRepository)
        {
            AppRepository = appRepository;
            LocalizationSourceName = AbpCMSConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 创建应用
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(App app)
        {
            await ValidateAppAsync(app);
            await AppRepository.InsertAsync(app);
        }

        /// <summary>
        ///  获取应用
        /// </summary>
        /// <returns></returns>
        public virtual async Task<App> GetByIdAsync(long id)
        {
            return await AppRepository.GetAsync(id);
        }

        /// <summary>
        ///  获取应用
        /// </summary>
        /// <returns></returns>
        public virtual App GetById(long id)
        {
            return AppRepository.Get(id);
        }

        /// <summary>
        ///  获取应用集合
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<App>> FindApp()
        {
            return await AppRepository.GetAllListAsync();
        }

        /// <summary>
        /// 获取默认APP
        /// </summary>
        /// <returns></returns>
        public virtual async Task<App> FindDefaultAsync()
        {
            return await AppRepository.FirstOrDefaultAsync(a => a.AppName == App.DefaultName);
        }

        /// <summary>
        /// 获取默认APP
        /// </summary>
        /// <returns></returns>
        public virtual App FindDefault()
        {
            return AppRepository.FirstOrDefault(a => a.AppName == App.DefaultName);
        }

        /// <summary>
        ///  校验应用
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        protected virtual async Task ValidateAppAsync(App app)
        {
            var siblings = (await FindApp())
                                  .Where(a => a.Id != app.Id)
                                  .ToList();
            if (siblings.Any(a => a.AppName == app.AppName))
            {
                throw new UserFriendlyException(L("AppDuplicateAppNameWarning", app.AppName));
            }
            else if (siblings.Any(a => a.AppDir == app.AppDir))
            {
                throw new UserFriendlyException(L("AppDuplicateAppDirWarning", app.AppDir));
            }
            else if (siblings.Any(a => a.DisplayName == app.DisplayName))
            {
                throw new UserFriendlyException(L("AppDuplicateDisplayNameWarning", app.DisplayName));
            }
        }
    }
}
