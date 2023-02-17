using LACHONG_QLHC_WEB_Entities.ExtendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_Contracts.RepoContracts
{
    public interface ILoginRepository
    {
        public Task<UserLoginReponseViewModel> LoginWeb(UserLoginRequestViewModel user);
        public Task<UserForSessionModel> GetUserLoginByUserName(string userName);
        public Task<UserLoginReponseViewModel> RefreshToken(UserForSessionModel user);
    }
}
