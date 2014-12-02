using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces;
using InterfaceBooster.Database.Interfaces.ErrorHandling;
using InterfaceBooster.Database.Interfaces.Structure;
using InterfaceBooster.Database.Core.Common;
using InterfaceBooster.Database.Core.Storage;
using InterfaceBooster.Database.Core.Structure;

namespace InterfaceBooster.Database.Core
{
    /// <summary>
    /// Manages the database. Can be used to load and store data from and to the database.
    /// </summary>
    public class SyneryDB : IDatabase
    {
        #region MEMBERS

        private static string _InstanceDataFileName = "data.xml";
        private static string _LockFileName = ".lock";
        private static string _TableSubdirectoryName = "tables";
        private InstanceDataHandler _InstanceDataHandler;
        private TableSerializationHandler _SerializationHandler;
        private IDictionary<string, ISchema> _Schemas;

        #endregion

        #region PROPERTIES

        public IReadOnlyDictionary<string, ISchema> Schemas { get { return new Dictionary<string, ISchema>(_Schemas); } }
        public bool IsDisposed { get; private set; }
        public string WorkingDirectoryPath { get; private set; }
        public string LockFilePath { get; private set; }
        public InstanceData InstanceData { get; private set; }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Manages the database. Can be used to load and store data from and to the database.
        /// Throws a SyneryDBException if the initialization fails.
        /// </summary>
        /// <param name="workingDirectoryPath">the root directory of the database files 
        /// or an empty directory to initialize a new databse.</param>
        public SyneryDB(string workingDirectoryPath)
        {
            if (Directory.Exists(workingDirectoryPath) == false)
                throw new SyneryDBException(String.Format("The given working directory doesn't exists. Path: {0}", workingDirectoryPath));

            if (FileSystemHelper.IsDirectoryWritable(workingDirectoryPath) == false)
                throw new SyneryDBException(String.Format("The given working directory isn't writable. Path: {0}", workingDirectoryPath));
            
            WorkingDirectoryPath = workingDirectoryPath;
            LockFilePath = Path.Combine(WorkingDirectoryPath, _LockFileName);

            if (File.Exists(LockFilePath))
                throw new SyneryDBException(String.Format(
                    "The database is locked by the lock file ('{0}'). Please assure that there is no other instance using this database at the same time. WorkingDirectoryPath: {1}"
                    , _LockFileName, workingDirectoryPath));


            // CREATE LOCK FILE
            // this guarantees that no other SyneryDB instance is accessing the database files at the same time
            File.WriteAllText(LockFilePath, "");

            // INSTANCE DATA
            // load instance data (e.g. schema)
            
            string instanceDataFilePath = Path.Combine(WorkingDirectoryPath, _InstanceDataFileName);
            _InstanceDataHandler = new InstanceDataHandler(instanceDataFilePath);
            InstanceData = _InstanceDataHandler.Load();
            _Schemas = InstanceData.Schemas;

            // TABLE SERIALIZATION HANDLER

            string tableDirectoryPath = Path.Combine(WorkingDirectoryPath, _TableSubdirectoryName);

            if (Directory.Exists(tableDirectoryPath) == false)
            {
                // create the table directory
                Directory.CreateDirectory(tableDirectoryPath);
            }

            _SerializationHandler = new TableSerializationHandler(tableDirectoryPath);
        }

        ~SyneryDB()
        {
            Dispose();
        }

        /// <summary>
        /// Releases all used ressources
        /// </summary>
        public void Dispose()
        {
            IsDisposed = true;

            if (File.Exists(LockFilePath))
                File.Delete(LockFilePath);
        }

        /// <summary>
        /// Creates a new empty schema. This is a factory method.
        /// </summary>
        /// <returns></returns>
        public ISchema NewSchema()
        {
            return new Schema();
        }

        /// <summary>
        /// Creates a new empty table from the given schema. This is a factory method.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public ITable NewTable(ISchema schema, IEnumerable<object[]> data = null)
        {
            return new Table(schema, data);
        }

        /// <summary>
        /// Adds the given table to the database.
        /// Throws an SyneryDBException if a table with the given name already exists.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="table"></param>
        public void CreateTable(string name, ITable table)
        {
            CheckState();
            ValidateTable(name, table);

            if (_Schemas.ContainsKey(name) == false)
            {
                CreateOrUpdateTable(name, table);
            }
            else
            {
                throw new SyneryDBException(String.Format("A schema with the name '{0}' already exists. Please use the UpdateTable-Method to update an existing table.", name));
            }
        }

        /// <summary>
        /// Updates the given table in the database.
        /// Throws an SyneryDBException if a table with the given name doesn't exists.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="table"></param>
        public void UpdateTable(string name, ITable table)
        {
            CheckState();
            ValidateTable(name, table);

            if (_Schemas.ContainsKey(name))
            {
                CreateOrUpdateTable(name, table);
            }
            else
            {
                throw new SyneryDBException(String.Format("A schema with the name '{0}' doestn't exists. Please use the CreateTable-Method to create a new table.", name));
            }
        }

        /// <summary>
        /// Adds or updates the given table. This method doesn't throw an exception if the table is new neither if it already exists.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="table"></param>
        public void CreateOrUpdateTable(string name, ITable table)
        {
            CheckState();
            ValidateTable(name, table);

            _SerializationHandler.Write(name, table);
            _Schemas[name] = table.Schema;
            UpdateInstanceData();
        }

        /// <summary>
        /// Removes the table with the given name from the database.
        /// Throws a SyneryDBException if the table doesn't exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteTable(string name)
        {
            CheckState();

            if (String.IsNullOrEmpty(name))
                throw new SyneryDBException("The SyneryDB delete method cannot work with table name that is null or empty.");

            bool filesDeleteResult = _SerializationHandler.Delete(name);
            bool schemaRemoveResult = _Schemas.Remove(name);
            UpdateInstanceData();

            return filesDeleteResult && schemaRemoveResult;
        }

        /// <summary>
        /// Renames the given table. Throws a SyneryDBException if the "from" table doesn't exists or the "to" name is already taken.
        /// </summary>
        /// <param name="from">current name</param>
        /// <param name="to">future name</param>
        /// <returns></returns>
        public bool RenameTable(string from, string to)
        {
            CheckState();

            if (String.IsNullOrEmpty(from))
                throw new SyneryDBException("The SyneryDB rename method cannot work with a source table name that is null or empty.");

            if (String.IsNullOrEmpty(to))
                throw new SyneryDBException("The SyneryDB rename method cannot work with a destination table name that is null or empty.");

            bool filesRenameResult = _SerializationHandler.Rename(from, to);
            _Schemas.Add(to, _Schemas[from]);
            bool schemaRenameResult = _Schemas.Remove(from);

            return filesRenameResult && schemaRenameResult;
        }

        /// <summary>
        /// Loads the table and it's schema.
        /// Throws a SyneryDBException if the table doesn't exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ITable LoadTable(string name)
        {
            CheckState();

            if (String.IsNullOrEmpty(name))
                throw new SyneryDBException("The SyneryDB load method cannot work with table name that is null or empty.");

            if (_Schemas.ContainsKey(name))
            {
                // search the schema and create a clone for the return
                ISchema schema = (ISchema)_Schemas[name].Clone();

                return _SerializationHandler.Read(name, schema);
            }

            throw new SyneryDBException(String.Format("The schema with the name '{0}' was not found.", name));
        }

        /// <summary>
        /// Checks whether the table with the given name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsTable(string name)
        {
            CheckState();

            if (_Schemas.ContainsKey(name))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region INTERNAL METHODS

        private void UpdateInstanceData()
        {
            InstanceData.Schemas = _Schemas;
            _InstanceDataHandler.Save(InstanceData);
        }

        private void ValidateTable(string name, ITable table)
        {
            if (String.IsNullOrEmpty(name))
                throw new SyneryDBException("SyneryDB cannot create a table with a name that is null or empty.");

            if (table.Schema == null)
                throw new SyneryDBException(String.Format("SyneryDB cannot create a table without a schema. Table name='{0}'.", name));

            if (table.Schema.Fields == null)
                throw new SyneryDBException(String.Format("SyneryDB cannot create a table without fields. Field list is missing. Table name='{0}'.", name));

            if (table.Schema.Fields.Count == 0)
                throw new SyneryDBException(String.Format("SyneryDB cannot create a table without fields. Zero fields are given. Table name='{0}'.", name));
        }

        private void CheckState()
        {
            if (IsDisposed)
                throw new SyneryDBException("The SyneryDB is disposed of and can no longer be used to access data. Please create a new instance to re-open the database.");
        }

        #endregion
    }
}
