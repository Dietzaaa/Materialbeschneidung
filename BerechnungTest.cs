using Xunit;

namespace Materialbeschneidung
{
    public class TestBerechnung
    {
        [Fact]
        public void Beschneide_ErzeugtWarnungWennKeinRestmaterialUndMengeAufgebraucht()
        {
            // Arrange
            var stueckliste = new List<Material>
            {
                new Material
                {
                    Bezeichnung = "Flachstahl",
                    Werkstoff = "S355",
                    Laenge = 4000
                }
            };

            var lager = new Material
            {
                Bezeichnung = "Flachstahl",
                Werkstoff = "S355",
                Laenge = 6000,
                Menge = 1
            };

            // Act
            Berechnung.Beschneide(stueckliste, lager);

            // Assert
            Assert.Single(Berechnung.warnmeldungen);
            Assert.Contains("Nicht genügend Lagerbestand", Berechnung.warnmeldungen[0]);
            Assert.Contains("Flachstahl", Berechnung.warnmeldungen[0]);
        }

        [Fact]
        public void Beschneide_EntferntStueckNachBeschneidungAusStueckliste()
        {
            // Arrange
            var stueckliste = new List<Material>
            {
                new Material
                {
                    Bezeichnung = "Flachstahl",
                    Werkstoff = "S355",
                    Laenge = 4000
                }
            };

            var lager = new Material
            {
                Bezeichnung = "Flachstahl",
                Werkstoff = "S355",
                Laenge = 6000,
                Menge = 1
            };

            // Act
            Berechnung.Beschneide(stueckliste, lager);

            // Assert
            Assert.Empty(stueckliste);
        }

        [Fact]
        public void Beschneide_FuegtStueckDerZuschnittlisteHinzu()
        {
            // Arrange
            var stueckliste = new List<Material>
            {
                new Material
                {
                    Bezeichnung = "Flachstahl",
                    Werkstoff = "S355",
                    Laenge = 4000
                }
            };

            var lager = new Material
            {
                Bezeichnung = "Flachstahl",
                Werkstoff = "S355",
                Laenge = 6000,
                Menge = 1
            };

            // Act
            Berechnung.Beschneide(stueckliste, lager);

            // Assert
            Assert.Single(Berechnung.zuschnittliste);
        }
    }
}
