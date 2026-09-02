using System;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.Repositories;
using M1.API.Utilities;

namespace M1.API.Models.Core;

public interface IAPIClientModel : IAPIBaseModel, IDisposable
{
	APIClientRepository clientRepository { get; set; }

	APIEnums.WebAPIModules ApiModuleId { get; set; }

	Task<APIClientContext> CreateApiDataClientAsync(APISessionDto apiSession, APIEnums.WebAPIModules module);

	Task<APISessionDto> GetDatabaseRelatedInfoFromDDAPIInfoAsync(APIMetadataDto apiMetadata);

	Task<bool> FillAPIKeyStoreAsync(string moduleId, string apiID);

	Task<bool> DisposeApiDataClientAsync(APIClientContext clientContextDto);
}
