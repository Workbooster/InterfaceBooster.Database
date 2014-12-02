using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using InterfaceBooster.Database.Interfaces.Structure;
using InterfaceBooster.Database.Core.Structure;

namespace InterfaceBooster.Database.Core.Storage
{
    /// <summary>
    /// every instance of a SyneryDB needs a config file. This class handels the loading 
    /// and the saving of that file using XML.
    /// </summary>
    public class InstanceDataHandler
    {
        public string InstanceDataFilePath { get; private set; }

        /// <summary>
        /// handels the SyneryDB instance config file
        /// </summary>
        /// <param name="instanceDataFilePath">the absolute path of the config file (XML)</param>
        public InstanceDataHandler(string instanceDataFilePath)
        {
            InstanceDataFilePath = instanceDataFilePath;
        }

        public InstanceData Load()
        {
            InstanceData data = new InstanceData(InstanceDataFilePath);

            if (File.Exists(InstanceDataFilePath))
            {
                XDocument doc = XDocument.Load(InstanceDataFilePath);
                XElement xmlRoot = doc.Element("SyneryDB");
                XElement xmlSchemasRoot = xmlRoot.Element("Schemas");

                data.Schemas = LoadSchemas(xmlSchemasRoot);
            }

            return data;
        }

        public void Save(InstanceData data)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("SyneryDB");

            root.Add(CreateSchemasNode(data.Schemas));

            doc.Add(root);
            doc.Save(InstanceDataFilePath);
        }

        protected IDictionary<string, ISchema> LoadSchemas(XElement xmlSchemasRoot)
        {
            Dictionary<string, ISchema> schemas = new Dictionary<string, ISchema>();

            foreach (XElement xmlSchema in xmlSchemasRoot.Elements("Schema"))
            {
                ISchema schema = new Schema();
                string name = xmlSchema.Attribute("name").Value;

                XElement xmlFieldsRoot = xmlSchema.Element("Fields");

                foreach (XElement xmlField in xmlFieldsRoot.Elements("Field"))
                {
                    schema.AddField(xmlField.Attribute("name").Value, Type.GetType(xmlField.Attribute("type").Value));
                }

                schemas.Add(name, schema);
            }

            return schemas;
        }

        protected XElement CreateSchemasNode(IDictionary<string, ISchema> schemas)
        {
            XElement xmlSchemasRoot = new XElement("Schemas");

            foreach (var item in schemas)
            {
                xmlSchemasRoot.Add(CreateSchemaNode(item.Key, item.Value));
            }

            return xmlSchemasRoot;
        }

        protected XElement CreateSchemaNode(string name, ISchema schema)
        {
            XElement xmlSchema = new XElement("Schema");
            XElement xmlFieldsRoot = new XElement("Fields");
            xmlSchema.Add(new XAttribute("name", name));
            xmlSchema.Add(xmlFieldsRoot);

            foreach (IField field in schema.Fields)
            {
                XElement xmlField = new XElement("Field");
                xmlField.Add(
                    new XAttribute("name", field.Name),
                    new XAttribute("type", field.Type.AssemblyQualifiedName));
                xmlFieldsRoot.Add(xmlField);
            }

            return xmlSchema;
        }
    }
}
