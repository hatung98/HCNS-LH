using LACHONG_QLHC_WEB_Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_Contracts.HoSoContracts
{
    public interface IHoSoRepository
    {
        public Task<List<HoSoModel>> GetListHoSo(int Nam, int ID_LoaiHoSo, string Tag);

        public int HoSo_InsertOrUpdate(HoSoModel obj);

        public int HoSoFileDinhKem_InsertOrUpdate(HoSo_TepDinhKemModel obj);

        public bool HoSo_Delete(string listID, string Modified_By);

        public bool HoSoFileDinhKem_Update(int ID, int ID_HoSo);

        public bool HoSoFileDinhKem_Delete(string listID, string Modified_By);

        public Task<List<HoSo_TepDinhKemModel>> GetListHoSo_TepDinhKem(int ID_HoSo);

    }
}
