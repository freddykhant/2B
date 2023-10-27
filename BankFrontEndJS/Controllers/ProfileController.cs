using Microsoft.AspNetCore.Mvc;

namespace BankFrontEndJS.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            //Response.Cookies.Delete("SessionID");
            return PartialView("Profile");
        }
    }
}
