using LACHONG_QLHC_WEB_API.Helpers;
using LACHONG_QLHC_WEB_Contracts.ChamCongContracts;
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
    [Route("api/chamcong")]
    [ApiController, Authorize]
    public class ChamCongController : ControllerBase
    {
        private readonly ILogger<ChamCongController> _logger;
        private readonly IChamCongRepository _chamCongRepo;

        public ChamCongController(ILogger<ChamCongController> logger, IChamCongRepository chamCongRepo)
        {
            _logger = logger;
            _chamCongRepo = chamCongRepo;
        }

        [HttpGet("getbangchamcong")]
        public async Task<IActionResult> GetBangChamCong(DateTime from, DateTime to)
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                List<ChamCong_BangChamCongViewModel> result = await _chamCongRepo.GetBangChamCong(from, to);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("gethomechamcong")]
        public async Task<IActionResult> GetHomeChamCong()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                ChamCong_HomeViewModel result = await _chamCongRepo.GetHomeChamCong();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("getnhanvienhomechamcong")]
        public async Task<IActionResult> GetNhanVienHomeChamCong()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                ChamCong_HomNhanVienViewModel result = await _chamCongRepo.GetNhanVienHomeChamCong(user.ID_ChamCong);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("getchitietchamcongthang")]
        public async Task<IActionResult> GetChiTietChamCongThang(DateTime thang, int idchamcong)
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                ChamCong_ChiTietBangChamCongViewModel result = await _chamCongRepo.GetChiTietChamCongThang(thang, idchamcong);
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
