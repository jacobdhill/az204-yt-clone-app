using Domain.Videos;
using Infrastructure.Search;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.List;

public class ListVideoQueryHandler : IRequestHandler<ListVideoQuery, List<ListVideoDto>>
{
    private readonly IConfiguration _configuration;
    private readonly SearchService _searchService;

    public ListVideoQueryHandler(IConfiguration configuration, SearchService searchService)
    {
        _configuration = configuration;
        _searchService = searchService;
    }

    public async Task<List<ListVideoDto>> Handle(ListVideoQuery request, CancellationToken cancellationToken)
    {
        var videos = await _searchService.SearchAsync<Video>(
            _configuration.GetSection("SearchService:VideosIndexName").Get<string>(),
            request.SearchText,
            cancellationToken);

        var videoDtos = videos
            .Select(video => ListVideoDto.Create(video))
            .ToList();

        return videoDtos;
    }
}
