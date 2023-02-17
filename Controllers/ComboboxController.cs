using LACHONG_QLHC_WEB_API.Helpers;
using LACHONG_QLHC_WEB_Contracts.ComboboxContracts;
using LACHONG_QLHC_WEB_Entities.ExtendModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LACHONG_QLHC_WEB_API.Controllers
{
    [Route("api/combobox")]
    [ApiController, Authorize]
    public class ComboboxController : ControllerBase
    {
        private readonly ILogger<ComboboxController> _logger;
        private readonly IComboboxRepository _comBoBoxRepo;

        public ComboboxController(ILogger<ComboboxController> logger, IComboboxRepository comBoBoxRepo)
        {
            _logger = logger;
            _comBoBoxRepo = comBoBoxRepo;
        }

        [HttpGet("getcombobox_loaihoso")]
        public async Task<IActionResult> GetComboBox_LoaiHoSo()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                List<object> result = await _comBoBoxRepo.GetComboBox_LoaiHoSo();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getcombobox_duan")]
        public async Task<IActionResult> GetComboBox_DuAn()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                List<object> result = await _comBoBoxRepo.GetComboBox_DuAn();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("gettreequanlybophan")]
        public async Task<IActionResult> GetTreeQuanLyBoPhan()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                List<BoPhanViewModel> result = await _comBoBoxRepo.GetTreeQuanLyBoPhan();
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
