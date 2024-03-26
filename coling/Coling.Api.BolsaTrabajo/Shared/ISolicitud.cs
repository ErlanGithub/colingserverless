using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.BolsaTrabajo.Shared
{
    public interface ISolicitud
    {
        public string _id { get; set; }
        public string idafiliado { get; set; }
        public string Nombres { get; set; }
        public string apellidos { get; set; }
        public string fechaPublicacion { get; set; }
        public float PretensionSalarial { get; set; }
        public string idoferta { get; set; }
    }
}
