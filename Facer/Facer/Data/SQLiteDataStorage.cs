using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;

namespace Facer.Data
{
    class SQLiteDataStorage : DataStorage
    {
        private SQLiteConnection Database;

        public SQLiteDataStorage(string dbPath, string dbName)
        {
            Database = new SQLiteConnection(Path.Combine(dbPath,dbName + "db3"));
            // Types that can be converted to columns in a table must have all of their fields primitive
            // Exact list of allowed field types https://github.com/praeclarum/sqlite-net/wiki/Features
            Database.CreateTable<Student>();
            Database.CreateTable<AttendanceRecord>();
        }

        public override void EnrollStudent(Student st)
        {
            _enrolledStudents.Add(st.ID, st);
            Database.InsertOrReplace(st);
        }

        public override void RemoveStudent(Student st)
        {
            // Caution: This method does not remove the student from attendance lists
            _enrolledStudents.Remove(st.ID);
            Database.Delete<Student>(st.ID);
        }

        public override void UpdateStudent(Student st)
        {
            Database.InsertOrReplace(st);
        }

        public override void ClearStudents()
        {
            _enrolledStudents.Clear();
            Database.DeleteAll<Student>();
        }

        public override void CreateAttendanceRecord(AttendanceRecord ar)
        {
            _attendanceRecords.Add(ar.Date,ar);
            Database.InsertOrReplace(ar);
        }

        public override void RemoveAttendanceRecord(AttendanceRecord ar)
        {
            _attendanceRecords.Remove(ar.Date);
            Database.Delete<AttendanceRecord>(ar);
        }

        public override void UpdateAttendanceRecord(AttendanceRecord ar)
        {
            Database.InsertOrReplace(ar);
        }

        public override void ClearAttendanceRecords()
        {
            Database.DeleteAll<AttendanceRecord>();
        }

        public override void ClearData()
        {
            ClearAttendanceRecords();
            ClearStudents();
        }

        public override void LoadData()
        {
            _enrolledStudents = new Dictionary<int, Student>();
            List<Student> stlist = Database.Query<Student>("SELECT * FROM Student");
            foreach (Student st in stlist)
            {
                _enrolledStudents.Add(st.ID, st);
            }
            _attendanceRecords = new SortedList<DateTime, AttendanceRecord>();
            List<AttendanceRecord> arlist = Database.Query<AttendanceRecord>("SELECT * FROM AttendanceRecord");
            foreach(AttendanceRecord ar in arlist)
            {
                ar.Deserialize(_enrolledStudents);
                _attendanceRecords.Add(ar.Date, ar);
            }
        }

        public override void Close()
        {
            Database.Close();
        }
    }
}
