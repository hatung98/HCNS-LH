using LACHONG_QLHC_WEB_API.Helpers;
using LACHONG_QLHC_WEB_Contracts.HomeContracts;
using LACHONG_QLHC_WEB_Entities.ExtendModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_API.Controllers
{
    [Route("api/home")]
    [ApiController, Authorize]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepo;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepo)
        {
            _logger = logger;
            _homeRepo = homeRepo;
        }

        [HttpGet("getchitietcongtheonhanvien")]
        public async Task<IActionResult> GetChiTietCongTheoNhanVien()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                var result = await _homeRepo.GetChiTietCongTheoNhanVien(user.ID_TaiKhoan);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        
        [HttpGet("getthongtinlamviec")]
        public async Task<IActionResult> GetThongTinLamViec()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                var result = await _homeRepo.GetThongTinLamViecTheoNgay();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
