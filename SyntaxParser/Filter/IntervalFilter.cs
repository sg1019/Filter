using System;
using System.Collections.Generic;
using System.Reflection;

namespace SyntaxParser.Filter
{
    public class IntervalFilter<TInterval> : Filter where TInterval : IComparable
    {
        private readonly TInterval _lowerBound;
        private readonly TInterval _upperBound;
        private readonly bool _lowerOpened;
        private readonly bool _upperOpened;
        
        public IntervalFilter(TInterval lowerBound, TInterval upperBound, bool lowerOpened, bool upperOpened)
        {
            if (lowerBound.CompareTo(upperBound) > 0) throw new ArgumentException($"{nameof(lowerBound)} cannot greater than {nameof(upperBound)}");
            _lowerBound = lowerBound;
            _upperBound = upperBound;
            _lowerOpened = lowerOpened;
            _upperOpened = upperOpened;
        }

        public override string ToString()
        {
            return $"{(_lowerOpened ? SyntaxHelper.CloseSquareBracket : SyntaxHelper.OpenSquareBracket)}{_lowerBound};{_upperBound}{(_upperOpened ? SyntaxHelper.OpenSquareBracket : SyntaxHelper.CloseSquareBracket)}";
        }

        public override bool Match<T>(T item, List<PropertyInfo> props, FilterOption options)
        {
            foreach (var prop in props)
            {
                var propValue = prop.GetValue(item);
                
                if (propValue is null || !(propValue is TInterval)) continue;

                var lowerComparison = _lowerBound.CompareTo(propValue);
                if (lowerComparison > 0) continue;
                if (_lowerOpened && lowerComparison == 0) continue;

                var upperComparison = _upperBound.CompareTo(propValue);
                if (upperComparison < 0) continue;
                if (_upperOpened && upperComparison == 0) continue;
                return true;
            }
            return false;
        }
    }
}