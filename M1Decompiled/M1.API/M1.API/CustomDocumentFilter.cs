using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace M1.API;

public class CustomDocumentFilter : IDocumentFilter
{
	public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
	{
		swaggerDoc.securityDefinitions.Add("amx", new SecurityScheme
		{
			name = "Authorization",
			@in = "header",
			description = "Hawk Access Token",
			type = "apiKey"
		});
		swaggerDoc.security = new List<IDictionary<string, IEnumerable<string>>>
		{
			new Dictionary<string, IEnumerable<string>>
			{
				{
					"apikey",
					new string[0]
				},
				{
					"amx",
					new string[0]
				}
			}
		};
	}
}
