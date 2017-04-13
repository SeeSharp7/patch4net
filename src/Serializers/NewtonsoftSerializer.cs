using Newtonsoft.Json;

namespace SeeSharp7.Patch4Net.Serializers
{
    /// <summary>
    /// Uses <see cref="NewtonsoftSerializer"/> with no special formatting
    /// </summary>
    public class NewtonsoftSerializer : ISerializer
    {
        /// <inheritdoc cref="ISerializer.DeserializeObject{TModelType}"/>
        public TModelType DeserializeObject<TModelType>(string serializedObject)
        {
            return JsonConvert.DeserializeObject<TModelType>(serializedObject);
        }

        /// <inheritdoc cref="ISerializer.SerializeObject{TModelType}"/>
        public string SerializeObject<TModelType>(TModelType modelObject)
        {
            return JsonConvert.SerializeObject(modelObject, Formatting.None);
        }
    }
}