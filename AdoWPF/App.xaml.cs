using System.Windows;

namespace AdoWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            string connectionString = @"server=(localdb)\mssqllocaldb;database=Bieren;integrated security=true";
            Application.Current.Properties["Bieren2"] = connectionString;
        }
    }
}
