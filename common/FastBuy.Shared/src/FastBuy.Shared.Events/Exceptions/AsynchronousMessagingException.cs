namespace FastBuy.Shared.Events.Exceptions
{
    [Serializable]
    public abstract class AsynchronousMessagingException : Exception
    {
        public Guid CorrelationId { get; }

        protected AsynchronousMessagingException(Guid correlationId) : base("Asynchronous communication error")
        {
            CorrelationId = correlationId;
        }

        protected AsynchronousMessagingException(Guid correlationId, string message) : base(message)
        {
            CorrelationId = correlationId;
        }

        protected AsynchronousMessagingException(Guid correlationId, string message, Exception innerException) : base(message, innerException)
        {
            CorrelationId = correlationId;
        }
    }
}
