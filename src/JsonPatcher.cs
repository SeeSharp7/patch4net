using Newtonsoft.Json.Linq;
using SeeSharp7.Patch4Net.Serializers;

namespace SeeSharp7.Patch4Net
{
    public class JsonPatcher
    {
        private readonly ISerializer _serializer;

        /// <summary>
        /// Creates a new instance of <see cref="JsonPatcher"/>
        /// with the default <see cref="NewtonsoftSerializer"/>
        /// </summary>
        public JsonPatcher()
        {
            _serializer = new NewtonsoftSerializer();
        }

        /// <summary>
        /// Creates a new instance of <see cref="JsonPatcher"/>
        /// with your specified instance of <see cref="ISerializer"/>
        /// </summary>
        /// <param name="serializer"></param>
        public JsonPatcher(ISerializer serializer)
        {
            _serializer = serializer;
        }

        /// <summary>
        /// Performs a json merge patch (RFC 7386)
        /// </summary>
        /// <typeparam name="TModel">The <see cref="System.Type"/> of your model</typeparam>
        /// <param name="mergePatchRequestBody">The request body containing the merge patch content</param>
        /// <param name="originalModel">The original model to modify</param>
        /// <returns>Returns a new instance of the <param name="originalModel"></param></returns>
        public TModel MergePatch<TModel>(string mergePatchRequestBody, TModel originalModel)
        {
            var jParsedOriginalModel = GetJObjectFromModel(originalModel);
            
            //do some cool merge patch stuff here

            return _serializer.DeserializeObject<TModel>(jParsedOriginalModel.ToString());
        }

        /// <summary>
        /// Performs a json patch (RFC 6902)
        /// </summary>
        /// <typeparam name="TModel">The <see cref="System.Type"/> of your model</typeparam>
        /// <param name="patchRequestBody">The request body containing the merge patch content</param>
        /// <param name="originalModel">The original model to modify</param>
        /// <returns>Returns a new instance of the <param name="originalModel"></param></returns>
        public TModel Patch<TModel>(string patchRequestBody, TModel originalModel)
        {
            var jParsedOriginalModel = GetJObjectFromModel(originalModel);

            //do some cool patch stuff here

            return _serializer.DeserializeObject<TModel>(jParsedOriginalModel.ToString());
        }

        /// <summary>
        /// Performs a deep cloning of <see cref="TModel"/>
        /// by serializing and returns a <see cref="JObject"/>
        /// </summary>
        /// <typeparam name="TModel">Type of your original model</typeparam>
        /// <param name="originalModel">Your original model to clone</param>
        /// <returns><see cref="JObject"/> of serialized <see cref="TModel"/></returns>
        private JObject GetJObjectFromModel<TModel>(TModel originalModel)
        {
            var serializeOriginalModel = _serializer.SerializeObject(originalModel);
            return JObject.Parse(serializeOriginalModel);
        }
    }
}