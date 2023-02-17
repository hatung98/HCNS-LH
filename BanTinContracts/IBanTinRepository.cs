using LACHONG_QLHC_WEB_Entities.Models;
using LACHONG_QLHC_WEB_Entities.ExtendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_Contracts.BanTinContracts
{
    public interface IBanTinRepository
    {
        public Task<List<BanTinViewModel>> BanTin_GetList();
        public int BanTin_InsertOrUpdate(BanTinModel obj);
        public int BanTin_BoPhan_Insert(BanTin_BoPhanModel obj);
        public bool BanTin_Delete(string listID, string Modified_By);
        public int BanTin_UpdateTrangThaiDaXem(int ID_BanTin, int ID_TaiKhoan, string User);
    }
}
