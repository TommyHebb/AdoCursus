using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace AdoOefeningenGemeenschap
{
    public class PlantenDbManager
    {
        private static ConnectionStringSettings conPlantenSetting = ConfigurationManager.ConnectionStrings["Planten"];
        private static DbProviderFactory factory = DbProviderFactories.GetFactory(conPlantenSetting.ProviderName);
        public DbConnection GetConnection()
        {
            var conPlanten = factory.CreateConnection();
            conPlanten.ConnectionString = conPlantenSetting.ConnectionString;
            return conPlanten;
        }
    }
}
