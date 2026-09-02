using M1.API.Utilities;

namespace M1.API.Models.Core;

public class APIBOMClientModel : APIClientModel
{
	public APIBOMClientModel()
	{
		base.ApiModuleId = APIEnums.WebAPIModules.BOM;
	}
}
