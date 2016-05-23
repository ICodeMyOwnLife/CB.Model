using System;
using System.Collections.Generic;
using System.Linq;
using CB.Model.Serialization;


namespace Test.CB.Model.Serialization
{
    internal class Program
    {
        #region Implementation
        private static IEnumerable<Item> GetItems()
        {
            for (var i = 0; i < 10; ++i)
            {
                yield return new Item { Value = i, Square = i * i };
            }
        }

        private static void Main()
        {
            //TestPrimitiveSerialization();
            TestInterfaceSerialization();
            Console.ReadLine();
        }

        private static void TestInterfaceSerialization()
        {
            var items = GetItems();
            var serialization = new JsonModelSerializer().Serialize(items);
            Console.WriteLine(serialization);

            items = new JsonModelSerializer().Deserialize<IEnumerable<Item>>(serialization);
            Console.WriteLine(string.Join(Environment.NewLine,
                items.Select(i => $"Value: {i.Value}, Square: {i.Square}")));
        }

        private static void TestPrimitiveSerialization()
        {
            var i15 = 15;
            var s15 = new JsonModelSerializer().Serialize(i15);
            Console.WriteLine(s15);

            i15 = new JsonModelSerializer().Deserialize<int>(s15);
            Console.WriteLine(i15);
        }
        #endregion
    }

    internal class Item
    {
        #region  Properties & Indexers
        public int Square { get; set; }
        public int Value { get; set; }
        #endregion
    }
}