using System;
using System.Windows;
using AdoOefeningenGemeenschap;
namespace AdoOpgave2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ButtonDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new PlantenDbManager();
                using (var conPlanten = manager.GetConnection())
                {
                    conPlanten.Open();
                    Resultaat.Content = "Planten geopend";
                }
            }
            catch (Exception ex)
            {
                Resultaat.Content = ex.Message;
            }
        }
    }
}
