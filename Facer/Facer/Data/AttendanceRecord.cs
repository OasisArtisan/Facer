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
        private Dictionary<string, Student> _attendedStudents;
        private string _attendedStudentsSerialized;

        public string Formatted { get
            {
                return _date.ToString();
            }
        }

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
                    foreach (string id in _attendedStudents.Keys)
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
            _attendedStudents = new Dictionary<string, Student>();
            _attendedStudentsSerialized = "";
        }

        // Populates the local dictionary with student objects that are obtained by looking up
        // the keys stored in the _attendedStudentsSerialized field
        public void Deserialize(Dictionary<string, Student> EnrolledStudents)
        {
            if(_attendedStudentsSerialized.Length == 0)
            {
                return;
            }
            string[] ids = _attendedStudentsSerialized.Split(',');
            foreach (string id in ids)
            {
                Student st = null;
                // Ignore ids that are not included anymore in the enrolled students list
                if (EnrolledStudents.TryGetValue(id, out st))
                {
                    _attendedStudents.Add(id, st);
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

        public IEnumerable<Student> EnumerateAttendedStudents()
        {
            return _attendedStudents.Values;
        }

        public Student GetStudent(string id)
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