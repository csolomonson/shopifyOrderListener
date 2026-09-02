using System.Collections.Generic;

namespace M1.Core;

public class QueryParseResult
{
	public Dictionary<string, string> ExternalLinks;

	public List<QueryParseError> Errors;

	public string PrimaryTable;

	public string UniqueRowFilter;

	public string Top;

	public string Fields;

	public string Into;

	public string From;

	public string Where;

	public string OrderBy;

	public string GroupBy;

	public string Having;
}
