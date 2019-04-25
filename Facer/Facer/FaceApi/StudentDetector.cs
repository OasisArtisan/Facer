﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Facer.Data;

namespace Facer.FaceApi
{
    public class StudentDetector 
    {
        #region Private Data
        private string _key = SharedData.SubscriptionKey;
        private PersonManager _pManager;
        private PersonGroupManager _gManager;
        private string _groupID;
        private string _groupName = "MainGroup";
        private List<Person> _allPeople = null;
        private bool _groupDeletedFlag;
        
        #endregion

        #region Public Data
        public bool TrainingUpdated;
        #endregion

        #region Constructor
        public async static Task<StudentDetector> CreateStudentDetector()
        {
            // Data for new group if a group will be created
            string groupName = "MainGroup";
            string groupID = "123456";

            // Make new group if needed and return the groupID or if group is already there return it ID
            string _grpID = await MakeServerGroup(groupID, groupName);
            groupID = _grpID == string.Empty ? groupID : _grpID;

            // Return the newly created "StudentDetector" object 
            return new StudentDetector(groupID);
        }

        private StudentDetector(string groupID)
        {
            _groupID = groupID;

            // Make instances of the needed tools
            _pManager = new PersonManager(_key);
            _gManager = new PersonGroupManager(_key);
        }
        #endregion

        #region Public Methods

        public async Task AddStudentAsync(Student s, params string[] images)
        {
            Console.WriteLine($"[StudentDetector] Adding {s.Formatted}");
            var studentID = await _pManager.CreatPerson(_groupID, ($"{s.ID}"));
            foreach(var image in images)
            {
                Console.WriteLine($"[StudentDetector] Adding Image Path: {image}");
                await _pManager.AddPersonFace(_groupID, studentID, image);
            }
            TrainingUpdated = false;
        }

        public async Task<Dictionary<string, FaceRectangle>> Detect(string imagePath, FaceApi api = null)
        {
            Console.WriteLine($"[StudentDetector] Detecting image in path: {imagePath}");
            // Assign appripriate FaceApi Tool >>> this is done this way for the sake of "Identify" method
            FaceApi faceTools;
            if(api == null)
                faceTools = new FaceApi(new ImageReady(imagePath));
            else
                faceTools = api;

            // Detect Faces
            var faces = await faceTools.DetectFaces();

            // Convert it into dictionary
            var dict = new Dictionary<string, FaceRectangle>();
            foreach(var face in faces)
            {
                dict.Add(face.faceId, face.faceRectangle);
            }

            // Return Dictionary with specified Features
            return dict;
        }

        public async Task<Dictionary<Person, IdentificationInfo>> Identify(string path)
        {
            Console.WriteLine($"[StudentDetector] Identifying image in path: {path}");
            if(!TrainingUpdated)
            {
                await TrainGroup();
            }
            var faceTools = new FaceApi(new ImageReady(path));

            // Detect Faces
            var facesDict = await Detect(path, faceTools);
            Console.WriteLine($"[StudentDetector] Detected {facesDict.Count} faces.");
            // Saperate facesID
            var facesIDs = facesDict.Keys.ToArray<string>();

            // Variable to store the result of Identification
            Dictionary<Person, IdentificationInfo> finalResult = new Dictionary<Person, IdentificationInfo>();

            // Identify faces (10 each time)
            int counter = 0;
            int NumberLeft = facesIDs.Length;
            while(NumberLeft > 0)
            {
                NumberLeft -= 10;
                var toBeIdentified = facesIDs.Skip(10 * counter).Take(10);

                var IdentificationResult = await faceTools.IdentifyPerson(_groupID, 1, 0.6, facesIDs);

                foreach(var iden in IdentificationResult)
                {
                    if(iden.candidates.Count() < 1)
                    {
                        finalResult.Add(new Person(), new IdentificationInfo(facesDict[iden.faceId], 0));
                    }
                    else
                    { 
                        var id = iden.candidates[0].personId;
                        var person = await GetStudentByID(id);
                        Console.WriteLine($"[StudentDetector] Found candidate {person.LocalID}");
                        finalResult.Add(person, new IdentificationInfo(facesDict[iden.faceId], iden.candidates[0].confidence));
                    }
                }

                // Delay for 30s after every two Identifcation request
                counter++;
                if(counter % 2 == 0)
                    await Task.Delay(30000);
            }
            string peopleIdentified = "";
            int identified = 0;
            Console.WriteLine("-------------------HohHOHOHOOHOHOHOHOHOOO----------------");
            foreach(Person p in finalResult.Keys)
            {
                if(p.LocalID == null)
                {
                    continue;
                }
                peopleIdentified += $"\nID:{p.LocalID} CONF:{finalResult[p].Confidence*100f}%";
                identified++;
            }
            Console.WriteLine($"[StudentDetector] Identified {identified}/{facesDict.Count}.{peopleIdentified}");
            return finalResult;

        }
        
        public async Task<bool> TrainGroup()
        {
            Console.WriteLine("[StudentDetector] Training Group...");
            var result = await _gManager.TrainPersonGroup(_groupID);
            TrainingUpdated = true;
            return result;
        }

        public async Task<bool> DeletePerson(Student student)
        {
            var person = await GetStudentByID(student.ID);

            return await _pManager.DeletePerson(_groupID, person.ServerID);
        }

        public async Task<bool> ResetGroup()
        {

            var result = await _gManager.DeletePersonGroup(_groupID);

            if(result)
            {
                _groupDeletedFlag = true;
                return result;
            }
            return result;
        }
        #endregion

        #region Helper Function
        private async Task<Person> GetStudentByID(string id)
        {
            if(_groupDeletedFlag == true)
            {
                _allPeople = await _pManager.GetAllPersons(_groupID);
            }
            
            foreach (Person p in _allPeople)
            {
                Console.WriteLine($"LocalID: {p.LocalID} ServerID: {p.ServerID} id: {id}");
            }
            return _allPeople.Find(x => x.ServerID == id);
        }

        /// <summary>
        /// If there is no groups in the cloud this function will make one with the paramerter provided and it will return Empty String, 
        /// if ther is a one in the cloud it will return the new ID for the group in the cloud.
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private async static Task<string> MakeServerGroup(string groupID, string groupName)
        {
            // Get List of all the available groups on the server
            var groups = await PersonGroupManager.GetPersongroups(SharedData.SubscriptionKey);
            Console.WriteLine($"List is here with lenght: {groups.Count}");

            // If no group are there, make one...
            if(groups.Count < 1)
            {
                await PersonGroupManager.CreatPersongroup(groupName, groupID, SharedData.SubscriptionKey);
                Console.WriteLine("New group is created...");
                return string.Empty;
            }
            else // If there is/are group/s take the ID of the first one
            {
                groupID = groups[0].PersonGroupID;
                Console.WriteLine($"Group ID got {groupID}");
                return groupID;
            }
        }

        #endregion
    }


}
