using Arca.Shared.Models;
using System.Data.SqlClient;

namespace Arca.Api.Repositories
{
    public class UbicacionRepository
    {
        private readonly SqlConnectionFactory _factory;
        public UbicacionRepository(SqlConnectionFactory factory) => _factory = factory;

        public async Task<List<Ubicacion>> GetAllAsync()
        {
            var list = new List<Ubicacion>();
            using var cn = _factory.Create();
            using var cmd = new SqlCommand(
                "SELECT Id, Nombre, Descripcion, Condiciones FROM Ubicacion ORDER BY Id DESC", cn);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Ubicacion
                {
                    Id = (int)rd["Id"],
                    Nombre = rd["Nombre"].ToString() ?? "",
                    Descripcion = rd["Descripcion"] == DBNull.Value ? null : rd["Descripcion"].ToString(),
                    Condiciones = rd["Condiciones"] == DBNull.Value ? null : rd["Condiciones"].ToString()
                });
            }
            return list;
        }

        public async Task<Ubicacion?> GetByIdAsync(int id)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand(
                "SELECT Id, Nombre, Descripcion, Condiciones FROM Ubicacion WHERE Id=@Id", cn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Ubicacion
                {
                    Id = (int)rd["Id"],
                    Nombre = rd["Nombre"].ToString() ?? "",
                    Descripcion = rd["Descripcion"] == DBNull.Value ? null : rd["Descripcion"].ToString(),
                    Condiciones = rd["Condiciones"] == DBNull.Value ? null : rd["Condiciones"].ToString()
                };
            }
            return null;
        }

        public async Task<int> InsertAsync(Ubicacion u)
        {
            using var cn = _factory.Create();
            var sql = @"
INSERT INTO Ubicacion (Nombre, Descripcion, Condiciones)
VALUES (@Nombre, @Descripcion, @Condiciones);
SELECT SCOPE_IDENTITY();";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)u.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Condiciones", (object?)u.Condiciones ?? DBNull.Value);
            await cn.OpenAsync();
            var obj = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(obj);
        }

        public async Task<bool> UpdateAsync(Ubicacion u)
        {
            using var cn = _factory.Create();
            var sql = @"
UPDATE Ubicacion SET
  Nombre=@Nombre,
  Descripcion=@Descripcion,
  Condiciones=@Condiciones
WHERE Id=@Id";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)u.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Condiciones", (object?)u.Condiciones ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", u.Id);
            await cn.OpenAsync();
            var n = await cmd.ExecuteNonQueryAsync();
            return n > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("DELETE FROM Ubicacion WHERE Id=@Id", cn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cn.OpenAsync();
            var n = await cmd.ExecuteNonQueryAsync();
            return n > 0;
        }
    }
}