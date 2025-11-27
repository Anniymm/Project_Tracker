using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project3.Api.Utilities;
using Project3.Domain.Common.Response;

namespace Project3.Api.Controllers;


[ApiController]
public abstract class ApiController(ISender _sender) : ControllerBase
{
    // Queries/Data Retrieval. Sends a MediatR Query and expects a specific data type ($TResponse$) back.
    protected async Task<IResult> Handle<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest<Result<TResponse>>
    {
        var result = await _sender.Send(request);
        return result.ToResult();
    }

    // Commands/Action Execution. Sends a MediatR Command for operations (Create, Update, Delete) that don't return specific data.
    protected async Task<IResult> Handle<TRequest>(TRequest request)
        where TRequest : IRequest<Result>
    {
        var result = await _sender.Send(request);
        return result.ToResult();
    }
    
    // File Retrieval. Sends a request that is expected to return a file's content as a byte array.
    // protected async Task<IResult> HandleFile<TRequest>(TRequest request, string contentType, string fileName)
    //     where TRequest : IRequest<Result<byte[]>>
    // {
    //     var result = await _sender.Send(request);
    //     return result.ToFileResult(contentType, fileName);
    // }
}