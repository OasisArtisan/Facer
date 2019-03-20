using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Facer.Data
{
    public class Student
    {
        //If the static datastorage object is not null, it will be notified each time a field is updated
        public static DataStorage DataStorage { get; set; }

        private string _firstname;
        private string _lastname;

        [PrimaryKey]
        public int ID { get; }

        public string FirstName { get { return _firstname; } set { _firstname = value; UpdateStorage(); } }
        public string LastName { get { return _lastname; } set { _lastname = value; UpdateStorage(); } }

        private void UpdateStorage()
        {
            if (DataStorage != null)
            {
                DataStorage.UpdateStudent(this);
            }
        }
    }
}
