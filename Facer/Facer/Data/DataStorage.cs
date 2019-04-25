using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Facer.Data
{
    public abstract class DataStorage
    {
        protected Dictionary<string, Student> _enrolledStudents;
        protected SortedList<DateTime, AttendanceRecord> _attendanceRecords;

        public DataStorage()
        {
            _enrolledStudents = new Dictionary<string, Student>();
            _attendanceRecords = new SortedList<DateTime, AttendanceRecord>();
        }

        // Mutator Methods
        public abstract void EnrollStudent(Student st);
        public abstract void RemoveStudent(Student st);
        public abstract void RemoveStudent(string id);
        public abstract void UpdateStudent(Student st);
        public abstract void ClearStudents();

        // Observer Methods
        public bool IDExists(string id)
        {
            return _enrolledStudents.ContainsKey(id);
        }

        public IEnumerable<Student> EnrolledStudentsEnumerable()
        {
            return _enrolledStudents.Values;
        }

        public int EnrolledStudentsCount
        {
            get { return _enrolledStudents.Count; }
        }

        public Student GetEnrolledStudent(string id)
        {
            Student st = null;
            _enrolledStudents.TryGetValue(id, out st);
            return st;
        }


        // Mutator Methods
        public abstract void CreateAttendanceRecord(AttendanceRecord ar);
        public abstract void RemoveAttendanceRecord(AttendanceRecord ar);
        public abstract void RemoveAttendanceRecord(DateTime dt);
        public abstract void UpdateAttendanceRecord(AttendanceRecord ar);
        public abstract void ClearAttendanceRecords();

        // Observer Methods
        public IEnumerable<AttendanceRecord> AttendanceRecordsEnumerable()
        {
            return _attendanceRecords.Values;
        }

        public int AttendanceRecordsCount
        {
            get { return _attendanceRecords.Count; }
        }

        public AttendanceRecord GetAttendanceRecord(DateTime dt)
        {
            AttendanceRecord ar = null;
            _attendanceRecords.TryGetValue(dt, out ar);
            return ar;
        }

        // General Methods
        public abstract void ClearData();
        public abstract void LoadData();
        public abstract void Close();

        public static void mm()
        {
            var data = new SQLiteDataStorage("","");
        }
    }
}
