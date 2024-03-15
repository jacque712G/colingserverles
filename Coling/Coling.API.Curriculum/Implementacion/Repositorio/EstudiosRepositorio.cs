using Azure.Data.Tables;
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Implementacion.Repositorio
{
    public class EstudiosRepositorio : IEstudiosRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string? tablaNombre;
        private readonly IConfiguration configuration;
        public EstudiosRepositorio(IConfiguration conf) 
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Estudios";

        }
        public async Task<bool> Create(Estudios estudios)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
                await tablaCliente.UpsertEntityAsync(estudios);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> Delete(string partitionkey, string rowkey)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
                await tablaCliente.DeleteEntityAsync(partitionkey, rowkey);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<Estudios>> GetAll()
        {
            List<Estudios> lista=new List<Estudios>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);           
            var estudios = tablaCliente.QueryAsync<Estudios>(filter:"");
            await foreach (Estudios estudio in estudios)
            {
                lista.Add(estudio);
            }
            return lista;
            
            
        }

        public async Task<Estudios> GetById(string rowkey)
        {
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var estudios = await tablaCliente.GetEntityAsync<Estudios>("Academico", rowkey);
            return estudios.Value;
        }

        public async Task<bool> Update(Estudios estudios)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
                await tablaCliente.UpdateEntityAsync(estudios,estudios.ETag);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
