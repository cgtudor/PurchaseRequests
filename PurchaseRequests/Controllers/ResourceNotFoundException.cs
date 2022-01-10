using System;
using System.Runtime.Serialization;

namespace PurchaseRequests.Controllers
{
    [Serializable]
    public class ResourceNotFoundException : Exception
    {
        private object p;

        public ResourceNotFoundException()
        {
        }

        public ResourceNotFoundException(object p)
        {
            this.p = p;
        }

        public ResourceNotFoundException(string message) : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}