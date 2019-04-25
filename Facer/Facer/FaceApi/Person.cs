using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facer.FaceApi
{
    public class Person
    {
        public string PersonID { get; set; }
        public string Name { get; set; }
        public object UserData { get; set; }
        public string[] PersistedFacesIDs { get; set; }
    }
}
