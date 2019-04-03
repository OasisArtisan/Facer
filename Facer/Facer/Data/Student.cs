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
        private int _id;
        private bool valid;

        [PrimaryKey]
        public int ID {
            get {
                return _id;
            }
            set {
                if (valid)
                {
                    throw new MemberAccessException("Cannot change id after setting it");
                }
                valid = true;
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

        public static void BindToStorage(DataStorage ds)
        {
            DataStorage = ds;
        }
    }
}
