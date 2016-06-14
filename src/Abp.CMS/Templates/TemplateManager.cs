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
using Abp.UI;
using Abp.Core.IO;
using Abp.Core.Enums;
using Abp.Core.Utils;

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
        /// <param name="template"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task CreateAsync(Template template)
        {
            await ValidateTemplateAsync(template);
            template = await TemplateRepository.InsertAsync(template);
            template.App = await AppManager.GetByIdAsync(template.AppId);
            //保存文件
            SaveAsTemplateFile(template);
        }

        /// <summary>
        /// 更新模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Template template)
        {
            await ValidateTemplateAsync(template);
            template = await TemplateRepository.UpdateAsync(template);
            template.App = await AppManager.GetByIdAsync(template.AppId);
            //保存文件
            SaveAsTemplateFile(template);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteAsync(long id)
        {
            Template template = TemplateRepository.Get(id);
            await TemplateRepository.DeleteAsync(id);

            //删除文件
            DeleteTemplateFile(template);
        }

        /// <summary>
        /// 获取模板
        /// </summary>
        /// <param name="appId">应用</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public async Task<List<Template>> FindTemplatesByTypeAsync(long appId, string type)
        {
            List<Template> list = new List<Template>();
            if (string.IsNullOrEmpty(type))
            {
                var query = from t in TemplateRepository.GetAll()
                            where t.AppId == appId
                            select t;

                query.ToList<Template>().ForEach(t =>
                {
                    t.TemplateContent = GetTemplateContent(t);
                    list.Add(t);
                });
                return await Task.FromResult(list);
            }
            else
            {
                var query = from t in TemplateRepository.GetAll()
                            where t.AppId == appId && t.Type == type
                            select t;
                query.ToList<Template>().ForEach(t =>
                {
                    t.TemplateContent = GetTemplateContent(t);
                    list.Add(t);
                });
                return await Task.FromResult(list);
            }
        }

        /// <summary>
        /// 校验模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public virtual async Task ValidateTemplateAsync(Template template)
        {
            if (!template.Name.StartsWith("T_"))
            {
                throw new UserFriendlyException(L("TemplateNameMustStartWithT_"));
            }

            var siblings = (await TemplateRepository.GetAllListAsync(t => t.AppId == template.AppId && t.Id != template.Id));

            if (siblings.Any(t => t.Title == template.Title))
            {
                throw new UserFriendlyException(L("TemplateDuplicateTitleWarning", template.Title));
            }

            if (siblings.Any(t => t.Name == template.Name))
            {
                throw new UserFriendlyException(L("TemplateDuplicateNameWarning", template.Name));
            }

        }

        /// <summary>
        /// 获取模板文件
        /// </summary>
        /// <param name="template"></param>
        public string GetTemplateContent(Template template)
        {
            if (template.TemplateContent == null) template.TemplateContent = string.Empty;
            string filePath = GetTemplateFilePath(template);
            return FileUtils.ReadText(filePath, ECharset.utf_8);
        }

        /// <summary>
        /// 删除模板文件
        /// </summary>
        /// <param name="template"></param>
        public void DeleteTemplateFile(Template template)
        {
            string filePath = GetTemplateFilePath(template);
            FileUtils.DeleteFileIfExists(filePath);
        }


        /// <summary>
        /// 保存模板文件
        /// </summary>
        /// <param name="template"></param>
        public void SaveAsTemplateFile(Template template)
        {
            if (template.TemplateContent == null) template.TemplateContent = string.Empty;
            string filePath = GetTemplateFilePath(template);
            DirectoryUtils.CreateDirectoryIfNotExists(filePath);
            FileUtils.WriteText(filePath, ECharset.utf_8, template.TemplateContent);
        }

        /// <summary>
        /// 获取模板文件路径
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public string GetTemplateFilePath(Template template)
        {
            if (template.Type == ETemplateTypeUtils.GetValue(ETemplateType.IndexTemplate))
            {
                return PathUtils.Combine(DirectoryUtils.Instance.PhysicalApplicationPath, template.App.AppDir, DirectoryUtils.Template.TemplateDirName, DirectoryUtils.Template.IndexTemplateDirName, template.Name + template.Extension);
            }
            else if (template.Type == ETemplateTypeUtils.GetValue(ETemplateType.ChannelTemplate))
            {
                return PathUtils.Combine(DirectoryUtils.Instance.PhysicalApplicationPath, template.App.AppDir, DirectoryUtils.Template.TemplateDirName, DirectoryUtils.Template.ChannelTemplateDirName, template.Name + template.Extension);
            }
            else if (template.Type == ETemplateTypeUtils.GetValue(ETemplateType.ContentTemplate))
            {
                return PathUtils.Combine(DirectoryUtils.Instance.PhysicalApplicationPath, template.App.AppDir, DirectoryUtils.Template.TemplateDirName, DirectoryUtils.Template.ContentTemplateDirName, template.Name + template.Extension);
            }
            else
            {
                return PathUtils.Combine(DirectoryUtils.Instance.PhysicalApplicationPath, template.App.AppDir, DirectoryUtils.Template.TemplateDirName, DirectoryUtils.Template.FileTemplateDirName, template.Name + template.Extension);
            }
        }
    }
}