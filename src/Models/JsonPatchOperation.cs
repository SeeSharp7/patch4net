namespace SeeSharp7.Patch4Net.Models
{
    public class JsonPatchOperation
    {
        public string Operation { get; set; }

        public string Path { get; set; }

        public string Value { get; set; }
    }
}