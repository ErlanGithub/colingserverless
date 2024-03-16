using Azure.Data.Tables;
using Coling.Api.Curriculum.Contratos.Repositorios;
using Coling.Api.Curriculum.Model;
using Coling.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.Api.Curriculum.implementacion.Repositorios
{
    public class EstudiosImplementacion:IEstudiosRepositorio
    {
        //cadena de conexion (Programa/propiedades/depurar)
        private readonly string cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public EstudiosImplementacion(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Estudios";
        }

        public async Task<bool> Create(EstudiosModel estudios)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(estudios);
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Delete(string partitionkey, string rowkey, string etaq)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.DeleteEntityAsync(partitionkey, rowkey);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<EstudiosModel> get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'estudios' and RowKey eq '{id}'";
            await foreach (EstudiosModel item in tablaCliente.QueryAsync<EstudiosModel>(filter: filtro))
            {
                return item;
            }
            return null;
        }

        public async Task<List<EstudiosModel>> GetAll()
        {
            List<EstudiosModel> lista = new List<EstudiosModel>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'estudios'";

            await foreach (EstudiosModel item in tablaCliente.QueryAsync<EstudiosModel>(filter: filtro))
            {
                lista.Add(item);
            }
            return lista;
        }

        public async Task<bool> Update(EstudiosModel estudios)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(estudios, estudios.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
