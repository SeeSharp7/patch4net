using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace SeeSharp7.Patch4Net
{
    internal class ModelWalker
    {
        private readonly List<string> _iterations = new List<string>();
        private readonly List<KeyValuePair<PropertyInfo, object>> _modifications = new List<KeyValuePair<PropertyInfo, object>>();

        internal TModel PatchThatModel<TModel>(string patchModel, TModel originalModel)
        {
            var deepCopy = originalModel.CloneJson();
            var properties = typeof(TModel).GetProperties(); //No CanWrite specification
            var jsonMergePatch = JObject.Parse(patchModel);

            Iterate(properties.ToList(), jsonMergePatch);

            foreach (var modification in _modifications)
            {
                if (modification.Value == null &&
                    Nullable.GetUnderlyingType(modification.Key.PropertyType) == null) 
                {
                    continue; //type not nullable, but null has been set
                }

                modification.Key.SetValue(deepCopy, modification.Value);
            }

            return deepCopy;
        }

        /// <summary>
        /// Recursive Method for parsing complex types
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="mergePatch"></param>
        private void Iterate(List<PropertyInfo> properties, JObject mergePatch)
        {
            _iterations.Add($"Iteration to {properties.Count} props, ");

            foreach (var token in mergePatch)
            {
                //Problem with dictionary here! cannot parse elementname to property...

                //Get equivalent from original models properties
                var property = properties.DefaultIfEmpty(null).FirstOrDefault(e => string.Equals(e.Name, token.Key, StringComparison.CurrentCultureIgnoreCase));

                if (property == null)
                    continue;

                _iterations.Add($"Iteration to {property.Name}");

                if (token.Value.Type == JTokenType.Object)
                {
                    var subPropertyList = property.PropertyType.GetProperties().Where(prop => prop.CanRead && prop.CanWrite).ToList();

                    _iterations.Add("----------RECURSION-------------");

                    Iterate(subPropertyList, (JObject)token.Value);
                    //when same property count -> overwrite?

                    continue;
                }

                var value = GetConvertedValue(token);

                _modifications.Add(new KeyValuePair<PropertyInfo, object>(property, value));
            }
        }

        private static object GetConvertedValue(KeyValuePair<string, JToken> token)
        {
            object value;

            switch (token.Value.Type)
            {
                case JTokenType.String:
                    value = token.Value.Value<string>();
                    break;
                case JTokenType.Boolean:
                    value = token.Value.Value<bool>();
                    break;
                case JTokenType.Date:
                    value = token.Value.Value<DateTime>();
                    break;
                case JTokenType.Float:
                    value = token.Value.Value<decimal>();
                    break;
                case JTokenType.Integer:
                    value = token.Value.Value<int>();
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