using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentApi.Services.Services;

namespace PaymentApi.Api.Controllers.Extensions
{
	public static class Extensions
	{
		public static IActionResult GetActionResultFromServiceResult(this ControllerBase controller, ServiceResult result)
		{
			switch (result.StatusCode)
			{
				case StatusCodes.Status200OK:
					{
						return controller.Ok(result.ContentResult);
					}
				case StatusCodes.Status201Created:
					{
						return controller.StatusCode(StatusCodes.Status201Created, result.ContentResult);
					}
				case StatusCodes.Status400BadRequest:
					{
						return controller.BadRequest(result.ContentResult);
					}
				case StatusCodes.Status404NotFound:
					{
						return controller.NotFound(result.ContentResult);
					}
				case StatusCodes.Status500InternalServerError:
					{
						return controller.StatusCode(StatusCodes.Status500InternalServerError, result.ContentResult);
					}
				default:
					{
						return controller.StatusCode(result.StatusCode, "Unhandled Status Code");
					}
			}
		}
	}
}