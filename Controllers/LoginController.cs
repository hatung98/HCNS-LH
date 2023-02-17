using LACHONG_QLHC_WEB_API.Helpers;
using LACHONG_QLHC_WEB_Entities.ExtendModels;
using LACHONG_QLHC_WEB_Contracts.RepoContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LACHONG_QLHC_WEB_API.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginRepository _loginRepo;

        public LoginController(ILogger<LoginController> logger, ILoginRepository loginRepo)
        {
            _logger = logger;
            _loginRepo = loginRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestViewModel userLoginRequestViewModel)
        {
            try
            {
                UserLoginReponseViewModel userLoginReponseViewModel = await _loginRepo.LoginWeb(userLoginRequestViewModel);

                _logger.LogInformation(JsonConvert.SerializeObject(userLoginReponseViewModel));

                if (userLoginReponseViewModel.erCode == 0)
                    return Ok(new { flag = false, msg = "Tài khoản hoặc mặt khẩu không đúng", value = userLoginReponseViewModel });
                else
                    return Ok(new { flag = true, msg = "Đăng nhập thành công", value = userLoginReponseViewModel });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Ok(new { flag = false, msg = "Tác vụ thất bại", value = new UserLoginReponseViewModel(), token = "" });
            }
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var user = (UserForSessionModel)HttpContext.Items["UserForSession"];
                if (user == null)
                    return Unauthorized();

                UserLoginReponseViewModel userLoginReponseViewModel = await _loginRepo.RefreshToken(user);

                if (userLoginReponseViewModel.TaiKhoan == null)
                    return Ok(new { flag = false, msg = "Tác vụ thất bại", value = userLoginReponseViewModel });
                else
                    return Ok(new { flag = true, msg = "Tác vụ thành công", value = userLoginReponseViewModel });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Ok(new { flag = false, msg = "Tác vụ thất bại", value = new UserLoginReponseViewModel(), token = "" });
            }
        }
    }
}
