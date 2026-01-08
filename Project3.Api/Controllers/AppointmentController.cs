using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project3.Application.Features.Commands;
using Project3.Application.Features.Queries;
using Project3.Domain.Common.Response;

namespace Project3.Api.Controllers;

[Route("api/appointments")]
[Produces("application/json")]
public class AppointmentsController(ISender sender) : ApiController(sender)
{
    // sheidzleba ProducesResponseType(typeof(Result) aq pirdapir typeofshi
    // response recordi gadavawodo -> rogorc working hourshi davwere
    
    [HttpPost]
    public async Task<IResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
    {
        return await Handle(command);
    }


    [HttpPut]
    public async Task<IResult> RescheduleAppointment([FromBody] RescheduleAppointmentCommand command)
    {
        return await Handle(command);
    }
    
    
    [HttpDelete("{appointmentId}")]
    public async Task<IResult> DeleteAppointment(Guid appointmentId, [FromQuery] string reason) 
    {
        return await Handle(new CancelAppointmentCommand(appointmentId, reason));
    }
    

    [HttpGet]
    [ProducesResponseType(typeof(List<GetAppointmentQueryResponsed>), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IResult> GetAllAppointments()
    {
        return await Handle<GetAllAppointmentsQuery, List<GetAppointmentQueryResponsed>>(
            new GetAllAppointmentsQuery());
    }
    
    [HttpGet("appointment/{id}")]
    [ProducesResponseType(typeof(GetAppointmentQueryResponse), StatusCodes.Status200OK)]
    public async Task<IResult> GetAppointmentById(Guid id)
    {
        return await Handle<GetAppointmentByIdQuery, GetAppointmentQueryResponse>(
            new GetAppointmentByIdQuery(id));
    }
    
    [HttpGet("provider/{providerId}")]
    [ProducesResponseType(typeof(List<GetAppointmentQueryResponse>), StatusCodes.Status200OK)]
    public async Task<IResult> GetAppointmentsByProvider(Guid providerId)
    {
        return await Handle<GetAppointmentsByProviderQuery, List<GetAppointmentQueryResponse>>(
            new GetAppointmentsByProviderQuery(providerId));
    }
}