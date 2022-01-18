using FinanceControlinator.Tests.Categories.Enums;
using FinanceControlinator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FinanceControlinator.Tests.Categories
{
    public class FunctionalTestCategoryAttribute : TestCategoryBaseAttribute
    {
        readonly TestMicroserviceEnum _microservice;
        readonly TestFeatureEnum _feature;

        public FunctionalTestCategoryAttribute(TestMicroserviceEnum microservice, TestFeatureEnum feature)
        {
            _microservice = microservice;
            _feature = feature;
        }

        public override IList<string> TestCategories => new[] {
            $"Functional - {_microservice.GetDescription()} - {_feature.GetDescription()}"
        };
    }
}
