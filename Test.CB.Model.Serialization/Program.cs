using System;
using CB.Model.Serialization;


namespace Test.CB.Model.Serialization
{
    internal class Program
    {
        #region Implementation
        private static void Main()
        {
            var i15 = 15;
            var s15 = new JsonModelSerializer().Serialize(i15);
            Console.WriteLine(s15);

            i15 = new JsonModelSerializer().Deserialize<int>(s15);
            Console.WriteLine(i15);
            Console.ReadLine();
        }
        #endregion
    }
}