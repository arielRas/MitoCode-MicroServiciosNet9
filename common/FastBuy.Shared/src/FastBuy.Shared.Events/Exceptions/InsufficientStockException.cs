namespace FastBuy.Shared.Events.Exceptions
{
    public class InsufficientStockException : AsynchronousMessagingException
    {
        public InsufficientStockException(Guid correlationId) : base(correlationId)
        {
        }

        public InsufficientStockException(Guid correlationId, string message) : base(correlationId, message)
        {
        }

        public InsufficientStockException(Guid correlationId, string message, Exception innerException) : base(correlationId, message, innerException)
        {
        }
    }
}
