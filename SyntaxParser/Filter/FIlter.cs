using System.Collections.Generic;
using System.Reflection;


namespace SyntaxParser.Filter
{
    public abstract class Filter
    {
        public abstract bool Match<T>(T item, List<PropertyInfo> props, FilterOption options);
    }
}
