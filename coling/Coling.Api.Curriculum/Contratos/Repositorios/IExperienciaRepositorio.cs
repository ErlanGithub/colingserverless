using Coling.Api.Curriculum.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.Curriculum.Contratos.Repositorios
{
     public interface IExperienciaRepositorio
    {
        public Task<bool> Creates(ExperienciaModel experiencia);
        public Task<bool> Update(ExperienciaModel experiencia);
        public Task<bool> Delete(string partitionkey, string rowkey, string etaq);
        public Task<List<ExperienciaModel>> GetAll();
        public Task<ExperienciaModel> get(string id);
    }
}
