using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxParser;

namespace GenericFilter.Tests
{
    [TestClass]
    public class GenericFilterTests
    {
        private const double DoubleComparisonEpsilon = 2 * double.Epsilon;

        [TestMethod]
        public void TestExpression()
        {
            var data = Data.GenerateTestData();

            var filter = "(\"Bank of America \" && [500000;1500000[) | Currenex ";
            var filtered = Program.GenericObjectFilter(data, filter);

            Assert.AreEqual(2, filtered.Count());
            Assert.AreEqual(500000, filtered.First().Amount, DoubleComparisonEpsilon);
            Assert.AreEqual(0.645578, filtered.Last().Price, DoubleComparisonEpsilon);

            filter = "Buy & <1.0";
            filtered = Program.GenericObjectFilter(data, filter);

            Assert.AreEqual(1, filtered.Count());
            Assert.AreEqual("NZD/USD", filtered.First().Product);
            Assert.AreEqual(600000, filtered.First().Amount, DoubleComparisonEpsilon);

            filter = "Sell & [500000;700000[";
            filtered = Program.GenericObjectFilter(data, filter);

            Assert.AreEqual(1, filtered.Count());
            Assert.AreEqual("Hotspot", filtered.First().Venue);
            Assert.AreEqual(1.39879, filtered.First().Price, DoubleComparisonEpsilon);

            filter = "!Buy";
            filtered = Program.GenericObjectFilter(data, filter);

            Assert.AreEqual(2, filtered.Count());
            Assert.AreNotEqual("Buy", filtered.First().Way);
            Assert.AreNotEqual("Buy", filtered.Last().Way);
        }

        [TestMethod]
        public void TestText()
        {
            var expected = @"Procter & Gamble Company";
            var parser = SyntaxHelper.GetSyntaxParser($"\"{expected}\"");
            var context = parser.text();

            Assert.IsNotNull(context);
            Assert.AreEqual(expected, context.GetText().Trim('"'));

            expected = @"@ # & | + - _ ^ % $ £ €";
            parser = SyntaxHelper.GetSyntaxParser($"\'{expected}\'");
            context = parser.text();

            Assert.IsNotNull(context);
            Assert.AreEqual(expected, context.GetText().Trim('\''));
        }

        [TestMethod]
        public void TestNumeric()
        {
            var expected = 10.5;
            var parser = SyntaxHelper.GetSyntaxParser(expected.ToString(CultureInfo.InvariantCulture));
            var context = parser.numeric();

            Assert.IsNotNull(context);
            Assert.AreEqual(expected, double.Parse(context.GetText()), DoubleComparisonEpsilon);

            expected = -1753634.65645;
            parser = SyntaxHelper.GetSyntaxParser(expected.ToString(CultureInfo.InvariantCulture));
            context = parser.numeric();

            Assert.IsNotNull(context);
            Assert.AreEqual(expected, double.Parse(context.GetText()), DoubleComparisonEpsilon);

            parser = SyntaxHelper.GetSyntaxParser(SyntaxHelper.InfinitySymbol);
            context = parser.numeric();

            Assert.IsNotNull(context);
            Assert.AreEqual(SyntaxHelper.InfinitySymbol, context.GetText());
        }
    }
}
