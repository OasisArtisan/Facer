using System;
using System.Collections.Generic;

namespace FaceAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            per[] p = { new per { A = "Hello", B = 123 },
                        new per { A = "Manlo", B = 222 },
                        new per { A = "Welcom", B = 312},
                        new per { A = "poore", B = 546 }
            };

            var l = new List<per>(p);
            
            foreach(var b in p)
            {
                var r = l.Find(x => x.A == b.A);
                Console.WriteLine(r.A);
            }
        }
    }
    class per
    {
        public string A { get; set; }
        public int B { get; set; }
    }
}
