using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimalShelter;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;


namespace AnimalShelter
{
    public class Login
    {
            
       public void CreateAccount(IMongoCollection<BsonDocument> usersCollection)
        {
            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();

            //Hash the password
            string hashedPassword = HashPassword(password);

            //Create a new user 
            var newUser = new BsonDocument
        {
            { "username", username },
            { "password", hashedPassword },
            { "email", email }
        };

            // Insert the new user into the database
            try
            {
                usersCollection.InsertOne(newUser);
                Console.WriteLine("Account created successfully!");
            }
            catch (MongoWriteException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public bool UserLogin(IMongoCollection<BsonDocument> usersCollection)
        {
            bool login = false;

            Console.WriteLine("Enter your username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();

            // Hash the entered password
            string hashedPassword = HashPassword(password);

            // Search for the user in the database
            var filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var user = usersCollection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                string storedPassword = user["password"].AsString;

                if (storedPassword == hashedPassword)
                {
                    Console.WriteLine("Login successful!");
                    login = true;
                }
                else
                {
                    Console.WriteLine("Incorrect password.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }

            return login;
        }

        static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //Create Has
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                //Convert byte array to hex string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
