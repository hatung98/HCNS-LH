using LACHONG_QLHC_WEB_Entities.ExtendModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_Contracts.ComboboxContracts
{
    public interface IComboboxRepository
    {
        public Task<List<object>> GetComboBox_LoaiHoSo();
        public Task<List<object>> GetComboBox_DuAn();
        public Task<List<BoPhanViewModel>> GetTreeQuanLyBoPhan();
    }
}
