namespace FastBuy.Stocks.Entities.Exceptions
{
    public class MicroserviceComunicationException : Exception
    {
        public MicroserviceComunicationException() : base ("A connection error occurred between services")
        {
            
        }

        public MicroserviceComunicationException(string message) : base(message)
        {
            
        }

        public MicroserviceComunicationException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
