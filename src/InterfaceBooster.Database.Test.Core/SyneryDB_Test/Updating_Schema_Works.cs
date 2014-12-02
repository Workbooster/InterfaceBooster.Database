using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces;
using InterfaceBooster.Database.Interfaces.ErrorHandling;
using InterfaceBooster.Database.Interfaces.Structure;
using InterfaceBooster.Database.Core.Structure;
using InterfaceBooster.Database.Test.Core.TestHelpers;
using Db = InterfaceBooster.Database.Core;

namespace InterfaceBooster.Database.Test.Core.SyneryDB_Test
{
    [TestFixture]
    public class Updating_Schema_Works
    {
        private string _WorkingDirectoryPath;
        private IDatabase _Database;
        private ITable _FirstTable;
        private ITable _SecondTable;

        [SetUp]
        public void SetupTest()
        {
            _WorkingDirectoryPath = Path.Combine(Environment.CurrentDirectory, "Updating_Schema_Works");

            if (Directory.Exists(_WorkingDirectoryPath))
            {
                Directory.Delete(_WorkingDirectoryPath, true);
            }

            Directory.CreateDirectory(_WorkingDirectoryPath);

            _Database = new Db.SyneryDB(_WorkingDirectoryPath);
            _FirstTable = TableHelper.GetLargeTestTable(500);
            _SecondTable = TableHelper.GetLargeTestTable(1000);
            _Database.CreateTable("FirstTable", _FirstTable);
            _Database.CreateTable("SecondTable", _SecondTable);
        }

        [TearDown]
        public void TearDown()
        {
            _Database.Dispose();

            if (Directory.Exists(_WorkingDirectoryPath))
            {
                Directory.Delete(_WorkingDirectoryPath, true);
            }
        }

        #region RENAME

        [Test]
        public void Test_Schema_Name_Changes_On_Rename()
        {
            _Database.RenameTable("FirstTable", "NewNameOfFirstTable");

            Assert.IsInstanceOf<ISchema>(_Database.Schemas["NewNameOfFirstTable"]);
        }

        [Test]
        public void Test_Loading_A_Renamed_Table_Works()
        {
            _Database.RenameTable("FirstTable", "NewNameOfFirstTable");

            Assert.IsInstanceOf<ITable>(_Database.LoadTable("NewNameOfFirstTable"));
        }

        [Test]
        public void Test_Loading_A_Renamed_Table_With_Its_Old_Name_Throws_Exception()
        {
            _Database.RenameTable("FirstTable", "NewNameOfFirstTable");

            Assert.Throws<SyneryDBException>(() => _Database.LoadTable("FirstTable"));
        }

        [Test]
        public void Test_Loading_A_Renamed_Table_Returns_The_Correct_Number_Of_Rows()
        {
            int exprectedNumberOfRows = _FirstTable.Count;
            _Database.RenameTable("FirstTable", "NewNameOfFirstTable");

            Assert.AreEqual(exprectedNumberOfRows, _Database.LoadTable("NewNameOfFirstTable").Count);
        }

        #endregion

        #region DELETE

        [Test]
        public void Test_Loading_A_Deleted_Table_With_Its_Old_Name_Throws_Exception()
        {
            _Database.DeleteTable("FirstTable");

            Assert.Throws<SyneryDBException>(() => _Database.LoadTable("FirstTable"));
        }

        #endregion

        #region SCHEMA UPDATE

        [Test]
        public void Test_New_Schema_Is_Stored_After_Update()
        {
            ISchema schema = new Schema();
            schema.AddField<int>("Nr");
            schema.AddField<string>("SomeText");
            List<object[]> data = new List<object[]>() {
                new object[] { 12, "Row 1" },
                new object[] { 17, "Row 2" },
            };
            ITable tbl = new Table(schema, data);

            _Database.UpdateTable("FirstTable", tbl);

            Assert.AreEqual(2, _Database.Schemas["FirstTable"].Fields.Count);
        }

        [Test]
        public void Test_New_Schema_Is_Available_In_The_Loaded_Table_After_Update()
        {
            ISchema schema = new Schema();
            schema.AddField<int>("Nr");
            schema.AddField<string>("SomeText");
            List<object[]> data = new List<object[]>() {
                new object[] { 12, "Row 1" },
                new object[] { 17, "Row 2" },
            };
            ITable tbl = new Table(schema, data);

            _Database.UpdateTable("FirstTable", tbl);

            Assert.AreEqual(2, _Database.LoadTable("FirstTable").Schema.Fields.Count);
        }

        #endregion
    }
}
