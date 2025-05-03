namespace FastBuy.Shared.Events.Exceptions
{
    public class ExistingResourceException : AsynchronousMessagingException
    {
        public ExistingResourceException(Guid correlationId) : base(correlationId)
        {

        }

        public ExistingResourceException(Guid correlationId, string message) : base(correlationId, message)
        {

        }

        public ExistingResourceException(Guid correlationId, string message, Exception innerException) : base(correlationId, message, innerException)
        {

        }
    }
}
