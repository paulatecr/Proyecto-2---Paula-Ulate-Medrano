using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arca.Shared.Models;
using Microsoft.Data.SqlClient;

namespace Arca.Api.Repositories
{
    public class UsuarioRepository
    {
        private readonly SqlConnectionFactory _factory;
        public UsuarioRepository(SqlConnectionFactory factory) => _factory = factory;

        public async Task<Usuario> GetByCredentialsAsync(string user, string contrasena)
        {
            using var cn = _factory.Create();
            const string sql = @"
SELECT TOP 1 Id, UserID, Nombre, Correo, Contrasena, Rol, Activo
FROM   Usuario
WHERE  (UserID = @user OR Correo = @user)
  AND  Contrasena = @pwd
  AND  Activo = 1";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@user", user ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@pwd", contrasena ?? (object)DBNull.Value);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Usuario
                {
                    Id = (int)rd["Id"],
                    UserId = rd["UserID"].ToString(),
                    Nombre = rd["Nombre"].ToString(),
                    Correo = rd["Correo"].ToString(),
                    Contrasena = rd["Contrasena"].ToString(), // (en producción, NO devolverías la contraseña)
                    Rol = rd["Rol"].ToString(),
                    Activo = (bool)rd["Activo"]
                };
            }
            return null;
        }

        // (Opcional) CRUD básico — útil si ya tienes controladores que los llaman
        public async Task<List<Usuario>> GetAllAsync()
        {
            var list = new List<Usuario>();
            using var cn = _factory.Create();
            using var cmd = new SqlCommand(
                "SELECT Id, UserID, Nombre, Correo, Contrasena, Rol, Activo FROM Usuario ORDER BY Id DESC", cn);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Usuario
                {
                    Id = (int)rd["Id"],
                    UserId = rd["UserID"].ToString(),
                    Nombre = rd["Nombre"].ToString(),
                    Correo = rd["Correo"].ToString(),
                    Contrasena = rd["Contrasena"].ToString(),
                    Rol = rd["Rol"].ToString(),
                    Activo = (bool)rd["Activo"]
                });
            }
            return list;
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand(
                "SELECT Id, UserID, Nombre, Correo, Contrasena, Rol, Activo FROM Usuario WHERE Id=@Id", cn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Usuario
                {
                    Id = (int)rd["Id"],
                    UserId = rd["UserID"].ToString(),
                    Nombre = rd["Nombre"].ToString(),
                    Correo = rd["Correo"].ToString(),
                    Contrasena = rd["Contrasena"].ToString(),
                    Rol = rd["Rol"].ToString(),
                    Activo = (bool)rd["Activo"]
                };
            }
            return null;
        }

        public async Task<int> InsertAsync(Usuario u)
        {
            using var cn = _factory.Create();
            var sql = @"
INSERT INTO Usuario (UserID, Nombre, Correo, Contrasena, Rol, Activo)
VALUES (@UserID, @Nombre, @Correo, @Contrasena, @Rol, @Activo);
SELECT SCOPE_IDENTITY();";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@UserID", u.UserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Nombre", u.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Correo", u.Correo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Contrasena", u.Contrasena ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Rol", u.Rol ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Activo", u.Activo);
            await cn.OpenAsync();
            var obj = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(obj);
        }

        public async Task<bool> UpdateAsync(Usuario u)
        {
            using var cn = _factory.Create();
            var sql = @"
UPDATE Usuario SET
  UserID=@UserID, Nombre=@Nombre, Correo=@Correo,
  Contrasena=@Contrasena, Rol=@Rol, Activo=@Activo
WHERE Id=@Id";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@UserID", u.UserId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Nombre", u.Nombre ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Correo", u.Correo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Contrasena", u.Contrasena ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Rol", u.Rol ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Activo", u.Activo);
            cmd.Parameters.AddWithValue("@Id", u.Id);
            await cn.OpenAsync();
            var n = await cmd.ExecuteNonQueryAsync();
            return n > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("DELETE FROM Usuario WHERE Id=@Id", cn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cn.OpenAsync();
            var n = await cmd.ExecuteNonQueryAsync();
            return n > 0;
        }
    }
}