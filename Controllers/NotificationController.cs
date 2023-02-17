using LACHONG_QLHC_WEB_API.Helpers;
using LACHONG_QLHC_WEB_Contracts.NotificationContracts;
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
    [Route("api/notification")]
    [ApiController, Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationRepository _notificationRepo;
        public NotificationController(ILogger<NotificationController> logger, INotificationRepository notificationRepo)
        {
            _logger = logger;
            _notificationRepo = notificationRepo;
        }

        [HttpGet("noti_bantin_getlist")]
        public async Task<IActionResult> Noti_BanTin_GetList()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                List<BanTinViewModel> result = await _notificationRepo.Noti_BanTin_GetList(user.ID_TaiKhoan);
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
