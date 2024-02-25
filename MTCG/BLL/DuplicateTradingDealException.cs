using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    [Serializable]
    internal class DuplicateTradingDealException : Exception
    {
        public DuplicateTradingDealException()
        {
        }

        public DuplicateTradingDealException(string? message) : base(message)
        {
        }

        public DuplicateTradingDealException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateTradingDealException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
