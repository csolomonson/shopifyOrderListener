using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;

namespace M1.API.Models.BOM.Job;

public interface IBOMJobModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	IDictionary<string, object> JobKeyDictionary { get; set; }

	Task<APIValidationInfoDto> ValidateRequest_GetJobGUIDs(string jobId, string partId);

	Task<APIValidationInfoDto> ValidateRequest_GetJobMethod(string jobId);

	Task<APIValidationInfoDto> ValidateRequest_GetJob(string jobId);

	/// <summary>
	/// Validates the request for posting a job.
	/// </summary>
	/// <param name="job">The job data transfer object (DTO) containing the details of the job to be validated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> with the validation information for the job request.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PostJob(CTMJobDto job);

	Task<BOMResponseMessageDto<BOMJobGuidsDto>> Process_GetJobGUIDs(string jobId, string partId);

	Task<BOMResponseMessageDto<CTMBOMJobMethodDto>> Process_GetJobMethod(string jobId);

	Task<BOMResponseMessageDto<IList<BOMJobDto>>> Process_GetAllJobs(int pageSize, int pageNumber);

	Task<BOMResponseMessageDto<BOMJobDto>> Process_GetJob(string jobId);

	/// <summary>
	/// Processes the posting of a job.
	/// </summary>
	/// <param name="job">The job data transfer object (DTO) containing the details of the job to be posted.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.BOM.BOMResponseMessageDto`1" /> with the response message and the job details.</returns>
	Task<BOMResponseMessageDto<CTMJobDto>> Process_PostJob(CTMJobDto job);
}
