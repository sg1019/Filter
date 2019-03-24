using System;
using System.Collections.Generic;
using System.Reflection;

namespace SyntaxParser.Filter
{
    public class ComparatorFilter: Filter
    {
        private readonly double _value;
        private readonly Comparator _comparator;

        public ComparatorFilter(SyntaxParser.ComparisonExpressionContext ctx)
        {
            var value = ctx.comparison().numeric().GetText();
            _value = SyntaxHelper.InfinitySymbol.Equals(value) ? double.PositiveInfinity : double.Parse(value);

            var comparator = ctx.comparison().comparator();

            if (comparator.LT() != null)
            {
                _comparator = Comparator.LessThan;
            }
            else if (comparator.LE() != null)
            {
                _comparator = Comparator.LessOrEqual;
            }
            else if (comparator.GT() != null)
            {
                _comparator = Comparator.GreaterThan;
            }
            else if (comparator.GE() != null)
            {
                _comparator = Comparator.GreaterOrEqual;
            }
            else
            {
                throw new NotImplementedException($"Comparator {comparator.GetText()} has not been implemented");
            }
        }

        public enum Comparator
        {
            LessThan = 1,
            LessOrEqual = 2,
            GreaterThan = 3,
            GreaterOrEqual = 4
        }

        public override bool Match<T>(T item, List<PropertyInfo> props, FilterOption options)
        {
            if (options == null) options = new FilterOption();

            foreach (var prop in props)
            {
                var propValue = prop.GetValue(item);
                
                if (propValue is null || !(propValue is double)) continue;
                var value = (double)propValue;
                switch (_comparator)
                {
                    case Comparator.LessThan:
                        if (value < _value) return true;
                        break;
                    case Comparator.LessOrEqual:
                        if (value <= _value + options.DoubleComparisonPrecision) return true;
                        break;
                    case Comparator.GreaterThan:
                        if (value > _value) return true;
                        break;
                    case Comparator.GreaterOrEqual:
                        if (value >= _value - options.DoubleComparisonPrecision) return true;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return false;
        }
    }
}
