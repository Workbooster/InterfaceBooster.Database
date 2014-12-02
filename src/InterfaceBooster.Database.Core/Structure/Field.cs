using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.Structure;

namespace InterfaceBooster.Database.Core.Structure
{
    /// <summary>
    ///  A field represents a column of a table.
    /// </summary>
    public class Field : IField
    {
        public string Name { get; set; }

        public Type Type { get; set; }
    }
}
