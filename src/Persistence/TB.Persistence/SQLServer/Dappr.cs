using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace TB.Persistence.SQLServer
{
    public class Dappr
    {
        private readonly IConfiguration configuration;
        private readonly string? connectionString;
        public Dappr(IConfiguration Configuration)
        {
            configuration = Configuration;
            connectionString = configuration.GetConnectionString("TB");
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(connectionString);
    }
}
