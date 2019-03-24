using System.Collections.Generic;
using System.Reflection;

namespace SyntaxParser.Filter
{
    public class TextFilter : Filter
    {
        private readonly string _text;

        public TextFilter(SyntaxParser.TextContext ctx)
        {
            _text = ctx.GetText().Trim('"', '\'');
        }

        public override bool Match<T>(T item, List<PropertyInfo> props, FilterOption options)
        {
            if (options is null) options = new FilterOption();
            
            foreach (var prop in props)
            {
                var propValue = prop.GetValue(item);

                if (propValue is null || !(propValue is string)) continue;

                var value = (string) propValue;
                var compareValue = _text;
                if (options.TrimBeforeComparison)
                {
                    value = value.Trim();
                    compareValue = compareValue.Trim();
                }

                if (string.Equals(value, compareValue, options.StrComparison)) return true;
            }
            return false;
        }
    }
}
