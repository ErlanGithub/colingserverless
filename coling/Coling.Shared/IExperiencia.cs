using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public interface IExperiencia
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string cargoTitulo { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFinal { get; set; }
        public string estado { get; set;}
    }
}
