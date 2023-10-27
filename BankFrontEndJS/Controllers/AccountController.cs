using Microsoft.AspNetCore.Mvc;

namespace BankFrontEndJS.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult GetView()
        {
            //Response.Cookies.Delete("SessionID");
            return PartialView("AccountSummary");
        }
    }
}
