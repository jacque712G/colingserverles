using Azure.Data.Tables;
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Modelo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coling.API.Curriculum.Implementacion.Repositorio
{
    public class ProfesionRepositorio : IProfesionRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string? tablaNombre;
        private readonly IConfiguration configuration;

        public ProfesionRepositorio(IConfiguration conf) 
        {
            configuration = conf;
            cadenaConexion = configuration.GetSection("cadenaconexion").Value;
            tablaNombre = "Profesion";
        }
        public async Task<bool> Create(Profesion profesion)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
                await tablaCliente.UpsertEntityAsync(profesion);
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

        public async Task<List<Profesion>> GetAll()
        {
            List<Profesion> lista= new List<Profesion>();
            var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
            var profesiones=tablaCliente.QueryAsync<Profesion>(filter:"");

            await foreach (Profesion profesion in profesiones)
            {
                lista.Add(profesion);
            }
            return lista;
        }

        public async Task<Profesion> GetById(string rowkey)
        {
            var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
            var profesion = await tablaCliente.GetEntityAsync<Profesion>("Profesion",rowkey);
            return profesion.Value;
        }

        public async Task<bool> Update(Profesion profesion)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
                await tablaCliente.UpdateEntityAsync(profesion,profesion.ETag);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
