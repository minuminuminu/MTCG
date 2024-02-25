using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    [Serializable]
    internal class CardNotFoundException : Exception
    {
        public CardNotFoundException()
        {
        }

        public CardNotFoundException(string? message) : base(message)
        {
        }

        public CardNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CardNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
