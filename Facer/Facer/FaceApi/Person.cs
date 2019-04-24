﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facer.FaceApi
{
    public struct Person
    {
        public string ServerID { get; set; }
        public string LocalID { get; set; }
        public object UserData { get; set; }
        public string[] PersistedFacesIDs { get; set; }
    }
}
