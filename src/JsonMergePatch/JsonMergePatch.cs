using Newtonsoft.Json.Linq;

namespace SeeSharp7.Patch4Net.JsonMergePatch
{
    internal class JsonMergePatch
    {
        /// <summary>
        /// Performs a merge on the original model using <see cref="JContainer.Merge(object)"/>
        /// </summary>
        /// <param name="originalModel">Original Model of type <see cref="JObject"/></param>
        /// <param name="mergeModel">Merge Patch Model of type <see cref="JObject"/></param>
        /// <returns></returns>
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