using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    [Serializable]
    internal class NotRequiredAmountOfCardsException : Exception
    {
        public NotRequiredAmountOfCardsException()
        {
        }

        public NotRequiredAmountOfCardsException(string? message) : base(message)
        {
        }

        public NotRequiredAmountOfCardsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotRequiredAmountOfCardsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
