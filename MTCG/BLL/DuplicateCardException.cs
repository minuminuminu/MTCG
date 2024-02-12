using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MTCG.BLL
{
    [Serializable]
    internal class DuplicateCardException : Exception
    {
        public DuplicateCardException()
        {
        }

        public DuplicateCardException(string? message) : base(message)
        {
        }

        public DuplicateCardException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateCardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
