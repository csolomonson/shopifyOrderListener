using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.Utilities;

namespace M1.API.Models.Core;

public class APIERPClientModel : APIClientModel
{
	public APIERPClientModel()
	{
		base.ApiModuleId = APIEnums.WebAPIModules.ERP;
	}

	public override Task<APISessionDto> GetDatabaseRelatedInfoFromDDAPIInfoAsync(APIMetadataDto apiMetadata)
	{
		return Task.FromResult(new APISessionDto
		{
			APIID = apiMetadata.APIID,
			DatabaseID = apiMetadata.DatabaseId,
			DatadictionaryID = apiMetadata.DataDictionaryID,
			APIUserID = apiMetadata.AdminUserID,
			APIUserPassword = apiMetadata.AdminPassword,
			Authenticated = true,
			Server = apiMetadata.Server,
			NetworkLibrary = apiMetadata.NetworkLibrary,
			SQLUserID = apiMetadata.SqlUserID,
			SQLUserPassword = apiMetadata.SqlPassword,
			TrustedConnection = apiMetadata.TrustedConnection,
			IsReadOnly = apiMetadata.IsReadOnly
		});
	}
}
