using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project3.Application.Features.Commands;
using Project3.Application.Features.Queries;

namespace Project3.Api.Controllers;

[Route("api/providers")]
public class ServiceProviderController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public async Task<IResult> AddProvider([FromBody] CreateServiceProviderCommand command)
    {
        return await Handle(command);
    }
    
    // [HttpPut]
    // public async Task<IResult> PutProviders([FromBody] RescheduleAppointmentCommand command)
    // {
    //     return await Handle(command);
    // }
    
    [HttpDelete("provider/{Id}")]
    public async Task<IResult> DeleteProvider(Guid Id)
    {
        return await Handle(new DeleteServiceProviderCommand(Id));
    }
    
    
    [HttpGet]
    public async Task<IResult> GetAllProviders()
    {
        return await Handle(new GetServiceProvidersQuery());
    }
    
    
    [HttpGet("provider/{id}")]
    public async Task<IResult> GetProviderById(Guid id)
    {
        return await Handle(new GetServiceProviderByIdQuery(id));
  }
}