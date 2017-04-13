using System.Collections.Generic;
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

            }
        }

        [TestClass]
        public class MergePatchTests : JsonPatcherTests
        {
            [TestMethod]
            public void ItShouldMergePatchTheSimpleModelCorrectly()
            {
                //arrange
                const string oldName = "Sarah Mayer";
                const string newName = "Sarah Richgirl";
                const int age = 26;
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


        private ComplexModel GetComplexModelExample()
        {
            return new ComplexModel
            {
                Id = 1,
                SubModel1 = new ComplexModelSub1
                {
                    Id = 11,
                    Data = new ComplexModelSub2
                    {
                        Name = "ReneSub",
                        Age = 26
                    }
                },
                SubModel2 = new ComplexModelSub2
                {
                    Name = "Rene",
                    Age = 27
                },
                DictionaryWithSubModel = new Dictionary<string, ComplexModelDictionarySub>
                {
                    {
                        "keyUno",
                        new ComplexModelDictionarySub
                        {
                            ExampleText = "This is some Text"
                        }
                    },
                                        {
                        "keyDuo",
                        new ComplexModelDictionarySub
                        {
                            ExampleText = "This is some duo Text"
                        }
                    },
                }
            };
        }
    }
}
