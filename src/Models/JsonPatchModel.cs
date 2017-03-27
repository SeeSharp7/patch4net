using System;
using System.Collections;
using System.Collections.Generic;

namespace SeeSharp7.Patch4Net.Models
{
    [Serializable]
    public class JsonPatchModel : IEnumerable<JsonPatchOperation>
    {
        readonly List<JsonPatchOperation> _jsonPatchOperations = new List<JsonPatchOperation>();

        public JsonPatchOperation this[int index]
        {
            get { return _jsonPatchOperations[index]; }
            set { _jsonPatchOperations.Insert(index, value); }
        }

        public IEnumerator<JsonPatchOperation> GetEnumerator()
        {
            return _jsonPatchOperations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}