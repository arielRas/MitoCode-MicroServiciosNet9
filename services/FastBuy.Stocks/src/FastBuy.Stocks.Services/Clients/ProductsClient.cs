using FastBuy.Stocks.Contracts;
using FastBuy.Stocks.Entities.Exceptions;
using System.Net;
using System.Net.Http.Json;

namespace FastBuy.Stocks.Services.Clients
{
    public class ProductsClient
    {
        private readonly HttpClient _httpClient;
        public ProductsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<ProductInfoDto> GetProductByIdAsync(Guid productId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/products/{productId}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new KeyNotFoundException($"The resource with id {productId} does not exist");

                if (!response.IsSuccessStatusCode)
                    throw new MicroserviceComunicationException(
                        $"An error occurred while trying to get information from the products microservice - StatusCode: {response.StatusCode}");

                return await response.Content.ReadFromJsonAsync<ProductInfoDto>()
                    ?? throw new KeyNotFoundException($"The resource with id {productId} does not exist");
            }
            catch (Exception)
            {
                throw;
            }
        }        
    }
}
