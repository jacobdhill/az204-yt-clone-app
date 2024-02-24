using MediatR;
using System;

namespace Application.Videos.Read;

public class ReadVideoQuery : IRequest<ReadVideoDto>
{
    public Guid Id { get; set; }
}
