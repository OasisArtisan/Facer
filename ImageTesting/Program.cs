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
            Console.WriteLine(Directory.GetCurrentDirectory());

            MainAsync();

            Console.WriteLine("Successfully done");
            Console.ReadKey();
        }

        public async static void MainAsync()
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


            ///////////////////////////////////// Start Of Image Recognition Test 1 //////////////////////////////////////
            //// Apply image detection
            //var tool = new FaceApi(new ImageReady(@"D:\ImageDataSet\id1.jpg"));
            //var result = await tool.DetectFaces();

            //// print all the found faces IDs
            //foreach(var item in result)
            //{
            //    Console.WriteLine(item.faceId);
            //}

            //// Identify faces
            //var result2 = await tool.IdentifyPerson(group1ID, 5, 0.7, result.Select<Face, string>(x => x.faceId).ToArray());

            //// Get all the available people on the API page
            //var persons = await PManager.GetAllPersons(group1ID);

            //// Try 
            //foreach(var res in result2)
            //{
            //    Person? person = persons.Find(
            //        x => persons.Exists(
            //            p => x.PersonID == p.PersonID
            //        )
            //    );

            //    if(person == null)
            //    {
            //        Console.WriteLine(person.Value.Name);
            //    }
            //    person = null;
            //}
            //////////////////////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////// Start Of Image Testing 2 ///////////////////////////////////////////

            //// Training Request
            //Console.WriteLine("Train Reuset Response" + await GManager.TrainPersonGroup(group1ID));
            //var StartTime = DateTime.Now.Second;

            //await Task.Delay(5000);

            //// Check for the end of trainging session
            //var state = await GManager.GetTainingStatus(group1ID);
            //while(state != "succeeded")
            //{
            //    await Task.Delay(TimeSpan.FromMilliseconds(1000));
            //    state = await GManager.GetTainingStatus(group1ID);
            //    Console.WriteLine("Passed Time: " + (StartTime - DateTime.Now.Second));
            //}

            //// Getting all the people on the web and printing their names
            //var persons = await PManager.GetAllPersons(group1ID);
            //foreach(var m in persons)
            //    Console.WriteLine(m.Name);

            //// 
            //Console.WriteLine(await GManager.CreatPersonGroup(group1Name, group1ID));
            //List<string> pgs = new List<string>();
            //using(var file = new StreamReader("./DataUsed.txt"))
            //{
            //    string line;
            //    while((line = file.ReadLine()) != null)
            //    {
            //        Console.WriteLine(line);
            //        pgs.Add(line);
            //    }
            //}


            //foreach(var folder in Directory.GetDirectories(@"D:\ImageDataSet\PersonGroup"))
            //{
            //    using(var file = new StreamReader("./DataUsed.txt"))
            //    {
            //        //This Need to be stored
            //       var personID = await PManager.CreatPerson(group1ID, folder.GetFileNameFromPath());
            //        await file.WriteLineAsync($"Person ID: {personID}");
            //        Console.WriteLine(personID);
            //        enm.MoveNext();
            //        foreach(var image in Directory.GetFiles(folder))
            //        {
            //            Console.WriteLine("Person Result: " + await PManager.AddPersonFace(group1ID, enm.Current, image));
            //        }
            //        await file.FlushAsync();
            //    }

            //    var directories = Directory.GetDirectories(@"D:\ImageDataSet\PersonGroup");
            //    var file1 = Directory.GetFiles(directories[6])[2];
            //    var file2 = Directory.GetFiles(directories[7])[0];
            //    var file3 = Directory.GetFiles(directories[7])[1];
            //    var file4 = Directory.GetFiles(directories[7])[2];

            //    Console.WriteLine("Person Result: " + await PManager.AddPersonFace(group1ID, pgs[6], file1));
            //    Console.WriteLine("Person Result: " + await PManager.AddPersonFace(group1ID, pgs[7], file2));
            //    Console.WriteLine("Person Result: " + await PManager.AddPersonFace(group1ID, pgs[7], file3));
            //    Console.WriteLine("Person Result: " + await PManager.AddPersonFace(group1ID, pgs[7], file4));

            //}
        }
    }
}
