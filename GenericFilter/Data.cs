using System.Collections.Generic;

namespace GenericFilter
{
    public class Data
    {
        public string Product { get; set; }
        public string Venue { get; set; }
        public string Way { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"{Product}\t{Venue}\t{Way}\t{Amount}\t{Price}";
        }

        public static List<Data> GenerateTestData()
        {
            var data = new List<Data>
            {
                new Data
                {
                    Product = "EUR/USD",
                    Venue = "Bank of America",
                    Way = "Buy",
                    Amount = 500000,
                    Price = 1.2356
                },
                new Data
                {
                    Product = "GBP/USD",
                    Venue = "Hotspot",
                    Way = "Sell",
                    Amount = 500000,
                    Price = 1.39879
                },
                new Data
                {
                    Product = "EUR/USD",
                    Venue = "Bank of America",
                    Way = "Sell",
                    Amount = 1500000,
                    Price = 1.2583
                },
                new Data
                {
                    Product = "NZD/USD",
                    Venue = "Currenex",
                    Way = "Buy",
                    Amount = 600000,
                    Price = 0.645578
                }
            };
            return data;
        }
    }
}
