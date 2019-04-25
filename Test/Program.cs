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
            var a = new SA("S", 12);
            var b = new SA("S", 12);

            var dict = new Dictionary<SA, int>();
            
            Console.WriteLine((a.Equals(b)));
            Console.WriteLine((b.Equals(a)));

            Console.ReadKey();
        }
    }

    struct SA
    {
        public SA(string name, int age)
        {
            Name = name;
            Age = age;
        }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    struct SB
    {
    }
}
