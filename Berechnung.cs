using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Media.Media3D;

namespace Materialbeschneidung
{
    public static class Berechnung
    {
        public static List<string> warnmeldungen = new List<string>();
        public static List<Material> zuschnittliste = new List<Material>();

        /// <summary>
        /// Berechnet die Zuschnittliste basierend auf der Stückliste und dem Lagerbestand.
        /// </summary>
        /// <param name="stueckliste"></param>
        /// <param name="lager"></param>
        /// <returns></returns>
        public static List<Material> Berechne(List<Material> stueckliste, List<Material> lager)
        {
            Logging.Debug("Starte: " + MethodBase.GetCurrentMethod().Name);
            try
            {
                //Es wird über jedes Lagerstück iteriert und geprüft, ob die Stückliste damit abgedeckt werden kann
                for (int i = 0; i < lager.Count; i++)
                {
                    Beschneide(stueckliste, lager[i]);
                }
            }
            catch (Exception ex)
            {
                Logging.Fatal("Fehler bei der Zuschnittberechnung: " + ex.Message);
                throw;
            }

            Logging.Info("Zuschnittberechnung abgeschlossen. Gesamtanzahl der Zuschnitte: " + zuschnittliste.Count);
            Logging.Debug("Beendet: " + MethodBase.GetCurrentMethod().Name + "Gesamtanzahl der Zuschnitte: " + zuschnittliste.Count);
            return zuschnittliste;
        }

        public static void Beschneide(List<Material> stueckliste, Material lager)
        {
            int ersterZaehler = 1;
            int zweiterZaehler = 0;
            int aktuelleMenge = lager.Menge;
            bool stuecklisteWeiterBearbeiten;
            bool keinRestmaterial = true;
            bool warnmeldungAusgegeben = true;

            if (lager.Laenge < 6000)
            {
                keinRestmaterial = false;
            }

            do
            {
                int zuBeschneideneLaenge = lager.Laenge;
                stuecklisteWeiterBearbeiten = false;

                for (int j = 0; j < stueckliste.Count; j++)
                {
                    //Wenn das zu beschneidene Material beschnitten werden kann, wird es der Zuschnittliste hinzugefügt und von der Stückliste entfernt
                    if (lager.Bezeichnung == stueckliste[j].Bezeichnung
                        && lager.Werkstoff == stueckliste[j].Werkstoff
                        && zuBeschneideneLaenge > stueckliste[j].Laenge)
                    {
                        stuecklisteWeiterBearbeiten = true;
                        zweiterZaehler++;
                        stueckliste[j].Position = Convert.ToDouble(ersterZaehler + "," + zweiterZaehler);
                        zuschnittliste.Add(stueckliste[j]);
                        zuBeschneideneLaenge -= stueckliste[j].Laenge;
                        stueckliste.RemoveAt(j);
                        j--;
                    }

                    //Wenn das Material maximal beschnitten wurde wird 1 Stück vom Lager abgezogen
                    if (stueckliste.Count - 1 == j && stuecklisteWeiterBearbeiten)
                    {
                        aktuelleMenge--;
                        ersterZaehler++;
                        zweiterZaehler = 0;
                    }

                    //Wenn das Material im Lager leer ist, wird eine Warnmeldung ausgegeben
                    if (keinRestmaterial && aktuelleMenge == 0 && warnmeldungAusgegeben)
                    {
                        warnmeldungen.Add($"Warnung: \nNicht genügend Lagerbestand für {lager.Bezeichnung}.");
                        warnmeldungAusgegeben = false;
                    }
                }
            } while (stuecklisteWeiterBearbeiten);
        }
    }
}
