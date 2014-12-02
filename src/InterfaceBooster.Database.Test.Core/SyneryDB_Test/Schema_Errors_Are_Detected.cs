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
    public class Schema_Errors_Are_Detected
    {
        private string _WorkingDirectoryPath;
        private IDatabase _Database;

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
        }

        [Test]
        public void Creating_Table_With_A_Schema_Without_Fields_Is_Prevented()
        {
            // create a dummy test table without fields
            ISchema schema = new Schema();

            List<object[]> data = new List<object[]> {
                new object[] { 15, "Test Name", true },
                new object[] { 522, "Has errors?", false }
            };

            Assert.Throws<SyneryDBException>(delegate { _Database.CreateTable(@"\Test\Table", new Table(schema, data)); });
        }

        [Test]
        public void Creating_Table_Without_A_Schema_Is_Prevented()
        {
            ISchema schema = null;

            List<object[]> data = new List<object[]> {
                new object[] { 15, "Test Name", true },
                new object[] { 522, "Has errors?", false }
            };

            Assert.Throws<SyneryDBException>(delegate { _Database.CreateTable(@"\Test\Table", new Table(schema, data)); });
        }
    }
}
