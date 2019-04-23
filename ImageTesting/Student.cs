using System;
using System.Collections.Generic;
using System.Text;

namespace ImageTesting
{
    public class Student
    {
        //private static DataStorage DataStorage;
        private string _firstname;
        private string _lastname;
        private int _id;
        private string _serverID; // This must be changed into "string"
        private bool _valid;

        public string Formatted { get { return ToString(); } }

        public bool Valid { get; }

        //[PrimaryKey]
        public int ID {
            get {
                return _id;
            }
            set {
                if (_valid)
                {
                    throw new MemberAccessException("Cannot change id after setting it");
                }
                _valid = true;
                _id = value;
            }
        }
        
        public string FirstName { get { return _firstname; } set { _firstname = value;  } }
        public string LastName { get { return _lastname; } set { _lastname = value; } }
        public string ServerID { get { return ""; } set { _serverID = value;  } }

        //private void UpdateStorage()
        //{
        //    if (DataStorage != null)
        //    {
        //        DataStorage.UpdateStudent(this);
        //    }
        //}

        //public static void BindToStorage(DataStorage ds)
        //{
        //    DataStorage = ds;
        //}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ID);
            sb.Append(" ");
            sb.Append(FirstName);
            sb.Append(" ");
            sb.Append(LastName);
            return sb.ToString();
        }

        public static string IsValidID(string id)
        {
            //int idi = 0;
            //if (int.TryParse(id, out idi))
            //{
            //    if (!App.Reference.Data.IDExists(idi))
            //    {
            //        return null;
            //    }
            //    else
            //    {
            //        return "ID already exists!";
            //    }
            //}
            //else
            //{
            return "ID Must be a valid integer!";
            //}
        }
    }
}
