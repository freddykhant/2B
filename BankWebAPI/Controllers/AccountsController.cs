using BankWebAPI.Data;
using BankWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IEnumerable<Account> GetAllAccounts()
        {
            List<Account> accounts = DBManager.GetAllAccounts();
            return accounts;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAccount(int id)
        {
            Account account = DBManager.GetAccountByNumber(id);
            if (account == null)
                return NotFound();

            return Ok(account);
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            if (DBManager.InsertAccount(account))
                return Ok("Successfully inserted");
            return BadRequest("Error in data insertion");
        }

        [HttpPut]
        public IActionResult UpdateAccount(Account account)
        {
            if (DBManager.UpdateAccount(account))
                return Ok("Successfully updated");
            return BadRequest("Could not update");
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            if (DBManager.DeleteAccount(id))
                return Ok("Successfully Deleted");
            return BadRequest("Could not delete");
        }
    }
}
