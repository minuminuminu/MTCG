using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MTCG.BLL
{
    [Serializable]
    internal class NotEnoughCoinsException : Exception
    {
        public NotEnoughCoinsException()
        {
        }

        public NotEnoughCoinsException(string? message) : base(message)
        {
        }

        public NotEnoughCoinsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotEnoughCoinsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
