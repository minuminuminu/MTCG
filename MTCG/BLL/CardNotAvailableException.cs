using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    [Serializable]
    internal class CardNotAvailableException : Exception
    {
        public CardNotAvailableException()
        {
        }

        public CardNotAvailableException(string? message) : base(message)
        {
        }

        public CardNotAvailableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CardNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
