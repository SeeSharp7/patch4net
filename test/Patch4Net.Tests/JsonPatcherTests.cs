using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using SeeSharp7.Patch4Net.Tests.Mocks;

namespace SeeSharp7.Patch4Net.Tests
{
    [TestClass]
    public class JsonPatcherTests
    {
        [TestClass]
        public class PatchTests : JsonPatcherTests
        {
            [TestMethod]
            public void ItShouldPatchTheSimpleModelCorrectly()
            {
                //arrange
                const int age = 26;
                const string oldName = "Sarah Mayer";
                const decimal oldAccountBalance = 30.5m;

                var simpleModel = new SimpleCustomerModel
                {
                    Name = oldName,
                    Age = age,
                    AccountBalance = oldAccountBalance
                };

                var patchJson = "[\r\n  { \"op\": \"replace\", \"path\": \"/baz\", \"value\": \"boo\" },\r\n  { \"op\": \"add\", \"path\": \"/hello\", \"value\": [\"world\"] },\r\n  { \"op\": \"remove\", \"path\": \"/foo\"}\r\n]";

                //act
                var jsonPatcher = new JsonPatcher();
                var patchedModel = jsonPatcher.Patch(patchJson, simpleModel);

                //assert
            }
        }

        [TestClass]
        public class MergePatchTests : JsonPatcherTests
        {
            [TestMethod]
            public void ItShouldMergePatchTheSimpleModelCorrectly()
            {
                //arrange
                const int age = 26;
                const string oldName = "Sarah Mayer";
                const string newName = "Sarah Richgirl";
                const decimal oldAccountBalance = 30.5m;
                const decimal newAccountBalance = 100000000.5m;

                var simpleModel = new SimpleCustomerModel
                {
                    Name = oldName,
                    Age = age,
                    AccountBalance = oldAccountBalance
                };

                var mergePatchJson = new JObject(
                    new JProperty("Name", newName),
                    new JProperty("AccountBalance", newAccountBalance)).ToString();

                //act
                var patchedModel = new JsonPatcher().MergePatch(mergePatchJson, simpleModel);

                //assert
                Assert.AreEqual(newName, patchedModel.Name, "Name is wrong");
                Assert.AreEqual(age, patchedModel.Age, "Age has been changed");
                Assert.AreEqual(newAccountBalance, patchedModel.AccountBalance, "AccountBalance is wrong");
            }
        }
    }
}