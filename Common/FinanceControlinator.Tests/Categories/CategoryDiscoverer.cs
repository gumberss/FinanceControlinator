using FinanceControlinator.Tests.Categories.Enums;
using FinanceControlinator.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FinanceControlinator.Tests.Categories
{
    /// <summary>
    /// For XUnit trait purpose
    /// </summary>
    public class UnitDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            String key = "Unit - ";

            var args = traitAttribute.GetConstructorArguments();

            var value = $"{((TestMicroserviceEnum)args.First()).GetDescription()} - {((TestFeatureEnum)args.Last()).GetDescription()}";

            yield return new KeyValuePair<string, string>(key, value);
        }
    }

    /// <summary>
    /// For XUnit trait purpose
    /// </summary>
    public class IntegrationDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            String key = "Integration - ";

            var args = traitAttribute.GetConstructorArguments();

            var value = $"{((TestMicroserviceEnum)args.First()).GetDescription()} - {((TestFeatureEnum)args.Last()).GetDescription()}";

            yield return new KeyValuePair<string, string>(key, value);
        }
    }

    /// <summary>
    /// For XUnit trait purpose
    /// </summary>
    public class JourneyDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            String key = "Journey - ";

            var args = traitAttribute.GetConstructorArguments();

            var value = ((TestUserJourneyEnum)args.First()).GetDescription();

            yield return new KeyValuePair<string, string>(key, value);
        }
    }
}
