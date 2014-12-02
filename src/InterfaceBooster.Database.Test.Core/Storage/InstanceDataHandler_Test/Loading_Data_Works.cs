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

namespace InterfaceBooster.Database.Test.Core.Storage.InstanceDataHandler_Test
{
    [TestFixture]
    public class Loading_Data_Works
    {
        private string _InstanceDataFilePath;
        private InstanceDataHandler _InstanceDataHandler;
        private InstanceData _TemplateInstanceData;

        [SetUp]
        public void SetupTest()
        {
            _InstanceDataFilePath = Path.Combine(Environment.CurrentDirectory, "data.xml");

            if (File.Exists(_InstanceDataFilePath))
            {
                File.Delete(_InstanceDataFilePath);
            }

            _InstanceDataHandler = new InstanceDataHandler(_InstanceDataFilePath);

            // prepare data and save them using an other InstanceDataHandler
            InstanceDataHandler handlerForSaving = new InstanceDataHandler(_InstanceDataFilePath);

            _TemplateInstanceData = new InstanceData(_InstanceDataFilePath);

            ISchema schema1 = new Schema();
            schema1.AddField("firstField", typeof(int));
            schema1.AddField("secondField", typeof(string));
            _TemplateInstanceData.Schemas.Add("firstSchema", schema1);

            ISchema schema2 = new Schema();
            schema2.AddField("firstField", typeof(int));
            schema2.AddField("secondField", typeof(string));
            schema2.AddField("thirdField", typeof(bool));
            schema2.AddField("fourthField", typeof(DateTime));
            schema2.AddField("fifthField", typeof(Loading_Data_Works));  // also test a custom type
            _TemplateInstanceData.Schemas.Add("secondSchema", schema2);

            handlerForSaving.Save(_TemplateInstanceData);
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
        public void Test_Loading_InstanceData_Works()
        {
            InstanceData data = _InstanceDataHandler.Load();

            Assert.IsInstanceOf(typeof(InstanceData), data);
        }

        [Test]
        public void Test_Two_Schemas_Are_Loaded()
        {
            InstanceData data = _InstanceDataHandler.Load();

            Assert.AreEqual(_TemplateInstanceData.Schemas.Count, data.Schemas.Count);
        }

        [Test]
        public void Test_All_Fields_Are_Loaded()
        {
            InstanceData data = _InstanceDataHandler.Load();

            int expectedNumberOfFields = _TemplateInstanceData.Schemas.Sum(i => i.Value.Fields.Count);
            int loadedNumberOfFields = data.Schemas.Sum(i => i.Value.Fields.Count);

            Assert.AreEqual(expectedNumberOfFields, loadedNumberOfFields);
        }

        [Test]
        public void Test_Loading_An_Unknown_File_Works()
        {
            string unknownFilePath = Path.Combine(Environment.CurrentDirectory, "unknown-file.xml");

            // assure that the file realy doesn't exist
            if (File.Exists(unknownFilePath))
            {
                File.Delete(unknownFilePath);
            }

            // create a new handler with an instanceDataFilePath that is unknown
            InstanceDataHandler handlerWithUnknownFile = new InstanceDataHandler(unknownFilePath);

            InstanceData data = handlerWithUnknownFile.Load();

            Assert.NotNull(data);
        }
    }
}
