using M1.API.Utilities;

namespace M1.API.Models.Core;

public class APIEDIClientModel : APIClientModel
{
	public APIEDIClientModel()
	{
		base.ApiModuleId = APIEnums.WebAPIModules.EDI;
	}
}
