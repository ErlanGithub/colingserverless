using Coling.Api.BolsaTrabajo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.BolsaTrabajo.Repositorios
{
    public interface IOfertaLabRepositorios
    {
        public Task<bool> Create(OfertaLaboralModel ofertaLab);
        public Task<bool> Update(OfertaLaboralModel ofertaLab, string id);
        public Task<bool> Delete(string id);
        public Task<List<OfertaLaboralModel>> GetAll();
        public Task<OfertaLaboralModel> get(string id);
    }
}
