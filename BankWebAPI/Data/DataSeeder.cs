using BankWebAPI.Models;
using Bogus;

namespace BankWebAPI.Data
{
    public class DataSeeder
    {
        public static void SeedData()
        {
            // Account Data Generation
            var accountFaker = new Faker<Account>()
                .RuleFor(u => u.AccountNumber, f => f.UniqueIndex) // Unique account numbers
                .RuleFor(u => u.Balance, f => f.Finance.Amount(1m, 10000m)) // Balance between 1 and 10,000
                .RuleFor(u => u.AccountName, f => f.Name.FullName());

            // Transaction Data Generation
            var transactionFaker = new Faker<Transaction>()
            .RuleFor(t => t.TransactionId, f => f.UniqueIndex) // Unique transaction IDs
            .RuleFor(t => t.AccountNumber, f => f.Random.Int(1, 50)) // Assuming a max of 50 accounts
            .RuleFor(t => t.Amount, f => f.Finance.Amount(1m, 1000m)) // Transaction amount between 1 and 1,000
            .RuleFor(t => t.Type, f => f.PickRandom<TransactionType>()) // Corrected rule
            .RuleFor(t => t.Timestamp, f => f.Date.Past(2)); // Transactions from the past two years


            // UserProfile Data Generation
            var userProfileFaker = new Faker<UserProfile>()
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Address, f => f.Address.FullAddress())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
               // .RuleFor(u => u.Picture, f => f.Internet.Url()) // Generates a random Lorem Picsum image URL                                                                  
                .RuleFor(u => u.Password, f => f.Internet.Password()); // In real apps, hash & salt this

            // Generate data
            var accounts = accountFaker.Generate(50);
            var transactions = transactionFaker.Generate(500); // Generate 500 random transactions
            var userProfiles = userProfileFaker.Generate(50);

            // Insert generated data into DB using DBManager (assuming DBManager has these methods)
            foreach (var account in accounts)
            {
                DBManager.InsertAccount(account);
            }

            foreach (var transaction in transactions)
            {
                DBManager.InsertTransaction(transaction);
            }

            foreach (var profile in userProfiles)
            {
                DBManager.InsertUserProfile(profile);
            }
        }
    }
}
