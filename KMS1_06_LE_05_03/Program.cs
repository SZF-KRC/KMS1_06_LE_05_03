﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KMS1_06_LE_05_03
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
          Menu menu = new Menu();
            menu.PrintMenu();
        }
    }
}
