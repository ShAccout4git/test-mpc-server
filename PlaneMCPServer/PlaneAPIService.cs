using System.Text;
using System.Text.Json;

public class PlaneAPIService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl;
    private readonly string _workSpace;
    private readonly string _projectId;
    private readonly string _apiKey;

    public PlaneAPIService(
        IHttpClientFactory httpClientFactory,
        string baseUrl,
        string workSpace,
        string projectId,
        string apiKey)
    {
       _httpClientFactory = httpClientFactory;
       _baseUrl = baseUrl.TrimEnd('/');
       _workSpace = workSpace;
       _projectId = projectId;
       _apiKey = apiKey; 
    }

    public async Task<string> GetProjectStatesAsync()
    {
        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);

        var url = $"{_baseUrl}/api/v2/workspaces/{_workSpace}/projects/{_projectId}/states/?order_by=sequence&per_page=50";

        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        
        return content;
    }

    public async Task<string> CreateWorkItemAsync(string name, string descriptionHtml, string stateId)
    {
        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);

        // var url = $"{_baseUrl}/api/v2/workspaces/{_workSpace}/projects/{_projectId}/work-items/";
        var url = $"https://api.plane.so/api/v2/workspaces//projects/{_projectId}/work-items/";

        var requestBody = new
        {
            name = name,
            description_html = descriptionHtml,
            // state = stateId,
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);

        var context = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, context);

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch(Exception ex)
        {
            Console.WriteLine("ExFromSh", ex.Message);
        }
        

        var responseContent = await response.Content.ReadAsStringAsync();

        return responseContent;
    }
}