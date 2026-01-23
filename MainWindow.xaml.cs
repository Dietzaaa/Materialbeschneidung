using System.Windows;
using System.Windows.Media;
using System;

namespace Materialbeschneidung
{
    public partial class MainWindow : Window
    {
        Dataprovider dataprovider = new Dataprovider();
        private Stuecklistenfenster stuecklistenfenster = new Stuecklistenfenster();

        public MainWindow()
        {
            InitializeComponent();
            Logging.Debug("Programm gestartet.");
        }

        private void Button_Click_Hinzufuegen(object sender, RoutedEventArgs e)
        {
            Logging.Debug("Button 'weitere Hinzufügen' geklickt.");
            AuftragsnummerAbfrage(false);
        }

        private void Button_Click_Suchen(object sender, RoutedEventArgs e)
        {
            Logging.Debug("Button 'Suchen' geklickt.");
            AuftragsnummerAbfrage(true);
        }

        public void AuftragsnummerAbfrage(bool neueSeiteLaden)
        {
            if (txtBox_auftragsnummer.Text == "" && dataprovider.auftragsListe.Count == 0)
            {
                label_fehlermeldung.Foreground = Brushes.Red;
                label_fehlermeldung.Content = "Bitte geben Sie eine Auftragsnummer ein!";
                Logging.Warning("Keine Auftragsnummer eingegeben.");
            }
            else if (dataprovider.AuftragExistiert(txtBox_auftragsnummer.Text))
            {
                FuegeAuftragZurListeHinzu();

                if (neueSeiteLaden)
                {
                    Logging.Info("Lade vorhandene Daten.");
                    stuecklistenfenster.KundenDataGrid.ItemsSource = dataprovider.DatenbankAbfrageCad().DefaultView;
                    stuecklistenfenster.Show();
                    this.Hide();
                }
            }
            else if (neueSeiteLaden && txtBox_auftragsnummer.Text == "" && dataprovider.auftragsListe.Count > 0)
            {
                Logging.Info("Leere Eingabe, aber Liste vorhanden – lade vorhandene Daten.");
                //dataprovider.DatenbankAbfrageCad();
                stuecklistenfenster.KundenDataGrid.ItemsSource = dataprovider.DatenbankAbfrageCad().DefaultView;
                stuecklistenfenster.Show();
                this.Hide();
            }
            else
            {
                label_fehlermeldung.Foreground = Brushes.Red;
                label_fehlermeldung.Content = "Die Auftragsnummer ist falsch oder existiert nicht";
                Logging.Warning("Auftrag " + txtBox_auftragsnummer.Text + " existiert nicht.");
                txtBox_auftragsnummer.Clear();
            }
        }

        public void FuegeAuftragZurListeHinzu()
        {
            if (!dataprovider.auftragsListe.Contains(txtBox_auftragsnummer.Text))
            {
                dataprovider.auftragsListe.Add(txtBox_auftragsnummer.Text);
                lstBox_Auftragsnummer.ItemsSource = dataprovider.auftragsListe;
                Logging.Info("Auftrag " + txtBox_auftragsnummer.Text + " zur Liste hinzugefügt.");
                txtBox_auftragsnummer.Clear();
            }
            else if (dataprovider.auftragsListe.Contains(txtBox_auftragsnummer.Text))
            {
                label_fehlermeldung.Foreground = Brushes.Red;
                label_fehlermeldung.Content = "Die Auftragsnummer wurde bereits hinzugefügt";
                Logging.Warning("Auftrag " + txtBox_auftragsnummer.Text + " wurde doppelt eingegeben.");
                txtBox_auftragsnummer.Clear();
            }
        }
    }
}