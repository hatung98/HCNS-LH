using LACHONG_QLHC_WEB_Entities.ExtendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_Contracts.NotificationContracts
{
    public interface INotificationRepository
    {
        public Task<List<BanTinViewModel>> Noti_BanTin_GetList(int ID_TaiKhoan);
    }
}
