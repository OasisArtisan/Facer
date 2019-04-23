using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ImageTesting
{
    class StudentDetector 
    {
        #region Private Data
        private string _key = SharedData.SubscriptionKey;
        private PersonManager _pManager;
        private PersonGroupManager _gManager;
        private string _groupID;
        private string _groupName = "MainGroup";
        private List<Person> _allPeople = null;
        #endregion

        #region Public Data

        #endregion

        #region Constructor
        public async static Task<StudentDetector> CreateStudentDetector()
        {
            // Data for new group if a group will be created
            string groupName = "MainGroup";
            string groupID = "123456";

            // Get List of all the available groups on the server
            var groups = await PersonGroupManager.GetPersongroups(SharedData.SubscriptionKey);

            // If no group are there, make one...
            if(groups.Count < 1)
            {
                await PersonGroupManager.CreatPersongroup(groupName, groupID, SharedData.SubscriptionKey);
            }
            else // If there is/are group/s take the ID of the first one
            {
                groupID = groups[0].PersonGroupID;
            }

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
            var studentID = await _pManager.CreatPerson(_groupID, ($"{s.FirstName} {s.LastName}"));
            s.ServerID = studentID;
            foreach(var image in images)
            {
                await _pManager.AddPersonFace(_groupID, studentID, image);
            }
        }

        public async Task<Dictionary<string, FaceRectangle>> Detect(string imagePath, FaceApi api = null)
        {
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
            var faceTools = new FaceApi(new ImageReady(path));

            // Detect Faces
            var facesDict = await Detect(path, faceTools);

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
                        finalResult.Add(person, new IdentificationInfo(facesDict[iden.faceId], iden.candidates[0].confidence));
                    }
                }

                // Delay for 30s after every two Identifcation request
                counter++;
                if(counter % 2 == 0)
                    await Task.Delay(30000);
            }

            return finalResult;

        }
        
        public async Task<bool> TrainGroup()
        {
            var result = await _gManager.TrainPersonGroup(_groupID);

            return result;
        }
        #endregion

        #region Helper Function
        private async Task<Person> GetStudentByID(string id)
        {
            if(_allPeople == null)
            {
                _allPeople = await _pManager.GetAllPersons(_groupID);
            }

            var ss = await _pManager.GetAllPersons(_groupID);

            return ss.Find(x => x.PersonID == id);
        }
        #endregion
    }

    
}
