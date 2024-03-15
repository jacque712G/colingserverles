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
    public class Profesion : IProfesion,ITableEntity
    {
        public string NombreProfesion { get; set; } = null!;
        public string NombreGrado { get; set; } = null!;
        public string Estado { get ; set ; } = null!;
        public string PartitionKey { get; set; } = null!;
        public string RowKey { get; set; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
