using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Appointments.Commands;

public sealed record DeleteWorkingHoursCommand(Guid Id)
    : IRequest<Result>;

public sealed class DeleteWorkingHoursHandler
    : IRequestHandler<DeleteWorkingHoursCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteWorkingHoursHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteWorkingHoursCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.WorkingHours.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync((cancellationToken));
        
        return Result.Success("Working hours deleted successfully");
    }
}