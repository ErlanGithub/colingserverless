using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Shared
{
    public interface IEstudios
    {
        public string nombre {  get; set; }
        public string apellido { get; set; }
        public string gradroAcademico {  get; set; }
        public string gradoEstado {  get; set; }
        public string nombreProfesion { get; set; }
        public string profesionEstado { get; set; }
        public string tituloRecibido { get; set; }
        public string anio {  get; set; }
        public string tituloEstado { get; set; }
    }
}
