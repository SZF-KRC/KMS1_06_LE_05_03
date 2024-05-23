using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KMS1_06_LE_05_03
{
    internal class DialogFunction
    {
        /// <summary>
        /// Liste zur Speicherung von Datei-Statistiken.
        /// Jeder Eintrag in der Liste besteht aus:
        /// <list type="bullet">
        /// <item>
        /// <description><c>FilePath</c>: Der Pfad zur Textdatei.</description>
        /// </item>
        /// <item>
        /// <description><c>CountWords</c>: Die Gesamtanzahl der Wörter in der Textdatei.</description>
        /// </item>
        /// <item>
        /// <description><c>Top5</c>: Ein Wörterbuch der Top 5 häufigsten Wörter und deren Vorkommen.</description>
        /// </item>
        /// <item>
        /// <description><c>MetaData</c>: Die Metadaten der Textdatei (wie Name, Größe, Erstellungsdatum usw.).</description>
        /// </item>
        /// </list>
        /// </summary>
        private List<(string FilePath, int CountWords, Dictionary<string, int> Top5, FileInfo MetaData)> _statistic = new List<(string, int, Dictionary<string, int>,FileInfo)>();
        
        /// <summary>
        /// Öffnen eines Textdokuments über ein Dialogfeld und Ausgeben an die Konsole
        /// Das geöffnete Dialogfeld wird automatisch auf den voreingestellten Ordner eingestellt und sucht nur nach der TXT-Datei im Ordner
        /// </summary>
        /// <returns>Gibt den Pfad des geöffneten Dokuments zurück</returns>
        public string OpenTextFile()
        {
           // List<string> temporaryList = new List<string>();// Temporäre Liste zur Speicherung der Zeilen des Dokuments
            OpenFileDialog openFileDialog = new OpenFileDialog();// Initialisierung des OpenFileDialog
            openFileDialog.InitialDirectory = "C:\\Users\\16358\\Documents\\examplesTXT";
            openFileDialog.Filter = "Text files (*.txt)|*.txt";// Filter für die Dateisuche
            if (openFileDialog.ShowDialog() == true)// Anzeigen des Dialogfelds und Überprüfung, ob eine Datei ausgewählt wurde
            {
                using(StreamReader streamreader  = new StreamReader(openFileDialog.FileName))// Öffnen der ausgewählten Datei zum Lesen
                {
                    while (!streamreader.EndOfStream)// Lesen bis zum Ende der Datei
                    {
                        string line = streamreader.ReadLine();
                        Console.WriteLine(line);
                    }               
                }
                // here I want call method for statistic where I sent my temporary list
            }
            // Call Statistic method with the file path
            if (!string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                Statistic(openFileDialog.FileName);
            }
           
            return openFileDialog.FileName;// Rückgabe des Dateipfads der geöffneten Datei
        }

        /// <summary>
        /// Bearbeitet den Text, indem er zum vorhandenen Dokument hinzugefügt wird, oder ermöglicht Ihnen, ihn als neues Dokument zu speichern
        /// Das geöffnete Dialogfeld wird automatisch auf den voreingestellten Ordner eingestellt und sucht nur nach der TXT-Datei im Ordner
        /// </summary>
        public void EditTextFile()
        {
            List<string> _temporaryList = new List<string>();// Temporäre Liste zur Speicherung der Zeilen des Dokuments
            string fileName = OpenTextFile();// Öffnen einer Datei und Speichern des Dateipfads    
            if(string.IsNullOrWhiteSpace(fileName)) { return; }
            Console.WriteLine("Zadajte text, ktorý chcete pridať. Zadajte 'exit' pre ukončenie.");
            while (true)
            {
                string newText = Console.ReadLine();
                if (newText == "exit") { break; }
                _temporaryList.Add(newText);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = "C:\\Users\\16358\\Documents\\examplesTXT";// Voreingestellter Ordner
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";// Filter für die Dateisuche
            saveFileDialog.FileName = fileName;
            if(saveFileDialog.ShowDialog() == true)// Anzeigen des Dialogfelds und Überprüfung, ob ein Speicherort ausgewählt wurde
            {
                using(StreamWriter writer = new StreamWriter(saveFileDialog.FileName, true))// Öffnen der Datei zum Schreiben (Anhängen)
                {
                    foreach (string line in _temporaryList)
                    {
                        writer.WriteLine(line);// Schreiben jeder Zeile in die Datei
                    }
                }
            }
            Statistic(saveFileDialog.FileName);
            _temporaryList.Clear();
        }

        /// <summary>
        /// Textdatei löschen
        /// </summary>
        public void DeleteTextFile()
        {
            string fileName = OpenTextFile();// Öffnen einer Datei und Speichern des Dateipfads
            if (string.IsNullOrWhiteSpace(fileName)) { return; }
            File.Delete(fileName);// Löschen der Datei
        }

        /// <summary>
        /// Berechnet die Statistik für eine Textdatei.
        /// </summary>
        /// <param name="filePath">Pfad zur Textdatei.</param>
        private void Statistic(string filePath)
        {
            Dictionary<string, int> wordCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);          
            using (StreamReader reader = new StreamReader(filePath))// Laden des Textes aus der Datei zeilenweise
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Regular expression to match words in the current line
                    string pattern = @"\b\w+\b";
                    MatchCollection matches = Regex.Matches(line, pattern);

                    foreach (Match match in matches)
                    {
                        string word = match.Value.ToLower();
                        if (wordCounts.ContainsKey(word))
                        {
                            wordCounts[word]++;
                        }
                        else
                        {
                            wordCounts[word] = 1;
                        }
                    }
                }
            }
            int totalWordCount = wordCounts.Values.Sum();
            var topWords = wordCounts.OrderByDescending(kv => kv.Value).Take(5).ToDictionary(kv => kv.Key, kv => kv.Value);// Erhalten der Top 5 häufigsten Wörter
            FileInfo fileInfo = new FileInfo(filePath);// Metadaten der Datei erhalten
            _statistic.Add((filePath, totalWordCount, topWords, fileInfo));// Hinzufügen der Statistik zur Liste
        }

        /// <summary>
        /// Gibt eine Liste der Statistiken für alle verarbeiteten Dateien zurück.
        /// </summary>
        /// <returns>Liste der Statistiken.</returns>
        public List<(string FilePath, int CountWords, Dictionary<string, int> Top5, FileInfo MetaData)> GetStatistic() => _statistic;

        /// <summary>
        /// Erstellt eine fortlaufende Linie aus Unterstrichen, die zur visuellen Trennung verwendet werden
        /// </summary>
        /// <param name="input">Anzahl der Wiederholungen</param>
        /// <returns>Gibt einen String in einer Größe zurück, die Sie im Voraus auswählen können</returns>
        public string Cage(int input)
        {
            char cageChar = '_';
            string result;
            return result = new string(cageChar, input);
        }
    }  
}
