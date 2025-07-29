using Proyecto_2___Paula_Ulate_Medrano.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Repositorios
{
    public class ReporteprogramadoRepository
    {
        private readonly string connectionString;

        public ReporteProgramadoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<ReporteProgramado> GetAll()
        {
            var reportes = new List<ReporteProgramado>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ReporteProgramado";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    reportes.Add(new ReporteProgramado
                    {
                        Id = (int)reader["Id"],
                        NombreReporte = reader["NombreReporte"].ToString(),
                        Frecuencia = reader["Frecuencia"].ToString(),
                        Parametros = reader["Parametros"]?.ToString(),
                        ProximoEnvio = (DateTime)reader["ProximoEnvio"],
                        Destinatarios = reader["Destinatarios"]?.ToString(),
                        FechaCreacion = (DateTime)reader["FechaCreacion"],
                        CreadoPor = (int)reader["CreadoPor"]
                    });
                }
            }
            return reportes;
        }

        public ReporteProgramado GetById(int id)
        {
            ReporteProgramado reporte = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ReporteProgramado WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    reporte = new ReporteProgramado
                    {
                        Id = (int)reader["Id"],
                        NombreReporte = reader["NombreReporte"].ToString(),
                        Frecuencia = reader["Frecuencia"].ToString(),
                        Parametros = reader["Parametros"]?.ToString(),
                        ProximoEnvio = (DateTime)reader["ProximoEnvio"],
                        Destinatarios = reader["Destinatarios"]?.ToString(),
                        FechaCreacion = (DateTime)reader["FechaCreacion"],
                        CreadoPor = (int)reader["CreadoPor"]
                    };
                }
            }
            return reporte;
        }

        public void Insert(ReporteProgramado reporte)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO ReporteProgramado 
                (NombreReporte, Frecuencia, Parametros, ProximoEnvio, Destinatarios, FechaCreacion, CreadoPor)
                VALUES 
                (@NombreReporte, @Frecuencia, @Parametros, @ProximoEnvio, @Destinatarios, @FechaCreacion, @CreadoPor)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@NombreReporte", reporte.NombreReporte);
                cmd.Parameters.AddWithValue("@Frecuencia", reporte.Frecuencia);
                cmd.Parameters.AddWithValue("@Parametros", (object)reporte.Parametros ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProximoEnvio", reporte.ProximoEnvio);
                cmd.Parameters.AddWithValue("@Destinatarios", (object)reporte.Destinatarios ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaCreacion", reporte.FechaCreacion);
                cmd.Parameters.AddWithValue("@CreadoPor", reporte.CreadoPor);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM ReporteProgramado WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}