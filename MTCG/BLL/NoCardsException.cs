using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MTCG.BLL
{
    [Serializable]
    internal class NoCardsException : Exception
    {
        public NoCardsException()
        {
        }

        public NoCardsException(string? message) : base(message)
        {
        }

        public NoCardsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoCardsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
