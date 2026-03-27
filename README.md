# Materialbeschneidung

## 📌 Projektbeschreibung
Dieses Projekt ist eine Desktop-Anwendung zur **teilautomatisierten Erstellung von Zuschnittlisten im Stahlbau**.

Die Software verarbeitet Stücklisten aus einem CAD-System und kombiniert diese mit Lagerbeständen, um optimale Zuschnittlisten zu berechnen. Ziel ist es, den Materialverbrauch zu optimieren und Verschnitt zu minimieren.

## 🎯 Ziel des Projekts
In der Praxis wurden Zuschnittlisten bisher manuell erstellt:
- Vergleich von Stücklisten mit Lagerbeständen
- Manuelle Berechnung der Materialverwendung
- Hoher Zeitaufwand und Fehleranfälligkeit

Diese Anwendung automatisiert diesen Prozess vollständig und sorgt für:
- Zeitersparnis
- Weniger Materialverschwendung
- Reduzierung von Berechnungsfehlern

## ⚙️ Funktionen
- 📥 Einlesen von Stücklisten aus einer CAD-Datenbank
- 📦 Abgleich mit Lagerbeständen
- 🧮 Automatische Berechnung von Zuschnittlisten
- ♻️ Berücksichtigung von Restmaterial
- ⚠️ Warnmeldungen bei fehlendem Material
- 📊 Darstellung in übersichtlichen Tabellen
- 📤 Export der Ergebnisse nach Excel

## 🏗️ Architektur
Die Anwendung basiert auf einem **3-Schichten-Modell**:
- **GUI (WPF)** – Benutzeroberfläche
- **Geschäftslogik** – Berechnung der Zuschnittlisten
- **Datenzugriff** – Zugriff auf CAD- und Lagerdatenbank

Diese Trennung sorgt für:
- bessere Wartbarkeit
- hohe Testbarkeit
- einfache Erweiterbarkeit

## 🖥️ Technologien
- C# / .NET
- WPF (Windows Presentation Foundation)
- SQL (Datenbankzugriff)
- ClosedXML (Excel-Export)
- xUnit (Testing)

## 🔄 Funktionsweise
1. Laden einer Stückliste aus der CAD-Datenbank  
2. Laden der Lagerbestände  
3. Automatische Berechnung der optimalen Zuschnitte  
4. Ausgabe der Zuschnittliste  
5. Optionaler Export nach Excel  

## 📊 Besonderheiten
- Minimierung von Materialverschnitt
- Automatische Restlängenberechnung
- Logging aller Berechnungen
- Fehler- und Ausnahmebehandlung
- Benutzerfreundliche Oberfläche

## 🚀 Installation
### Voraussetzungen
- Windows Betriebssystem
- .NET Runtime
- Microsoft Excel (für Exportfunktion)

## ⚠️ Hinweis zur Ausführung

Diese Anwendung greift auf die Testdatenbanken zu und es müssen die Connectionstrings in der Dataprovider.cs in Zeile 12 und 13 angepasst werden!
