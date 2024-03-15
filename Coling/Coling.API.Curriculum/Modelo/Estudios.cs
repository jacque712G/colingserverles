using Azure;
using Azure.Data.Tables;
using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Modelo
{
    public class Estudios : IEstudios,ITableEntity
    {
        public int IdAfiliado { get; set; }
        public string IdProfesion { get; set; } = null!;
        public string IdInstitucion { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string TituloRecibido { get; set; } = null!;
        public int Anio { get; set; }
        public string Estado { get ; set ; } = null!;
        public string PartitionKey { get ; set ; } = null!;
        public string RowKey { get ; set ; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
