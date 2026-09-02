using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPPartRepository : APIBaseRepository, IERPPartRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartExist(Guid partId)
	{
		InitializeParameterLists();
		base.filterList.Add("impUniqueID|C", partId);
		base.selectList.Add("impUniqueID");
		return Task.FromResult(GetAsObject("Parts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartInformationDto>> GetAllParts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartInformationDto> collection = new List<ERPPartInformationDto>();
		InitializeParameterLists();
		string[] array = new string[30]
		{
			"impPartID", "impContractLength", "impContractLengthType", "impCreatedBy", "impCreatedDate", "impCycleCodeID", "impDeliveryType", "impUniqueID", "impInactiveDate", "impInactive",
			"impAlwaysNonTaxable", "impBuyForInventory", "impNonPhysicalShipment", "impNonStockedItem", "impPhantomOrKitPart", "impTrackLotNumbers", "impTrackSerialNumbers", "impLongDescriptionRtf", "impLongDescriptionText", "impNextSerialNumberIDFormula",
			"impNonTaxReasonID", "impOEMOrganizationID", "impPartClassID", "impPartGroupID", "impPartType", "impReorderMethod", "impRowVersion", "impSecondTaxCodeID", "impShortDescription", "impTaxCodeID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Parts");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("Parts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartInformationDto eRPPartInformationDto = new ERPPartInformationDto();
				eRPPartInformationDto.impPartID = dataTable.Rows[i].Field<string>("impPartID");
				eRPPartInformationDto.impContractLength = dataTable.Rows[i].Field<short>("impContractLength");
				eRPPartInformationDto.impContractLengthType = dataTable.Rows[i].Field<string>("impContractLengthType");
				eRPPartInformationDto.impCreatedBy = dataTable.Rows[i].Field<string>("impCreatedBy");
				eRPPartInformationDto.impCreatedDate = dataTable.Rows[i].Field<DateTime?>("impCreatedDate");
				eRPPartInformationDto.impCycleCodeID = dataTable.Rows[i].Field<string>("impCycleCodeID");
				eRPPartInformationDto.impDeliveryType = dataTable.Rows[i].Field<byte>("impDeliveryType");
				eRPPartInformationDto.impUniqueID = dataTable.Rows[i].Field<Guid>("impUniqueID");
				eRPPartInformationDto.impInactiveDate = dataTable.Rows[i].Field<DateTime?>("impInactiveDate");
				eRPPartInformationDto.impInactive = dataTable.Rows[i].Field<bool>("impInactive");
				eRPPartInformationDto.impAlwaysNonTaxable = dataTable.Rows[i].Field<bool>("impAlwaysNonTaxable");
				eRPPartInformationDto.impBuyForInventory = dataTable.Rows[i].Field<bool>("impBuyForInventory");
				eRPPartInformationDto.impNonPhysicalShipment = dataTable.Rows[i].Field<bool>("impNonPhysicalShipment");
				eRPPartInformationDto.impNonStockedItem = dataTable.Rows[i].Field<bool>("impNonStockedItem");
				eRPPartInformationDto.impPhantomOrKitPart = dataTable.Rows[i].Field<bool>("impPhantomOrKitPart");
				eRPPartInformationDto.impTrackLotNumbers = dataTable.Rows[i].Field<bool>("impTrackLotNumbers");
				eRPPartInformationDto.impTrackSerialNumbers = dataTable.Rows[i].Field<bool>("impTrackSerialNumbers");
				eRPPartInformationDto.impLongDescriptionRtf = dataTable.Rows[i].Field<string>("impLongDescriptionRtf");
				eRPPartInformationDto.impLongDescriptionText = dataTable.Rows[i].Field<string>("impLongDescriptionText");
				eRPPartInformationDto.impNextSerialNumberIDFormula = dataTable.Rows[i].Field<string>("impNextSerialNumberIDFormula");
				eRPPartInformationDto.impNonTaxReasonID = dataTable.Rows[i].Field<string>("impNonTaxReasonID");
				eRPPartInformationDto.impOEMOrganizationID = dataTable.Rows[i].Field<string>("impOEMOrganizationID");
				eRPPartInformationDto.impPartClassID = dataTable.Rows[i].Field<string>("impPartClassID");
				eRPPartInformationDto.impPartGroupID = dataTable.Rows[i].Field<string>("impPartGroupID");
				eRPPartInformationDto.impPartType = dataTable.Rows[i].Field<byte>("impPartType");
				eRPPartInformationDto.impReorderMethod = dataTable.Rows[i].Field<byte>("impReorderMethod");
				eRPPartInformationDto.impRowVersion = dataTable.Rows[i].Field<byte[]>("impRowVersion");
				eRPPartInformationDto.impSecondTaxCodeID = dataTable.Rows[i].Field<string>("impSecondTaxCodeID");
				eRPPartInformationDto.impShortDescription = dataTable.Rows[i].Field<string>("impShortDescription");
				eRPPartInformationDto.impTaxCodeID = dataTable.Rows[i].Field<string>("impTaxCodeID");
				eRPPartInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartInformationDto> GetPart(Guid partId)
	{
		ERPPartInformationDto eRPPartInformationDto = new ERPPartInformationDto();
		InitializeParameterLists();
		string[] collection = new string[30]
		{
			"impPartID", "impContractLength", "impContractLengthType", "impCreatedBy", "impCreatedDate", "impCycleCodeID", "impDeliveryType", "impUniqueID", "impInactiveDate", "impInactive",
			"impAlwaysNonTaxable", "impBuyForInventory", "impNonPhysicalShipment", "impNonStockedItem", "impPhantomOrKitPart", "impTrackLotNumbers", "impTrackSerialNumbers", "impLongDescriptionRtf", "impLongDescriptionText", "impNextSerialNumberIDFormula",
			"impNonTaxReasonID", "impOEMOrganizationID", "impPartClassID", "impPartGroupID", "impPartType", "impReorderMethod", "impRowVersion", "impSecondTaxCodeID", "impShortDescription", "impTaxCodeID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("impUniqueID|C", partId);
		AddCustomFieldsToSelectList("Parts");
		using (DataTable dataTable = GetAsDataTable("Parts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartInformationDto);
			}
			eRPPartInformationDto.impPartID = dataTable.Rows[0].Field<string>("impPartID");
			eRPPartInformationDto.impContractLength = dataTable.Rows[0].Field<short>("impContractLength");
			eRPPartInformationDto.impContractLengthType = dataTable.Rows[0].Field<string>("impContractLengthType");
			eRPPartInformationDto.impCreatedBy = dataTable.Rows[0].Field<string>("impCreatedBy");
			eRPPartInformationDto.impCreatedDate = dataTable.Rows[0].Field<DateTime?>("impCreatedDate");
			eRPPartInformationDto.impCycleCodeID = dataTable.Rows[0].Field<string>("impCycleCodeID");
			eRPPartInformationDto.impDeliveryType = dataTable.Rows[0].Field<byte>("impDeliveryType");
			eRPPartInformationDto.impUniqueID = dataTable.Rows[0].Field<Guid>("impUniqueID");
			eRPPartInformationDto.impInactiveDate = dataTable.Rows[0].Field<DateTime?>("impInactiveDate");
			eRPPartInformationDto.impInactive = dataTable.Rows[0].Field<bool>("impInactive");
			eRPPartInformationDto.impAlwaysNonTaxable = dataTable.Rows[0].Field<bool>("impAlwaysNonTaxable");
			eRPPartInformationDto.impBuyForInventory = dataTable.Rows[0].Field<bool>("impBuyForInventory");
			eRPPartInformationDto.impNonPhysicalShipment = dataTable.Rows[0].Field<bool>("impNonPhysicalShipment");
			eRPPartInformationDto.impNonStockedItem = dataTable.Rows[0].Field<bool>("impNonStockedItem");
			eRPPartInformationDto.impPhantomOrKitPart = dataTable.Rows[0].Field<bool>("impPhantomOrKitPart");
			eRPPartInformationDto.impTrackLotNumbers = dataTable.Rows[0].Field<bool>("impTrackLotNumbers");
			eRPPartInformationDto.impTrackSerialNumbers = dataTable.Rows[0].Field<bool>("impTrackSerialNumbers");
			eRPPartInformationDto.impLongDescriptionRtf = dataTable.Rows[0].Field<string>("impLongDescriptionRtf");
			eRPPartInformationDto.impLongDescriptionText = dataTable.Rows[0].Field<string>("impLongDescriptionText");
			eRPPartInformationDto.impNextSerialNumberIDFormula = dataTable.Rows[0].Field<string>("impNextSerialNumberIDFormula");
			eRPPartInformationDto.impNonTaxReasonID = dataTable.Rows[0].Field<string>("impNonTaxReasonID");
			eRPPartInformationDto.impOEMOrganizationID = dataTable.Rows[0].Field<string>("impOEMOrganizationID");
			eRPPartInformationDto.impPartClassID = dataTable.Rows[0].Field<string>("impPartClassID");
			eRPPartInformationDto.impPartGroupID = dataTable.Rows[0].Field<string>("impPartGroupID");
			eRPPartInformationDto.impPartType = dataTable.Rows[0].Field<byte>("impPartType");
			eRPPartInformationDto.impReorderMethod = dataTable.Rows[0].Field<byte>("impReorderMethod");
			eRPPartInformationDto.impRowVersion = dataTable.Rows[0].Field<byte[]>("impRowVersion");
			eRPPartInformationDto.impSecondTaxCodeID = dataTable.Rows[0].Field<string>("impSecondTaxCodeID");
			eRPPartInformationDto.impShortDescription = dataTable.Rows[0].Field<string>("impShortDescription");
			eRPPartInformationDto.impTaxCodeID = dataTable.Rows[0].Field<string>("impTaxCodeID");
			eRPPartInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartInformationDto);
	}

	public Task<APIValidationInfoDto> SavePart(ERPPartDto part)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Parts WHERE impUniqueID = " + M1Util.ConvertToLinq(part.impUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["impPartID"] = part.impPartID.ToUpper();
				part.impUniqueID = ((part.impUniqueID == Guid.Empty) ? Guid.NewGuid() : part.impUniqueID);
				dataRow["impUniqueID"] = part.impUniqueID;
				dataRow["impCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["impCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Part could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (part.impRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Part is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["impRowVersion"], part.impRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Part has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Part again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["impContractLength"] = part.impContractLength;
			dataRow["impContractLengthType"] = part.impContractLengthType;
			dataRow["impCycleCodeID"] = part.impCycleCodeID;
			dataRow["impDeliveryType"] = part.impDeliveryType;
			DataRow dataRow2 = dataRow;
			DateTime? impInactiveDate = part.impInactiveDate;
			dataRow2["impInactiveDate"] = (impInactiveDate.HasValue ? ((object)impInactiveDate.GetValueOrDefault()) : dataRow["impInactiveDate"]);
			dataRow["impInactive"] = part.impInactive;
			dataRow["impAlwaysNonTaxable"] = part.impAlwaysNonTaxable;
			dataRow["impBuyForInventory"] = part.impBuyForInventory;
			dataRow["impNonPhysicalShipment"] = part.impNonPhysicalShipment;
			dataRow["impNonStockedItem"] = part.impNonStockedItem;
			dataRow["impPhantomOrKitPart"] = part.impPhantomOrKitPart;
			dataRow["impTrackLotNumbers"] = part.impTrackLotNumbers;
			dataRow["impTrackSerialNumbers"] = part.impTrackSerialNumbers;
			dataRow["impLongDescriptionRtf"] = part.impLongDescriptionRtf ?? dataRow["impLongDescriptionRtf"];
			dataRow["impLongDescriptionText"] = part.impLongDescriptionText ?? dataRow["impLongDescriptionText"];
			dataRow["impNextSerialNumberIDFormula"] = part.impNextSerialNumberIDFormula ?? dataRow["impNextSerialNumberIDFormula"];
			dataRow["impNonTaxReasonID"] = part.impNonTaxReasonID;
			dataRow["impOEMOrganizationID"] = part.impOEMOrganizationID;
			dataRow["impPartClassID"] = part.impPartClassID;
			dataRow["impPartGroupID"] = part.impPartGroupID;
			dataRow["impPartType"] = part.impPartType;
			dataRow["impReorderMethod"] = part.impReorderMethod;
			dataRow["impSecondTaxCodeID"] = part.impSecondTaxCodeID;
			dataRow["impShortDescription"] = part.impShortDescription;
			dataRow["impTaxCodeID"] = part.impTaxCodeID;
			if (part.CustomFields != null && part.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in part.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Part [{part.impUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Part [{part.impUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
