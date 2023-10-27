using BankFrontEnd.Models;
using BankWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BankFrontEndJS.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpGet("defaultview")]
        public IActionResult GetDefaultView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("LoginViewAuthenticated");
                }

            }
            // Return the partial view as HTML
            return PartialView("LoginDefaultView");
        }

        [HttpGet("authview")]
        public IActionResult GetLoginAuthenticatedView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];
                if (cookieValue == "1234567")
                {
                    return PartialView("LoginViewAuthenticated");
                }

            }
            // Return the partial view as HTML
            return PartialView("LoginErrorView");
        }

        [HttpGet("error")]
        public IActionResult GetLoginErrorView()
        {
            return PartialView("LoginErrorView");
        }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] User user)
        {
            // Return the partial view as HTML

            // this is temporary just to check the html n stuff
            var response = new { login = false };
            //var response = new { login = true };

            RestClient restClient = new RestClient("http://localhost:5246");
            RestRequest restRequest = new RestRequest("/api/userprofile/email/" + user.UserName, Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            UserProfile userProfile;

            userProfile = JsonConvert.DeserializeObject<UserProfile>(restResponse.Content);

            if (userProfile != null)
            {
                if (user != null && user.PassWord.Equals(userProfile.Password))
                {
                    Response.Cookies.Append("SessionID", "1234567");
                    response = new { login = true };
                }
            }
            return Json(response);

        }
    }
}
