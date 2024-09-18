using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rems_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleError(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
}
