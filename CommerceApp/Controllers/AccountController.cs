using CommerceCore.Application.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        [HttpGet("init")]
        public IActionResult HelloWord()
        {
            return Ok(new { message = "HelloWord" });
        }

        [HttpPost("signup")]
        public IActionResult TestSignUp()
        {
            Console.WriteLine($"SignUp body: {Request.Body}");
            return Ok(new BaseResult()
            {
                StatusCode = 201,
                Message = "SignUp successful"
            });
        }
    }
}
