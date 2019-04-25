using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTesting
{
    public class Person
    {
        // Needed properties (From JSON returned from server)
        public string personID { get; set; }
        public string name { get; set; }
        public object UserData { get; set; }
        public string[] PersistedFacesIDs { get; set; }

        // Convenient names
        public string ServerID { get { return personID; } set {personID = value; } }
        public string LocalID { get { return name; } set { name = value; } }
    }
}
