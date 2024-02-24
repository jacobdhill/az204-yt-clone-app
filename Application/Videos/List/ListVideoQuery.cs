using MediatR;
using System.Collections.Generic;

namespace Application.Videos.List;

public class ListVideoQuery : IRequest<List<ListVideoDto>>
{
}
