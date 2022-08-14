using FinanceControlinator.Tests.Categories.Enums;
using FinanceControlinator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Xunit.Sdk;

namespace FinanceControlinator.Tests.Categories
{
    [TraitDiscoverer("FinanceControlinator.Tests.Categories.UnitDiscoverer", "FinanceControlinator.Tests")]
    public class UnitTestCategoryAttribute : TestCategoryBaseAttribute, ITraitAttribute
    {
        public TestMicroserviceEnum Microservice { get; private set; }
        public TestFeatureEnum Feature { get; private set; }

        public UnitTestCategoryAttribute(TestMicroserviceEnum microservice, TestFeatureEnum feature)
        {
            Microservice = microservice;
            Feature = feature;
        }

        public override IList<string> TestCategories => new[] {
            $"Unit - {Microservice.GetDescription()} - {Feature.GetDescription()}"
        };
    }
}
