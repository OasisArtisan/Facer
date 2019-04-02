using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Cognitive.Face;

// SubscriptionKey1 = 3a0ddda1ac4a4990a8061356b38c4e75
// SubscriptionKey2 = 737a8767fdf64d6b8d364b2a907c580c
// EndPoint = https://westcentralus.api.cognitive.microsoft.com/face/v1.0
// Group 1: Name: Test02, ID: 000121
// Group 2: Name: Test01, ID: 000122
// Person 1: Name: dodo,  ID: 2c65ab34-4853-48c6-8e19-3cd612902ec6         Note: this ID is given from Face API

namespace ImageTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //FaceClient.Shared.Endpoint = Endpoints.WestUS;
            //FaceClient.Shared.SubscriptionKey = "3a0ddda1ac4a4990a8061356b38c4e75";
            //ImageReady im = new ImageReady("C:/Users/HP/Pictures/Facer/brothers.jpeg");
            //FaceDetectionRequest faceRequest = new FaceDetectionRequest(im);

            //Task<string> Result = faceRequest.MakeAnalysisRequest();

            //ImageModify.AddImage(im);
            //ImageModify.FaceRectngle(Result.Result);
            //ImageModify.Show();

            //GroupManager.CreatPersonGroup("Test01", "000122");
            //var groups = GroupManager.GetGroups();
            //foreach(var grp in groups.Result)
            //{
            //    Console.WriteLine(grp.Name);
            //}

            //if(PersonManager.Creatperson("000121", "dodo", SharedData.SubscriptionKey2).Result)
            //{
            //Console.WriteLine("Successfully person added");
            var path = "C:/Users/HP/Pictures/Facer/dad.jpeg";
            if (PersonManager.AddpersonFace("000121", "2c65ab34-4853-48c6-8e19-3cd612902ec6", path, SharedData.SubscriptionKey2).Result)
                {
                    Console.WriteLine("Face is Added successfully");
                }
            //}
            Console.ReadKey();
        }
    }
}
