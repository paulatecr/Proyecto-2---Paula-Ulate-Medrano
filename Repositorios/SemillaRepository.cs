using Proyecto_2___Paula_Ulate_Medrano.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Repositorios
{
    public class SemillaRepository
    {
        private readonly string connectionString;

        public SemillaRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Semilla> GetAll()
        {
            var semillas = new List<Semilla>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Semilla";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    semillas.Add(new Semilla
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        EspecieId = (int)reader["EspecieId"],
                        UbicacionId = (int)reader["UbicacionId"],
                        Cantidad = (int)reader["Cantidad"],
                        FechaAlmacenamiento = (DateTime)reader["FechaAlmacenamiento"],
                        FechaCreacion = (DateTime)reader["FechaCreacion"],
                        CreadoPor = (int)reader["CreadoPor"],
                        FechaModificacion = reader["FechaModificacion"] == DBNull.Value ? null : (DateTime?)reader["FechaModificacion"],
                        ModificadoPor = reader["ModificadoPor"] == DBNull.Value ? null : (int?)reader["ModificadoPor"]
                    });
                }
            }
            return semillas;
        }

        public Semilla GetById(int id)
        {
            Semilla semilla = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Semilla WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    semilla = new Semilla
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        EspecieId = (int)reader["EspecieId"],
                        UbicacionId = (int)reader["UbicacionId"],
                        Cantidad = (int)reader["Cantidad"],
                        FechaAlmacenamiento = (DateTime)reader["FechaAlmacenamiento"],
                        FechaCreacion = (DateTime)reader["FechaCreacion"],
                        CreadoPor = (int)reader["CreadoPor"],
                        FechaModificacion = reader["FechaModificacion"] == DBNull.Value ? null : (DateTime?)reader["FechaModificacion"],
                        ModificadoPor = reader["ModificadoPor"] == DBNull.Value ? null : (int?)reader["ModificadoPor"]
                    };
                }
            }
            return semilla;
        }

        public void Insert(Semilla semilla)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Semilla 
                (Nombre, EspecieId, UbicacionId, Cantidad, FechaAlmacenamiento, 
                 FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor)
                VALUES 
                (@Nombre, @EspecieId, @UbicacionId, @Cantidad, @FechaAlmacenamiento, 
                 @FechaCreacion, @CreadoPor, @FechaModificacion, @ModificadoPor)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", semilla.Nombre);
                cmd.Parameters.AddWithValue("@EspecieId", semilla.EspecieId);
                cmd.Parameters.AddWithValue("@UbicacionId", semilla.UbicacionId);
                cmd.Parameters.AddWithValue("@Cantidad", semilla.Cantidad);
                cmd.Parameters.AddWithValue("@FechaAlmacenamiento", semilla.FechaAlmacenamiento);
                cmd.Parameters.AddWithValue("@FechaCreacion", semilla.FechaCreacion);
                cmd.Parameters.AddWithValue("@CreadoPor", semilla.CreadoPor);
                //cmd.Parameters.AddWithValue("@FechaModificacion", (object?)semilla.FechaModificacion ?? DBNull.Value);
               // cmd.Parameters.AddWithValue("@ModificadoPor", (object?)semilla.ModificadoPor ?? DBNull.Value);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Semilla semilla)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Semilla SET 
                Nombre = @Nombre,
                EspecieId = @EspecieId,
                UbicacionId = @UbicacionId,
                Cantidad = @Cantidad,
                FechaAlmacenamiento = @FechaAlmacenamiento,
                FechaModificacion = @FechaModificacion,
                ModificadoPor = @ModificadoPor
                WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", semilla.Nombre);
                cmd.Parameters.AddWithValue("@EspecieId", semilla.EspecieId);
                cmd.Parameters.AddWithValue("@UbicacionId", semilla.UbicacionId);
                cmd.Parameters.AddWithValue("@Cantidad", semilla.Cantidad);
                cmd.Parameters.AddWithValue("@FechaAlmacenamiento", semilla.FechaAlmacenamiento);
                //cmd.Parameters.AddWithValue("@FechaModificacion", (object?)semilla.FechaModificacion ?? DBNull.Value);
                //cmd.Parameters.AddWithValue("@ModificadoPor", (object?)semilla.ModificadoPor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", semilla.Id);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Semilla WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}