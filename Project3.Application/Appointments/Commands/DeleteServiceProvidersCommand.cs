using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.ServiceProviders.Commands;

public sealed record DeleteServiceProviderCommand(Guid Id)
    : IRequest<Result>;

public sealed class DeleteServiceProviderHandler
    : IRequestHandler<DeleteServiceProviderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteServiceProviderHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteServiceProviderCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ServiceProviders.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Provider deleted successfully.");
    }
    
}