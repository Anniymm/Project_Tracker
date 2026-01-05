using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project3.Application.Features.Commands;
using Project3.Application.Features.Queries;
using Project3.Domain.Common.Response;

namespace Project3.Api.Controllers;

[Route("api/working-hours")]
[Produces("application/json")]
public class WorkingHoursController(ISender sender) : ApiController(sender)
{

    [HttpGet("provider/{providerId:guid}")]
    [ProducesResponseType(typeof(Result<List<WorkingHourResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetByProvider(Guid providerId)
    {
        return await Handle<GetWorkingHoursByProviderQuery, List<WorkingHourResponse>>(
            new GetWorkingHoursByProviderQuery(providerId));
    }
    

    [HttpPost]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IResult> Create([FromBody] CreateWorkingHoursCommand command)
    {
        return await Handle(command);
    }
  
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IResult> Update(Guid id, [FromBody] UpdateWorkingHoursRequest request)
    {
        var command = new UpdateWorkingHoursCommand(
            id,
            request.DayOfWeek,
            request.StartTime,
            request.EndTime,
            request.IsActive
        );
        
        return await Handle(command);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IResult> Delete(Guid id)
    {
        return await Handle(new DeleteWorkingHoursCommand(id));
    }
}

public record UpdateWorkingHoursRequest(
    int? DayOfWeek,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    bool? IsActive
);