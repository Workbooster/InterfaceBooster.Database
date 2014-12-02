using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.Structure;
using InterfaceBooster.Database.Core.Storage;
using InterfaceBooster.Database.Test.Core.TestHelpers;

namespace InterfaceBooster.Database.Test.Core.Storage.TableSerializationHandler_Test
{
    [TestFixture]
    public class Reading_From_Table_Files_Works
    {
        private string _DataDirectoryPath;
        private TableSerializationHandler _TableSerializationHandler;

        [SetUp]
        public void SetupTest()
        {
            _DataDirectoryPath = Path.Combine(Environment.CurrentDirectory, @"Reading_From_Database_Files_Works");

            if (Directory.Exists(_DataDirectoryPath))
            {
                Directory.Delete(_DataDirectoryPath, true);
            }

            Directory.CreateDirectory(_DataDirectoryPath);

            _TableSerializationHandler = new TableSerializationHandler(_DataDirectoryPath);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_DataDirectoryPath))
            {
                Directory.Delete(_DataDirectoryPath, true);
            }
        }

        [Test]
        public void Test_Reading_From_Multiple_Table_Files_Works()
        {
            string testString = "Test_Reading_From_Multiple_Table_Files_Works";
            string tableName = @"\group1\group2\tbl";

            // prepare the table
            // calculate needed rows for at least 5 table files
            int approximateSizePerRow = 300; // Byte
            int neededSize = 5 * TableSerializationHandler.MAX_FILESIZE;
            int neededRows = neededSize / approximateSizePerRow;
            int manipulationRowIndex = neededRows / 2;

            ITable tableForWriting = TableHelper.GetLargeTestTable(neededRows);

            // manipulate a row somewhere in the middle
            tableForWriting[manipulationRowIndex] = new object[] {
                -1, // Id
                testString, // First
                "Dummy value", // Second
                "Dummy value", // Third
                "Dummy value", // Fourth
                "Dummy value", // Fifth
                true // IsActive
            };

            // write the test table from file
            _TableSerializationHandler.Write(tableName, tableForWriting);

            // read the test table from file
            ITable tableFromReading = _TableSerializationHandler.Read(tableName, tableForWriting.Schema);

            // check whether the manipulated row is there
            object[] manipulatedRow = tableFromReading.Where(o => (int)o[0] == -1).First();

            Assert.AreEqual(testString, manipulatedRow[1]);
        }
    }
}
