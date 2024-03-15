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
    public class ExperienciaLaboral : IExperienciaLaboral,ITableEntity
    {
        public int IdAfiliado { get ; set; }
        public string IdInstitucion { get; set; } = null!;
        public string CargoTitulo { get; set ; } = null!;
        public DateTimeOffset FechaInicio { get ; set ; }
        public DateTimeOffset FechaFin { get ; set ; }
        public string Estado { get ; set; } = null!;
        public string PartitionKey { get; set; } = null!;
        public string RowKey { get; set; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
