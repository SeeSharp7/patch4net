using Newtonsoft.Json.Linq;

namespace SeeSharp7.Patch4Net.JsonPatch
{
    internal class JsonPatch
    {
        internal JObject Patch(JArray jsonPatchModel, JObject originalModel)
        {
            foreach (var token in jsonPatchModel)
            {
                var operation = token["op"].Value<string>();
                var path = token["path"].Value<string>();
                var value = token["value"].Value<string>();

                ApplyPatchOperation(originalModel, operation, path, value);
            }

            return originalModel;
        }

        private void ApplyPatchOperation(JObject originalModel, string operation, string path, string value)
        {
            
        }
    }
}