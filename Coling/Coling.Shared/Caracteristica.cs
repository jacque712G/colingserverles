using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    [Serializable, BsonIgnoreExtraElements]
    public class Caracteristica
    {
        [BsonElement("nombre"), BsonRepresentation(BsonType.String)]
        public string? nombre { get; set; }
    }
}
