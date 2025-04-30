namespace FastBuy.Stocks.Services.Exceptions
{
    [Serializable]
    public class InsufficientStockException : AsynchronousMessagingException
    {
        public InsufficientStockException(Guid correlationId, string message) : base(correlationId, message)
        {
            
        }
    }
}
