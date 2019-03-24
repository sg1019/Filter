using System;
using System.Linq;
using SyntaxParser.Type;
using SyntaxParser.Visitor;

namespace SyntaxParser.Filter
{
    public class FilterHelper
    {
        public static Filter GetFilter(string syntax)
        {
            var syntaxParser = SyntaxHelper.GetSyntaxParser(syntax);

            var expression = syntaxParser.expression();
            var visitor = new ExpressionVisitor();
            return visitor.VisitExpression(expression);
        }

        public static double ParseDouble(string value)
        {
            return SyntaxHelper.InfinitySymbol.Equals(value) ? double.PositiveInfinity : double.Parse(value);
        }

        public static Filter CreateIntervalFilter(SyntaxParser.IntervalContext ctx)
        {
            var lowerOpened = SyntaxHelper.CloseSquareBracket.Equals(ctx.boundary().First().GetText(), StringComparison.OrdinalIgnoreCase);
            var upperOpened = SyntaxHelper.OpenSquareBracket.Equals(ctx.boundary().Last().GetText(), StringComparison.OrdinalIgnoreCase);

            var numeric = ctx.numeric();

            if (numeric != null)
            {
                var lowerBound = ParseDouble(numeric.First().GetText());
                var upperBound = ParseDouble(numeric.Last().GetText());
                return new IntervalFilter<double>(lowerBound, upperBound, lowerOpened, upperOpened);
            }

            var range = ctx.RANGE();

            if (range != null)
            {
                var lowerBound = new Range(range.First().GetText());
                var upperBound = new Range(range.Last().GetText());
                return new IntervalFilter<Range>(lowerBound, upperBound, lowerOpened, upperOpened);
            }

            throw new NotImplementedException($"Interval {ctx.GetText()} has not been implemented");
        }
    }
}
