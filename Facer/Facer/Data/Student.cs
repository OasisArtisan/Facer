using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Facer.Data
{
    public class Student
    {
        private static DataStorage DataStorage;
        private string _firstname;
        private string _lastname;
        private string _id;
        private bool _valid;

        public string Formatted { get { return ToString(); } }

        public bool Valid { get; }

        [PrimaryKey]
        public string ID {
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
        
        public string FirstName { get { return _firstname; } set { _firstname = value; UpdateStorage(); } }
        public string LastName { get { return _lastname; } set { _lastname = value; UpdateStorage(); } }

        private void UpdateStorage()
        {
            if (DataStorage != null)
            {
                DataStorage.UpdateStudent(this);
            }
        }

        public void BindToStorage(DataStorage ds)
        {
            DataStorage = ds;
        }

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
            if (!App.Reference.Data.IDExists(id))
            {
                return null;
            }
            else
            {
                return "ID already exists!";
            }
        }
    }
}
