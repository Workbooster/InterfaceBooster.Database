using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces;
using InterfaceBooster.Database.Interfaces.Structure;
using InterfaceBooster.Database.Core.Structure;
using InterfaceBooster.Database.Test.Core.TestHelpers;
using Db = InterfaceBooster.Database.Core;

namespace InterfaceBooster.Database.Test.Core.SyneryDB_Test
{
    [TestFixture]
    public class Reopening_Database_Works
    {
        private string _WorkingDirectoryPath;
        private IDatabase _InitialDatabase;
        private IDatabase _TestDatabase;
        private ITable _FirstTable;
        private ITable _SecondTable;

        [SetUp]
        public void SetupTest()
        {
            _WorkingDirectoryPath = Path.Combine(Environment.CurrentDirectory, "Reopening_Database_Works");

            if (Directory.Exists(_WorkingDirectoryPath))
            {
                Directory.Delete(_WorkingDirectoryPath, true);
            }

            Directory.CreateDirectory(_WorkingDirectoryPath);

            // create new database with the first instance of SyneryDB

            using (_InitialDatabase = new Db.SyneryDB(_WorkingDirectoryPath))
            {
                _FirstTable = TableHelper.GetLargeTestTable(500);
                _SecondTable = TableHelper.GetLargeTestTable(1000);
                _InitialDatabase.CreateTable("FirstTable", _FirstTable);
                _InitialDatabase.CreateTable("SecondTable", _SecondTable);
            }

            // create a second database to run the tests
            _TestDatabase = new Db.SyneryDB(_WorkingDirectoryPath);
        }

        [TearDown]
        public void TearDown()
        {
            _InitialDatabase.Dispose();

            if (Directory.Exists(_WorkingDirectoryPath))
            {
                Directory.Delete(_WorkingDirectoryPath, true);
            }
        }

        [Test]
        public void Test_Loading_A_Table_Works()
        {
            ITable tbl = _TestDatabase.LoadTable("FirstTable");

            Assert.AreEqual(_FirstTable.Count, tbl.Count);
            Assert.AreEqual(_FirstTable[0].Count(), tbl[0].Count());
        }

        [Test]
        public void Test_Loading_A_Schema_Works()
        {
            ISchema schema = _TestDatabase.LoadTable("FirstTable").Schema;

            Assert.AreEqual(_FirstTable.Schema.Fields.Count, schema.Fields.Count);

            for (int i = 0; i < _FirstTable.Schema.Fields.Count; i++)
            {
                Assert.AreEqual(_FirstTable.Schema.Fields[i].Name, schema.Fields[i].Name);
                Assert.AreEqual(_FirstTable.Schema.Fields[i].Type, schema.Fields[i].Type);
            }
        }

        [Test]
        public void Test_Creating_A_New_Table_Works()
        {
            ISchema schema = new Schema();
            schema.AddField<int>("Nr");
            schema.AddField<string>("SomeText");
            List<object[]> data = new List<object[]>() {
                new object[] { 12, "Row 1" },
                new object[] { 17, "Row 2" },
            };
            ITable tbl = new Table(schema, data);

            _TestDatabase.CreateTable("Test", tbl);
            ITable loadedTable = _TestDatabase.LoadTable("Test");

            Assert.AreEqual(tbl, loadedTable);
        }

        [Test]
        public void Test_Updating_An_Existing_Table_Works()
        {
            ISchema schema = new Schema();
            schema.AddField<int>("Nr");
            schema.AddField<string>("SomeText");
            List<object[]> data = new List<object[]>() {
                new object[] { 12, "Row 1" },
                new object[] { 17, "Row 2" },
            };
            ITable tbl = new Table(schema, data);

            _TestDatabase.UpdateTable("FirstTable", tbl);
            ITable loadedTable = _TestDatabase.LoadTable("FirstTable");

            Assert.AreEqual(tbl, loadedTable);
        }
    }
}
