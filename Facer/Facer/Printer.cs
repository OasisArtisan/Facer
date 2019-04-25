using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Facer
{
    public class Printer
    {
        private List<IWriteLine> _outputs;

        public Printer()
        {
            _outputs = new List<IWriteLine>();
        }
        public Printer(List<IWriteLine> outputs)
        {
            _outputs = outputs;
        }
        public void Subscribe(IWriteLine iw)
        {
            _outputs.Add(iw);
        }
        public void PrintLine(string s)
        {
            Console.WriteLine(s);
            foreach(var w in _outputs)
            {
                w.WriteLine(s);
            }
        }

        public void PrintStatus(string tag, string msg)
        {
            PrintLine($"[{tag}] {msg}");
        }
    }
}
