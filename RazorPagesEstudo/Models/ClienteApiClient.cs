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

    public async Task<bool> UpdatePontosFidelidadeAsync(int clienteId, int novosPontos)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/clientes/{clienteId}/pontos", novosPontos);
        return response.IsSuccessStatusCode;
    }
}
