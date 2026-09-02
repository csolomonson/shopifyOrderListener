using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;
using M1.API.Models.EDI;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Controllers;

public static class ResponseMessageBuilderFunctions
{
	public enum ResponseContentHeaderStatus
	{
		Error,
		Success,
		SuccessWithWarnings,
		ErrorsAndWarnings,
		Info
	}

	public static APIResponseMessageDto BuildResponseObject(string error, string warning, string additionalMessage, string payLoadId, ResponseContentHeaderStatus headerStatus)
	{
		List<string> errors = new List<string> { error };
		List<string> warnings = new List<string> { warning };
		_ = string.Empty;
		return BuildResponseObject(errors, warnings, additionalMessage, payLoadId, headerStatus);
	}

	public static APIResponseMessageDto BuildResponseObject(IList<string> errors, IList<string> warnings, string additionalMessage, string payLoadId, ResponseContentHeaderStatus headerStatus)
	{
		APIResponseMessageDto aPIResponseMessageDto = new APIResponseMessageDto();
		List<Error> collection = new List<Error>();
		List<Warning> collection2 = new List<Warning>();
		aPIResponseMessageDto.Status = headerStatus.ToString();
		aPIResponseMessageDto.PayloadID = payLoadId;
		aPIResponseMessageDto.ResponseID = 1;
		if (errors != null)
		{
			collection = errors.Select((string response) => new Error
			{
				ResponseItem = response.ToString()
			}).ToList();
		}
		if (warnings != null)
		{
			collection2 = warnings.Select((string response) => new Warning
			{
				ResponseItem = response.ToString()
			}).ToList();
		}
		if (headerStatus.Equals(ResponseContentHeaderStatus.Error))
		{
			aPIResponseMessageDto.Errors.Clear();
			aPIResponseMessageDto.Errors.AddRange(new List<Error>(collection));
			if (!string.IsNullOrWhiteSpace(additionalMessage))
			{
				aPIResponseMessageDto.Description = additionalMessage.Trim();
			}
		}
		if (headerStatus.Equals(ResponseContentHeaderStatus.SuccessWithWarnings))
		{
			aPIResponseMessageDto.Warnings.Clear();
			aPIResponseMessageDto.Warnings.AddRange(new List<Warning>(collection2));
			if (!string.IsNullOrWhiteSpace(additionalMessage))
			{
				aPIResponseMessageDto.Description = additionalMessage.Trim();
			}
		}
		if (headerStatus.Equals(ResponseContentHeaderStatus.ErrorsAndWarnings))
		{
			aPIResponseMessageDto.Errors.Clear();
			aPIResponseMessageDto.Warnings.Clear();
			aPIResponseMessageDto.Errors.AddRange(new List<Error>(collection));
			aPIResponseMessageDto.Warnings.AddRange(new List<Warning>(collection2));
		}
		else if (headerStatus.Equals(ResponseContentHeaderStatus.Success))
		{
			if (!string.IsNullOrWhiteSpace(additionalMessage))
			{
				aPIResponseMessageDto.Description = additionalMessage.Trim();
			}
			else
			{
				aPIResponseMessageDto.Description = ((errors == null || errors.Count == 0) ? string.Empty : errors[0].ToString());
			}
		}
		return aPIResponseMessageDto;
	}

	public static EDIOrderResponseMessageDto BuildResponseOblect(List<string> errors, List<string> warnings, PostOrderResponseDto postOrderResponseDto, string additionalMessage, string payLoadId)
	{
		EDIOrderResponseMessageDto eDIOrderResponseMessageDto = new EDIOrderResponseMessageDto();
		List<Error> errors2 = new List<Error>();
		List<Warning> warnings2 = new List<Warning>();
		List<EDISalesOrderResponseItemDto> eDIOrderResponses = new List<EDISalesOrderResponseItemDto>();
		eDIOrderResponseMessageDto.PayloadID = payLoadId;
		eDIOrderResponseMessageDto.ResponseID = 1;
		if (postOrderResponseDto != null)
		{
			eDIOrderResponses = postOrderResponseDto.M1OrderCollection.Select((CTMSalesOrderDto response) => new EDISalesOrderResponseItemDto
			{
				SalesOrderID = response.EDIOrderID,
				Status = (response.DoesOrderCreated ? APIEnums.APISalesOrderProcessingStatus.Created.ToString() : (response.DoesRequestValidated ? APIEnums.APISalesOrderProcessingStatus.Validated.ToString() : (response.DoesRequestProcessed ? APIEnums.APISalesOrderProcessingStatus.Processed.ToString() : APIEnums.APISalesOrderProcessingStatus.Failed.ToString()))),
				CustomerPO = response.M1SalesOrder.CustomerPO,
				M1SalesOrderID = (response.DoesOrderCreated ? response.M1SalesOrder.SalesOrderID : string.Empty),
				Warnings = response.M1SalesOrderValidatationInfo.WarningsList.Select((string x) => new Warning
				{
					ResponseItem = x
				}).ToList(),
				Errors = response.M1SalesOrderValidatationInfo.ErrorsList.Select((string x) => new Error
				{
					ResponseItem = x
				}).ToList()
			}).ToList();
		}
		if (errors != null)
		{
			errors2 = errors.Select((string response) => new Error
			{
				ResponseItem = response.ToString()
			}).ToList();
		}
		if (warnings != null)
		{
			warnings2 = warnings.Select((string response) => new Warning
			{
				ResponseItem = response.ToString()
			}).ToList();
		}
		eDIOrderResponseMessageDto.Errors = errors2;
		eDIOrderResponseMessageDto.Warnings = warnings2;
		eDIOrderResponseMessageDto.EDIOrderResponses = eDIOrderResponses;
		eDIOrderResponseMessageDto.TotalOrders = (postOrderResponseDto?.M1OrderCollection.Count()).Value;
		eDIOrderResponseMessageDto.OrdersCreated = (from x in postOrderResponseDto.M1OrderCollection.ToList()
			where x.DoesOrderCreated
			select x).Count();
		if (!string.IsNullOrWhiteSpace(additionalMessage))
		{
			eDIOrderResponseMessageDto.Description = additionalMessage.Trim();
		}
		return eDIOrderResponseMessageDto;
	}

	public static string GetCustomHttpResponse(string msgString, HttpStatusCode statusCode, ResponseContentHeaderStatus contentHeaderStatus)
	{
		List<string> errors = new List<string> { msgString };
		_ = string.Empty;
		new XmlDocument();
		return XMLSerializer.SerializaObjectToXMLDocument(BuildResponseObject(errors, null, string.Empty, "", contentHeaderStatus)).InnerXml;
	}
}
