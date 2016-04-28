using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.CMS;
using System;
using Abp.Channels;
using Abp.Apps;

namespace Abp.Templates
{
    /// <summary>
    /// 领域逻辑：模板
    /// </summary>
    public class TemplateManager : DomainService
    {
        /// <summary>
        /// 模板仓储
        /// </summary>
        public IRepository<Template, long> TemplateRepository { get; private set; }

        /// <summary>
        /// 应用仓储
        /// </summary>
        protected AppManager AppManager { get; private set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templateRepository"></param>
        /// <param name="appManager"></param>
        public TemplateManager(
            IRepository<Template, long> templateRepository,
            AppManager appManager)
        {
            TemplateRepository = templateRepository;
            AppManager = appManager;
            LocalizationSourceName = AbpCMSConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 创建模板
        /// </summary>
        /// <param name="Template"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task CreateAsync(Template Template)
        {
            await ValidateTemplateAsync(Template);
            await TemplateRepository.InsertAsync(Template);
        }

        /// <summary>
        /// 更新模板
        /// </summary>
        /// <param name="Template"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Template Template)
        {
            await ValidateTemplateAsync(Template);
            await TemplateRepository.UpdateAsync(Template);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteAsync(long id)
        {
            await TemplateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="appId">应用</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public async Task<List<Template>> FindTemplatesByTypeAsync(long appId, string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                var query = from t in TemplateRepository.GetAll()
                            where t.AppId == appId
                            select t;
                return await Task.FromResult(query.ToList<Template>());
            }
            else
            {
                var query = from t in TemplateRepository.GetAll()
                            where t.AppId == appId && t.Type == type
                            select t;
                return await Task.FromResult(query.ToList<Template>());
            }
        }

        protected virtual async Task ValidateTemplateAsync(Template template)
        {

        }

    }
}