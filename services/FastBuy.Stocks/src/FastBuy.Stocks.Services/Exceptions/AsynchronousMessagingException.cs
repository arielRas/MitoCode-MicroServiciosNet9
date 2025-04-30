namespace FastBuy.Stocks.Services.Exceptions
{
    public abstract class AsynchronousMessagingException : Exception
    {        
        public Guid CorrelationId { get;}

        protected AsynchronousMessagingException(Guid correlationId, string message) : base(message)
        {
            CorrelationId = correlationId;
        }
    }
}
