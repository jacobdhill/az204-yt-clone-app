using Application.Reminders.Create;
using Application.Reminders.Delete;
using Application.Reminders.List;
using Application.Reminders.Update;
using Domain.Reminders;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReminderController : Controller
{
    private readonly ILogger _logger = Log.ForContext<ReminderController>();
    private readonly ISender _sender;

    public ReminderController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateReminderCommand command)
    {
		try
		{
			await _sender.Send(command);

            return Results.Ok();
        }
		catch (Exception e)
		{
            return Results.Problem(e.Message);
		}
    }

    [HttpGet]
    public async Task<IResult> List([FromQuery] ListReminderQuery query)
    {
        try
        {
            var result = await _sender.Send(query);

            return Results.Ok(result);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }

    [HttpPut("{reminderId:guid}")]
    public async Task<IResult> Update(Guid reminderId, [FromBody] UpdateReminderCommand command)
    {
        try
        {
            command.ReminderId = new ReminderId(reminderId);
            await _sender.Send(command);

            return Results.Ok();
        }
        catch (ReminderNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }

    [HttpDelete("{reminderId:guid}")]
    public async Task<IResult> Delete(Guid reminderId)
    {
        try
        {
            var command = new DeleteReminderCommand
            {
                ReminderId = new ReminderId(reminderId)
            };
            await _sender.Send(command);

            return Results.Ok();
        }
        catch (ReminderNotFoundException e)
        {
            return Results.NotFound(e.Message);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }
}
