using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SeeSharp7.Patch4Net.Models;

namespace SeeSharp7.Patch4Net
{
    public static class JsonPatcher
    {
        /// <summary>
        /// Performs a merge patch and returns a modified clone of the <param name="originalModel"></param>
        /// </summary>
        /// <typeparam name="TModel">The <see cref="System.Type"/> of your model</typeparam>
        /// <param name="mergePatchRequestBody">The request body containing the merge patch content</param>
        /// <param name="originalModel">The original model to modify</param>
        /// <returns></returns>
        public static TModel MergePatch<TModel>(string mergePatchRequestBody, TModel originalModel)
        {
            if (originalModel == null)
                throw new ArgumentNullException(nameof(originalModel));

            var deepCopy = originalModel.CloneJson();
            var mergePatch = JObject.Parse(mergePatchRequestBody);

            //Extract properties of model
            var t = typeof(TModel);
            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite).ToList();

            foreach (var node in mergePatch)
            {
                var value = GetConvertedValue(node);
                var property = properties.First(p => string.Equals(p.Name, node.Key, StringComparison.CurrentCultureIgnoreCase));

                if (value == null &&
                    Nullable.GetUnderlyingType(property.PropertyType) == null) //type not nullable
                {
                    throw new ArgumentNullException(property.Name);
                }

                property.SetValue(deepCopy, value);
            }

            return deepCopy;
        }

        public static TModel Patch<TModel>(JsonPatchModel jsonPatchModel, TModel originalModel)
        {
            return default(TModel);
        }

        private static object GetConvertedValue(KeyValuePair<string, JToken> node)
        {
            object value;

            switch (node.Value.Type)
            {
                case JTokenType.String:
                    value = node.Value.Value<string>();
                    break;
                case JTokenType.Boolean:
                    value = node.Value.Value<bool>();
                    break;
                case JTokenType.Date:
                    value = node.Value.Value<DateTime>();
                    break;
                case JTokenType.Float:
                    value = node.Value.Value<decimal>();
                    break;
                case JTokenType.Integer:
                    value = node.Value.Value<int>();
                    break;
                //case JTokenType.Object:
                //    break;
                //case JTokenType.Array:
                //    break;
                //case JTokenType.Raw:
                //    break;
                //case JTokenType.Bytes:
                //    break;
                //case JTokenType.Guid:
                //    break;
                //case JTokenType.Uri:
                //    break;
                //case JTokenType.TimeSpan:
                //    break;
                //case JTokenType.None:
                //case JTokenType.Undefined:
                //case JTokenType.Constructor:
                //case JTokenType.Property:
                //case JTokenType.Comment:
                //case JTokenType.Null:
                //    break;
                default:
                    value = null;
                    break;
            }

            return value;
        }
    }
}
