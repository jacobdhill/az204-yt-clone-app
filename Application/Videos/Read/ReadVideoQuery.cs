using MediatR;
using System;

namespace Application.Videos.Get;

public class ReadVideoQuery : IRequest<ReadVideoDto>
{
    public Guid Id { get; set; }
}
