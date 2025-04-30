namespace FastBuy.Shared.Events.Exceptions
{
    public class NonExistentProductException : AsynchronousMessagingException
    {
        public NonExistentProductException(Guid correlationId) : base(correlationId)
        {
            
        }

        public NonExistentProductException(Guid correlationId, string message) : base(correlationId, message)
        {
            
        }

        public NonExistentProductException(Guid correlationId, string message, Exception innerException) : base(correlationId, message, innerException)
        {
            
        }
    }
}
