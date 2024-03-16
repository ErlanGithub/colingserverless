using Coling.Api.Curriculum.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.Curriculum.Contratos.Repositorios
{
    public interface IEstudiosRepositorio
    {
        public Task<bool> Create(EstudiosModel estudios);
        public Task<bool> Update(EstudiosModel estudios);
        public Task<bool> Delete(string partitionkey, string rowkey, string etaq);
        public Task<List<EstudiosModel>> GetAll();
        public Task<EstudiosModel> get(string id);
    }
}
