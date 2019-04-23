using System;

namespace Facer.FaceApi
{
    public class Face
    {
        public string faceId { get; set; }
        public FaceRectangle faceRectangle { get; set; }
        public  DateTime DetectionDate {get;set;}
    }
}
