using FORZASYS1.Models;
using Nest; // Assuming you have installed Nest library

public class ElasticsearchService
{
    private readonly IElasticClient _client;

    public ElasticsearchService(IElasticClient client)
    {
        _client = client;
    }

    public async Task<List<Video>> SearchHighlights(string searchTerm)
    {
        var searchResults = await _client.SearchAsync<Video>(s => s
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Title) // Assuming Title exists in Video class
                    .Query(searchTerm)
                )
            )
        );

        return searchResults.Documents.ToList();
    }
}