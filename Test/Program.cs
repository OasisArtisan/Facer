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
            var t = DateTime.Now.Second;
            Thread.Sleep(4010);
            Console.WriteLine(DateTime.Now.Second - t);
            Console.ReadKey();
        }

    }
}
