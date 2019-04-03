using System;
using System.Collections.Generic;
using System.Text;

namespace Facer.Data
{
    public abstract class DataStorage
    {
        protected Dictionary<int, Student> _enrolledStudents;
        protected SortedList<DateTime, AttendanceRecord> _attendanceRecords;

        // Mutator Methods
        public abstract void EnrollStudent(Student st);
        public abstract void RemoveStudent(Student st);
        public abstract void RemoveStudent(int id);
        public abstract void UpdateStudent(Student st);
        public abstract void ClearStudents();

        // Observer Methods
        public Dictionary<int, Student>.Enumerator EnumerateEnrolledStudents()
        {
            return _enrolledStudents.GetEnumerator();
        }
        public Student GetEnrolledStudent(int id)
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
        public IEnumerator<KeyValuePair<DateTime, AttendanceRecord>> EnumerateAttendanceRecords()
        {
            return _attendanceRecords.GetEnumerator();
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
