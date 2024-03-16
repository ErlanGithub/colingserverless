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
    public class ExperienciaImplementacion:IExperienciaRepositorio
    {
        //cadena de conexion (Programa/propiedades/depurar)
        private readonly string cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public ExperienciaImplementacion(IConfiguration conf)
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "ExperienciaLaboral";
        }


        public async Task<bool> Creates(ExperienciaModel experiencia)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(experiencia);
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

        public async Task<ExperienciaModel> get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'profesional' and RowKey eq '{id}'";
            await foreach (ExperienciaModel item in tablaCliente.QueryAsync<ExperienciaModel>(filter: filtro))
            {
                return item;
            }
            return null;
        }

        public async Task<List<ExperienciaModel>> GetAll()
        {
            List<ExperienciaModel> lista = new List<ExperienciaModel>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'profesional'";

            await foreach (ExperienciaModel item in tablaCliente.QueryAsync<ExperienciaModel>(filter: filtro))
            {
                lista.Add(item);
            }
            return lista;
        }

        public async Task<bool> Update(ExperienciaModel experiencia)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(experiencia, experiencia.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
