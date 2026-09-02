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

public class ERPPartGroupRepository : APIBaseRepository, IERPPartGroupRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartGroupRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartGroupExist(Guid partGroupId)
	{
		InitializeParameterLists();
		base.filterList.Add("imuUniqueID|C", partGroupId);
		base.selectList.Add("imuUniqueID");
		return Task.FromResult(GetAsObject("PartGroups", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartGroupInformationDto>> GetAllPartGroups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartGroupInformationDto> collection = new List<ERPPartGroupInformationDto>();
		InitializeParameterLists();
		string[] array = new string[31]
		{
			"imuArDepositGlAccountID", "imuAvalaraTaxCodeID", "imuPartGroupID", "imuCogsLaborGlAccountID", "imuCogsMaterialGlAccountID", "imuCogsOverheadGlAccountID", "imuCogsSubcontractGlAccountID", "imuCommissionRate", "imuCommissionType", "imuCreatedBy",
			"imuCreatedDate", "imuDescription", "imuDiscountGlAccountID", "imuUniqueID", "imuInactiveDate", "imuInactive", "imuNextSerialNumberIDFormula", "imuNextSerialNumberOption", "imuNextSerialNumberValue", "imuParentPartGroupID",
			"imuPartImageFileName", "imuQmLaborMarkup", "imuQmMarkupOption", "imuQmMaterialMarkup", "imuQmOverHeadMarkup", "imuQmPurchaseToOrderMarkup", "imuQmQuoteMarkupType", "imuQmQuotingMarkup", "imuQmSubcontractMarkup", "imuRowVersion",
			"imuSalesGlAccountID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartGroups");
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
		using (DataTable dataTable = GetAsDataTable("PartGroups", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartGroupInformationDto eRPPartGroupInformationDto = new ERPPartGroupInformationDto();
				eRPPartGroupInformationDto.imuArDepositGlAccountID = dataTable.Rows[i].Field<string>("imuArDepositGlAccountID");
				eRPPartGroupInformationDto.imuAvalaraTaxCodeID = dataTable.Rows[i].Field<string>("imuAvalaraTaxCodeID");
				eRPPartGroupInformationDto.imuPartGroupID = dataTable.Rows[i].Field<string>("imuPartGroupID");
				eRPPartGroupInformationDto.imuCogsLaborGlAccountID = dataTable.Rows[i].Field<string>("imuCogsLaborGlAccountID");
				eRPPartGroupInformationDto.imuCogsMaterialGlAccountID = dataTable.Rows[i].Field<string>("imuCogsMaterialGlAccountID");
				eRPPartGroupInformationDto.imuCogsOverheadGlAccountID = dataTable.Rows[i].Field<string>("imuCogsOverheadGlAccountID");
				eRPPartGroupInformationDto.imuCogsSubcontractGlAccountID = dataTable.Rows[i].Field<string>("imuCogsSubcontractGlAccountID");
				eRPPartGroupInformationDto.imuCommissionRate = dataTable.Rows[i].Field<decimal>("imuCommissionRate");
				eRPPartGroupInformationDto.imuCommissionType = dataTable.Rows[i].Field<byte>("imuCommissionType");
				eRPPartGroupInformationDto.imuCreatedBy = dataTable.Rows[i].Field<string>("imuCreatedBy");
				eRPPartGroupInformationDto.imuCreatedDate = dataTable.Rows[i].Field<DateTime?>("imuCreatedDate");
				eRPPartGroupInformationDto.imuDescription = dataTable.Rows[i].Field<string>("imuDescription");
				eRPPartGroupInformationDto.imuDiscountGlAccountID = dataTable.Rows[i].Field<string>("imuDiscountGlAccountID");
				eRPPartGroupInformationDto.imuUniqueID = dataTable.Rows[i].Field<Guid>("imuUniqueID");
				eRPPartGroupInformationDto.imuInactiveDate = dataTable.Rows[i].Field<DateTime?>("imuInactiveDate");
				eRPPartGroupInformationDto.imuInactive = dataTable.Rows[i].Field<bool>("imuInactive");
				eRPPartGroupInformationDto.imuNextSerialNumberIDFormula = dataTable.Rows[i].Field<string>("imuNextSerialNumberIDFormula");
				eRPPartGroupInformationDto.imuNextSerialNumberOption = dataTable.Rows[i].Field<byte>("imuNextSerialNumberOption");
				eRPPartGroupInformationDto.imuNextSerialNumberValue = dataTable.Rows[i].Field<string>("imuNextSerialNumberValue");
				eRPPartGroupInformationDto.imuParentPartGroupID = dataTable.Rows[i].Field<string>("imuParentPartGroupID");
				eRPPartGroupInformationDto.imuPartImageFileName = dataTable.Rows[i].Field<string>("imuPartImageFileName");
				eRPPartGroupInformationDto.imuQmLaborMarkup = dataTable.Rows[i].Field<decimal>("imuQmLaborMarkup");
				eRPPartGroupInformationDto.imuQmMarkupOption = dataTable.Rows[i].Field<byte>("imuQmMarkupOption");
				eRPPartGroupInformationDto.imuQmMaterialMarkup = dataTable.Rows[i].Field<decimal>("imuQmMaterialMarkup");
				eRPPartGroupInformationDto.imuQmOverHeadMarkup = dataTable.Rows[i].Field<decimal>("imuQmOverHeadMarkup");
				eRPPartGroupInformationDto.imuQmPurchaseToOrderMarkup = dataTable.Rows[i].Field<decimal>("imuQmPurchaseToOrderMarkup");
				eRPPartGroupInformationDto.imuQmQuoteMarkupType = dataTable.Rows[i].Field<byte>("imuQmQuoteMarkupType");
				eRPPartGroupInformationDto.imuQmQuotingMarkup = dataTable.Rows[i].Field<decimal>("imuQmQuotingMarkup");
				eRPPartGroupInformationDto.imuQmSubcontractMarkup = dataTable.Rows[i].Field<decimal>("imuQmSubcontractMarkup");
				eRPPartGroupInformationDto.imuRowVersion = dataTable.Rows[i].Field<byte[]>("imuRowVersion");
				eRPPartGroupInformationDto.imuSalesGlAccountID = dataTable.Rows[i].Field<string>("imuSalesGlAccountID");
				eRPPartGroupInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartGroupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartGroupInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartGroupInformationDto> GetPartGroup(Guid partGroupId)
	{
		ERPPartGroupInformationDto eRPPartGroupInformationDto = new ERPPartGroupInformationDto();
		InitializeParameterLists();
		string[] collection = new string[31]
		{
			"imuArDepositGlAccountID", "imuAvalaraTaxCodeID", "imuPartGroupID", "imuCogsLaborGlAccountID", "imuCogsMaterialGlAccountID", "imuCogsOverheadGlAccountID", "imuCogsSubcontractGlAccountID", "imuCommissionRate", "imuCommissionType", "imuCreatedBy",
			"imuCreatedDate", "imuDescription", "imuDiscountGlAccountID", "imuUniqueID", "imuInactiveDate", "imuInactive", "imuNextSerialNumberIDFormula", "imuNextSerialNumberOption", "imuNextSerialNumberValue", "imuParentPartGroupID",
			"imuPartImageFileName", "imuQmLaborMarkup", "imuQmMarkupOption", "imuQmMaterialMarkup", "imuQmOverHeadMarkup", "imuQmPurchaseToOrderMarkup", "imuQmQuoteMarkupType", "imuQmQuotingMarkup", "imuQmSubcontractMarkup", "imuRowVersion",
			"imuSalesGlAccountID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imuUniqueID|C", partGroupId);
		AddCustomFieldsToSelectList("PartGroups");
		using (DataTable dataTable = GetAsDataTable("PartGroups", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartGroupInformationDto);
			}
			eRPPartGroupInformationDto.imuArDepositGlAccountID = dataTable.Rows[0].Field<string>("imuArDepositGlAccountID");
			eRPPartGroupInformationDto.imuAvalaraTaxCodeID = dataTable.Rows[0].Field<string>("imuAvalaraTaxCodeID");
			eRPPartGroupInformationDto.imuPartGroupID = dataTable.Rows[0].Field<string>("imuPartGroupID");
			eRPPartGroupInformationDto.imuCogsLaborGlAccountID = dataTable.Rows[0].Field<string>("imuCogsLaborGlAccountID");
			eRPPartGroupInformationDto.imuCogsMaterialGlAccountID = dataTable.Rows[0].Field<string>("imuCogsMaterialGlAccountID");
			eRPPartGroupInformationDto.imuCogsOverheadGlAccountID = dataTable.Rows[0].Field<string>("imuCogsOverheadGlAccountID");
			eRPPartGroupInformationDto.imuCogsSubcontractGlAccountID = dataTable.Rows[0].Field<string>("imuCogsSubcontractGlAccountID");
			eRPPartGroupInformationDto.imuCommissionRate = dataTable.Rows[0].Field<decimal>("imuCommissionRate");
			eRPPartGroupInformationDto.imuCommissionType = dataTable.Rows[0].Field<byte>("imuCommissionType");
			eRPPartGroupInformationDto.imuCreatedBy = dataTable.Rows[0].Field<string>("imuCreatedBy");
			eRPPartGroupInformationDto.imuCreatedDate = dataTable.Rows[0].Field<DateTime?>("imuCreatedDate");
			eRPPartGroupInformationDto.imuDescription = dataTable.Rows[0].Field<string>("imuDescription");
			eRPPartGroupInformationDto.imuDiscountGlAccountID = dataTable.Rows[0].Field<string>("imuDiscountGlAccountID");
			eRPPartGroupInformationDto.imuUniqueID = dataTable.Rows[0].Field<Guid>("imuUniqueID");
			eRPPartGroupInformationDto.imuInactiveDate = dataTable.Rows[0].Field<DateTime?>("imuInactiveDate");
			eRPPartGroupInformationDto.imuInactive = dataTable.Rows[0].Field<bool>("imuInactive");
			eRPPartGroupInformationDto.imuNextSerialNumberIDFormula = dataTable.Rows[0].Field<string>("imuNextSerialNumberIDFormula");
			eRPPartGroupInformationDto.imuNextSerialNumberOption = dataTable.Rows[0].Field<byte>("imuNextSerialNumberOption");
			eRPPartGroupInformationDto.imuNextSerialNumberValue = dataTable.Rows[0].Field<string>("imuNextSerialNumberValue");
			eRPPartGroupInformationDto.imuParentPartGroupID = dataTable.Rows[0].Field<string>("imuParentPartGroupID");
			eRPPartGroupInformationDto.imuPartImageFileName = dataTable.Rows[0].Field<string>("imuPartImageFileName");
			eRPPartGroupInformationDto.imuQmLaborMarkup = dataTable.Rows[0].Field<decimal>("imuQmLaborMarkup");
			eRPPartGroupInformationDto.imuQmMarkupOption = dataTable.Rows[0].Field<byte>("imuQmMarkupOption");
			eRPPartGroupInformationDto.imuQmMaterialMarkup = dataTable.Rows[0].Field<decimal>("imuQmMaterialMarkup");
			eRPPartGroupInformationDto.imuQmOverHeadMarkup = dataTable.Rows[0].Field<decimal>("imuQmOverHeadMarkup");
			eRPPartGroupInformationDto.imuQmPurchaseToOrderMarkup = dataTable.Rows[0].Field<decimal>("imuQmPurchaseToOrderMarkup");
			eRPPartGroupInformationDto.imuQmQuoteMarkupType = dataTable.Rows[0].Field<byte>("imuQmQuoteMarkupType");
			eRPPartGroupInformationDto.imuQmQuotingMarkup = dataTable.Rows[0].Field<decimal>("imuQmQuotingMarkup");
			eRPPartGroupInformationDto.imuQmSubcontractMarkup = dataTable.Rows[0].Field<decimal>("imuQmSubcontractMarkup");
			eRPPartGroupInformationDto.imuRowVersion = dataTable.Rows[0].Field<byte[]>("imuRowVersion");
			eRPPartGroupInformationDto.imuSalesGlAccountID = dataTable.Rows[0].Field<string>("imuSalesGlAccountID");
			eRPPartGroupInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartGroupInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartGroupInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartGroup(ERPPartGroupDto partGroup)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartGroups WHERE imuUniqueID = " + M1Util.ConvertToLinq(partGroup.imuUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imuPartGroupID"] = partGroup.imuPartGroupID.ToUpper();
				partGroup.imuUniqueID = ((partGroup.imuUniqueID == Guid.Empty) ? Guid.NewGuid() : partGroup.imuUniqueID);
				dataRow["imuUniqueID"] = partGroup.imuUniqueID;
				dataRow["imuCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imuCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartGroup could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partGroup.imuRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartGroup is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imuRowVersion"], partGroup.imuRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartGroup has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartGroup again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imuArDepositGlAccountID"] = partGroup.imuArDepositGlAccountID;
			dataRow["imuAvalaraTaxCodeID"] = partGroup.imuAvalaraTaxCodeID;
			dataRow["imuCogsLaborGlAccountID"] = partGroup.imuCogsLaborGlAccountID;
			dataRow["imuCogsMaterialGlAccountID"] = partGroup.imuCogsMaterialGlAccountID;
			dataRow["imuCogsOverheadGlAccountID"] = partGroup.imuCogsOverheadGlAccountID;
			dataRow["imuCogsSubcontractGlAccountID"] = partGroup.imuCogsSubcontractGlAccountID;
			dataRow["imuCommissionRate"] = partGroup.imuCommissionRate;
			dataRow["imuCommissionType"] = partGroup.imuCommissionType;
			dataRow["imuDescription"] = partGroup.imuDescription;
			dataRow["imuDiscountGlAccountID"] = partGroup.imuDiscountGlAccountID;
			DataRow dataRow2 = dataRow;
			DateTime? imuInactiveDate = partGroup.imuInactiveDate;
			dataRow2["imuInactiveDate"] = (imuInactiveDate.HasValue ? ((object)imuInactiveDate.GetValueOrDefault()) : dataRow["imuInactiveDate"]);
			dataRow["imuInactive"] = partGroup.imuInactive;
			dataRow["imuNextSerialNumberIDFormula"] = partGroup.imuNextSerialNumberIDFormula ?? dataRow["imuNextSerialNumberIDFormula"];
			dataRow["imuNextSerialNumberOption"] = partGroup.imuNextSerialNumberOption;
			dataRow["imuNextSerialNumberValue"] = partGroup.imuNextSerialNumberValue;
			dataRow["imuParentPartGroupID"] = partGroup.imuParentPartGroupID;
			dataRow["imuPartImageFileName"] = partGroup.imuPartImageFileName;
			dataRow["imuQmLaborMarkup"] = partGroup.imuQmLaborMarkup;
			dataRow["imuQmMarkupOption"] = partGroup.imuQmMarkupOption;
			dataRow["imuQmMaterialMarkup"] = partGroup.imuQmMaterialMarkup;
			dataRow["imuQmOverHeadMarkup"] = partGroup.imuQmOverHeadMarkup;
			dataRow["imuQmPurchaseToOrderMarkup"] = partGroup.imuQmPurchaseToOrderMarkup;
			dataRow["imuQmQuoteMarkupType"] = partGroup.imuQmQuoteMarkupType;
			dataRow["imuQmQuotingMarkup"] = partGroup.imuQmQuotingMarkup;
			dataRow["imuQmSubcontractMarkup"] = partGroup.imuQmSubcontractMarkup;
			dataRow["imuSalesGlAccountID"] = partGroup.imuSalesGlAccountID;
			if (partGroup.CustomFields != null && partGroup.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partGroup.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartGroup [{partGroup.imuUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartGroup [{partGroup.imuUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
