using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.Structure;
using InterfaceBooster.Database.Core.Structure;

namespace InterfaceBooster.Database.Test.Core.TestHelpers
{
    public static class TableHelper
    {
        public static ITable GetSmallTestTable()
        {
            // create a dummy test table with a simple schema that contains three fields
            ISchema schema = new Schema();
            schema.AddField<int>("Id");
            schema.AddField<string>("Name");
            schema.AddField<bool>("IsActive");

            List<object[]> data = new List<object[]> {
                new object[] { 15, "Test Name", true },
                new object[] { 522, "Has errors?", false }
            };

            return new Table(schema, data);
        }

        public static ITable GetLargeTestTable(int numberOfRows = 50000)
        {
            Random random = new Random();

            ISchema schema = new Schema();
            schema.AddField<int>("Id");
            schema.AddField<string>("First");
            schema.AddField<string>("Second");
            schema.AddField<string>("Third");
            schema.AddField<string>("Fourth");
            schema.AddField<string>("Fifth");
            schema.AddField<bool>("IsActive");

            List<object[]> data = new List<object[]>();

            for (int i = 0; i < numberOfRows; i++)
            {
                data.Add(
                    new object[] {
                        random.Next(), //Id
                        GetRandomSentence(random), // First
                        GetRandomSentence(random), // Second
                        GetRandomSentence(random), // Third
                        GetRandomSentence(random), // Fourth
                        GetRandomSentence(random), // Fifth
                        random.Next(1) == 1 ? true : false // IsActive
                    });
            }

            return new Table(schema, data);
        }

        public static string GetRandomSentence(Random random = null)
        {
            if (random == null)
                random = new Random();

            StringBuilder sb = new StringBuilder();
            string[] dummyWords = { "Dummy", "table", "content", "Bla!", "aha?", "Rhubarb" };
            int dummyWordsCount = dummyWords.Count();

            for (int i = 0; i < 10; i++)
            {
                sb.Append(dummyWords[random.Next(dummyWordsCount - 1)]);
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}
