using Abp.DMUsers;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.CMS.SampleApp.DMUsers
{
    public class DMUserStore : DMUserStore<DMUser>
    {
        public DMUserStore(
            IRepository<DMUser, long> userRepository,
            IRepository<DMUserLogin, long> userLoginRepository,
            IUnitOfWorkManager unitOfWorkManager
            )
            : base(userRepository,
                  userLoginRepository,
                  unitOfWorkManager)
        {

        }
    }
}
