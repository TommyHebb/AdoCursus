using System;
using System.Windows;
using AdoOefeningenGemeenschap;
namespace AdoOpgave2
{
    /// <summary>
    /// Interaction logic for WPFOpgave2.xaml
    /// </summary>
    public partial class WPFOpgave2 : Window
    {
        public WPFOpgave2()
        {
            InitializeComponent();
        }
        private void ButtonDb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conTuin = new TuinDbManager().GetConnection())
                {
                    conTuin.Open();
                    labelStatus.Content = "Tuincentrum geopend";
                }
            }
            catch (Exception ex)
            {
                labelStatus.Content = ex.Message;
            }
        }
    }
}
