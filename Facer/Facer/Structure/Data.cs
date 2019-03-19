using System;
using System.Collections.Generic;
using System.Text;

namespace Facer.Structure
{
    public class Data
    {
        public List<AttendanceDate> AttendanceDates { get; }
        public List<Student> EnrolledStudents { get; }

        public Data()
        {
            AttendanceDates = new List<AttendanceDate>();
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }

    public class AttendanceDate
    {
        public DateTime Date { get; set; }
        public List<Student> AttendedStudents {get;}

        public AttendanceDate()
        {
            AttendedStudents = new List<Student>();
        }
    }
}
