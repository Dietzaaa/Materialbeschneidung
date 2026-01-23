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
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel-Datei (*.xlsx)|*.xlsx",
                Title = "Speichern unter"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Daten");

                    // Geht die Kopfspalten durch und schreibt sie in die erste Zeile
                    for (int c = 0; c < dgZuschnitte.Columns.Count; c++)
                    {
                        worksheet.Cell(1, c + 1).Value = dgZuschnitte.Columns[c].Header?.ToString();
                    }


                    // Datenzeilen
                    for (int r = 0; r < dgZuschnitte.Items.Count; r++)
                    {
                        var item = dgZuschnitte.Items[r];
                        if (item == null) continue;

                        for (int c = 0; c < dgZuschnitte.Columns.Count; c++)
                        {
                            var column = dgZuschnitte.Columns[c];
                            var binding = (column as DataGridBoundColumn)?.Binding as Binding;

                            if (binding != null)
                            {
                                var propertyName = binding.Path.Path;
                                var value = item.GetType().GetProperty(propertyName)?.GetValue(item);
                                worksheet.Cell(r + 2, c + 1).Value = value?.ToString() ?? "";
                            }
                            else
                            {
                                worksheet.Cell(r + 2, c + 1).Value = "";
                            }
                        }
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(saveFileDialog.FileName);
                }
                Logging.Info("Excel-Datei erfolgreich exportiert: " + saveFileDialog.FileName);
                MessageBox.Show("Excel-Datei erfolgreich exportiert!");
            }
        }
    }
}
