using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceBooster.Database.Interfaces.Structure
{
    /// <summary>
    /// A field represents a column of a table.
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// the identifier of the field
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// data type
        /// </summary>
        Type Type { get; set; }
    }
}
