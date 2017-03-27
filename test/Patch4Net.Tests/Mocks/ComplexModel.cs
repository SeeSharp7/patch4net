using System.Collections.Generic;

namespace SeeSharp7.Patch4Net.Tests.Mocks
{
    internal class ComplexModel
    {
        public int Id { get; set; }
        public ComplexModelSub1 SubModel1 { get; set; }

        public ComplexModelSub2 SubModel2 { get; set; }

        public Dictionary<string, ComplexModelDictionarySub> DictionaryWithSubModel { get; set; }
    }
}
