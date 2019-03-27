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
            string combinedPath = Path.Combine(dbPath, dbName + ".db3");
            Console.WriteLine("Coco}" + combinedPath);
            Database = new SQLiteConnection(combinedPath);
            // Types that can be converted to columns in a table must have all of their properites primitive
            // Only properties that have a get and set access will be converted to a column
            // Exact list of allowed field types https://github.com/praeclarum/sqlite-net/wiki/Features
            Database.CreateTable<Student>();
            Database.CreateTable<AttendanceRecord>();
        }

        public override void EnrollStudent(Student st)
        {
            st.BindToStorage(this);
            _enrolledStudents.Add(st.ID, st);
            Database.Insert(st);
        }

        public override void RemoveStudent(Student st)
        {
            RemoveStudent(st.ID);
        }
        public override void RemoveStudent(int id)
        {
            // Caution: This method does not remove the student from attendance records
            _enrolledStudents.Remove(id);
            Database.Delete<Student>(id);
        }

        public override void UpdateStudent(Student st)
        {
            Database.Update(st);
        }

        public override void ClearStudents()
        {
            _enrolledStudents.Clear();
            Database.DeleteAll<Student>();
        }

        public override void CreateAttendanceRecord(AttendanceRecord ar)
        {
            ar.BindToStorage(this);
            _attendanceRecords.Add(ar.Date,ar);
            Database.InsertOrReplace(ar);
        }

        public override void RemoveAttendanceRecord(AttendanceRecord ar)
        {
            RemoveAttendanceRecord(ar.Date);
        }

        public override void RemoveAttendanceRecord(DateTime dt)
        {
            _attendanceRecords.Remove(dt);
            Database.Delete<AttendanceRecord>(dt);
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
                st.BindToStorage(this);
                _enrolledStudents.Add(st.ID, st);
            }
            _attendanceRecords = new SortedList<DateTime, AttendanceRecord>();
            List<AttendanceRecord> arlist = Database.Query<AttendanceRecord>("SELECT * FROM AttendanceRecord");
            foreach(AttendanceRecord ar in arlist)
            {
                ar.BindToStorage(this);
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
