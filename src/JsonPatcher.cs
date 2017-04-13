using System;
using Newtonsoft.Json.Linq;
using SeeSharp7.Patch4Net.Exceptions;
using SeeSharp7.Patch4Net.Serializers;

namespace SeeSharp7.Patch4Net
{
    /// <summary>
    /// Makes your RESTful life easier by patching around your models the correct way
    /// </summary>
    public class JsonPatcher
    {
        private readonly ISerializer _serializer;
        private const string ContentTypePatch = "application/json-patch+json";
        private const string ContentTypeMergePatch = "application/merge-patch+json";

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
        /// Performs a patch or merge patch operation indicated by the content type header value
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="requestBody">The request body containing the specific patch information</param>
        /// <param name="originalModel">The original model to modify</param>
        /// <param name="contentTypeHeaderValue">Value of ContentType header. Allowed are "application/json-patch+json" or "application/merge-patch+json" as described in the specific RFC</param>
        /// <returns>Returns a new, patched instance of the <param name="originalModel"></param></returns>
        /// <exception cref="UnknownContentTypeException"></exception>
        public TModel UniversalPatch<TModel>(string requestBody, TModel originalModel, string contentTypeHeaderValue)
        {
            if (string.Compare(contentTypeHeaderValue, ContentTypePatch, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return Patch(requestBody, originalModel);
            }
            if (string.Compare(contentTypeHeaderValue, ContentTypeMergePatch, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return MergePatch(requestBody, originalModel);
            }

            throw new UnknownContentTypeException($"The value {contentTypeHeaderValue} of ContentType Header (specified in parameter {nameof(contentTypeHeaderValue)}) is unknown");
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
            var jParsedOriginalModel = CloneToJObject(originalModel);
            var jParsedRequestBody = JObject.Parse(mergePatchRequestBody);

            var mergePatch = new JsonMergePatch.JsonMergePatch();
            mergePatch.MergeJson(jParsedOriginalModel, jParsedRequestBody);

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
            var jParsedOriginalModel = CloneToJObject(originalModel);
            var jParsedRequestBody = JObject.Parse(patchRequestBody);
            
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
        private JObject CloneToJObject<TModel>(TModel originalModel)
        {
            var serializeOriginalModel = _serializer.SerializeObject(originalModel);
            return JObject.Parse(serializeOriginalModel);
        }
    }
}