using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AdoOefeningenGemeenschap;

namespace AdoOpgave2
{
    /// <summary>
    /// Interaction logic for WPFOpgave3.xaml
    /// </summary>
    public partial class WPFOpgave3 : Window
    {
        public WPFOpgave3()
        {
            InitializeComponent();
        }
        private void buttonToevoegen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuinManager();
                var deLeverancier = new Leverancier();
                deLeverancier.Naam = textBoxNaam.Text;
                deLeverancier.Adres = textBoxAdres.Text;
                deLeverancier.PostNr = textBoxPostcode.Text;
                deLeverancier.Woonplaats = textBoxPlaats.Text;
                manager.LeverancierToevoegen(deLeverancier);
                labelStatus.Content = "nieuwe leverancier is toegevoegd";
            }
            catch (Exception ex)
            {
                labelStatus.Content = ex.Message;
            }
        }
        private void buttonEindejaarskorting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                labelStatus.Content = new TuinManager().EindejaarsKorting().ToString()
                + " plantenprijzen aangepast";
            }
            catch (Exception ex)
            {
                labelStatus.Content = ex.Message;
            }
        }

        private void buttonVervangLeverancier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var manager = new TuinManager();
                manager.VervangLeverancier(2, 3);
                labelStatus.Content = "Leverancier 2 is verwijderd en vervangen door 3";
            }
            catch (Exception ex)
            {
                labelStatus.Content = ex.Message;
            }
        }
    }
}
