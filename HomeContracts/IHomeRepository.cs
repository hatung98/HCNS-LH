using LACHONG_QLHC_WEB_Entities.ExtendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_Contracts.HomeContracts
{
    public interface IHomeRepository
    {
        public Task<List<ChiTietCongNhanVienViewModel>> GetChiTietCongTheoNhanVien(int ID_NhanVien);
        public Task<ThongTinLamViecViewModel> GetThongTinLamViecTheoNgay();
    }
}
