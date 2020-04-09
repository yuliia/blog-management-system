using System;
using System.Net;
using System.Threading.Tasks;
using BlogManagementSystem.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BlogManagementSystem.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
		
		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)	
		{
			try
			{
				await _next(context);
			}
			catch (PostNotFoundException)
			{
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int) HttpStatusCode.NotFound;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, context.Request.Path.HasValue ? context.Request.Path.Value : string.Empty);
				await HandleExceptionAsync(context, ex);
			}
		}
		
		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			await context.Response.WriteAsync(exception.Message);
		}
    }
}