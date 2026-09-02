using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace M1.API.Filters;

public class HelpAuthorizationHeaderParameterOperationFilter : IOperationFilter
{
	public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
	{
		operation.produces.Add("application/xml");
		operation.produces.Add("text/xml");
	}
}
