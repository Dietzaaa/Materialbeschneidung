using ClosedXML.Excel;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Materialbeschneidung
{
    public partial class Zuschnittlistenfenster : Window
    {
        public Zuschnittlistenfenster()
        {
            InitializeComponent();
        }

        private void Button_Click_Zurueck(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            Dataprovider.stueckListe.Clear();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_Excel_Export(object sender, RoutedEventArgs e)
        {
            var dateiSpeichernBenachrichtigung = new SaveFileDialog
            {
                Filter = "Excel-Datei (*.xlsx)|*.xlsx",
                Title = "Speichern unter"
            };

            if (dateiSpeichernBenachrichtigung.ShowDialog() == true)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Daten");

                    // Geht die Kopfspalten durch und schreibt sie in die erste Zeile
                    for (int i = 0; i < dgZuschnitte.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dgZuschnitte.Columns[i].Header?.ToString();
                    }


                    // Datenzeilen
                    for (int r = 0; r < dgZuschnitte.Items.Count; r++)
                    {
                        var reihe = dgZuschnitte.Items[r];
                        if (reihe == null) continue;

                        for (int s = 0; s < dgZuschnitte.Columns.Count; s++)
                        {
                            var spalten = dgZuschnitte.Columns[s];
                            var spaltenZuordnung = (spalten as DataGridBoundColumn)?.Binding as Binding;

                            if (spaltenZuordnung != null)
                            {
                                var bezeichnung = spaltenZuordnung.Path.Path;
                                var spaltenInhalt = reihe.GetType().GetProperty(bezeichnung)?.GetValue(reihe);
                                worksheet.Cell(r + 2, s + 1).Value = spaltenInhalt?.ToString() ?? "";
                            }
                            else
                            {
                                worksheet.Cell(r + 2, s + 1).Value = "";
                            }
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(dateiSpeichernBenachrichtigung.FileName);
                }
                Logging.Info("Excel-Datei erfolgreich exportiert: " + dateiSpeichernBenachrichtigung.FileName);
                MessageBox.Show("Excel-Datei erfolgreich exportiert!");
            }
        }
    }
}
