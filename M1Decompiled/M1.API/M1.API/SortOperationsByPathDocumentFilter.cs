using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace M1.API;

public class SortOperationsByPathDocumentFilter : IDocumentFilter
{
	/// <summary>
	/// Applies an ordering to the paths based on the group name of the operation.
	/// </summary>
	public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
	{
		List<KeyValuePair<string, PathItem>> source = (from e in swaggerDoc.paths
			group e by GetGroupName(e.Value) into g
			orderby g.Key
			select g).SelectMany((IGrouping<string, KeyValuePair<string, PathItem>> g) => g).ToList();
		swaggerDoc.paths = source.ToDictionary((KeyValuePair<string, PathItem> e) => e.Key, (KeyValuePair<string, PathItem> e) => e.Value);
	}

	/// <summary>
	/// Gets the group name (controller name or group by name) for sorting purposes.
	/// </summary>
	private string GetGroupName(PathItem value)
	{
		if (value.get?.tags != null)
		{
			return value.get.tags[0];
		}
		if (value.delete?.tags != null)
		{
			return value.delete.tags[0];
		}
		if (value.patch?.tags != null)
		{
			return value.patch.tags[0];
		}
		if (value.post?.tags != null)
		{
			return value.post.tags[0];
		}
		if (value.put?.tags != null)
		{
			return value.put.tags[0];
		}
		return string.Empty;
	}
}
