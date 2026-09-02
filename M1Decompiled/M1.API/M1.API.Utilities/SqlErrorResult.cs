using System.Net;

namespace M1.API.Utilities;

public class SqlErrorResult
{
	public HttpStatusCode StatusCode { get; set; }

	public string ErrorDescription { get; set; }
}
