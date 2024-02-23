using Application.Videos.Create;
using Application.Videos.Get;
using Domain.Videos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoController : Controller
{
    private readonly ILogger _logger = Log.ForContext<VideoController>();
    private readonly ISender _sender;

    public VideoController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateVideoCommand command)
    {
        try
        {
            var videoId = await _sender.Send(command);

            return Results.Ok(videoId);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }

    [HttpGet("{videoId:guid}")]
    public async Task<IResult> Get(Guid videoId)
    {
        try
        {
            var query = new ReadVideoQuery()
            {
                Id = videoId
            };

            var video = await _sender.Send(query);

            return Results.Ok(video);
        }
        catch (VideoNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }
}
