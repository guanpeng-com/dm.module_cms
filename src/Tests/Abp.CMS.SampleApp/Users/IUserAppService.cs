using Abp.Application.Services;
using Abp.CMS.SampleApp.Users.Dto;

namespace Abp.CMS.SampleApp.Users
{
    public interface IUserAppService : IApplicationService
    {
        void CreateUser(CreateUserInput input);
    }
}
