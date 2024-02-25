using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.BLL
{
    [Serializable]
    internal class NoTradingDealsException : Exception
    {
        public NoTradingDealsException()
        {
        }

        public NoTradingDealsException(string? message) : base(message)
        {
        }

        public NoTradingDealsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoTradingDealsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
