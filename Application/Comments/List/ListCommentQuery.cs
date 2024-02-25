using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Comments.List;

public class ListCommentQuery : IRequest<List<ListCommentDto>>
{
    public Guid VideoId { get; set; }
}
