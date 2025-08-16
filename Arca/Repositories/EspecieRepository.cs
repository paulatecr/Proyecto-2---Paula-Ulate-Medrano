using Arca.Shared.Models;
using Microsoft.Data.SqlClient;

namespace Arca.Api.Repositories
{
    public class EspecieRepository
    {
        private readonly SqlConnectionFactory _factory;
        public EspecieRepository(SqlConnectionFactory factory) => _factory = factory;

        public async Task<List<Especie>> GetAllAsync()
        {
            var list = new List<Especie>();
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("SELECT Id, NombreCientifico, NombreComun, Familia, Descripcion FROM Especie ORDER BY Id DESC", cn);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Especie
                {
                    Id = (int)rd["Id"],
                    NombreCientifico = rd["NombreCientifico"].ToString() ?? "",
                    NombreComun = rd["NombreComun"].ToString() ?? "",
                    Familia = rd["Familia"].ToString() ?? "",
                    Descripcion = rd["Descripcion"] == DBNull.Value ? null : rd["Descripcion"].ToString()
                });
            }
            return list;
        }

        public async Task<Especie?> GetByIdAsync(int id)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand(
                "SELECT Id, NombreCientifico, NombreComun, Familia, Descripcion FROM Especie WHERE Id=@Id", cn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Especie
                {
                    Id = (int)rd["Id"],
                    NombreCientifico = rd["NombreCientifico"].ToString() ?? "",
                    NombreComun = rd["NombreComun"].ToString() ?? "",
                    Familia = rd["Familia"].ToString() ?? "",
                    Descripcion = rd["Descripcion"] == DBNull.Value ? null : rd["Descripcion"].ToString()
                };
            }
            return null;
        }

        public async Task<int> InsertAsync(Especie e)
        {
            using var cn = _factory.Create();
            var sql = @"
INSERT INTO Especie (NombreCientifico, NombreComun, Familia, Descripcion)
VALUES (@NombreCientifico, @NombreComun, @Familia, @Descripcion);
SELECT SCOPE_IDENTITY();";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@NombreCientifico", (object?)e.NombreCientifico ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NombreComun", (object?)e.NombreComun ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Familia", (object?)e.Familia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)e.Descripcion ?? DBNull.Value);
            await cn.OpenAsync();
            var obj = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(obj);
        }

        public async Task<bool> UpdateAsync(Especie e)
        {
            using var cn = _factory.Create();
            var sql = @"
UPDATE Especie SET
  NombreCientifico=@NombreCientifico,
  NombreComun=@NombreComun,
  Familia=@Familia,
  Descripcion=@Descripcion
WHERE Id=@Id";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@NombreCientifico", (object?)e.NombreCientifico ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NombreComun", (object?)e.NombreComun ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Familia", (object?)e.Familia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)e.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", e.Id);
            await cn.OpenAsync();
            var n = await cmd.ExecuteNonQueryAsync();
            return n > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("DELETE FROM Especie WHERE Id=@Id", cn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cn.OpenAsync();
            var n = await cmd.ExecuteNonQueryAsync();
            return n > 0;
        }
    }
}