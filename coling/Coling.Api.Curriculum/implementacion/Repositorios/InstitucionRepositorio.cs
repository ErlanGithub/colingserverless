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
    public class InstitucionRepositorio : IInstitucionRepositorio
    {
        //cadena de conexion (Programa/propiedades/depurar)
        private readonly string cadenaConexion;
        private readonly string tablaNombre;
        private readonly IConfiguration configuration;

        public InstitucionRepositorio(IConfiguration conf)
        { 
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Institucion";
        }

        public async Task<bool> Delete(string partitionkey, string rowkey, string etag)
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

        public async Task<Institucion> get(string id)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'educacion' and RowKey eq '{id}'";
            await foreach (Institucion institucion in tablaCliente.QueryAsync<Institucion>(filter: filtro))
            {
                return institucion;
            }
            return null;
        }

        public async Task<List<Institucion>> GetAll()
        {
            List<Institucion> lista = new List<Institucion> ();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = $"PartitionKey eq 'educacion'";

            await foreach (Institucion institucion in tablaCliente.QueryAsync<Institucion>(filter:filtro))
            {
                lista.Add(institucion);
            }
            return lista;
        }

        public async Task<bool> Create(Institucion institucion)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpsertEntityAsync(institucion);
                return true;
                    
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Update(Institucion institucion)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
                await tablaCliente.UpdateEntityAsync(institucion, institucion.ETag);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
