using Arca.Shared.Models;
using System.Data.SqlClient;

namespace Arca.Api.Repositories
{
    public class SemillaRepository
    {
        private readonly SqlConnectionFactory _factory;
        public SemillaRepository(SqlConnectionFactory factory) => _factory = factory;

        public async Task<List<Semilla>> GetAllAsync()
        {
            var list = new List<Semilla>();
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("SELECT * FROM Semilla", cn);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new Semilla
                {
                    Id = (int)rd["Id"],
                    Nombre = rd["Nombre"].ToString() ?? "",
                    EspecieId = (int)rd["EspecieId"],
                    UbicacionId = (int)rd["UbicacionId"],
                    Cantidad = (int)rd["Cantidad"],
                    FechaAlmacenamiento = (DateTime)rd["FechaAlmacenamiento"],
                    FechaCreacion = (DateTime)rd["FechaCreacion"],
                    CreadoPor = (int)rd["CreadoPor"],
                    FechaModificacion = rd["FechaModificacion"] == DBNull.Value ? null : (DateTime?)rd["FechaModificacion"],
                    ModificadoPor = rd["ModificadoPor"] == DBNull.Value ? null : (int?)rd["ModificadoPor"]
                });
            }
            return list;
        }

        public async Task<Semilla?> GetByIdAsync(int id)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("SELECT * FROM Semilla WHERE Id=@Id", cn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            if (await rd.ReadAsync())
            {
                return new Semilla
                {
                    Id = (int)rd["Id"],
                    Nombre = rd["Nombre"].ToString() ?? "",
                    EspecieId = (int)rd["EspecieId"],
                    UbicacionId = (int)rd["UbicacionId"],
                    Cantidad = (int)rd["Cantidad"],
                    FechaAlmacenamiento = (DateTime)rd["FechaAlmacenamiento"],
                    FechaCreacion = (DateTime)rd["FechaCreacion"],
                    CreadoPor = (int)rd["CreadoPor"],
                    FechaModificacion = rd["FechaModificacion"] == DBNull.Value ? null : (DateTime?)rd["FechaModificacion"],
                    ModificadoPor = rd["ModificadoPor"] == DBNull.Value ? null : (int?)rd["ModificadoPor"]
                };
            }
            return null;
        }

        public async Task InsertAsync(Semilla s)
        {
            using var cn = _factory.Create();
            var sql = @"
INSERT INTO Semilla
(Nombre, EspecieId, UbicacionId, Cantidad, FechaAlmacenamiento, 
 FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor)
VALUES
(@Nombre, @EspecieId, @UbicacionId, @Cantidad, @FechaAlmacenamiento, 
 @FechaCreacion, @CreadoPor, @FechaModificacion, @ModificadoPor)";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Nombre", s.Nombre);
            cmd.Parameters.AddWithValue("@EspecieId", s.EspecieId);
            cmd.Parameters.AddWithValue("@UbicacionId", s.UbicacionId);
            cmd.Parameters.AddWithValue("@Cantidad", s.Cantidad);
            cmd.Parameters.AddWithValue("@FechaAlmacenamiento", s.FechaAlmacenamiento);
            cmd.Parameters.AddWithValue("@FechaCreacion", s.FechaCreacion == default ? DateTime.Now : s.FechaCreacion);
            cmd.Parameters.AddWithValue("@CreadoPor", s.CreadoPor == 0 ? 1 : s.CreadoPor);
            cmd.Parameters.AddWithValue("@FechaModificacion", (object?)s.FechaModificacion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ModificadoPor", (object?)s.ModificadoPor ?? DBNull.Value);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateAsync(Semilla s)
        {
            using var cn = _factory.Create();
            var sql = @"
UPDATE Semilla SET
  Nombre=@Nombre,
  EspecieId=@EspecieId,
  UbicacionId=@UbicacionId,
  Cantidad=@Cantidad,
  FechaAlmacenamiento=@FechaAlmacenamiento,
  FechaModificacion=@FechaModificacion,
  ModificadoPor=@ModificadoPor
WHERE Id=@Id";
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Nombre", s.Nombre);
            cmd.Parameters.AddWithValue("@EspecieId", s.EspecieId);
            cmd.Parameters.AddWithValue("@UbicacionId", s.UbicacionId);
            cmd.Parameters.AddWithValue("@Cantidad", s.Cantidad);
            cmd.Parameters.AddWithValue("@FechaAlmacenamiento", s.FechaAlmacenamiento);
            cmd.Parameters.AddWithValue("@FechaModificacion", (object?)s.FechaModificacion ?? DateTime.Now);
            cmd.Parameters.AddWithValue("@ModificadoPor", (object?)s.ModificadoPor ?? 1);
            cmd.Parameters.AddWithValue("@Id", s.Id);
            await cn.OpenAsync();
            var n = await cmd.ExecuteNonQueryAsync();
            return n > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("DELETE FROM Semilla WHERE Id=@Id", cn);
            cmd.Parameters.AddWithValue("@Id", id);
            await cn.OpenAsync();
            var n = await cmd.ExecuteNonQueryAsync();
            return n > 0;
        }

        public async Task<List<SemillaGrid>> GetAllWithNamesAsync()
        {
            var list = new List<SemillaGrid>();
            using var cn = _factory.Create();
            var sql = @"
SELECT s.Id, s.Nombre, s.EspecieId, s.UbicacionId, s.Cantidad, s.FechaAlmacenamiento,
       e.NombreComun AS NombreEspecie,
       u.Nombre      AS NombreUbicacion
FROM   Semilla s
JOIN   Especie e   ON e.Id = s.EspecieId
JOIN   Ubicacion u ON u.Id = s.UbicacionId
ORDER BY s.Id DESC";
            using var cmd = new SqlCommand(sql, cn);
            await cn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new SemillaGrid
                {
                    Id = (int)rd["Id"],
                    Nombre = rd["Nombre"].ToString() ?? "",
                    EspecieId = (int)rd["EspecieId"],
                    UbicacionId = (int)rd["UbicacionId"],
                    Cantidad = (int)rd["Cantidad"],
                    FechaAlmacenamiento = (DateTime)rd["FechaAlmacenamiento"],
                    NombreEspecie = rd["NombreEspecie"].ToString() ?? "",
                    NombreUbicacion = rd["NombreUbicacion"].ToString() ?? ""
                });
            }
            return list;
        }
    }
}