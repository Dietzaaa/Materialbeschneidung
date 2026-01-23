using System.Windows;
using System.Windows.Media.Media3D;

namespace Materialbeschneidung
{
    public partial class Stuecklistenfenster : Window
    {
        public static List<Material> zuschnittliste = new List<Material>();
        Dataprovider dataprovider = new Dataprovider();

        public Stuecklistenfenster()
        {
            InitializeComponent();
            Logging.Debug("Stuecklistenfenster geöffnet.");
        }
        private void Button_Click_Zurueck(object sender, RoutedEventArgs e)
        {
            Logging.Debug("Button 'Zurück' geklickt.");
            MainWindow mainWindow = new MainWindow();
            Dataprovider.stueckListe.Clear();
            mainWindow.Show();
            this.Close();
        }
        private void Button_Click_Berechnen(object sender, RoutedEventArgs e)
        {
            Logging.Debug("Button 'Berechnen' geklickt.");
            dataprovider.DatenbankAbfrageLager();

            Zuschnittlistenfenster zuschnittslistenfenster = new Zuschnittlistenfenster();
            OeffneZuschnittlistenfenster(zuschnittslistenfenster);
        }
        private void OeffneZuschnittlistenfenster(Zuschnittlistenfenster zuschnittlistenfenster)
        {
            zuschnittlistenfenster.dgZuschnitte.ItemsSource = zuschnittliste = Berechnung.Berechne(Dataprovider.stueckListe, Dataprovider.lager);
            //Wenn Warnmeldungen während der Berechnung auftreten, werden diese angezeigt
            if (Berechnung.warnmeldungen.Count > 0)
            {
                Logging.Warning("Warnmeldungen während der Berechnung aufgetreten: " + string.Join("; ", Berechnung.warnmeldungen));
                string alleWarnmeldungen = string.Join("\n", Berechnung.warnmeldungen);
                MessageBox.Show(alleWarnmeldungen, "Warnmeldungen", MessageBoxButton.OK, MessageBoxImage.Warning);
                Berechnung.warnmeldungen.Clear();

                zuschnittlistenfenster.Show();
                this.Hide();
                Logging.Info("Zuschnittslistenfenster geöffnet. Stuecklistenfenster ausgeblendet.");
            }
            else
            {
                // zuschnittlistenfenster.dgZuschnitte.ItemsSource = zuschnittliste = Berechnung.Berechne(Dataprovider.stueckListe, Dataprovider.lager);              
                zuschnittlistenfenster.Show();
                this.Hide();
                Logging.Info("Zuschnittslistenfenster geöffnet. Stuecklistenfenster ausgeblendet.");
            }
        }
    }
}
