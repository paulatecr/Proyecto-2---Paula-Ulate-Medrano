using Proyecto_2___Paula_Ulate_Medrano.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Proyecto_2___Paula_Ulate_Medrano.Repositorios
{
    public class UsuarioRepository
    {
        private readonly string connectionString;

        public UsuarioRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Usuario> GetAll()
        {
            var usuarios = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Usuario";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        ContrasenaHash = reader["ContrasenaHash"].ToString(),
                        Rol = reader["Rol"].ToString(),
                        Activo = (bool)reader["Activo"]
                    });
                }
            }
            return usuarios;
        }

        public Usuario GetById(int id)
        {
            Usuario usuario = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Usuario WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        ContrasenaHash = reader["ContrasenaHash"].ToString(),
                        Rol = reader["Rol"].ToString(),
                        Activo = (bool)reader["Activo"]
                    };
                }
            }
            return usuario;
        }

        public Usuario GetByCorreo(string correo)
        {
            Usuario usuario = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Usuario WHERE Correo = @Correo";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Correo", correo);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        ContrasenaHash = reader["ContrasenaHash"].ToString(),
                        Rol = reader["Rol"].ToString(),
                        Activo = (bool)reader["Activo"]
                    };
                }
            }
            return usuario;
        }

        public void Insert(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Usuario (Nombre, Correo, ContrasenaHash, Rol, Activo)
                             VALUES (@Nombre, @Correo, @ContrasenaHash, @Rol, @Activo)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@ContrasenaHash", usuario.ContrasenaHash);
                cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                cmd.Parameters.AddWithValue("@Activo", usuario.Activo);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Usuario 
                             SET Nombre = @Nombre, Correo = @Correo, ContrasenaHash = @ContrasenaHash, 
                                 Rol = @Rol, Activo = @Activo
                             WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@ContrasenaHash", usuario.ContrasenaHash);
                cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                cmd.Parameters.AddWithValue("@Activo", usuario.Activo);
                cmd.Parameters.AddWithValue("@Id", usuario.Id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Usuario WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}