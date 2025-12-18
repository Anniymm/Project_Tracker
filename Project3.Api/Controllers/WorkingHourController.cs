using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project3.Application.Features.Commands;
using Project3.Application.Features.Queries;

namespace Project3.Api.Controllers;

[Route("api/working-hours")]
public class WorkingHoursController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public async Task<IResult> GetAll(Guid providerId)
    {
        return await Handle(new GetWorkingHoursByProviderQuery(providerId));
    }
    
    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateWorkingHoursCommand command)
    {
        return await Handle(command);
    }
    

    [HttpPut("{id:guid}")]
    public async Task<IResult> Update([FromBody] UpdateWorkingHoursCommand command)
    {
        
        return await Handle(command);
    }

    
    [HttpDelete("{id:guid}")]
    public async Task<IResult> Delete(Guid id)
    {
        return await Handle(new DeleteWorkingHoursCommand(id));
    }
}
