using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTesting
{
    public struct Person
    {
        public string PersonID { get; set; }
        public string Name { get; set; }
        public object UserData { get; set; }
        public string[] PersistedFacesIDs { get; set; }
    }
}
