
namespace Facer.FaceApi
{
    public class FaceIdentifyResult
    {
        public string faceId { get; set; }
        public IdentificationCandidate[] candidates { get; set; }
    }
}
