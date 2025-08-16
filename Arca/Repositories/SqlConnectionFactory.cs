using Microsoft.Data.SqlClient;

namespace Arca.Api.Repositories
{
    public class SqlConnectionFactory
    {
        private readonly string _cn;
        public SqlConnectionFactory(IConfiguration cfg)
        {
            _cn = cfg.GetConnectionString("ConexionBaseDatos")
                ?? throw new InvalidOperationException("Falta ConnectionStrings:ConexionBaseDatos en appsettings.json");
        }
        public SqlConnection Create() => new SqlConnection(_cn);
    }
}