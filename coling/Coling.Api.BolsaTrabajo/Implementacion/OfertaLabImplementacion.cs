using Coling.Api.BolsaTrabajo.Model;
using Coling.Api.BolsaTrabajo.Repositorios;
using Coling.Api.BolsaTrabajo.Shared;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.BolsaTrabajo.Implementacion
{
    public class OfertaLabImplementacion : IOfertaLabRepositorios
    {
        private readonly string cadenaConexion;
        private readonly IMongoCollection<OfertaLaboralModel> coleccion;

        public OfertaLabImplementacion(IConfiguration conf)
        {
            cadenaConexion = conf.GetSection("MongoClient").Value;
            var client = new MongoClient(cadenaConexion);
            var database = client.GetDatabase("BolsaTrabajo");
            coleccion = database.GetCollection<OfertaLaboralModel>("OfertaLaboral");
        }

        public async Task<bool> Create(OfertaLaboralModel ofertaLab)
        {
            try
            {
                await coleccion.InsertOneAsync(ofertaLab);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var filter = Builders<OfertaLaboralModel>.Filter.Eq("_id", id);
                var result = await coleccion.DeleteOneAsync(filter);
                if (result.DeletedCount > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<OfertaLaboralModel> get(string id)
        {
            try
            {
                OfertaLaboralModel oferta = coleccion.Find(x => x._id == id).FirstOrDefault();
                return oferta;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<OfertaLaboralModel>> GetAll()
        {
            var lista = await coleccion.Find(new BsonDocument()).ToListAsync();
            return lista;
        }

        public async Task<bool> Update(OfertaLaboralModel ofertaLab, string id)
        {
            try
            {
                coleccion.ReplaceOne(x => x._id == id, ofertaLab);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
