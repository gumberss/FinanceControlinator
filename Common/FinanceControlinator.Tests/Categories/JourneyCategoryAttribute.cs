using FinanceControlinator.Tests.Categories.Enums;
using FinanceControlinator.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FinanceControlinator.Tests.Categories
{
    public class JourneyCategoryAttribute : TestCategoryBaseAttribute
    {
        readonly TestUserJourneyEnum _journey;

        public JourneyCategoryAttribute(TestUserJourneyEnum journey)
        {
            _journey = journey;
        }

        public override IList<string> TestCategories => new[] {
            $"Journey - {_journey.GetDescription()}"
        };
    }
}
