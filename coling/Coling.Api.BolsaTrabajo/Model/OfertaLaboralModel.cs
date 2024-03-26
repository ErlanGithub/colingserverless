using Coling.Api.BolsaTrabajo.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.BolsaTrabajo.Model
{
    public class OfertaLaboralModel : IOfertaLaboral
    {
        public string _id { get; set; }
        public int idinstitucion { get; set; }
        public string fechaOferta { get; set; }
        public string fechaLimite { get; set; }
        public string descripcion { get; set; }
        public string titulocargo { get; set; }
        public string tipocontrato { get; set; }
        public string area { get; set; }
        public string[] carasteristicas { get; set; }
        public string estado { get; set; }
    }
}
