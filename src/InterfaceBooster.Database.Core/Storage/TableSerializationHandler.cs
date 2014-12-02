using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.ErrorHandling;
using InterfaceBooster.Database.Interfaces.Structure;
using InterfaceBooster.Database.Core.Common;
using InterfaceBooster.Database.Core.Structure;

namespace InterfaceBooster.Database.Core.Storage
{
    public class TableSerializationHandler
    {
        public const string FILENAME_EXTENSION = "syd";
        public const int MAX_FILESIZE = 2 * 1024 * 1024;

        #region PROPERTIES

        public string TableDirectoryPath { get; private set; }

        #endregion

        #region PUBLIC METHODS

        public TableSerializationHandler(string tableDirectoryPath)
        {
            if (Directory.Exists(tableDirectoryPath) == false)
                throw new SyneryDBException("The given table directory doesn't exists.");

            if (FileSystemHelper.IsDirectoryWritable(tableDirectoryPath) == false)
                throw new SyneryDBException("The given table directory isn't writable.");

            TableDirectoryPath = tableDirectoryPath;
        }

        public ITable Read(string tableName, ISchema schema)
        {
            IList<object[]> result = new List<object[]>();
            int fieldsCount = schema.Fields.Count;

            tableName = GetEscapedTableName(tableName);

            try
            {
                Parallel.ForEach(GetAllTableFiles(tableName), filePath =>
                {
                    FileStream fileStream = null;

                    try
                    {
                        fileStream = new FileStream(filePath, FileMode.Open);

                        while (fileStream.Position < fileStream.Length)
                        {
                            object[] row = new object[fieldsCount];
                            int fieldIndex = 0;

                            foreach (IField field in schema.Fields)
                            {
                                try
                                {
                                    row[fieldIndex++] = PrimitiveSerializer.Read(fileStream, field.Type, true);
                                }
                                catch (Exception ex)
                                {
                                    throw new SyneryDBException(String.Format("Error while reading field index '{0}' with name'{1}' and type '{2}' on row '{3}' from '{4}'.",
                                        fieldIndex, field.Name, field.Type.Name, result.Count + 1, filePath), ex);
                                }
                            }

                            //yield return row;
                            lock (result)
                            {
                                result.Add(row);
                            }
                        }
                    }
                    catch (Exception ex) {
                        throw new SyneryDBException(String.Format("Error while reading data from '{0}'.", filePath), ex);
                    }
                    finally
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                });

                return new Table(schema, result);
            }
            finally
            {
            }

        }

        public void Write(string tableName, ITable table)
        {
            tableName = GetEscapedTableName(tableName);
            string tempTableName = String.Format("-temp-{0}", tableName);

            int fileCounter = 1;
            FileStream fileStream = null;
            string filePath;

            try
            {
                // loop threw all rows in the table
                foreach (var row in table)
                {
                    // create a new file stream at the beginning and every time the stream exceeds the maximum file size
                    if (fileStream == null || fileStream.Length > MAX_FILESIZE)
                    {
                        if (fileStream != null)
                        {
                            // close the last file stream
                            fileStream.Close();
                        }

                        // get the file stream for the next file
                        filePath = GetNumeratedFilepath(tempTableName, fileCounter);
                        File.Delete(filePath);
                        fileStream = new FileStream(filePath, FileMode.CreateNew);
                        fileCounter++;
                    }

                    // loop threw each field of the current row and write the serialized value to the file stream
                    foreach (object field in row)
                    {
                        PrimitiveSerializer.WriteNullable(fileStream, field);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

            // delete the old files
            foreach (string oldFilePath in GetAllTableFiles(tableName))
            {
                File.Delete(oldFilePath);
            }

            Rename(tempTableName, tableName);
        }

        public bool Delete(string tableName)
        {
            tableName = GetEscapedTableName(tableName);
            bool filesFound = false;

            foreach (string oldFilePath in GetAllTableFiles(tableName))
            {
                File.Delete(oldFilePath);
                filesFound = true;
            }

            return filesFound;
        }

        public bool Rename(string currentTableName, string futureTableName)
        {
            currentTableName = GetEscapedTableName(currentTableName);
            futureTableName = GetEscapedTableName(futureTableName);
            bool filesFound = false;
            int fileCounter = 1;

            foreach (string currentFilePath in GetAllTableFiles(currentTableName))
            {
                string futureFilePath = GetNumeratedFilepath(futureTableName, fileCounter);
                File.Delete(futureFilePath);
                File.Move(currentFilePath, futureFilePath);
                fileCounter++;

                filesFound = true;
            }

            return filesFound;
        }

        #endregion

        #region INTERNAL METHODS

        private string GetEscapedTableName(string tableName)
        {
            // replace backslashes with a minus
            tableName = tableName.Replace('\\', '-');

            // remove all illegal characters
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            tableName = r.Replace(tableName, "");

            return tableName;
        }

        private IEnumerable<string> GetAllTableFiles(string tableName)
        {
            int fileCounter = 1;
            string filePath = GetNumeratedFilepath(tableName, fileCounter);

            while (true)
            {
                if (File.Exists(filePath))
                {
                    yield return filePath;

                    fileCounter++;

                    filePath = GetNumeratedFilepath(tableName, fileCounter);
                }
                else
                {
                    break;
                }
            }
        }

        private string GetNumeratedFilepath(string tableName, int fileNumber)
        {
            return Path.Combine(TableDirectoryPath, GetNumeratedFilename(tableName, fileNumber));
        }

        private string GetNumeratedFilename(string tableName, int fileNumber)
        {
            return String.Format("{0}.{1}.{2}", tableName, fileNumber, FILENAME_EXTENSION);
        }

        #endregion
    }
}
