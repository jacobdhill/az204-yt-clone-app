using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Search;

public class SearchService
{
    private readonly SearchIndexClient _client;

    public SearchService(IConfiguration configuration)
    {
        var serviceEndpoint = new Uri(configuration.GetSection("SearchService:Url").Get<string>());
        var credential = new AzureKeyCredential(configuration.GetSection("SearchService:ApiKey").Get<string>());

        _client = new SearchIndexClient(serviceEndpoint, credential);
    }

    public async Task<List<T>> SearchAsync<T>(string indexName, string query, CancellationToken cancellationToken = default)
    {
        var searchClient = _client.GetSearchClient(indexName);
        var searchOptions = new SearchOptions();

        var searchResult = await searchClient.SearchAsync<T>(query, searchOptions, cancellationToken);
        var results = searchResult.Value.GetResults();

        return results
            .Select(item => item.Document)
            .ToList();
    }
}
