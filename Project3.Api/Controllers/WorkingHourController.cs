using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project3.Application.Common.DTOs;
using Project3.Application.Features.Commands;
using Project3.Application.Features.Queries;

namespace Project3.Api.Controllers;

[Route("api/working-hours")]
public class WorkingHoursController(ISender sender) : ApiController(sender)
{
    [HttpGet("provider/{providerId:guid}")]
    // swaggershi response formati egreve rom chandes, requestamde
    // [ProducesResponseType(typeof(List<WorkingHourResponse>), StatusCodes.Status200OK)]
    public async Task<IResult> GetByProvider(Guid providerId)
    {
        return await Handle<GetWorkingHoursByProviderQuery, List<WorkingHourResponse>>(
            new GetWorkingHoursByProviderQuery(providerId));
    }
    
    [HttpPost]
    // [ProducesResponseType(typeof(WorkingHourResponse), StatusCodes.Status200OK)]
    public async Task<IResult> Create([FromBody] CreateWorkingHoursCommand command)
    {
        return await Handle(command);
    }
    
    [HttpPut("{id:guid}")] 
    // id vabshe ar moitxovo, radgan body-shi modis 
    // da UpdateWorkingHoursDto-s magivrad egreve chawere [FromBody] atributshi pawuk :3
    public async Task<IResult> Update(Guid id, [FromBody] UpdateWorkingHoursDto dto)
    {
        var command = new UpdateWorkingHoursCommand(
            id,
            dto.DayOfWeek,
            dto.StartTime,
            dto.EndTime,
            dto.IsActive
        );
        
        return await Handle(command);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IResult> Delete(Guid id)
    {
        return await Handle(new DeleteWorkingHoursCommand(id));
    }
}

