using System.Net.Http.Json;

namespace Kairudev.Web.Services;

public sealed class SprintApiClient
{
    private readonly HttpClient _http;

    public SprintApiClient(HttpClient http) => _http = http;

    public async Task<List<SprintSessionDto>> GetTodaySprintsAsync()
    {
        var response = await _http.GetAsync("api/sprints/today");
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<SprintSessionDto>>() ?? [];
    }

    public async Task<SprintSessionDto?> RecordSprintAsync(RecordSprintRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/sprints", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<SprintSessionDto>();
    }
}
