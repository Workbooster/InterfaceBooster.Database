using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.Structure;

namespace InterfaceBooster.Database.Core.Storage
{
    /// <summary>
    /// Contains the data of a Synery Database instance.
    /// </summary>
    public class InstanceData
    {
        public string InstanceDataFilePath { get; private set; }
        public IDictionary<string, ISchema> Schemas { get; set; }

        public InstanceData(string instanceDataFilePath)
        {
            InstanceDataFilePath = instanceDataFilePath;
            Schemas = new Dictionary<string, ISchema>();
        }
    }
}
