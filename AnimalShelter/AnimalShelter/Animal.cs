using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalShelter
{
    //This is a new animal class for an animal object creation for data that will be used to store in the database
    //This is a new enhancement for data
    public class Animal 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public string AnimalType { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string adoptionStatus { get; set; }
        public string dateOfBirth { get; set; }
        public string Color { get; set; } 
    }
}
