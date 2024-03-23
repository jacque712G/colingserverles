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
    public class ExperienciaLaboralRepositorio : IExperienciaLaboralRepositorio
    {
        private readonly string? cadenaConexion;
        private readonly string? tablaNombre;
        private readonly IConfiguration configuration;
        public ExperienciaLaboralRepositorio(IConfiguration conf) 
        { 
            configuration = conf;
            cadenaConexion = Environment.GetEnvironmentVariable("cadenaconexion");
            tablaNombre = "ExperienciaLaboral";
        }      

        public async Task<bool> Create(ExperienciaLaboral experiencia)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
                await tablaCliente.UpsertEntityAsync(experiencia);
                return true;
            }
            catch (Exception)
            {

                return false ;
            }
        }

        public async Task<bool> Delete(string partitionkey, string rowkey)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
                await tablaCliente.DeleteEntityAsync(partitionkey,rowkey);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<ExperienciaLaboral>> GetAll()
        {
            List<ExperienciaLaboral> lista=new List<ExperienciaLaboral>();
            var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
            var experiencias = tablaCliente.QueryAsync<ExperienciaLaboral>(filter:"");

            await foreach (ExperienciaLaboral experiencia in experiencias)
            {
                lista.Add(experiencia);
            }
            return lista;
        }

        public async Task<ExperienciaLaboral> GetById(string rowkey)
        {
            var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
            var experiencia =await tablaCliente.GetEntityAsync<ExperienciaLaboral>("Experiencia",rowkey);
            return experiencia.Value;
        }

        public async Task<bool> Update(ExperienciaLaboral experiencia)
        {
            try
            {
                var tablaCliente = new TableClient(cadenaConexion,tablaNombre);
                await tablaCliente.UpdateEntityAsync(experiencia,experiencia.ETag);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<List<ExperienciaLaboral>> BuscarAfiliadoExperiencia(int idAfiliado)
        {
            List<ExperienciaLaboral> lista = new List<ExperienciaLaboral>();
            var tablaCliente = new TableClient(cadenaConexion, tablaNombre);
            var filtro = "IdAfiliado eq " + idAfiliado + "";
            var experiencias = tablaCliente.QueryAsync<ExperienciaLaboral>(filter: filtro);

            await foreach (ExperienciaLaboral experiencia in experiencias)
            {
                lista.Add(experiencia);
            }
            return lista;
        }
    }
}
