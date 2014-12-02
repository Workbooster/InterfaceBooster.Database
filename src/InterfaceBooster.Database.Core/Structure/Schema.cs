using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.Structure;

namespace InterfaceBooster.Database.Core.Structure
{
    /// <summary>
    /// the schema contains the defintion of a table.
    /// </summary>
    public class Schema : ISchema
    {
        protected List<IField> _Fields = new List<IField>();

        public IReadOnlyList<IField> Fields
        {
            get { return _Fields; }
        }

        public IField AddField<T>(string name)
        {
            return AddField(name, typeof(T));
        }

        public IField AddField(string name, Type type)
        {
            IField field = new Field()
            {
                Name = name,
                Type = type
            };

            _Fields.Add(field);

            return field;
        }

        public IField GetField(string name)
        {
            return _Fields.FirstOrDefault(f => f.Name == name);
        }

        public int GetFieldPosition(string name)
        {
            return _Fields.FindIndex(f => f.Name == name);
        }

        public bool DeleteField(IField field)
        {
            return _Fields.Remove(field);
        }

        public bool DeleteField(string name)
        {
            return _Fields.RemoveAll(f => f.Name == name) > 0;
        }

        public object Clone()
        {
            Schema copy = new Schema();

            foreach (var field in _Fields)
            {
                copy.AddField(field.Name, field.Type);
            }

            return copy;
        }
    }
}
