using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project3.Application.Features.Commands;
using Project3.Application.Features.Queries;

namespace Project3.Api.Controllers;

[Route("api/appointments")]
public class AppointmentsController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public async Task<IResult> CreateAppointment([FromBody] CreateAppointmentCommand command)
    {
        return await Handle(command);
    }

    // UNDA DAVCHECKO KIDEV 
    [HttpPut]
    public async Task<IResult> RescheduleAppointment([FromBody] RescheduleAppointmentCommand command)
    {
        return await Handle(command);
    }
    
    
    [HttpDelete("{appointmentId}")]
    public async Task<IResult> DeleteAppointment(Guid appointmentId, string reason)
    {
        return await Handle(new CancelAppointmentCommand(appointmentId, reason));
    }
    
    [HttpGet]
    public async Task<IResult> GetAllAppointments()
    {
        return await Handle(new GetAllAppointmentsQuery());
    }
    
    
    [HttpGet("/{id}")]
    public async Task<IResult> GetAppointmentById(Guid id)
    {
        return await Handle(new GetAppointmentByIdQuery(id));
    }

    
    // GET: api/appointments/provider/{providerId}
    [HttpGet("provider/{providerId}")]
    public async Task<IResult> GetAppointmentsByProvider(Guid providerId)
    {
        return await Handle(new GetAppointmentsByProviderQuery(providerId));
    }
    
    
    
        
}