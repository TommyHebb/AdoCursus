using System;
using System.Windows;
using AdoOefeningenGemeenschap;

namespace AdoOpgave2
{
    /// <summary>
    /// Interaction logic for WPFOpgave5.xaml
    /// </summary>
    public partial class WPFOpgave5 : Window
    {
        public WPFOpgave5()
        {
            InitializeComponent();
        }

        private void buttonGemiddelde_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuinManager();
                labelResultaat.Content = String.Format("Gemiddelde prijs : {0:C}", 
                    manager.GemiddeldePrijsVanEenSoort(textBoxSoort.Text));
            }
            catch (Exception ex)
            {
                labelResultaat.Content = ex.Message;
            }
        }
    }
}
