using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.Structure;

namespace InterfaceBooster.Database.Interfaces
{
    /// <summary>
    /// Manages the database. Can be used to load and store data from and to the database.
    /// </summary>
    public interface IDatabase : IDisposable
    {
        /// <summary>
        /// contains the schemas for all managed tables
        /// </summary>
        IReadOnlyDictionary<string, ISchema> Schemas { get; }

        /// <summary>
        /// When this property returns true, the databse is disposed of and can no longer be used.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// this factory method creates a new empty schema.
        /// </summary>
        /// <returns></returns>
        ISchema NewSchema();

        /// <summary>
        /// this factory methode creates a new table with the given schema containing the given data.
        /// the number of items in the object-arrays must equal to the number of fields of given the schema.
        /// </summary>
        /// <param name="schema">the definition of the table</param>
        /// <param name="data">the content of the table</param>
        /// <returns></returns>
        ITable NewTable(ISchema schema, IEnumerable<object[]> data = null);

        /// <summary>
        /// add a new table to the database
        /// throws an exception if a table with the given name already exists
        /// </summary>
        /// <param name="name">unique identifier of the new table</param>
        /// <param name="table">the data</param>
        void CreateTable(string name, ITable table);

        /// <summary>
        /// overwrites an existing table with the given table
        /// throws an exception if a table with the given name doesn't exists
        /// </summary>
        /// <param name="name">unique identifier of an existing table</param>
        /// <param name="table">the new data</param>
        void UpdateTable(string name, ITable table);

        /// <summary>
        /// if a table with the given name already exists the old table will be overwritten. Otherwise a new table
        /// with the given name will be created.
        /// </summary>
        /// <param name="name">unique identifier of existing or new table</param>
        /// <param name="table">the data</param>
        void CreateOrUpdateTable(string name, ITable table);

        /// <summary>
        /// deletes an existing table. If no table with the given name exists the return value will be "false".
        /// </summary>
        /// <param name="name">unique identifier of the table to delete</param>
        /// <returns>true if table was found</returns>
        bool DeleteTable(string name);

        /// <summary>
        /// renames an existing table. If no table with the given name exists the return value will be "false".
        /// if a table with the target name already exists the return value also will be "false".
        /// </summary>
        /// <param name="from">current table unique identifier</param>
        /// <param name="to">new table unique identifier</param>
        /// <returns>true if table was found</returns>
        bool RenameTable(string from, string to);

        /// <summary>
        /// load the table with the given name.
        /// returns null if the table wasn't found.
        /// </summary>
        /// <param name="name">unique identifier of the existing table</param>
        /// <returns>table with the given name or null if table doesn't exist</returns>
        ITable LoadTable(string name);

        /// <summary>
        /// check whether a table with the given name exists.
        /// </summary>
        /// <param name="name">unique identifier of the table</param>
        /// <returns>true if table exists</returns>
        bool IsTable(string name);
    }
}
