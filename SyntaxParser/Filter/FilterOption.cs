using System;

namespace SyntaxParser.Filter
{
    public class FilterOption
    {
        public StringComparison StrComparison = StringComparison.OrdinalIgnoreCase;
        public bool TrimBeforeComparison = true;

        public double DoubleComparisonPrecision = 2 * double.Epsilon;
    }
}
