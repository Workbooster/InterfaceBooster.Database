using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBooster.Database.Interfaces.ErrorHandling;
using InterfaceBooster.Database.Interfaces.Structure;

namespace InterfaceBooster.Database.Core.Structure
{
    /// <summary>
    /// Contains the schema and the data of a table. The rows are represented by object arrays.
    /// </summary>
    public class Table : ITable
    {
        #region MEMBERS

        private ISchema _Schema;
        private IList<object[]> _Data;

        #endregion

        #region PROPERTIES

        public ISchema Schema
        {
            get
            {
                return _Schema;
            }
            set
            {
                _Schema = value;
            }
        }

        #endregion

        #region PUBLIC METHODS

        #region CONSTRUCTORS

        public Table()
        {
            _Data = new List<object[]>();
        }

        public Table(ISchema schema, IEnumerable<object[]> data = null)
        {
            _Schema = schema;

            if (data != null)
            {
                SetData(data);
            }
            else
            {
                _Data = new List<object[]>();
            }
        }

        #endregion

        #region IMPLEMENTATION OF ITable

        /// <summary>
        /// overwrites all data of the table
        /// </summary>
        /// <param name="data"></param>
        public void SetData(IEnumerable<object[]> data)
        {
            if (_Schema == null)
                throw new SyneryDBException("Cannot set data of a table before having a schema.");

            _Data = new List<object[]>(data);
        }

        #endregion

        #region IMPLEMENTATION OF IList<object[]>

        public int IndexOf(object[] item)
        {
            return _Data.IndexOf(item);
        }

        public void Insert(int index, object[] item)
        {
            _Data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _Data.RemoveAt(index);
        }

        public object[] this[int index]
        {
            get
            {
                return _Data[index];
            }
            set
            {
                _Data[index] = value;
            }
        }

        public void Add(object[] item)
        {
            _Data.Add(item);
        }

        public void Clear()
        {
            _Data.Clear();
        }

        public bool Contains(object[] item)
        {
            return _Data.Contains(item);
        }

        public void CopyTo(object[][] array, int arrayIndex)
        {
            _Data.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _Data.Count; }
        }

        public bool IsReadOnly
        {
            get { return _Data.IsReadOnly; }
        }

        public bool Remove(object[] item)
        {
            return _Data.Remove(item);
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            return _Data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Data.GetEnumerator();
        }

        #endregion

        #endregion
    }
}
