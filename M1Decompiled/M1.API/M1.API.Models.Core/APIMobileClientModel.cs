using System.Threading.Tasks;
using M1.API.Utilities;

namespace M1.API.Models.Core;

public class APIMobileClientModel : APIClientModel
{
	public Task CreateWebSession(APIClientContext clientContextDto)
	{
		base.clientRepository.InsertWebSession(clientContextDto, APIEnums.WebAPIModules.SFE);
		return Task.FromResult(0);
	}
}
