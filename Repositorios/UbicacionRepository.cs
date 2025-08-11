using Proyecto_2___Paula_Ulate_Medrano.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Repositorios
{
    public class UbicacionRepository
    {
        private readonly string connectionString;

        public UbicacionRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Ubicacion> GetAll()
        {
            var ubicaciones = new List<Ubicacion>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Ubicacion";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ubicaciones.Add(new Ubicacion
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"]?.ToString(),
                        Condiciones = reader["Condiciones"]?.ToString()
                    });
                }
            }
            return ubicaciones;
        }

        public Ubicacion GetById(int id)
        {
            Ubicacion ubicacion = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Ubicacion WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ubicacion = new Ubicacion
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"]?.ToString(),
                        Condiciones = reader["Condiciones"]?.ToString()
                    };
                }
            }
            return ubicacion;
        }

        public void Insert(Ubicacion ubicacion)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Ubicacion (Nombre, Descripcion, Condiciones)
                             VALUES (@Nombre, @Descripcion, @Condiciones)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", ubicacion.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", (object)ubicacion.Descripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Condiciones", (object)ubicacion.Condiciones ?? DBNull.Value);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Ubicacion ubicacion)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Ubicacion
                             SET Nombre = @Nombre, Descripcion = @Descripcion, Condiciones = @Condiciones
                             WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", ubicacion.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", (object)ubicacion.Descripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Condiciones", (object)ubicacion.Condiciones ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", ubicacion.Id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Ubicacion WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private Ubicacion MapUbicacion(SqlDataReader rd)
        {
            return new Ubicacion
            {
                Id = (int)rd["Id"],
                Nombre = rd["Nombre"].ToString(),
                Descripcion = rd["Descripcion"]?.ToString(),
                Condiciones = rd["Condiciones"]?.ToString()
            };
        }


    }
}