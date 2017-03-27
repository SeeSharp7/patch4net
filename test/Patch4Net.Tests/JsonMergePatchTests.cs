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
            var originalModel = new SimpleCustomerModel {Name = "Ralle", AccountBalance = 123001.33m };

            var patchModel = new SimpleCustomerModel
            {
                Name = "Rene",
                Age = 26,
                AccountBalance = 53.01m
            };

            var patchModelString = JsonConvert.SerializeObject(patchModel);

            var modifiedModel = JsonMergePatch.MergePatch(patchModelString, originalModel);

            Assert.AreEqual(patchModel.Name, modifiedModel.Name, "Name stimmt nicht überein");
            Assert.AreEqual(patchModel.Age, modifiedModel.Age, "Age stimmt nicht überein");
            Assert.AreEqual(patchModel.AccountBalance, modifiedModel.AccountBalance, "AccountBalance stimmt nicht überein");
        }
    }
}
