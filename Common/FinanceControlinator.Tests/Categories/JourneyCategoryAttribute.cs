using FinanceControlinator.Tests.Categories.Enums;
using FinanceControlinator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Xunit.Sdk;

namespace FinanceControlinator.Tests.Categories
{
    [TraitDiscoverer("FinanceControlinator.Tests.Categories.JourneyDiscoverer", "FinanceControlinator.Tests")]
    public class JourneyCategoryAttribute : TestCategoryBaseAttribute, ITraitAttribute
    {
        public TestUserJourneyEnum Journey { get; private set; }

        public JourneyCategoryAttribute(TestUserJourneyEnum journey)
        {
            Journey = journey;
        }

        public override IList<string> TestCategories => new[] {
            $"Journey - {Journey.GetDescription()}"
        };
    }
}
