using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace MenuPlan.Model
{
    public class Menu
    {
        [BsonId] // Primärschlüssel des Dokuments
        [BsonRepresentation(BsonType.ObjectId)] // Übergabe des Parameters als Typ string anstelle einer ObjectId-Struktur zu ermöglichen. Mongo behandelt die Konvertierung von string zu ObjectId.
        public string Id { get; set; }

        [BsonElement("Name")]
        [JsonProperty("Name")]
        public string Name { get; set; }

        public decimal Preis { get; set; }       
    }
}
