using System;
using System.Collections.Generic;
using System.Reflection;

namespace SyntaxParser.Filter
{
    public class BinaryExpressionFilter : Filter
    {
        private readonly Filter _left;
        private readonly Filter _right;
        private readonly Operator _op;

        public BinaryExpressionFilter(Filter left, Filter right, SyntaxParser.BinaryExpressionContext ctx)
        {
            _left = left;
            _right = right;

            var op = ctx.op;

            if (op.OR() != null || op.COND_OR() != null)
            {
                _op = Operator.Or;
            }
            else if (op.AND() != null || op.COND_AND() != null)
            {
                _op = Operator.And;
            }
            else
            {
                throw new NotImplementedException($"Operator {op.GetText()} has not been implemented");
            }
        }

        public enum Operator
        {
            And = 1,
            Or = 2
        }

        public override bool Match<T>(T item, List<PropertyInfo> props, FilterOption options)
        {
            switch (_op)
            {
                case Operator.And:
                    return _left.Match(item, props, options) && _right.Match(item, props, options);
                case Operator.Or:
                    return _left.Match(item, props, options) || _right.Match(item, props, options);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
