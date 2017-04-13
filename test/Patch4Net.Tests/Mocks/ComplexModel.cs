using System.Collections.Generic;

namespace SeeSharp7.Patch4Net.Tests.Mocks
{
    internal class ComplexModel
    {
        public int Id { get; set; }
        public ComplexModelSub1 SubModel1 { get; set; }

        public ComplexModelSub2 SubModel2 { get; set; }

        public Dictionary<string, ComplexModelDictionarySub> DictionaryWithSubModel { get; set; }

        public static ComplexModel GetExample()
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