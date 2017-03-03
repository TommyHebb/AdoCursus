using System.Configuration;
using System.Data.Common;

namespace AdoOefeningenGemeenschap
{
    public class TuinDbManager
    {
        private static ConnectionStringSettings conTuinSetting = ConfigurationManager.ConnectionStrings["Tuin"];
        private static DbProviderFactory factory = DbProviderFactories.GetFactory(conTuinSetting.ProviderName);
        public DbConnection GetConnection()
        {
            var conTuin = factory.CreateConnection();
            conTuin.ConnectionString = conTuinSetting.ConnectionString;
            return conTuin;
        }
    }
}
