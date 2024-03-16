using Azure;
using Azure.Data.Tables;
using Coling.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.Curriculum.Model
{
    public class ExperienciaModel : IExperiencia, ITableEntity
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string cargoTitulo { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFinal { get; set; }
        public string estado { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
