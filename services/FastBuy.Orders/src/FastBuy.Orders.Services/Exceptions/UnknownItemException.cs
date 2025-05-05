namespace FastBuy.Orders.Services.Exceptions
{
    [Serializable]
    public class UnknownItemException : Exception
    {
        public UnknownItemException(Guid id) : base($"The resource with id {id} is unknown")
        {
            ResourseId = id;
        }

        public Guid ResourseId { get; }
    }
}
