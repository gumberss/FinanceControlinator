using FinanceControlinator.Tests.Categories.Enums;
using FinanceControlinator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceControlinator.Tests.Categories
{
    public class IntegrationTestCategoryAttribute : TestCategoryBaseAttribute
    {
        readonly TestMicroserviceEnum _microservice;
        readonly TestFeatureEnum _feature;

        public IntegrationTestCategoryAttribute(TestMicroserviceEnum microservice, TestFeatureEnum feature)
        {
            this._microservice = microservice;
            this._feature = feature;
        }

        public override IList<string> TestCategories => new[] {
            $"Integration - {_microservice.GetDescription()} - {_feature.GetDescription()}"
        };
    }
}
