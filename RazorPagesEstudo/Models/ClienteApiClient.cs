using RazorPagesEstudo.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ClienteApiClient
{
    private readonly HttpClient _httpClient;

    public ClienteApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> UpdatePontosFidelidadeAsync(Cliente cliente)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/clientes/{cliente.Id}/pontos", cliente);
        return response.IsSuccessStatusCode;
    }
}
