using LACHONG_QLHC_WEB_API.Helpers;
using LACHONG_QLHC_WEB_Contracts.BanTinContracts;
using LACHONG_QLHC_WEB_Entities.ExtendModels;
using LACHONG_QLHC_WEB_Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_API.Controllers
{
    [Route("api/bantin")]
    [ApiController, Authorize]
    public class BanTinController : ControllerBase
    {
        private readonly ILogger<BanTinController> _logger;
        private readonly IBanTinRepository _banTinRepo;

        public BanTinController(ILogger<BanTinController> logger, IBanTinRepository banTinRepo)
        {
            _logger = logger;
            _banTinRepo = banTinRepo;
        }

        [HttpGet("bantin_getlist")]
        public async Task<IActionResult> BanTin_GetList()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                List<BanTinViewModel> result = await _banTinRepo.BanTin_GetList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("bantin_insertorupdate")]
        public IActionResult BanTin_InsertOrUpdate(BanTinModel obj)
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

                int result = _banTinRepo.BanTin_InsertOrUpdate(obj);

                if (result > 0)
                {
                    //if (obj.listSanPham_Delete != "")
                    //{
                    //    int check = _phieuYeuCauVPPRepo.PhieuYeuCauVPP_SanPham_Delete(obj.listSanPham_Delete);
                    //}

                    bool flag = true;
                    foreach (BanTin_BoPhanModel banTin_BoPhanModel in obj.listBoPhan)
                    {
                        banTin_BoPhanModel.ID_BanTin = result;
                        banTin_BoPhanModel.Modified_By = user.TenDangNhap;

                        int id_chitiet = _banTinRepo.BanTin_BoPhan_Insert(banTin_BoPhanModel);

                        if (id_chitiet <= 0)
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

        [HttpGet("bantin_delete")]
        public IActionResult BanTin_Delete(string listID)
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                string Modified_By = user.TenDangNhap;

                bool result = _banTinRepo.BanTin_Delete(listID, Modified_By);

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

        [HttpGet("bantin_updatetrangthaidaxem")]
        public IActionResult BanTin_UpdateTrangThaiDaXem(int ID_BanTin)
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                int result = _banTinRepo.BanTin_UpdateTrangThaiDaXem(ID_BanTin, user.ID_TaiKhoan, user.TenDangNhap);

                if (result > 0)
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
    }
}
