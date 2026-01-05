using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project3.Application.Features.Commands;
using Project3.Application.Features.Queries;

namespace Project3.Api.Controllers;

[Route("api/blocked-times")]
public class BlockedTimesController(ISender sender) : ApiController(sender)
{
    [HttpGet("provider/{providerId:guid}")]
    public async Task<IResult> GetByProvider(Guid providerId) 
        => await Handle(new GetBlockedTimesByProviderQuery(providerId));

    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateBlockedTimeCommand command) 
        => await Handle(command);

    [HttpPut("{id:guid}")]
    public async Task<IResult> Update([FromBody] UpdateBlockedTimeCommand command)
        =>  await Handle(command);
    
    [HttpDelete("{id:guid}")]
    public async Task<IResult> Delete(Guid id) 
        => await Handle(new DeleteBlockedTimeCommand(id));
}


