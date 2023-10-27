using BankWebAPI.Models;
using System.Data.SQLite;
using System.Reflection;

namespace BankWebAPI.Data
{
    public class DBManager
    {
        private static string connectionString = "Data Source=BankWebService.db;Version=3;";
        
        // file path for the database to delete it every time before startup
        // to get the filepath of it just right click on BankWebService.db and select 'Copy Full Path' then just paste it in the string below
        string filePath = @"C:\Users\jolyo\Desktop\working dc\BankWebAPI\BankWebAPI\BankWebService.db";

        public static bool CreateTables()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {

                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Account (
                            AccountNumber INTEGER PRIMARY KEY,
                            Balance REAL,
                            AccountHolderName TEXT
                        )";

                    command.ExecuteNonQuery();
                    // this has to be called AccountTransaction because Transaction is a reserved keyword in SQL lol
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS AccountTransaction (
                            TransactionId INTEGER PRIMARY KEY,
                            AccountNumber INTEGER,
                            Type TEXT,
                            Amount REAL,
                            Timestamp TEXT,
                            FOREIGN KEY(AccountNumber) REFERENCES Account(AccountNumber)
                        )";
                    command.ExecuteNonQuery();

                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS UserProfile (
                            Username TEXT PRIMARY KEY,
                            Email TEXT,
                            Name TEXT,
                            Address TEXT,
                            Phone TEXT,
                            Picture BLOB,
                            Password TEXT
                        )";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return true;
        }

        // Account methods

        public static bool InsertAccount(Account account)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                INSERT INTO Account (AccountNumber, Balance, AccountHolderName)
                VALUES (@AccountNumber, @Balance, @AccountHolderName)";

                    command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                    
                    command.Parameters.AddWithValue("@Balance", account.Balance);

                    command.Parameters.AddWithValue("@AccountHolderName", account.AccountName);

                    int rowsInserted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsInserted > 0;
                }
            }
        }

        public static bool UpdateAccount(Account account)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                UPDATE Account 
                SET Username = @Username, Balance = @Balance 
                WHERE AccountNumber = @AccountNumber";

                    command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                    command.Parameters.AddWithValue("@Username", account.AccountName);
                    command.Parameters.AddWithValue("@Balance", account.Balance);

                    int rowsUpdated = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsUpdated > 0;
                }
            }
        }

        public static bool DeleteAccount(int accountNumber)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Account WHERE AccountNumber = @AccountNumber";
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    int rowsDeleted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsDeleted > 0;
                }
            }
        }

        public static Account GetAccountByNumber(int accountNumber)
        {
            Account account = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Account WHERE AccountNumber = @AccountNumber";
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            account = new Account
                            {
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                AccountName = reader["Username"].ToString(),
                                Balance = (decimal)Convert.ToDouble(reader["Balance"])
                            };
                        }
                    }
                }
                connection.Close();
            }
            return account;
        }

        public static List<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Account";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Account account = new Account
                            {
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                AccountName = reader["Username"].ToString(),
                                Balance = (decimal)Convert.ToDouble(reader["Balance"])
                            };
                            accounts.Add(account);
                        }
                    }
                }
                connection.Close();
            }
            return accounts;
        }

        // Transaction methods

        public static bool InsertTransaction(Transaction transaction)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO AccountTransaction (AccountNumber, Type, Amount, Timestamp)
                        VALUES (@AccountNumber, @Type, @Amount, @Timestamp)";

                    command.Parameters.AddWithValue("@AccountNumber", transaction.AccountNumber);
                    command.Parameters.AddWithValue("@Type", transaction.Type);
                    command.Parameters.AddWithValue("@Amount", transaction.Amount);
                    command.Parameters.AddWithValue("@Timestamp", transaction.Timestamp);

                    int rowsInserted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsInserted > 0;
                }
            }
        }

        public static bool UpdateTransaction(Transaction transaction)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                    UPDATE Transaction 
                    SET AccountNumber = @AccountNumber, Type = @Type, Amount = @Amount, Timestamp = @Timestamp 
                    WHERE TransactionId = @TransactionId";

                    command.Parameters.AddWithValue("@TransactionId", transaction.TransactionId);
                    command.Parameters.AddWithValue("@AccountNumber", transaction.AccountNumber);
                    command.Parameters.AddWithValue("@Type", transaction.Type);
                    command.Parameters.AddWithValue("@Amount", transaction.Amount);
                    command.Parameters.AddWithValue("@Timestamp", transaction.Timestamp);

                    int rowsUpdated = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsUpdated > 0;
                }
            }
        }

        public static bool DeleteTransaction(int transactionId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Transaction WHERE TransactionId = @TransactionId";
                    command.Parameters.AddWithValue("@TransactionId", transactionId);

                    int rowsDeleted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsDeleted > 0;
                }
            }
        }

        public static Transaction GetTransactionById(int id)
        {
            Transaction transaction = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Transaction WHERE TransactionId = @TransactionId";
                    command.Parameters.AddWithValue("@TransactionId", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            transaction = new Transaction
                            {
                                TransactionId = Convert.ToInt32(reader["TransactionId"]),
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                Type = (TransactionType)reader["Type"],
                                Amount = (decimal)Convert.ToDouble(reader["Amount"]),
                                Timestamp = DateTime.Parse(reader["Timestamp"].ToString())
                            };
                        }
                    }
                }
                connection.Close();
            }

            return transaction;
        }

        public static List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Transaction";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction transaction = new Transaction
                            {
                                TransactionId = Convert.ToInt32(reader["TransactionId"]),
                                AccountNumber = Convert.ToInt32(reader["AccountNumber"]),
                                Type = (TransactionType)reader["Type"],
                                Amount = (decimal)Convert.ToDouble(reader["Amount"]),
                                Timestamp = DateTime.Parse(reader["Timestamp"].ToString())
                            };
                            transactions.Add(transaction);
                        }
                    }
                }
                connection.Close();
            }
            return transactions;
        }


        // User profile methods
        public static bool InsertUserProfile(UserProfile userProfile)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO UserProfile (Username, Email, Name, Address, Phone, Picture, Password)
                        VALUES (@Username, @Email, @Name, @Address, @Phone, @Picture, @Password)";

                    command.Parameters.AddWithValue("@Username", userProfile.Username);
                    command.Parameters.AddWithValue("@Email", userProfile.Email);
                    command.Parameters.AddWithValue("@Name", userProfile.Name);
                    command.Parameters.AddWithValue("@Address", userProfile.Address);
                    command.Parameters.AddWithValue("@Phone", userProfile.Phone);
                    command.Parameters.AddWithValue("@Picture", userProfile.Picture);
                    command.Parameters.AddWithValue("@Password", userProfile.Password);

                    int rowsInserted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsInserted > 0;
                }
            }
        }

        public static bool UpdateUserProfile(UserProfile userProfile)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                    UPDATE UserProfile 
                    SET Email = @Email, Name = @Name, Address = @Address, Phone = @Phone, Picture = @Picture, Password = @Password 
                    WHERE Username = @Username";

                    command.Parameters.AddWithValue("@Username", userProfile.Username);
                    command.Parameters.AddWithValue("@Email", userProfile.Email);
                    command.Parameters.AddWithValue("@Name", userProfile.Name);
                    command.Parameters.AddWithValue("@Address", userProfile.Address);
                    command.Parameters.AddWithValue("@Phone", userProfile.Phone);
                    command.Parameters.AddWithValue("@Picture", userProfile.Picture);
                    command.Parameters.AddWithValue("@Password", userProfile.Password);

                    int rowsUpdated = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsUpdated > 0;
                }
            }
        }

        public static bool DeleteUserProfile(string username)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM UserProfile WHERE Username = @Username";
                    command.Parameters.AddWithValue("@Username", username);

                    int rowsDeleted = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsDeleted > 0;
                }
            }
        }

        public static UserProfile GetUserProfileById(int id)
        {
            UserProfile userProfile = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM UserProfile WHERE UserProfileId = @UserProfileId";
                    command.Parameters.AddWithValue("@UserProfileId", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userProfile = new UserProfile
                            {
                                Username = reader["Username"].ToString(),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                // Assuming Picture is stored as a blob in SQLite
                                Picture = (byte[])reader["Picture"],
                                Password = reader["Password"].ToString() 
                            };
                        }
                    }
                }
                connection.Close();
            }

            return userProfile;
        }

        public static UserProfile GetUserProfileByEmail(string email)
        {
            UserProfile userProfile = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM UserProfile WHERE Email = @Email";
                    command.Parameters.AddWithValue("@Email", email);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userProfile = new UserProfile
                            {
                                Username = reader["Username"].ToString(),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                // not gonna to pictures for the moment
                                //Picture = (byte[])reader["Picture"],
                                Password = reader["Password"].ToString() 
                            };
                        }
                    }
                }
                connection.Close();
            }

            return userProfile;
        }



        public static List<UserProfile> GetAllUserProfiles()
        {
            List<UserProfile> userProfiles = new List<UserProfile>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM UserProfile";
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserProfile userProfile = new UserProfile
                            {
                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Picture = (byte[])reader["Picture"],
                                Password = reader["Password"].ToString()
                            };
                            userProfiles.Add(userProfile);
                        }
                    }
                }
                connection.Close();
            }
            return userProfiles;
        }

        public static void addAdmin()
        {
            UserProfile admin = new UserProfile();

            admin.Email = "admin";
            admin.Password = "password";

            DBManager.InsertUserProfile(admin);
        }

        public static void deleteDB()
        {
            //string filePath = @"C:\Users\jolyo\Desktop\working dc\BankWebAPI\BankWebAPI\BankWebService.db";
            string filePath = @"C:\Users\fredd\Downloads\dc assignment LOL\working dc\BankWebAPI\BankWebAPI\BankWebService.db";

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine("File deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
            }
        }

        public static void DBInitialize()
        {
            if (CreateTables())
            {
                /*Account account1 = new Account { AccountNumber = 1001, Balance = 5000, AccountName = "John Doe" };
                InsertAccount(account1);*/
            }
        }
    }
}
