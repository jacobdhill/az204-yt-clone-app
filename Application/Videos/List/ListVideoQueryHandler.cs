using Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.List;

public class ListVideoQueryHandler : IRequestHandler<ListVideoQuery, List<ListVideoDto>>
{
    private readonly ApplicationDbContext _dbContext;

    public ListVideoQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ListVideoDto>> Handle(ListVideoQuery request, CancellationToken cancellationToken)
    {
        var videos = await _dbContext.Videos
            .Select(video => ListVideoDto.Create(video))
            .ToListAsync(cancellationToken);

        return videos;
    }
}
