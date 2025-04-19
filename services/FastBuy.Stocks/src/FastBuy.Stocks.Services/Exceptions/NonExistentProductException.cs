namespace FastBuy.Stocks.Services.Exceptions
{
    [Serializable]
    public class NonExistentProductException : Exception
    {
        public Guid ProductId { get; }

        public NonExistentProductException(Guid ProductId) : base($"product with id {ProductId} does not exist")
        {
            this.ProductId = ProductId;
        }
    }
}
