using Coling.Api.BolsaTrabajo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.BolsaTrabajo.Repositorios
{
    public interface ISolicitudRepositorios
    {
        public Task<bool> Create(SolicitudModel solicitud);
        public Task<bool> Update(SolicitudModel solicitud, string id);
        public Task<bool> Delete(string id);
        public Task<List<SolicitudModel>> GetAll();
        public Task<SolicitudModel> get(string id);
    }
}
