using Azure.Core;
using Microsoft.Identity.Client;
using SmartTodoAPI.Exceptions;
using SmartTodoAPI.Models;
using SmartTodoAPI.Responses;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartTodoAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next; //That "next middleware" is stored in RequestDelegate _next
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context) //Think of HttpContext as a container that holds everything about the current HTTP request and response.
        {
            try
            {
                await _next(context); //Without this line... The request stops and The controller is never reached.
                                      // Why await - If middleware didn't wait,it would continue immediately without waiting for the controller to finish. Means "Pause here until the remaining pipeline finishes, then continue."
            }
            catch (Exception ex) //Catch any object whose type is Exception or inherits from Exception, and store it in the variable ex.
            {
                Console.WriteLine(ex.StackTrace);
                await HandleExceptionAsync(context, ex);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception) //Why static? ..Because this method doesn't use:_next or any instance fields.
        //It only works with the parameters passed to it: A method that doesn't depend on object state is a good candidate for static.
        // HttpContext contains:Request,Response,User,Headers,Cookies,Items To return an HTTP response, we need:context.Response...That's why we pass the entire HttpContext.
        {
            //to convert that exception into a proper HTTP response
            context.Response.ContentType = "application/json"; //When your API sends a response, the client (Swagger, Postman, React, etc.) needs to know what type of data it's receiving.
            var statusCode = exception switch //How does the switch know it's a BadRequestException if the parameter type is Exception - This is called runtime type checking, and the switch expression checks the object's actual type.
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                //NotFoundException => StatusCodes.Status404NotFound,
                //UnauthorizedException => StatusCodes.Status401Unauthorized,
                //ForbiddenException => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };
            context.Response.StatusCode = statusCode; //This line tells the HTTP response: Send this status code back to the client.
            var response = new ApiResponse<object> //At this moment, it's not JSON.It's simply an object in memory.
            {
                Success = false,
                Message = exception.Message, //exception is not just a message.It's a complete object that contains a lot of information about what went wrong
                Data = null,
                Errors = null
            };
            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse); //This writes the JSON string into the HTTP response body
        }



    }
}
