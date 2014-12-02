using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceBooster.Database.Interfaces.ErrorHandling
{
    [Serializable]
    public class SyneryDBException : Exception
    {
        public SyneryDBException() { }
        public SyneryDBException(string message) : base(message) { }
        public SyneryDBException(string message, Exception inner) : base(message, inner) { }
        protected SyneryDBException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
