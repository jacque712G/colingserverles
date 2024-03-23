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
    public class Institucion
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nombre"), BsonRepresentation(BsonType.String)]
        public string Nombre { get; set; } = null!;

        [BsonElement("tipo"), BsonRepresentation(BsonType.String)]
        public string Tipo { get; set; } = null!;

        [BsonElement("direccion"), BsonRepresentation(BsonType.String)]
        public string Direccion { get; set; } = null!;

        [BsonElement("descripcion"), BsonRepresentation(BsonType.String)]
        public string Descripcion { get; set; } = null!;

        [BsonElement("estado"), BsonRepresentation(BsonType.String)]
        public string Estado { get; set; } = null!;
      

    }
}
