using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SeeSharp7.Patch4Net.Examples
{
    /// <summary>
    /// Got it from here: http://blog.abodit.com/2014/05/json-patch-c-implementation/
    /// </summary>
    internal class JsonPatchExample
    {
        public string op { get; set; }        // add, remove, replace
        public string path { get; set; }
        public JToken value { get; set; }

        private JsonPatchExample() { }

        public static string Extend(string path, string extension)
        {
            // TODO: JSON property name needs escaping for path ??
            return path + "/" + extension;
        }

        private static JsonPatchExample Build(string op, string path, string key, JToken value)
        {
            if (string.IsNullOrEmpty(key))
                return new JsonPatchExample { op = op, path = path, value = value };
            else
                return new JsonPatchExample { op = op, path = Extend(path, key), value = value };
        }

        public static JsonPatchExample Add(string path, string key, JToken value)
        {
            return Build("add", path, key, value);
        }

        public static JsonPatchExample Remove(string path, string key)
        {
            return Build("remove", path, key, null);
        }

        public static JsonPatchExample Replace(string path, string key, JToken value)
        {
            return Build("replace", path, key, value);
        }

        public static string CalculatePatch(string leftString, string rightString)
        {
            if (string.IsNullOrEmpty(leftString)) leftString = "{}";
            var left = JToken.Parse(leftString);
            var right = JToken.Parse(rightString);
            var result = JsonPatchExample.CalculatePatch(left, right);
            var pts = JsonConvert.SerializeObject(result);
            return pts;
        }

        public static IEnumerable<JsonPatchExample> CalculatePatch(JToken left, JToken right, string path = "")
        {
            if (left.Type != right.Type)
            {
                yield return JsonPatchExample.Replace(path, "", right);
                yield break;
            }

            if (left.Type == JTokenType.Array)
            {
                if (left.Children().SequenceEqual(right.Children()))        // TODO: Need a DEEP EQUALS HERE
                    yield break;

                // No array insert or delete operators in jpatch (yet?)
                yield return JsonPatchExample.Replace(path, "", right);
                yield break;
            }

            if (left.Type == JTokenType.Object)
            {
                var lprops = ((IDictionary<string, JToken>)left).OrderBy(p => p.Key);
                var rprops = ((IDictionary<string, JToken>)right).OrderBy(p => p.Key);

                foreach (var removed in lprops.Except(rprops, MatchesKey.Instance))
                {
                    yield return JsonPatchExample.Remove(path, removed.Key);
                }

                foreach (var added in rprops.Except(lprops, MatchesKey.Instance))
                {
                    yield return JsonPatchExample.Add(path, added.Key, added.Value);
                }

                var matchedKeys = lprops.Select(x => x.Key).Intersect(rprops.Select(y => y.Key));
                var zipped = matchedKeys.Select(k => new { key = k, left = left[k], right = right[k] });

                foreach (var match in zipped)
                {
                    string newPath = path + "/" + match.key;
                    foreach (var patch in CalculatePatch(match.left, match.right, newPath))
                        yield return patch;
                }
                yield break;
            }
            else
            {
                // Two values, same type, not JObject so no properties

                if (left.ToString() == right.ToString())
                    yield break;
                else
                    yield return JsonPatchExample.Replace(path, "", right);
            }
        }

        /// <summary>
        /// Special sentinel value meaning that there has been no change
        /// </summary>
        public static readonly JToken NoChange = new JObject();

        public static string CalculateDelta(string leftString, string rightString)
        {
            if (string.IsNullOrEmpty(leftString)) return rightString;
            var left = JToken.Parse(leftString);
            var right = JToken.Parse(rightString);
            var result = JsonPatchExample.CalculateDelta(left, right);
            var pts = JsonConvert.SerializeObject(result);
            return pts;
        }

        public static JToken CalculateDelta(JToken left, JToken right)
        {
            if (left.Type != right.Type)
            {
                return right;
            }

            if (left.Type == JTokenType.Array)
            {
                if (left.Children().SequenceEqual(right.Children()))        // TODO: Need a DEEP EQUALS HERE
                    return NoChange;

                return right;
            }

            if (left.Type == JTokenType.Object)
            {
                // Both sides are JObjects with properties, build a delta from
                // any properties that are different (or null for have gone away)
                var newObject = new JObject();

                var lprops = ((IDictionary<string, JToken>)left).OrderBy(p => p.Key);
                var rprops = ((IDictionary<string, JToken>)right).OrderBy(p => p.Key);

                foreach (var removed in lprops.Except(rprops, MatchesKey.Instance))
                {
                    newObject.Add(removed.Key, null);
                }

                foreach (var added in rprops.Except(lprops, MatchesKey.Instance))
                {
                    newObject.Add(added.Key, added.Value);
                }

                var matchedKeys = lprops.Select(x => x.Key).Intersect(rprops.Select(y => y.Key));
                var zipped = matchedKeys.Select(k => new { key = k, left = left[k], right = right[k] });

                foreach (var match in zipped)
                {
                    var deltaSub = CalculateDelta(match.left, match.right);
                    if (object.ReferenceEquals(deltaSub, NoChange)) continue;             // No difference
                    newObject.Add(match.key, deltaSub);
                }
                return newObject;
            }
            else
            {
                // Two values, same type, not JObject so no properties
                if (left.ToString() == right.ToString())
                    return NoChange;
                else
                    return right;
            }
        }

        public static string ApplyDelta(string leftString, string deltaString)
        {
            if (string.IsNullOrEmpty(leftString)) return deltaString;
            var left = JToken.Parse(leftString);
            var delta = JToken.Parse(deltaString);
            var result = JsonPatchExample.ApplyDelta(left, delta);
            var pts = JsonConvert.SerializeObject(result);
            return pts;
        }

        public static JToken ApplyDelta(JToken left, JToken right)
        {
            if (left.Type == JTokenType.Object && right.Type == JTokenType.Object)
            {
                JObject newObject = new JObject();

                // Both sides are JObjects with properties
                var lprops = ((IDictionary<string, JToken>)left).OrderBy(p => p.Key);
                var rprops = ((IDictionary<string, JToken>)right).OrderBy(p => p.Key);

                foreach (var keep in lprops.Except(rprops, MatchesKey.Instance))
                {
                    newObject.Add(keep.Key, keep.Value);
                }

                foreach (var added in rprops.Except(lprops, MatchesKey.Instance))
                {
                    newObject.Add(added.Key, added.Value);
                }

                var matchedKeys = lprops.Select(x => x.Key).Intersect(rprops.Select(y => y.Key));
                var zipped = matchedKeys.Select(k => new { key = k, left = left[k], right = right[k] });

                foreach (var match in zipped)
                {
                    // Descend into the property and apply delta for value of property
                    var deltaSub = ApplyDelta(match.left, match.right);
                    newObject.Add(match.key, deltaSub);
                }
                return newObject;
            }
            else
            {
                // In all other cases, e.g. different types, same primitive type, both array, ...
                return right;
            }
        }


        private class MatchesKey : IEqualityComparer<KeyValuePair<string, JToken>>
        {
            public static MatchesKey Instance = new MatchesKey();
            public bool Equals(KeyValuePair<string, JToken> x, KeyValuePair<string, JToken> y)
            {
                return x.Key.Equals(y.Key);
            }

            public int GetHashCode(KeyValuePair<string, JToken> obj)
            {
                return obj.Key.GetHashCode();
            }
        }
    }
}