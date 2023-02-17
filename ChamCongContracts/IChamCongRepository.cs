using LACHONG_QLHC_WEB_Entities.ExtendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_Contracts.ChamCongContracts
{
    public interface IChamCongRepository
    {
        public Task<List<ChamCong_BangChamCongViewModel>> GetBangChamCong(DateTime from, DateTime to);
        public Task<ChamCong_HomeViewModel> GetHomeChamCong(); 
        public Task<ChamCong_HomNhanVienViewModel> GetNhanVienHomeChamCong(int idchamcong);
        public Task<ChamCong_ChiTietBangChamCongViewModel> GetChiTietChamCongThang(DateTime Thang, int idchamcong);
    }
}
