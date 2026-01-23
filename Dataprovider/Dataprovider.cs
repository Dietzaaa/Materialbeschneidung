using DocumentFormat.OpenXml.Office2010.Excel;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Materialbeschneidung
{
    public class Dataprovider
    {
        public string connectionStringCad = "Server=localhost;Database=cad_database;User Id=service_listCreator;Password=321321";
        public string connectionStringLager = "Server=localhost;Database=lager;User Id=service_lager;Password=321321";
        public ObservableCollection<string> auftragsListe = new ObservableCollection<string>();
        public static List<Material> stueckListe = new List<Material>();
        public static List<Material> lager = new List<Material>();
        bool darfLagerSehen = false;

        public DataTable DatenbankAbfrageCad()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringCad))
            {
                string auftragsnummern = string.Join("','", auftragsListe);
                string abfrage = $@"
                    SELECT a.auftragsnummer AS 'Auftragsnummer', h.bezeichnung AS 'Bezeichnung', w.werkstoff AS 'Werkstoff',e.laenge_mm AS 'Laenge'
                    FROM auftraege AS a
                    JOIN einzelteile AS e ON e.id_auftraege = a.id
                    JOIN halbzeuge AS h ON e.id_halbzeuge = h.id
                    JOIN werkstoffe_halbzeuge AS wh ON h.id = wh.id_halbzeuge 
                    JOIN werkstoffe AS w ON wh.id_werkstoffe = w.id
                    WHERE auftragsnummer IN ('{auftragsnummern}')
                    ORDER BY h.bezeichnung, e.laenge_mm DESC;
                ";
                Logging.Debug("Starte Datenbankabfrage für Aufträge " + auftragsListe);

                //Datenbankverbindung wird aufgebaut und die Materialien werden zur Stückliste hinzugefügt       
                try
                {
                    connection.Open();
                    Logging.Info("Datenbankverbindung erfolgreich geöffnet.");
                    MySqlCommand command = new MySqlCommand(abfrage, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    Logging.Debug("Datenbankabfrage erfolgreich ausgeführt. " + dataTable.Rows.Count + " Zeilen erhalten.");

                    DatenAuslesen(abfrage, connection, darfLagerSehen);
                    return dataTable;
                }
                catch (Exception ex)
                {
                    Logging.Error("Fehler beim Lesen der Datenbankergebnisse: " + ex.Message);
                    throw;
                }
            }
        }
        public void DatenbankAbfrageLager()
        {
            //Datenbankverbindung wird aufgebaut und der Lagerbestand wird zur Liste hinzugefügt
            using (MySqlConnection connectionLager = new MySqlConnection(connectionStringLager))
            {
                string abfrage = $@"SELECT h.Bezeichnung, w.Werkstoff, h.laenge, h.Menge
                                FROM halbzeuge AS h 
                                JOIN werkstoffe_halbzeuge AS wh ON wh.id_halbzeuge = h.id 
                                JOIN werkstoffe AS w ON w.id = wh.id_werkstoffe 
                                ORDER BY h.Bezeichnung, h.laenge;"
                ;
                try
                {
                    darfLagerSehen = true;
                    connectionLager.Open();
                    Logging.Info("Datenbankverbindung erfolgreich geöffnet.");
                    DatenAuslesen(abfrage, connectionLager, darfLagerSehen);
                }
                catch (Exception ex)
                {
                    Logging.Fatal("Fehler beim Öffnen der Datenbankverbindung: " + ex.Message);
                    MessageBox.Show("Fehler beim Verbinden mit der Datenbank. Bitte überprüfen Sie Ihre Verbindungseinstellungen.", "Datenbankverbindungsfehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Logging.Debug("Starte Berechnung der Zuschnittsliste.");
            }
        }
        private static void DatenAuslesen(string abfrage, MySqlConnection connection, bool darfLagerSehen)
        {
            using (MySqlCommand command = new MySqlCommand(abfrage, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        if (darfLagerSehen)
                        {
                            while (reader.Read())
                            {
                                Material material = new Material
                                {
                                    Bezeichnung = reader["Bezeichnung"].ToString(),
                                    Werkstoff = reader["Werkstoff"].ToString(),
                                    Laenge = Convert.ToInt32(reader["laenge"]),
                                    Menge = Convert.ToInt32(reader["Menge"]),
                                };
                                lager.Add(material);
                            }
                            Logging.Debug("Lagerdaten erfolgreich aus der Datenbank abgerufen.");
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                Material material = new Material
                                {
                                    Bezeichnung = reader["Bezeichnung"].ToString(),
                                    Werkstoff = reader["Werkstoff"].ToString(),
                                    Laenge = Convert.ToInt32(reader["Laenge"]),
                                    Auftragsnummer = reader["Auftragsnummer"].ToString(),
                                };
                                stueckListe.Add(material);
                            }
                            Logging.Debug("Stückliste erfolgreich aus der Datenbank abgerufen.");

                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Fatal("Fehler beim Abrufen der Lagerdaten aus der Datenbank: " + ex.Message);
                        throw;
                    }
                    finally
                    {
                        reader.Close();
                        connection.Close();
                        Logging.Info("Datenbankreader für Lagerdaten geschlossen.");
                    }
                }
            }
        }
        public bool AuftragExistiert(string auftragsnummer)
        {
            Logging.Info("Überprüfe Existenz des Auftrags " + auftragsnummer + " in der Datenbank.");
            string abfrage = "SELECT COUNT(*) FROM auftraege WHERE Auftragsnummer = '" + auftragsnummer + "'";

            using (MySqlConnection connection = new MySqlConnection(connectionStringCad))
            using (MySqlCommand command = new MySqlCommand(abfrage, connection))
            {
                connection.Open();

                int anzahl = Convert.ToInt32(command.ExecuteScalar());
                return anzahl > 0;
            }
        }
    }
}
