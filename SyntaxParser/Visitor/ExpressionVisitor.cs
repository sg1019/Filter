using SyntaxParser.Filter;

namespace SyntaxParser.Visitor
{
    public class ExpressionVisitor : SyntaxBaseVisitor<Filter.Filter>
    {
        public override Filter.Filter VisitExpression(SyntaxParser.ExpressionContext context)
        {
            switch (context)
            {
                case SyntaxParser.ComparisonExpressionContext comparatorExpressionCtx:
                    return new ComparatorFilter(comparatorExpressionCtx);
                case SyntaxParser.BinaryExpressionContext binaryExpressionContext:
                    var left = VisitExpression(binaryExpressionContext.left);
                    var right = VisitExpression(binaryExpressionContext.right);
                    return new BinaryExpressionFilter(left, right, binaryExpressionContext);
                case SyntaxParser.ParenExpressionContext parenExpressionContext:
                    return VisitExpression(parenExpressionContext.expression());
                case SyntaxParser.NotExpressionContext notExpressionContext:
                    var filter = VisitExpression(notExpressionContext.expression());
                    return new NotExpressionFilter(filter);
                case SyntaxParser.IntervalExpressionContext intervalExpressionContext:
                    return FilterHelper.CreateIntervalFilter(intervalExpressionContext.interval());
                case SyntaxParser.TextExpressionContext textExpressionContext:
                    return new TextFilter(textExpressionContext.text());
                default:
                    return base.VisitExpression(context);
            }
        }
    }
}
