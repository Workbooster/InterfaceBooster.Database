using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceBooster.Database.Interfaces.Structure
{
    /// <summary>
    /// A table is composed by the schema for the data definition and the data itself. The data
    /// is stored in the form of a list of arrays. Each array represents one row.
    /// All arrays must have the same size and each array item must be compatible with the fields
    /// that are defined in the tables' schema.
    /// </summary>
    public interface ITable : IList<object[]>
    {
        /// <summary>
        /// The schema containes the definition of the table.
        /// </summary>
        ISchema Schema { get; set; }

        /// <summary>
        /// overwrites all data of the table
        /// </summary>
        /// <param name="data"></param>
        void SetData(IEnumerable<Object[]> data);
    }
}
