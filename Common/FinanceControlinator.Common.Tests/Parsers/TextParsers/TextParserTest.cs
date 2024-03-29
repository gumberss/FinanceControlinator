﻿using FinanceControlinator.Common.Parsers.TextParsers;
using FinanceControlinator.Tests.Categories;
using FinanceControlinator.Tests.Categories.Enums;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinanceControlinator.Common.Tests.Parsers.TextParsers
{
    [TestClass]
    public class TextParserTest
    {
        private readonly TextParser _service;

        public TextParserTest()
        {
            _service = new TextParser();
        }

        [TestMethod]
        [JourneyCategory(TestUserJourneyEnum.RecordingExpenses)]
        [UnitTestCategory(TestMicroserviceEnum.Expenses, TestFeatureEnum.ExpenseUpdate)]
        public void Should_parse_text_replacing_key_for_value()
        {
            var result = _service.Parse("Hi, [[THE_KEY]], how [[OTHER_KEY]]"
                , ("THE_KEY", "Jarbas")
                , ("OTHER_KEY", "are you?"));

            result.Should().Be("Hi, Jarbas, how are you?");
        }
    }
}
