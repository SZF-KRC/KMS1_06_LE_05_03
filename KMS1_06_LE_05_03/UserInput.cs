using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS1_06_LE_05_03
{
    internal class UserInput
    {
        /// <summary>
        /// Liest eine Ganzzahl vom Benutzer ein und stellt sicher, dass die Eingabe nicht leer ist.
        /// </summary>
        /// <param name="prompt">Die Anzeigeaufforderung.</param>
        /// <returns>Die Benutzereingabe als Ganzzahl.</returns>
        public int InputNumber(string prompt)
        {
            int number;
            while (true)
            {
                Console.Write($"\n{prompt}");// Anzeige der Eingabeaufforderung an den Benutzer
                string input = Console.ReadLine();// Benutzereingabe lesen
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("\n--- Eingabe darf nicht leer sein ---\n");
                    continue;// Schleife fortsetzen, wenn die Eingabe leer ist
                }
                try
                {
                    number = Convert.ToInt32(input);// Versuch, die Eingabe in eine Ganzzahl zu konvertieren
                    break;// Schleife unterbrechen, wenn die Konvertierung erfolgreich is
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n--- Geben Sie nur eine Ganzzahl ein ---\n" + e.Message);
                }
            }
            return number;// Rückgabe der Ganzzahl
        }
    }
}
