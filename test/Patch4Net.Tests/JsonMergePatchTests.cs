using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SeeSharp7.Patch4Net.Tests.Mocks;

namespace SeeSharp7.Patch4Net.Tests
{
    [TestClass]
    public class JsonMergePatchTests
    {
        [TestMethod]
        public void TestJson()
        {
            var originalModel = new SimpleCustomerModel { Name = "Ralle", AccountBalance = 123001.33m };

            var patchModel = new SimpleCustomerModel
            {
                Name = "Rene",
                Age = 26,
                AccountBalance = 53.01m
            };

            var patchModelString = JsonConvert.SerializeObject(patchModel);

            var modifiedModel = new JsonPatcher().MergePatch(patchModelString, originalModel);

            Assert.AreEqual(patchModel.Name, modifiedModel.Name, "Name stimmt nicht überein");
            Assert.AreEqual(patchModel.Age, modifiedModel.Age, "Age stimmt nicht überein");
            Assert.AreEqual(patchModel.AccountBalance, modifiedModel.AccountBalance, "AccountBalance stimmt nicht überein");
        }

        [TestMethod]
        public void ComplexTest()
        {
            var originalComplexModel = new ComplexModel
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

            var result = JsonConvert.SerializeObject(originalComplexModel);


            var patchModel = @"{
	                            ""SubModel1"": {
		                            ""Data"": {
			                            ""Name"": ""ReneSubModified""
		                            }
	                            },
	                            ""SubModel2"": {
		                            ""Name"": ""ReneModifiedToo""
	                            },
	                            ""DictionaryWithSubModel"": {
		                            ""keyDuo"": {
			                            ""ExampleText"": ""This is some modified duo Text""
		                            }
	                            }
                            }";


            //var modifiedModel = JsonPatcher.MergePatch(patchModelString, originalModel);

            //Assert.AreEqual(patchModel.Name, modifiedModel.Name, "Name stimmt nicht überein");
            //Assert.AreEqual(patchModel.Age, modifiedModel.Age, "Age stimmt nicht überein");
            //Assert.AreEqual(patchModel.AccountBalance, modifiedModel.AccountBalance, "AccountBalance stimmt nicht überein");
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
