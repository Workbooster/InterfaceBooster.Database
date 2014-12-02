using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceBooster.Database.Interfaces.Structure
{
    /// <summary>
    /// the schema contains the defintion of a table.
    /// </summary>
    public interface ISchema : ICloneable
    {
        /// <summary>
        /// all columns of the table in the order of occurance in the rows' object-array items.
        /// </summary>
        IReadOnlyList<IField> Fields { get; }

        /// <summary>
        /// alias for AddField(string name, Type type)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IField AddField<T>(string name);

        /// <summary>
        /// add a new field.
        /// throws an exception if there already exists a field with the given name.
        /// </summary>
        /// <param name="name">unique identifier</param>
        /// <param name="type">data type</param>
        /// <returns>the new field</returns>
        IField AddField(string name, Type type);

        /// <summary>
        /// get a field by name. Returns null if no field with the given name wasn't found.
        /// </summary>
        /// <param name="name">unique identifier</param>
        /// <returns>field or null</returns>
        IField GetField(string name);

        /// <summary>
        /// get index of the filed in the schemas' field list. Returns -1 if the field with the given name wasn't found.
        /// </summary>
        /// <param name="name">unique identifier</param>
        /// <returns>position or -1 if field wasn't found</returns>
        int GetFieldPosition(string name);

        /// <summary>
        /// removes an existing field. If the field doesn't exist it returns false.
        /// </summary>
        /// <param name="field">field to remove</param>
        /// <returns>true if field was found</returns>
        bool DeleteField(IField field);

        /// <summary>
        /// removes an existing field. If the field doesn't exist it returns false.
        /// </summary>
        /// <param name="name">unique identifier</param>
        /// <returns>true if field was found</returns>
        bool DeleteField(string name);
    }
}
