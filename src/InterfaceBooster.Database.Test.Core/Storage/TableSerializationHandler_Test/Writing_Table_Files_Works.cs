using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.Structure;
using InterfaceBooster.Database.Core.Storage;
using InterfaceBooster.Database.Core.Structure;
using InterfaceBooster.Database.Test.Core.TestHelpers;

namespace InterfaceBooster.Database.Test.Core.Storage.TableSerializationHandler_Test
{
    [TestFixture]
    public class Writing_Table_Files_Works
    {
        private string _DataDirectoryPath;
        private TableSerializationHandler _TableSerializationHandler;

        [SetUp]
        public void SetupTest()
        {
            _DataDirectoryPath = Path.Combine(Environment.CurrentDirectory, @"Writing_Database_Files_Works");

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
        public void Test_Writing_Table_File_Works()
        {
            string tableName = @"\group1\group2\tbl";
            string expectedTableFileName = string.Format("-group1-group2-tbl.1.{0}", TableSerializationHandler.FILENAME_EXTENSION);
            string expectedtableFilePath = Path.Combine(_DataDirectoryPath, expectedTableFileName);

            _TableSerializationHandler.Write(tableName, TableHelper.GetSmallTestTable());

            Assert.True(File.Exists(expectedtableFilePath));
        }

        [Test]
        public void Test_Writing_Multiple_Table_Files_Works()
        {
            string tableName = @"\group1\group2\tbl";
            string expectedTableFilePattern = string.Format("-group1-group2-tbl*{0}", TableSerializationHandler.FILENAME_EXTENSION);

            int approximateSizePerRow = 300; // Byte
            int neededSize = TableSerializationHandler.MAX_FILESIZE + 1 * 1024 * 1024; // MAX_FILESIZE + 1 MB
            int neededRows = neededSize / approximateSizePerRow;

            _TableSerializationHandler.Write(tableName, TableHelper.GetLargeTestTable(neededRows));

            string[] files = Directory.GetFiles(_DataDirectoryPath, expectedTableFilePattern);

            Assert.Greater(files.Count(), 1);
        }
    }
}
