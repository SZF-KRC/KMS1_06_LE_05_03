using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS1_06_LE_05_03
{
    internal class Menu
    {
        UserInput userInput = new UserInput();
        DialogFunction dialogFunction = new DialogFunction();

        /// <summary>
        /// Zeigt das Hauptmenü der Anwendung an und verarbeitet die Benutzereingaben.
        /// </summary>
        public void PrintMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n*** Textbearbeitungsdiensten ***\n[0] Programm beenden\n[1] Öffnen die Textdatei\n[2] Textdaten überschreiben\n[3] Statistik\n[4] Textdatei löschen");
                switch (userInput.InputNumber("Geben Sie den Index Ihrer Wahl ein: "))
                {
                    case 0:exit = true; break;
                    case 1:dialogFunction.OpenTextFile(); break;
                    case 2:dialogFunction.EditTextFile(); break;
                    case 3:Statistic(); break;
                    case 4:dialogFunction.DeleteTextFile(); break;
                    default: Console.WriteLine("\n--- Geben Sie nur den Index von 0-4 ein ---\n"); break;
                }
            }
        }

        /// <summary>
        /// Zeigt die Statistiken für alle verarbeiteten Dateien an.
        /// </summary>
        private void Statistic()
        {
            if(dialogFunction.GetStatistic().Count == 0)
            {
                Console.WriteLine("\n--- Statistic is leer ---\n");
                return;
            }
            foreach (var item in dialogFunction.GetStatistic())
            {
                Console.WriteLine(dialogFunction.Cage(120));
                Console.WriteLine($"File Full Path: {item.FilePath}\nGesamtzahl der Wörter: {item.CountWords}");
                Console.WriteLine($"Name: {item.MetaData.Name}\nDateigröße: {item.MetaData.Length} bytes\nErstellungszeitpunkt: {item.MetaData.CreationTime}\nLetzter Zugriffszeitpunkt: {item.MetaData.LastAccessTime}");
                Console.WriteLine("Top 5 häufigste Wörter:");
                foreach(var word in item.Item3)
                {
                    Console.WriteLine($"{word.Key}:{word.Value}");
                }
                Console.WriteLine(dialogFunction.Cage(120));
            }
        }
    }
}