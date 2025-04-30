namespace FastBuy.Shared.Events.Exceptions
{
    public class NonExistentProductException : AsynchronousMessagingException
    {
        public Guid ProductId { get; }

        public NonExistentProductException(Guid correlationId, Guid productId) : base(correlationId)
        {
            ProductId = productId;
        }

        public NonExistentProductException(Guid correlationId, Guid productId, string message) : base(correlationId, message)
        {
            ProductId = productId;
        }

        public NonExistentProductException(Guid correlationId, Guid productId, string message, Exception innerException) : base(correlationId, message, innerException)
        {
            ProductId = productId;
        }
    }
}
