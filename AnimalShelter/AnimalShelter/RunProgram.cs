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

namespace AnimalShelter
{
    public class RunProgram
    {
        //Variable decleration
        public Animal animalInformation { get; set; } = new Animal();
        public char action { get; set; } = 'y';
        public AnimalShelterHome AnimalShelter { get; set; } = new AnimalShelterHome();
        public BsonDocument animalToRead { get; set; } = new BsonDocument();
        public BsonDocument animalToDelete { get; set; } = new BsonDocument();
        public Animal animalToUpdate { get; set; } = new Animal();
        public BsonDocument updatedAnimal { get; set; } = new BsonDocument();
        public Animal animalToCreate { get; set; } = new Animal();
        public Login login { get; set; } = new Login();
        public string parameterToUpdate { get; set; } = "";
        public string LoginChoice { get; set; } = "";
        bool loginSuccess { get; set; } = false;

        //Declare Collection Variables for DB
        private static IMongoCollection<BsonDocument> usersCollection;
        private static IMongoCollection<BsonDocument> animalsCollection;

        public void main(string[] args)
        {
            Console.WriteLine("Welcome to the Animal Shelter system!");
            Console.WriteLine("Do you want to Login (L) or Create an Account (C)?");
            LoginChoice = Console.ReadLine().ToUpper();

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("AnimalShelter");
            usersCollection = database.GetCollection<BsonDocument>("Users");
            animalsCollection = database.GetCollection<BsonDocument>("Animals");

            if (LoginChoice == "C")
            {
                login.CreateAccount(usersCollection);
            }
            else if (LoginChoice == "L")
            {
                while (loginSuccess == false)
                {
                    loginSuccess = login.UserLogin(usersCollection);
                }

            }
            else
            {
                Console.WriteLine("Invalid choice. Exiting...");
            }

            while (action != 'x')
            {
                //Choose Action
                Console.WriteLine("Enter the action you want to perform (c for create, r for read, d for delete, u for update, x for exit:");
                action = char.Parse(Console.ReadLine());

                switch (action)
                {
                    case 'c':
                        //Create the animal algorithm change/enhancement by making its own method
                        CreateAnimal();

                        break;
                    case 'r':
                        //Read the animal
                        ReadAnimal();
                        break;
                    case 'd':
                        //Delete Animal
                        DeleteAnimal();
                        break;
                    case 'u':
                        //Update Animal
                        UpdateAnimal();

                        break;
                    case 'x':
                        //Exit Program
                        Console.WriteLine("Exiting...");
                        break;

                    default:
                        //Invalid Action Entered
                        Console.WriteLine("Enter a valid action.");
                        break;

                }
            }
        }
        //Methods for animal interaction
        public void CreateAnimal()
        {
            //Ask the user to enter each piece of animal information
            //This will be assigned to the newly created animal object that will be created and inserted into the databse
            //This new data collection is part of the data and algorithms section
            Console.WriteLine("Enter the animal name");
            animalToCreate.Name = Console.ReadLine();
            Console.WriteLine("Enter the animal breed");
            animalToCreate.Breed = (Console.ReadLine());
            Console.WriteLine("Enter the animal's Color");
            animalToCreate.Color = Console.ReadLine();
            Console.WriteLine("Enter the animal type");
            animalToCreate.AnimalType = Console.ReadLine();
            Console.WriteLine("Enter the animal Sex");
            animalToCreate.Sex = Console.ReadLine();
            Console.WriteLine("Enter the animal Age");
            animalToCreate.Age = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the animal Adoption Status");
            animalToCreate.adoptionStatus = Console.ReadLine();
            Console.WriteLine("Enter the animal date of birth");
            animalToCreate.dateOfBirth = Console.ReadLine();

            //Pass all of the information to the create function
            AnimalShelter.Create(animalToCreate, animalsCollection);
        }
        public void ReadAnimal()
        {
            //Ask the user to enter id and name to enter to get the animal
            Console.WriteLine("Enter the animal Id");
            animalInformation.Id = Console.ReadLine();
            //Console.WriteLine("Enter the animal name");
            //animalInformation.Name = Console.ReadLine();

            //Pass in the information to read an animal
            animalToRead = AnimalShelter.Read(animalInformation, animalsCollection); 
            if (animalToRead != null)
            {
                Console.WriteLine("Animal Information");
                Console.WriteLine($"ID: {animalToRead["_id"]}, Name: {animalToRead["name"]}, Breed: {animalToRead["breed"]}, " +
                           $"Sex: {animalToRead["sex"]}, Color: {animalToRead["color"]}, Age: {animalToRead["age"]}, Adopted: {animalToRead["adopted"]}");
                //Console.WriteLine(animalToRead.Id + " " + animalToRead.Name + " " + animalToRead.Breed + " " + animalToRead.AnimalType + " " + animalToRead.Sex + " " + animalToRead.adoptionStatus);
            }
            else
            {
                Console.WriteLine("There is no animal with that Id");
            }
        }
        public void DeleteAnimal()
        {
            //Ask the user to enter id and name to enter to get the animal
            Console.WriteLine("Enter the animal Id");
            animalInformation.Id = Console.ReadLine();
            //Console.WriteLine("Enter the animal name");
            //animalInformation.Name = Console.ReadLine();

            //Pass in the information to delete an
            animalToDelete = AnimalShelter.Delete(animalInformation, animalsCollection);

            //Console message for animal deleted
            Console.WriteLine($"ID: {animalToDelete["Id"]}, Name: {animalToDelete["name"]}, " + " deleted successfully");
            //Console.WriteLine("Animal Deleted: " + animalToDelete.Id + " " + animalToDelete.Name + " successfully!");
        }
        public void UpdateAnimal()
        {
            //Ask the user to enter id and name to enter to get the animal
            Console.WriteLine("Enter the animal Id");
            animalInformation.Id = Console.ReadLine();
            //Console.WriteLine("Enter the animal name");
            //animalInformation.Name = Console.ReadLine();

            //Pass in information to get the updated animal
            //This will be an initial read
            //animalToUpdate = 

            //We read the animal to see if it exists before trying to update it
            animalToRead = AnimalShelter.Read(animalInformation, animalsCollection);
            if (animalToRead != null)
            {
                //Run through values and ask if the user wants to change
                Console.WriteLine("Enter in a new name or enter in the current name to keep is the same.");
                animalToUpdate.Name = Console.ReadLine();
                Console.WriteLine("Enter in a new breed or enter in the current breed to keep is the same.");
                animalToUpdate.Breed = Console.ReadLine();
                Console.WriteLine("Enter in a new animal type or enter in the current animal type to keep is the same.");
                animalToUpdate.AnimalType = Console.ReadLine();
                Console.WriteLine("Enter in a new Sex or enter in the current Sex to keep is the same.");
                animalToUpdate.Sex = Console.ReadLine();
                Console.WriteLine("Enter in a new age or enter in the current age to keep is the same. ");
                animalToUpdate.Age = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter in a new adoption status or enter in the current adoption status to keep is the same. ");
                animalToUpdate.adoptionStatus = Console.ReadLine();
                Console.WriteLine("Enter in a new date of birth or enter in the current date of birth to keep is the same. : ");
                animalToUpdate.dateOfBirth = Console.ReadLine();

                updatedAnimal = AnimalShelter.Update(animalInformation, animalToUpdate, animalsCollection);
                Console.WriteLine($"ID: {updatedAnimal["_id"]}, Name: {updatedAnimal["name"]}, " + " updated successfully");
            }
            else
            {
                Console.WriteLine("Animal you are trying to update does not exist");
            }

        }
    }
}
