using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;


// SubscriptionKey1 = 33a588f8f7684e14a6bd94ce725cdb4c
// SubscriptionKey2 = fea5d65e88dc4b59a897543537aa19d1
// EndPoint = https://westcentralus.api.cognitive.microsoft.com/face/v1.0
// Group 1: Name: Test1, ID: 111111

namespace ImageTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Commented Area
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
            //var path = "C:/Users/HP/Pictures/Facer/dad.jpeg";
            //if (PersonManager.AddpersonFace("000121", "2c65ab34-4853-48c6-8e19-3cd612902ec6", path, SharedData.SubscriptionKey2).Result)
            //    {
            //        Console.WriteLine("Face is Added successfully");
            //    }
            //}
            #endregion
            

            MainAsync();

            Console.WriteLine("Main is done");
            Console.ReadKey();
        }

        public async static void _MainAsync()
        {
            var manager = new PersonGroupManager(SharedData.SubscriptionKey);
            var list = await manager.GetPersonGroups();
            foreach(var g in list)
                Console.WriteLine($"{g.Name} : {g.PersonGroupID}");
        }

        public async static void MainAsync()
        {
            Console.WriteLine("Async Started");
            StudentDetector sd = await StudentDetector.CreateStudentDetector();
            //var paths = Directory.GetFiles(@"D:\ImageDataSet\A");
            //Console.WriteLine("Images Paths:");
            //Console.WriteLine(string.Join("\n", paths));
            //Console.WriteLine("\nStart adding Student");
            //await sd.AddStudentAsync(new Student { FirstName = "Abdullah", LastName = "Alamoodi" }, paths);
            //Console.WriteLine("Every body is added");
            //Console.WriteLine("Training Started...");
            //await sd.TrainGroup();
            //Console.WriteLine("Done Training.");
            //var time = DateTime.Now.Second;
            Console.WriteLine("Start Identification Process...");
            var dict = await sd.Identify(@"D:\ImageDataSet\id5.jpg");
            Console.WriteLine("Done Identification...");
            foreach(var p in dict.Keys)
            {
                if(p.Name != null && p.Name.Length > 1)
                {
                    Console.WriteLine($"Person:{p.Name}, Confidence: {dict[p].Confidence}");
                }
                else
                    Console.WriteLine("Unknown Person...");
            }
        }
        public async static void __MainAsync()
        {
            // Creating instances of our objects
            var GManager = new PersonGroupManager(SharedData.SubscriptionKey);
            var PManager = new PersonManager(SharedData.SubscriptionKey);

            // Setting group name and group ID
            var group1Name = "Test1";
            var group1ID = "111111";

            var test = new FaceApi(new ImageReady(@"D:\ImageDataSet\id1.jpg"));
            Console.WriteLine(test.JSONIdentificationBodyWriter("1121", 10, 0.8, "abcdefg", "hijklmnop", "qrstuv", "wxyz"));

            // Make The Group Manager
            //await GManager.CreatPersonGroup(group1Name, group1ID);
            //Console.WriteLine("Person Group is Made");

            //// Add people and their faces to the web
            //foreach(var folder in Directory.GetDirectories(@"D:\ImageDataSet\PersonGroup"))
            //{
            //    // Make a person and get his\her ID
            //    var personID = await PManager.CreatPerson(group1ID, folder.GetFileNameFromPath());
            //    Console.WriteLine($"Person {folder.GetFileNameFromPath()} with ID {personID} is added");

            //    // Add all the images connected to this person
            //    foreach(var image in Directory.GetFiles(folder))
            //    {
            //        await PManager.AddPersonFace(group1ID, personID, image);
            //    }

            //    Console.WriteLine("One Man is Added With All His Faces...");
            //    await Task.Delay(10000);
            //}


            //Console.WriteLine("Satrt Training...");

            //// Train the added people
            //await GManager.TrainPersonGroup(group1ID);

            //// Wiat Until The End Of Training
            //while(true)
            //{
            //    var trainingResult = await GManager.GetTainingStatus(group1ID);
            //    if(trainingResult == "succeeded")
            //        break;
            //    await Task.Delay(300);
            //    Console.WriteLine("Still Training");
            //}

            //Console.WriteLine("Training is Done.");

            //// Detect an image and dentify the people in this image

            // Rerun This
            var faceApi = new FaceApi(new ImageReady(@"D:\ImageDataSet\identification3.jpg"));
            var faces = await faceApi.DetectFaces();
            Console.WriteLine("Faces is Detected...");

            var persons = await PManager.GetAllPersons(group1ID);
            Console.WriteLine("All Persons are here...");

            var identified = await faceApi.IdentifyPerson(group1ID, 3, 0.7, faces.Select<Face, string>(x => x.faceId).ToArray<string>());
            foreach(var iden in identified)
            {
                Person? person = null;
                if(iden.candidates.Length == 0)
                {
                    Console.WriteLine("Face is not identified");
                }
                else
                {
                    person = persons.Find(x => x.PersonID == iden.candidates[0].personId);
                    if(person != null)
                        Console.WriteLine($"The person in the image is: {person.Value.Name}");
                    else
                        Console.WriteLine("No person is found");
                }
                

                person = null;
            }
            // Until Here
        }
    }
}
