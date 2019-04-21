using System;
using System.IO;

namespace ImageTesting
{
    public static class StringHelper
    {
        public static string GetFileNameFromPath(this string path)
        {
            var splittedpath = path.Split(Path.DirectorySeparatorChar);

            return splittedpath[splittedpath.Length - 1];
        }

        public static string GetFileNameFromPath(this string path, bool deleteType)
        {

            var lastName = GetFileNameFromPath(path);
            
            if (deleteType)
            {
                int index = lastName.IndexOf('.');
                if (index > 0)
                    lastName = lastName.Remove(index);
            }

            return lastName;
        }
    }
}
