namespace FastBuy.Stocks.Services.Exceptions
{
    [Serializable]
    public class NonExistentProductException : AsynchronousMessagingException
    {

        public NonExistentProductException(Guid correlationId, string message) : base(correlationId, message)
        {
            
        }
    }
}
