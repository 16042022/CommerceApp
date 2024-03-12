using CommerceCore.Application.Base;
using CommerceCore.Application.Feature.Shop.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ISender _sender;

        public AccountController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("init")]
        public IActionResult HelloWord()
        {
            return Ok(new { message = "HelloWord" });
        }

        [HttpPost("signup")]
        public async Task<IActionResult> TestSignUp([FromBody] CreateShopCommand models)
        {
            var result = await _sender.Send(models);
            // Set refreshToken into cookie
            SetTokenCookie(result.Data!.RefreshToken.Last());
            return Ok(result);
        }

        private void SetTokenCookie(string v)
        {
            var option = new CookieOptions()
            {
                HttpOnly = true, Expires = DateTime.Now.AddDays(7),
                SameSite = SameSiteMode.Lax
            };
            Response.Cookies.Append("logToken", v, option);
        }
    }
}
