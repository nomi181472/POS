using BS.CustomExceptions.Common;
using BS.CustomExceptions.CustomExceptionMessage;
using BS.Services.TillManagementService;
using BS.Services.TillManagementService.Request;
using BS.Services.TillManagementService.Response;
using FluentValidation;
using Logger;
using PaymentGateway.API.Common;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Till.Common;

namespace Till.Feature.TillManagement
{
    public class ListAllTills : ITillFeature
    {
        public static void Map(IEndpointRouteBuilder app) => app
            .MapPost($"/{nameof(ListAllTills)}", Handle)
            .WithSummary("List all tills")
            .Produces(200)
            .Produces(201)
            .Produces(400)
            .Produces(404)
            .Produces(500)
            .Produces<List<ListAllTillsResponse>>();

        private static async Task<IResult> Handle(ITillManagementService till, ICustomLogger _logger, CancellationToken cancellationToken)
        {
            int statusCode = HTTPStatusCode200.Created;
            string message = "Success";
            List<ListAllTillsResponse> response = new List<ListAllTillsResponse>();
            try
            {
                response = await till.ListAllTills(cancellationToken);
                return ApiResponseHelper.Convert(true, true, message, statusCode, response);
            }
            catch (RecordNotFoundException ex)
            {
                statusCode = HTTPStatusCode400.NotFound;
                message = ex.Message;
                _logger.LogError(message, ex);
                return ApiResponseHelper.Convert(false, false, message, statusCode, response);
            }
            catch (ArgumentNullException ex)
            {
                statusCode = HTTPStatusCode400.BadRequest;
                message = ex.Message;
                _logger.LogError(message, ex);
                return ApiResponseHelper.Convert(false, false, message, statusCode, response);
            }
            catch (Exception e)
            {
                statusCode = HTTPStatusCode500.InternalServerError;
                message = ExceptionMessage.SWW;
                _logger.LogError(message, e);
                return ApiResponseHelper.Convert(false, false, message, statusCode, response);
            }
        }
    }
}
