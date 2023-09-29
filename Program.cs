﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lexii2
{
    class Program
    {
        static void Main(string[] args) 
        {
            try
            {
                using (Lexico L = new Lexico())
                {
                    while (!L.FinArchivo())
                    {
                        L.nextToken();
                    }
                }
            } 
            catch (Exception e)
            {
                Console.WriteLine("Error: "+e.Message);
            }
        }
    }
}