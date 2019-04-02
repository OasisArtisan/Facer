using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageTesting
{
    public class ImageReady
    {
        public FileInfo file { get; private set; }

        public ImageReady(string path)
        {
            file = new FileInfo(path);

            if (!file.Exists)
            {
                throw new FileNotFoundException("File doesn't exist, check it again");
            }

            if(!(file.Name.EndsWith("jpeg") || file.Name.EndsWith("jpg") || file.Name.EndsWith("png")))
            {
                throw new TypeInitializationException("This is not supported type, image should be either JPEG or PNG", null);
            }

        }


        public byte[] GetImageAsByteArray()
        {
            using (FileStream fileStream =
                new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
