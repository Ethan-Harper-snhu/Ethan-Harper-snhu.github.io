using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnimalShelter
{
    public class AnimalShelterHome
    {
       
        //Used to Create an Animal to add into the Animal Shelter
        public int Create(Animal animal, IMongoCollection<BsonDocument> animalsCollection)
        {
            //Here we would insert a record into the database this will be worked on when I develop the database enhancement
            //Create a new animal document
            var animalToInsert = new BsonDocument
            {
                { "animalType", animal.AnimalType},
                { "breed", animal.Breed },
                { "name", animal.Name },
                { "sex", animal.Sex },
                { "color", animal.Color },
                { "dob", animal.dateOfBirth }, //cannot insert dob with this type had to change to string
                { "adopted", animal.adoptionStatus },
                { "age", animal.Age }
             };

            //Insert the animal into the database
            try
            {
                animalsCollection.InsertOne(animalToInsert);
                Console.WriteLine("Animal added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding animal: " + ex.Message);
            }
            return 1;
        }
        public BsonDocument Read(Animal animalSelected, IMongoCollection<BsonDocument> animalsCollection)
        {

            //Took me a while to figure out why I cannot find the id realized string does not work and it needs to be object Id
            //I realised I could get the value when I queried based on the animal name
            //Need to handle it this way because the Id gets read from the console as a string and then we need to convert it to a BsonDocument ObjectId for MongoDB
            ObjectId animalId;
            if (!ObjectId.TryParse(animalSelected.Id, out animalId))
            {
                Console.WriteLine("Invalid animal ID format.");
                return null;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("_id", animalId);
            BsonDocument animal = animalsCollection.Find(filter).FirstOrDefault();

            if (animal == null)
            {
                Console.WriteLine("No animals found in the database.");
                return null;
            }
            else
            {

                return animal;
            }
        }
        public BsonDocument Update(Animal animal, Animal newUpdates, IMongoCollection<BsonDocument> animalsCollection)
        {
           
            //Here we would update a record in the database this will be worked on when I develop the database enhancement
            //I will return an animal that will state is been updated temporarly for this first part
            //Need to convert the animaal Id we need to update
            ObjectId animalId;
            if (!ObjectId.TryParse(animal.Id, out animalId))
            {
                Console.WriteLine("Invalid animal ID format.");
                return null;
            }

            //get the animal to update
            //the animal varaible passed in used to get the old data
            var filter = Builders<BsonDocument>.Filter.Eq("_id", animalId);
            BsonDocument animalSelected = animalsCollection.Find(filter).FirstOrDefault();

            if(animalSelected == null)
            {
                Console.WriteLine("Error finding Animal");
                return null;
            }

            //Created the Bson ducoment for the animal to update
            //replace the values with the newly entered animal values
            //the newUpdates variable is used to pass in the data to be changed
            var update = Builders<BsonDocument>.Update
                .Set("breed", string.IsNullOrEmpty(newUpdates.Breed) ? animalSelected["breed"].AsString : newUpdates.Breed)
                .Set("name", string.IsNullOrEmpty(newUpdates.Name) ? animalSelected["name"].AsString : newUpdates.Name)
                .Set("sex", string.IsNullOrEmpty(newUpdates.Sex) ? animalSelected["sex"].AsString : newUpdates.Sex)
                .Set("color", string.IsNullOrEmpty(newUpdates.Color) ? animalSelected["color"].AsString : newUpdates.Color)
                .Set("dob", newUpdates.dateOfBirth)
                .Set("adopted", newUpdates.adoptionStatus)
                .Set("age", newUpdates.Age);

            // animalSelected = update.ToBsonDocument();
            

            try
            {
                //Update the animal in the database
                animalsCollection.UpdateOne(filter, update);
                animalSelected = animalsCollection.Find(filter).FirstOrDefault();
                return animalSelected;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating animal: " + ex.Message);
                return null;
            }


           
        }
        public BsonDocument Delete(Animal animal, IMongoCollection<BsonDocument> animalsCollection)
        {
            // Animal animal3 = new Animal();
            //Here we would update a record in the database this will be worked on when I develop the database enhancement
            //I will return an animal that will state is been updated temporarly for this first part
            //animal3.Name = animal.Name;

            ObjectId animalId;
            if (!ObjectId.TryParse(animal.Id, out animalId))
            {
                Console.WriteLine("Invalid animal ID format.");
                return null;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("_id", animal.Id);
            BsonDocument animaldocument = animalsCollection.Find(filter).FirstOrDefault();


            try
            {
                //Delete the animal from the database
                animalsCollection.DeleteOne(filter);
                return animaldocument;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting animal: " + ex.Message);
                return animaldocument;
            }

            
        }
    }
}
