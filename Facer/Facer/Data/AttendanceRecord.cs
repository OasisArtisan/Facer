using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Facer.Data
{
    public class AttendanceRecord
    {
        private DataStorage DataStorage;
        private bool _valid;
        private DateTime _date;
        private Dictionary<int, Student> _attendedStudents;
        private string _attendedStudentsSerialized;

        public bool Valid { get; }

        [PrimaryKey]
        public DateTime Date {
            get {
                return _date;
            }
            set {
                if (_valid)
                {
                    throw new MemberAccessException("Cannot change date after setting it");
                }
                _valid = true;
                _date = value;
            }
        }

        public string AttendedStudentsSerialized
        {
            get
            {
                if (_attendedStudentsSerialized == null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (int id in _attendedStudents.Keys)
                    {
                        sb.Append(id);
                        sb.Append(",");
                    }
                    //Remove trailing comma
                    if(sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    _attendedStudentsSerialized = sb.ToString();
                }
                return _attendedStudentsSerialized;
            }
            set
            {
                _attendedStudentsSerialized = value;
            }
        }

        public AttendanceRecord()
        {
            _attendedStudents = new Dictionary<int, Student>();
            _attendedStudentsSerialized = "";
        }

        // Populates the local dictionary with student objects that are obtained by looking up
        // the keys stored in the _attendedStudentsSerialized field
        public void Deserialize(Dictionary<int, Student> EnrolledStudents)
        {
            string[] ids = _attendedStudentsSerialized.Split(',');
            foreach (string id in ids)
            {
                int i = int.Parse(id);
                Student st = null;
                // Ignore ids that are not included anymore in the enrolled students list
                if (EnrolledStudents.TryGetValue(i, out st))
                {
                    _attendedStudents.Add(i, st);
                }
            }
        }

        // Mutator Methods
        // To preserve the integrity of the general data structure, the student object passed must be enrolled.
        public void AddStudent(Student st)
        {
            _attendedStudents.Add(st.ID, st);
            _attendedStudentsSerialized = null;
            UpdateStorage();
        }

        public void RemoveStudent(Student st)
        {
            _attendedStudents.Remove(st.ID);
            _attendedStudentsSerialized = null;
            UpdateStorage();
        }

        // Observer Methods

        public Dictionary<int, Student>.Enumerator EnumerateAttendedStudents()
        {
            return _attendedStudents.GetEnumerator();
        }

        public Student GetStudent(int id)
        {
            Student st = null;
            _attendedStudents.TryGetValue(id, out st);
            return st;
        }

        public bool ContainsStudent(Student st)
        {
            return _attendedStudents.ContainsKey(st.ID);
        }

        // Utility Methods
        private void UpdateStorage()
        {
            if(DataStorage != null)
            {
                DataStorage.UpdateAttendanceRecord(this);
            }
        }

        public void BindToStorage(DataStorage ds)
        {
            DataStorage = ds;
        }
    }
}