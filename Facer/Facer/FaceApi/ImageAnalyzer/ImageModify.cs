

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Newtonsoft.Json;
using System.Drawing;

namespace Facer.FaceApi
{
    public class ImageModify
    {
        public static Mat MainImage{get; private set;}

        public static void AddImage(ImageReady image)
        {
            MainImage = CvInvoke.Imread(image.file.FullName);
        }

        public static void AddImage(Mat image)
        {
            MainImage = image;
        }

        public static void FaceRectngle(string squares, ImageReady image = null)
        {
            Mat imageMat;
            if (image == null)
            {
                imageMat = MainImage;
            }
            else
            {
                imageMat = CvInvoke.Imread(image.file.FullName);
            }
                

            // Read JSON file
            Face[] faces = JsonConvert.DeserializeObject<List<Face>>(squares).ToArray();

            // Drawing the rectangle
            foreach(var face in faces)
            {
                CvInvoke.Rectangle(imageMat, new Rectangle(face.faceRectangle.left,
                    face.faceRectangle.top, face.faceRectangle.width, face.faceRectangle.height), new MCvScalar(25, 25, 25), 2);
            }

            if(image != null)
            {
                AddImage(imageMat);

            }
        }

        public static void Show(Mat imageMat = null, ImageReady imageReady = null)
        {
            if(imageMat != null)
            {
                CvInvoke.Imshow("_", imageMat);
            }
            else if(imageReady != null)
            {
                CvInvoke.Imshow("_", CvInvoke.Imread(imageReady.file.FullName));
            }
            else
            {
                CvInvoke.Imshow("_", MainImage);
            }
            CvInvoke.WaitKey();
            
        }
        
    }
}
