using Coling.Api.Curriculum.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.Curriculum.Contratos.Repositorios
{
    public interface IInstitucionRepositorio
    {
        public Task<bool> Create(Institucion institucion);
        public Task<bool> Update(Institucion institucion);
        public Task<bool> Delete(string partitionkey, string rowkey,string etaq);
        public Task<List<Institucion>> GetAll();
        public Task<Institucion> get(string id);
    }
}
