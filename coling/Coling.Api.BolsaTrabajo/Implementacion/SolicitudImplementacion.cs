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
using ZstdSharp;

namespace Coling.Api.BolsaTrabajo.Implementacion
{
    public class SolicitudImplementacion : ISolicitudRepositorios
    {
        private readonly string cadenaConexion;
        private readonly IMongoCollection<SolicitudModel> coleccion;

        public SolicitudImplementacion(IConfiguration conf)
        {
            cadenaConexion = conf.GetSection("MongoClient").Value;
            var client = new MongoClient(cadenaConexion);
            var database = client.GetDatabase("BolsaTrabajo");
            coleccion = database.GetCollection<SolicitudModel>("Solicitud");
        }

        public async Task<bool> Create(SolicitudModel solicitud)
        {
            try
            {
                await coleccion.InsertOneAsync(solicitud);
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
                var filter = Builders<SolicitudModel>.Filter.Eq("_id",id);
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

        public async Task<SolicitudModel> get(string id)
        {
            try
            {
                SolicitudModel solicitud = coleccion.Find(x => x._id == id).FirstOrDefault();
                return solicitud;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<SolicitudModel>> GetAll()
        {
            var lista = await coleccion.Find(new BsonDocument()).ToListAsync();
            return lista;
        }

        public async Task<bool> Update(SolicitudModel solicitud, string id)
        {
            try
            {
                coleccion.ReplaceOne(x => x._id == id, solicitud);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
