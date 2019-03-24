using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SyntaxParser.Filter;

namespace GenericFilter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var data = Data.GenerateTestData();
            Console.WriteLine("All items");
            WriteToConsole(data);

            var syntax = "!Sell";

            var filtered = GenericObjectFilter(data, syntax);
            Console.WriteLine($"Filtered items using \"{syntax}\"");
            WriteToConsole(filtered);

            syntax = "Buy && (<1.2356 & >=-1)";
            filtered = GenericObjectFilter(data, syntax);
            Console.WriteLine($"Filtered items using \"{syntax}\"");
            WriteToConsole(filtered);

            syntax = "[1.2356;1.2583]";
            filtered = GenericObjectFilter(data, syntax);
            Console.WriteLine($"Filtered items using \"{syntax}\"");
            WriteToConsole(filtered);

            syntax = "[500000;600000]";
            filtered = GenericObjectFilter(data, syntax);
            Console.WriteLine($"Filtered items using \"{syntax}\"");
            WriteToConsole(filtered);

            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        private static void WriteToConsole(IEnumerable data)
        {
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }

        public static IEnumerable<T> GenericObjectFilter<T>(IEnumerable<T> items, string syntax,
            FilterOption option = null)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            if (string.IsNullOrWhiteSpace(syntax)) return items;

            var props = new List<PropertyInfo>(typeof(T).GetProperties());
            
            var filteredItems = new List<T>();

            var filter = FilterHelper.GetFilter(syntax);

            foreach (var item in items)
            {
                if (filter.Match(item, props, option)) filteredItems.Add(item);
            }

            return filteredItems;
        }
    }
}
