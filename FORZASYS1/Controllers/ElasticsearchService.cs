using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using FORZASYS1.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Serialization;

public class ElasticsearchService
{
    private readonly HttpClient _httpClient;
    private readonly string _searchApiUrl;

    public ElasticsearchService(HttpClient httpClient, IOptions<ElasticConfig> config)
    {
        _httpClient = httpClient;
        _searchApiUrl = $"{config.Value.Uri}/_application/search_application/testsøk/_search";
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"ApiKey {config.Value.ApiKey}");
    }

    public async Task<List<Video>> SearchHighlights(string searchTerm)
    {
        var searchPayload = new
        {
            @params = new
            {
                query_string = searchTerm
            }
        };

        var jsonPayload = JsonConvert.SerializeObject(searchPayload, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()  // Ensure proper JSON casing
            }
        });

        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_searchApiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var rawJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw Elasticsearch response: {rawJson}");  // Debugging the raw response

            var searchResults = JsonConvert.DeserializeObject<SearchResultsResponse>(rawJson);
            return searchResults?.Hits?.Hits.Select(hit =>
            {
                Console.WriteLine($"_id from hit: {hit._id}");  // Debugging to check _id values
                if (string.IsNullOrEmpty(hit._id))
                {
                    Console.WriteLine("Warning: _id is null or empty.");  // Alert for null _id
                    return null;  // Return null or handle it appropriately
                }

                return new Video
                {
                    Id = hit._id,
                    Title = hit.Source?.title ?? "No title",
                    Url = ConstructBlobUrl(hit._id),
                    ThumbnailUrl = hit.Source?.thumbnailUrl ?? "default_thumbnail.jpg"
                };
            }).Where(video => video != null).ToList() ?? new List<Video>();  // Filter out null videos
        }

        Console.WriteLine($"Elasticsearch request failed. Status code: {response.StatusCode}, Content: {await response.Content.ReadAsStringAsync()}");
        return new List<Video>();
    }


    public string ConstructBlobUrl(string filename)
    {
        string baseUrl = "https://forzasysstorage.blob.core.windows.net";
        string sasToken = "sv=2023-01-03&st=2024-04-12T12%3A02%3A23Z&se=2024-04-13T12%3A17%3A23Z&sr=c&sp=r&sig=Jkwn4RvIGpcxyP6mjRSfkvgkeOacBViycvg0hkOZvVE%3D";

        // Ensure the filename is URL encoded properly to handle special characters
        string encodedFilename = Uri.EscapeDataString(filename).Replace("%2F", "/");

        // Construct the full URL to the blob
        string fullBlobUrl = $"{baseUrl}/{encodedFilename}?{sasToken}";
        Console.WriteLine("Constructed URL: " + fullBlobUrl);  // Log the URL to verify correctness
        return fullBlobUrl;
    }
}

public class ElasticConfig
{
    public string Uri { get; set; }
    public string DefaultIndex { get; set; }
    public string ApiKey { get; set; }
}

public class SearchResultsResponse
{
    public HitsWrapper Hits { get; set; }
}

public class HitsWrapper
{
    public List<Hit> Hits { get; set; }
}

public class Hit
{
    [JsonProperty("_id")]
    public string _id { get; set; }
    [JsonProperty("source")]
    public Source Source { get; set; }
}

public class Source
{
    public string title { get; set; }
    public string id { get; set; }
    public string thumbnailUrl { get; set; }
}
