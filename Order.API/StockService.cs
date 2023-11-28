
using Order.API.Models;

namespace Order.API
{
    public class StockService
    {
        private readonly HttpClient _httpClient;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public  async Task<string> SaveStock()
        {

            var result = await _httpClient.PostAsJsonAsync<StockSaveDto>("/api/basket/save", new() { Id = 10, Count = 20 });

            if(result.IsSuccessStatusCode)
            {
                var content=await result.Content.ReadAsStringAsync();


                return content;

            }

            return string.Empty;



        }
    }
}
