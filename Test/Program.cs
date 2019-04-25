using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Test
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var c = new A();
            c.printer();
            Console.ReadKey();
        }
    }

    class A
    {
        public void printer()
        {
            string value = "AAA";
            stringChanger(value);
            Console.WriteLine(value);
        }
        private void stringChanger(string val)
        {
            val = "hhh";
        }
    }
}
