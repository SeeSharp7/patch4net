using Newtonsoft.Json.Linq;

namespace SeeSharp7.Patch4Net.JsonMergePatch
{
    internal class JsonMergePatch
    {
        internal JObject MergeJson(JObject originalModel, JObject mergeModel)
        {
            originalModel.Merge(mergeModel, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Replace
            });

            return originalModel;
        }
    }
}
