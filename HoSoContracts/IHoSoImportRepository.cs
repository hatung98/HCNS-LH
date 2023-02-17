using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_Contracts.HoSoContracts
{
    public interface IHoSoImportRepository
    {
        public Task<string> UploadFile(IFormFileCollection file);

        public MemoryStream DownLoadFileDinhKem(string filepath);
    }
}
