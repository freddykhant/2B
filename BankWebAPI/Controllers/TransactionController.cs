using BankWebAPI.Data;
using BankWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using static BankWebAPI.Models.Transaction;

namespace BankWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        [HttpGet]
        public IEnumerable<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = DBManager.GetAllTransactions();
            return transactions;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetTransactionById(int id)
        {
            var transaction = DBManager.GetTransactionById(id);
            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        [HttpPost]
        public IActionResult CreateTransaction([FromBody] Transaction transaction)
        {
            Account account = DBManager.GetAccountByNumber(transaction.AccountNumber);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            if (transaction.Type == TransactionType.Withdrawal && account.Balance < transaction.Amount)
            {
                return BadRequest("Insufficient funds.");
            }

            if (transaction.Type == TransactionType.Deposit)
            {
                account.Balance += transaction.Amount;
            }
            else
            {
                account.Balance -= transaction.Amount;
            }

            DBManager.UpdateAccount(account);

            if (DBManager.InsertTransaction(transaction))
                return Ok("Successfully inserted");
            return BadRequest("Error in data insertion");
        }


        [HttpPut]
        public IActionResult UpdateTransaction([FromBody] Transaction transaction)
        {
            if (DBManager.UpdateTransaction(transaction))
                return Ok("Successfully updated");
            return BadRequest("Could not update");
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            if (DBManager.DeleteTransaction(id))
                return Ok("Successfully Deleted");
            return BadRequest("Could not delete");
        }
    }
}
