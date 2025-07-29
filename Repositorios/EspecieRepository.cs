using Proyecto_2___Paula_Ulate_Medrano.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Repositorios
{
    public class EspecieRepository
    {
        private readonly string connectionString;

        public EspecieRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Especie> GetAll()
        {
            var especies = new List<Especie>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Especie";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    especies.Add(new Especie
                    {
                        Id = (int)reader["Id"],
                        NombreCientifico = reader["NombreCientifico"].ToString(),
                        NombreComun = reader["NombreComun"].ToString(),
                        Familia = reader["Familia"].ToString(),
                        Descripcion = reader["Descripcion"]?.ToString()
                    });
                }
            }
            return especies;
        }

        public Especie GetById(int id)
        {
            Especie especie = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Especie WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    especie = new Especie
                    {
                        Id = (int)reader["Id"],
                        NombreCientifico = reader["NombreCientifico"].ToString(),
                        NombreComun = reader["NombreComun"].ToString(),
                        Familia = reader["Familia"].ToString(),
                        Descripcion = reader["Descripcion"]?.ToString()
                    };
                }
            }
            return especie;
        }

        public void Insert(Especie especie)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Especie 
                (NombreCientifico, NombreComun, Familia, Descripcion)
                VALUES (@NombreCientifico, @NombreComun, @Familia, @Descripcion)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@NombreCientifico", especie.NombreCientifico);
                cmd.Parameters.AddWithValue("@NombreComun", especie.NombreComun);
                cmd.Parameters.AddWithValue("@Familia", especie.Familia);
                cmd.Parameters.AddWithValue("@Descripcion", (object)especie.Descripcion ?? DBNull.Value);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Especie especie)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Especie SET 
                NombreCientifico = @NombreCientifico,
                NombreComun = @NombreComun,
                Familia = @Familia,
                Descripcion = @Descripcion
                WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@NombreCientifico", especie.NombreCientifico);
                cmd.Parameters.AddWithValue("@NombreComun", especie.NombreComun);
                cmd.Parameters.AddWithValue("@Familia", especie.Familia);
                cmd.Parameters.AddWithValue("@Descripcion", (object)especie.Descripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", especie.Id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Especie WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}