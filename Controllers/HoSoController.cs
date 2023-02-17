using Aspose.Cells;
using LACHONG_QLHC_WEB_API.Helpers;
using LACHONG_QLHC_WEB_Contracts.HoSoContracts;
using LACHONG_QLHC_WEB_Entities.ExtendModels;
using LACHONG_QLHC_WEB_Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_API.Controllers
{
    [Route("api/hoso")]
    [ApiController, Authorize]

    public class HoSoController : ControllerBase
    {
        private readonly string path = "/File/Upload/";
        private readonly ILogger<HoSoController> _logger;
        private readonly IHoSoRepository _hosoRepo;
        private readonly IHoSoImportRepository _hosoImportRepository;

        public HoSoController(ILogger<HoSoController> logger, IHoSoRepository hosoRepo, IHoSoImportRepository hosoImportRepository)
        {
            _logger = logger;
            _hosoRepo = hosoRepo;
            _hosoImportRepository = hosoImportRepository;
        }

        [HttpGet("getlisthoso")]
        public async Task<IActionResult> GetListHoSo(int Nam, int ID_LoaiHoSo, string Tag)
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();
                if (Tag == null)
                    Tag = "";
                List<HoSoModel> result = await _hosoRepo.GetListHoSo(Nam, ID_LoaiHoSo, Tag);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("hoso_insertorupdate")]
        public IActionResult HoSo_InsertOrUpdate(HoSoModel obj)
        {
            _logger.LogInformation("API: " + Request.Path);
            _logger.LogInformation("Param: " + JsonConvert.SerializeObject(obj));

            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                obj.ID_TaiKhoan = user.ID_TaiKhoan;
                obj.Modified_By = user.TenDangNhap;

                int result = _hosoRepo.HoSo_InsertOrUpdate(obj);

                if (result > 0)
                {
                    if(obj.ListIDDelete != "")
                    {
                        bool re = _hosoRepo.HoSoFileDinhKem_Delete(obj.ListIDDelete, user.TenDangNhap);
                    }

                    bool flag = true;
                    foreach (HoSo_TepDinhKemModel filedinhkem in obj.listfiledinhkem)
                    {
                        //filedinhkem.Modified_By = user.TenDangNhap;

                        //int id_chitiet = _hosoRepo.HoSoFileDinhKem_InsertOrUpdate(filedinhkem);

                        bool id_chitiet = _hosoRepo.HoSoFileDinhKem_Update(filedinhkem.ID, result);

                        if (!id_chitiet)
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                        return Ok(new { flag = true, msg = "Tác vụ thành công", value = result });
                    else
                        return Ok(new { flag = false, msg = "Tác vụ thất bại", value = 0 });
                }
                else
                    return Ok(new { flag = false, msg = "Tác vụ thất bại", value = 0 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("hoso_delete")]
        public IActionResult HoSo_Delete(string listID)
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                string Modified_By = user.TenDangNhap;

                bool result = _hosoRepo.HoSo_Delete(listID, Modified_By);

                if (result)
                    return Ok(new { flag = true, msg = "Tác vụ thành công", value = result });
                else
                    return Ok(new { flag = false, msg = "Tác vụ thất bại", value = 0 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("getlisthosotepdinhkem")]
        public async Task<IActionResult> GetListHoSo_TepDinhKem(int ID_HoSo)
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                List<HoSo_TepDinhKemModel> result = await _hosoRepo.GetListHoSo_TepDinhKem(ID_HoSo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost("savefileExcel")]
        public async Task<IActionResult> Uploadfile()
        {
            try
            {
                var file = HttpContext.Request.Form.Files;
                foreach (var item in file)
                {
                    if (item.Length > 5 * 1024 * 1024)
                        return Ok(new { flag = false, msg = "File không được vượt quá 5MB" });
                }
                string fileName = await _hosoImportRepository.UploadFile(file);
                return Ok(new { flag = true, value = fileName, msg = "File không được vượt quá 5MB" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];

                var file = HttpContext.Request.Form.Files;

               

                string fileNamePath = await _hosoImportRepository.UploadFile(file);

                var file_upload = file[0];
                string fileName = file_upload.FileName;
                string ticks = DateTime.Now.Ticks.ToString();
                fileName = fileName.Trim();
                fileName = fileName.Replace(" ", "_");
                string extention = fileName.Substring(fileName.LastIndexOf('.') + 1);
                fileName = fileName.Substring(0, fileName.LastIndexOf('.')).Replace('.', '_').ToLower();
                string path_date = DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString() + @"\" + DateTime.Now.Day.ToString();
                string folder_upload = path + path_date;
                string path_file_upload = folder_upload + @"\" + fileName + ticks + "." + extention;

                HoSo_TepDinhKemModel obj = new HoSo_TepDinhKemModel();
                obj.ID = 0;
                obj.ID_HoSo = 0;
                obj.FileName = file[0].FileName;
                obj.FilePath = path_file_upload;
                obj.URL = fileNamePath;
                obj.Modified_By = user.TenDangNhap;

                int id = _hosoRepo.HoSoFileDinhKem_InsertOrUpdate(obj);

                return Ok(new { flag = true, value = new { ID = id, URL = fileNamePath, FilePath = path_file_upload, FileName = file[0].FileName }, msg = "Thành công" }); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("downloadfile")]
        public IActionResult DownloadFile(string URL)
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();


                MemoryStream memoryStream = new MemoryStream();
                memoryStream = _hosoImportRepository.DownLoadFileDinhKem(URL);

                return File(memoryStream.GetBuffer(), "application/octet-stream");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
