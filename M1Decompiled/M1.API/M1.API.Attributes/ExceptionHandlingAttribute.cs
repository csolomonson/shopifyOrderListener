using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Filters;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using WebApiContrib.Messages;

namespace M1.API.Attributes;

public class ExceptionHandlingAttribute : ExceptionFilterAttribute
{
	public IDictionary<Type, HttpStatusCode> Mappings { get; private set; }

	public ExceptionHandlingAttribute()
	{
		Mappings = new Dictionary<Type, HttpStatusCode>
		{
			{
				typeof(ArgumentNullException),
				HttpStatusCode.BadRequest
			},
			{
				typeof(ArgumentException),
				HttpStatusCode.BadRequest
			}
		};
	}

	public override void OnException(HttpActionExecutedContext actionExecutedContext)
	{
		if (actionExecutedContext?.Exception != null)
		{
			HttpRequestMessage request = actionExecutedContext.Request;
			Exception exception = actionExecutedContext.Exception;
			MediaTypeHeaderValue contentType = request.Content.Headers.ContentType;
			string mediaType = ((contentType != null) ? contentType.MediaType : null) ?? "application/xml";
			if (actionExecutedContext.Exception is HttpException)
			{
				HttpException ex = (HttpException)exception;
				actionExecutedContext.Response = request.CreateResponse((HttpStatusCode)ex.GetHttpCode(), new WebApiContrib.Messages.Error
				{
					Message = exception.Message
				});
			}
			else if (Mappings.ContainsKey(exception.GetType()))
			{
				HttpStatusCode statusCode = Mappings[exception.GetType()];
				actionExecutedContext.Response = request.CreateResponse(statusCode, new WebApiContrib.Messages.Error
				{
					Message = exception.Message
				});
			}
			else
			{
				APIResponseMessageDto value = ResponseMessageBuilderFunctions.BuildResponseObject(exception.Message + " - " + exception.InnerException?.Message, string.Empty, string.Empty, string.Empty, ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Error);
				actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, value, mediaType);
			}
		}
	}
}
