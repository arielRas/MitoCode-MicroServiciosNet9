namespace FastBuy.Shared.Events.Exceptions
{
    public class NonExistentResourceException : AsynchronousMessagingException
    {
        public NonExistentResourceException(Guid correlationId) : base(correlationId)
        {
            
        }

        public NonExistentResourceException(Guid correlationId, string message) : base(correlationId, message)
        {
            
        }

        public NonExistentResourceException(Guid correlationId, string message, Exception innerException) : base(correlationId, message, innerException)
        {
            
        }
    }
}
