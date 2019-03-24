using System.Collections.Generic;
using System.Reflection;

namespace SyntaxParser.Filter
{
    public class NotExpressionFilter : Filter
    {
        private readonly Filter _filter;

        public NotExpressionFilter(Filter filter)
        {
            _filter = filter;
        }

        public override bool Match<T>(T item, List<PropertyInfo> props, FilterOption options)
        {
            return !_filter.Match(item, props, options);
        }
    }
}
