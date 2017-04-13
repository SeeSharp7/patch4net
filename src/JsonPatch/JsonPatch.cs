using Newtonsoft.Json.Linq;

namespace SeeSharp7.Patch4Net.JsonPatch
{
    internal class JsonPatch
    {
        internal JObject Patch(JObject jsonPatchModel, JObject originalModel)
        {
            foreach (var token in jsonPatchModel)
            {
                ApplyPatchOperation(token.Value, originalModel);
            }

            return originalModel;
        }

        private void ApplyPatchOperation(JToken token, JObject originalModel)
        {
            
        }
    }
}