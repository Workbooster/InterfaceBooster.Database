using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using InterfaceBooster.Database.Interfaces.Structure;
using InterfaceBooster.Database.Core.Storage;
using InterfaceBooster.Database.Core.Structure;

namespace InterfaceBooster.Database.Test.Core.Storage.InstanceDataHandler_Test
{
    [TestFixture]
    public class Saving_Data_Works
    {
        private string _InstanceDataFilePath;
        private InstanceDataHandler _InstanceDataHandler;

        [SetUp]
        public void SetupTest()
        {
            _InstanceDataFilePath = Path.Combine(Environment.CurrentDirectory, "data.xml");

            if (File.Exists(_InstanceDataFilePath))
            {
                File.Delete(_InstanceDataFilePath);
            }

            _InstanceDataHandler = new InstanceDataHandler(_InstanceDataFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_InstanceDataFilePath))
            {
                File.Delete(_InstanceDataFilePath);
            }
        }

        [Test]
        public void Test_Saving_Empty()
        {
            InstanceData data = new InstanceData(_InstanceDataFilePath);

            _InstanceDataHandler.Save(data);

            Assert.True(File.Exists(_InstanceDataFilePath));
        }

        [Test]
        public void Test_Saving_With_One_Schema()
        {
            InstanceData data = new InstanceData(_InstanceDataFilePath);
            ISchema schema = new Schema();
            schema.AddField("first", typeof(int));
            schema.AddField("second", typeof(string));
            data.Schemas.Add("one", schema);

            _InstanceDataHandler.Save(data);

            Assert.True(File.Exists(_InstanceDataFilePath));
        }

        [Test]
        public void Test_Is_Schema_Stored()
        {
            string schemaName = "one";

            InstanceData data = new InstanceData(_InstanceDataFilePath);
            ISchema schema = new Schema();
            schema.AddField("first", typeof(int));
            schema.AddField("second", typeof(string));
            data.Schemas.Add(schemaName, schema);

            _InstanceDataHandler.Save(data);

            XDocument doc = XDocument.Load(_InstanceDataFilePath);
            string xmlSchemaName = doc.Element("SyneryDB").Element("Schemas").Element("Schema").Attribute("name").Value;

            Assert.AreEqual(schemaName, xmlSchemaName);
        }

        [Test]
        public void Test_Are_Fields_Stored()
        {
            InstanceData data = new InstanceData(_InstanceDataFilePath);
            ISchema schema = new Schema();
            schema.AddField("first", typeof(int));
            schema.AddField("second", typeof(string));
            data.Schemas.Add("one", schema);

            _InstanceDataHandler.Save(data);

            XDocument doc = XDocument.Load(_InstanceDataFilePath);
            IEnumerable<XElement> xmlFields = doc.Element("SyneryDB").Element("Schemas").Element("Schema").Element("Fields").Elements("Field");

            Assert.AreEqual(2, xmlFields.Count());
        }
    }
}
