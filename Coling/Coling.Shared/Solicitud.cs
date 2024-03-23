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
    public class Solicitud
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("idAfiliado"), BsonRepresentation(BsonType.Int32)]
        public int IdAfiliado { get; set; }

        [BsonElement("idOferta"), BsonRepresentation(BsonType.String)]
        public string IdOferta { get; set; } = null!;

        [BsonElement("nombre"), BsonRepresentation(BsonType.String)]
        public string Nombre { get; set; } = null!;

        [BsonElement("apellidos"), BsonRepresentation(BsonType.String)]
        public string apellidos { get; set; } = null!;

        [BsonElement("fechaPostulacion"), BsonRepresentation(BsonType.DateTime)]
        public DateTime FechaPostulacion { get; set; }

        [BsonElement("pretensionSalarial"), BsonRepresentation(BsonType.Double)]
        public double PretensionSalarial { get; set; }
        [BsonElement("acercade"), BsonRepresentation(BsonType.String)]
        public string AcercaDe { get; set; } = null!;

        [BsonElement("curriculum"), BsonRepresentation(BsonType.String)]
        public string? Curriculum { get; set; }

        [BsonElement("estado"), BsonRepresentation(BsonType.String)]
        public string Estado { get; set; } = null!;
    }
}
