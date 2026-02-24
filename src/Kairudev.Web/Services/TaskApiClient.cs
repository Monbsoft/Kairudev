using System.Net.Http.Json;

namespace Kairudev.Web.Services;

public sealed class TaskApiClient
{
    private readonly HttpClient _http;

    public TaskApiClient(HttpClient http) => _http = http;

    public async Task<List<TaskDto>> GetAllAsync() =>
        await _http.GetFromJsonAsync<List<TaskDto>>("api/tasks") ?? [];

    public async Task<TaskDto?> AddAsync(string title)
    {
        var response = await _http.PostAsJsonAsync("api/tasks", new { Title = title });
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<TaskDto>();
    }

    public async Task<bool> CompleteAsync(Guid id)
    {
        var response = await _http.PutAsync($"api/tasks/{id}/complete", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/tasks/{id}");
        return response.IsSuccessStatusCode;
    }
}
