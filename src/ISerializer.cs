namespace SeeSharp7.Patch4Net
{
    public interface ISerializer
    {
        TModelType DeserializeObject<TModelType>(string serializedObject);
        string SerializeObject<TModelType>(TModelType modelObject);
    }
}