using BankWebAPI.Models;

namespace BankWebAPI.Data
{
    public class Seed
    {
        public static void SeedData()
        {
            Random random = new Random();

            // Seed user profiles
            for (int i = 0; i < 100; i++)
            {
                UserProfile user = new UserProfile
                {
                    Username = $"user{i}",
                    Email = $"user{i}@example.com",
                    Name = $"User {i}",
                    Address = $"Address {i}",
                    Phone = $"123-456-78{i}",
                    Password = $"password{i}" // Remember to hash & salt in a real application!
                };
                DBManager.InsertUserProfile(user);
            }

            // Seed accounts
            for (int i = 0; i < 100; i++)
            {
                Account account = new Account
                {
                    AccountNumber = 1000 + i,
                    Balance = random.Next(1000, 10000),
                    AccountName = $"user{i}"
                };
                DBManager.InsertAccount(account);
            }

            // Seed transactions
            for (int i = 0; i < 500; i++)
            {
                Transaction transaction = new Transaction
                {
                    AccountNumber = 1000 + random.Next(0, 99),
                    Type = (TransactionType)random.Next(0, 2),
                    Amount = random.Next(10, 500),
                    Timestamp = DateTime.Now.AddDays(-random.Next(0, 365))
                };
                DBManager.InsertTransaction(transaction);
            }
        }
    }
}
