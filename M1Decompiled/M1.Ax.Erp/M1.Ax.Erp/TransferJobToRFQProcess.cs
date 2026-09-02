using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferJobToRFQProcess : ProcessParameters
{
	public TransferJobToRFQProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "jmmJobID" };
		PromptFieldAllowMultiples = false;
		ExtraFieldNames = new string[5] { "jmmPartID", "jmmPartRevisionID", "jmmSupplierOrganizationID", "jmmPurchaseLocationID", "ItemTypeDesc" };
		KeyValueFieldNames = new string[3] { "jmmJobID", "jmmJobAssemblyID", "jmmJobMaterialID" };
		KeyValueTableName = "JobMaterials";
		Description = "Use this tool to pull information from a job into this RFQ. Alternate suppliers and parts will be shown in the grid and will be included in the RFQ if checked.";
		GridID = "M1ADDFROMRFQJOB";
		BindingSourceTable = "RFQs";
		HelpLink = "RQ_TransferJobToRFQ.htm";
		ContinueMessage = "This will add to an RFQ from the {0} selected job detail(s). Are you sure you want to continue?";
		CreatedBindingSourceCaption = "Create RFQ from Job";
		PromptFieldValidations.Add(new PromptFieldValidationBool("jmmClosed", fieldValue: false, "Job is closed."));
		DefaultValueFieldNames = new string[1] { "ProductionProperties.xapRQIncludeAlternateParts" };
		DefaultValueFilterExpression = "(AltPart <= (CASE WHEN xapRQIncludeAlternateParts = 1 THEN 1 ELSE 0 END))";
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.DefaultFieldValues;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("JobMaterials", "RFQLines", new string[12]
		{
			"jmmJobID", "jmmJobAssemblyID", "AltPart", "jmmEstimatedQuantity", "jmmPartID", "jmmPartRevisionID", "jmmPartShortDescription", "jmmPartLongDescriptionRTF", "jmmPartLongDescriptionText", "jmmUnitOfMeasure",
			"jmmPurchaseUnitOfMeasure", "jmoDocuments"
		}, new string[12]
		{
			"rqlJobID", "rqlJobAssemblyID", "rqlAlternatePart", "rqlJobEstimatedQty", "rqlPartID", "rqlPartRevisionID", "rqlPartShortDescription", "rqlPartLongDescriptionRTF", "rqlPartLongDescriptionText", "rqlInventoryUnitOfMeasure",
			"rqlPurchaseUnitOfMeasure", "rqlDocuments"
		});
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("JobMaterials", "RFQSuppliers", new string[4] { "jmmSupplierOrganizationID", "jmmPurchaseLocationID", "cmlPurchaseContactID", "imxOrgPartID" }, new string[4] { "rqsSupplierOrganizationID", "rqsPurchaseLocationID", "rqsPurchaseContactID", "rqsOrgPartID" });
		MatchingFieldsInfo quantityMatches = m1DataDictionary.FindMatchingFields("JobMaterials", "RFQQuantities", new string[0], new string[0]);
		DataTable dataTable = databaseForRow.GetDataTable("select ItemType,ItemTypeDesc,PartType,PartTypeDesc,jmmJobMaterialID,cmlCurrencyRateID,imrConversionFactor " + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + " from (select 1 as ItemType,'Material' as ItemTypeDesc,1 As PartType, '' As PartTypeDesc,0 as AltPart,jmmJobID,jmmJobAssemblyID,jmmJobMaterialID,jmmEstimatedQuantity,jmmPartID,jmmPartRevisionID,CASE WHEN imxOrgPartID IS NULL THEN CASE WHEN imzOrgPartID IS NULL THEN '' ELSE imzOrgPartID END ELSE imxOrgPartID END AS imxOrgPartID,jmmPartShortDescription,jmmPartLongDescriptionRTF,jmmPartLongDescriptionText,jmmUnitOfMeasure,CASE WHEN imrPurchaseUnitOfMeasure IS NULL OR imrPurchaseUnitOfMeasure = '' THEN jmmUnitOfMeasure ELSE imrPurchaseUnitOfMeasure END AS jmmPurchaseUnitOfMeasure,jmmSupplierOrganizationID,jmmPurchaseLocationID,Isnull(cmlPurchaseContactID,'') As cmlPurchaseContactID,imrQuantityOnHand,imrQuantityAllocated,imrLastMaterialCost,CASE WHEN IsNull(imxConversionFactor,IsNull(imzConversionFactor,IsNull(imrConversionFactor,1))) = 0 THEN 1 ELSE IsNull(imxConversionFactor,IsNull(imzConversionFactor,IsNull(imrConversionFactor,1))) END As imrConversionFactor,'' as jmoDocuments,cmlCurrencyRateID from JobMaterials left outer join PartRevisions on jmmPartID = imrPartID And jmmPartRevisionID = imrPartRevisionID left outer join OrganizationLocations on jmmSupplierOrganizationID = cmlOrganizationID and jmmPurchaseLocationID=cmlLocationID left outer join PartCrossReferences on jmmPartID = imxPartID And jmmPartRevisionID = imxPartRevisionID and jmmSupplierOrganizationID = imxOrganizationID and jmmPurchaseLocationID = imxLocationID Left Outer Join PartOrgReferences On jmmPartID = imzPartID And jmmPartRevisionID = imzPartRevisionID and jmmSupplierOrganizationID = imzOrganizationID where jmmReceivedComplete = 0 And JmmPurchaseOrderID = '' And jmmRFQID = ''  Union All (select 1 as ItemType,'Material' as ItemTypeDesc,2 As PartType,'Alt-Supp' As PartTypeDesc,0 as AltPart,jmmJobID,jmmJobAssemblyID,jmmJobMaterialID,jmmEstimatedQuantity,jmmPartID,jmmPartRevisionID,imxOrgPartID,jmmPartShortDescription,jmmPartLongDescriptionRTF,jmmPartLongDescriptionText,jmmUnitOfMeasure,CASE WHEN imrPurchaseUnitOfMeasure IS NULL OR imrPurchaseUnitOfMeasure = '' THEN jmmUnitOfMeasure ELSE imrPurchaseUnitOfMeasure END AS jmmPurchaseUnitOfMeasure,imxOrganizationID As jmmSupplierOrganizationID,imxLocationID As jmmPurchaseLocationID,Isnull(cmlPurchaseContactID,'') As cmlPurchaseContactID,imrQuantityOnHand,imrQuantityAllocated,imrLastMaterialCost,CASE WHEN IsNull(imxConversionFactor,IsNull(imrConversionFactor,1)) = 0 THEN 1 ELSE IsNull(imxConversionFactor,IsNull(imrConversionFactor,1)) END As imrConversionFactor,'' as jmoDocuments,cmlCurrencyRateID from JobMaterials a inner join PartRevisions on jmmPartID = imrPartID And jmmPartRevisionID = imrPartRevisionID inner join PartCrossReferences on jmmPartID = imxPartID And jmmPartRevisionID = imxPartRevisionID And imxPurchased = 1 and imxInactive = 0 and imxOrganizationID <> '' left outer join OrganizationLocations on imxOrganizationID = cmlOrganizationID and imxLocationID = cmlLocationID left outer join Organizations on imxOrganizationID = cmoOrganizationID where jmmReceivedComplete = 0 And JmmPurchaseOrderID = '' And jmmRFQID = '' And (cmoSupplierStatus = 1 or cmoSupplierStatus = 2) And imxOrganizationID+imxLocationID Not In (Select jmmSupplierOrganizationID+jmmPurchaseLocationID From JobMaterials b Where b.jmmJobID = a.jmmJobID And b.jmmJobAssemblyID = a.jmmJobAssemblyID And b.jmmJobMaterialID = a.jmmJobMaterialID)) Union All (select 1 as ItemType,'Material' as ItemTypeDesc,3 As PartType,'Alt-Part' As PartTypeDesc,1 as AltPart,jmmJobID,jmmJobAssemblyID,jmmJobMaterialID,jmmEstimatedQuantity,imeAlternatePartID As jmmPartID,imeAlternatePartRevisionID As jmmPartRevisionID,CASE WHEN imxOrgPartID IS NULL THEN CASE WHEN imzOrgPartID IS NULL THEN '' ELSE imzOrgPartID END ELSE imxOrgPartID END AS imxOrgPartID,imrShortDescription As jmmPartShortDescription,imrLongDescriptionRTF As jmmPartLongDescriptionRTF,imrLongDescriptionText As jmmPartLongDescriptionText,imrInventoryUnitOfMeasure As jmmUnitOfMeasure,CASE WHEN imrPurchaseUnitOfMeasure IS NULL OR imrPurchaseUnitOfMeasure = '' THEN jmmUnitOfMeasure ELSE imrPurchaseUnitOfMeasure END AS jmmPurchaseUnitOfMeasure,IsNull(imxOrganizationID,'') As jmmSupplierOrganizationID,IsNull(imxLocationID,'') As jmmPurchaseLocationID,Isnull(cmlPurchaseContactID,'') As cmlPurchaseContactID,imrQuantityOnHand,imrQuantityAllocated,imrLastMaterialCost,CASE WHEN IsNull(imxConversionFactor,IsNull(imzConversionFactor,IsNull(imrConversionFactor,1))) = 0 THEN 1 ELSE IsNull(imxConversionFactor,IsNull(imzConversionFactor,IsNull(imrConversionFactor,1))) END As imrConversionFactor,'' as jmoDocuments,cmlCurrencyRateID from JobMaterials inner join PartAlternates On jmmPartID = imePartID And jmmPartRevisionID = imePartRevisionID Inner join PartRevisions on imeAlternatePartID = imrPartID And imeAlternatePartRevisionID = imrPartRevisionID Left outer join (Select imxPartID,imxPartRevisionID,imzOrgPartID,imxOrgPartID,imxOrganizationID,imxLocationID,cmlPurchaseContactID,imxPurchaseUnitOfMeasure,imzPurchaseUnitOfMeasure,imxConversionFactor,imzConversionFactor,cmlCurrencyRateID From PartCrossReferences Inner join PartOrgReferences on imxPartID = imzPartID And imxPartRevisionID = imzPartRevisionID And imzPurchased = 1 and imzInactive = 0 and imzOrganizationID <> '' Inner join OrganizationLocations on imxOrganizationID = cmlOrganizationID and imxLocationID = cmlLocationID Inner join Organizations on imxOrganizationID = cmoOrganizationID Where imxPurchased = 1 and imxInactive = 0 and imxOrganizationID <> '' And (cmoSupplierStatus = 1 or cmoSupplierStatus = 2) ) As AltTest On imeAlternatePartID = imxPartID And imeAlternatePartRevisionID = imxPartRevisionID where jmmReceivedComplete = 0 And JmmPurchaseOrderID = '' And jmmRFQID = '') Union All (select 2 as ItemType,'Subcontract' as ItemTypeDesc,0 As PartType,'' as PartTypeDesc,0 as AltPart,jmoJobID,jmoJobAssemblyID,jmoJobOperationID,jmoOperationQuantity,jmoPartID,jmoPartRevisionID,CASE WHEN imxOrgPartID IS NULL THEN CASE WHEN imzOrgPartID IS NULL THEN '' ELSE imzOrgPartID END ELSE imxOrgPartID END AS imxOrgPartID,jmoProcessShortDescription,jmoProcessLongDescriptionRTF,jmoProcessLongDescriptionText,jmoUnitOfMeasure,CASE WHEN imrPurchaseUnitOfMeasure IS NULL OR imrPurchaseUnitOfMeasure = '' THEN jmoUnitOfMeasure ELSE imrPurchaseUnitOfMeasure END AS jmoPurchaseUnitOfMeasure,jmoSupplierOrganizationID,jmoPurchaseLocationID,cmlPurchaseContactID,0,0,0,IsNull(imrConversionFactor,1) As imrConversionFactor,jmoDocuments,cmlCurrencyRateID from JobOperations left outer join PartRevisions on jmoPartID=imrPartID And jmoPartRevisionID = imrPartRevisionID left outer join OrganizationLocations on jmoSupplierOrganizationID=cmlOrganizationID and jmoPurchaseLocationID=cmlLocationID left outer join PartCrossReferences on jmoPartID = imxPartID And jmoPartRevisionID = imxPartRevisionID and jmoSupplierOrganizationID = imxOrganizationID and jmoPurchaseLocationID = imxLocationID left outer join PartOrgReferences on jmoPartID = imzPartID And jmoPartRevisionID = imzPartRevisionID and jmoSupplierOrganizationID = imzOrganizationID  where jmoOperationType = 2 And JmoPurchaseOrderID = '' And jmoRFQID = '') ) X, ProductionProperties where " + text + " and " + DefaultValueFilterExpression + "order by jmmJobID,jmmJobAssemblyID,jmmJobMaterialID,PartType");
		List<DataRow> list = new List<DataRow>();
		foreach (ProcessSelectedItemValues selectedItem in arg.SelectedItems)
		{
			string s = selectedItem.KeyValues[0].ToString();
			int num = Convert.ToInt32(selectedItem.KeyValues[1]);
			int num2 = Convert.ToInt32(selectedItem.KeyValues[2]);
			string s2 = selectedItem.ExtraFieldValues["jmmPartID"].ToString();
			string s3 = selectedItem.ExtraFieldValues["jmmPartRevisionID"].ToString();
			string s4 = selectedItem.ExtraFieldValues["jmmSupplierOrganizationID"].ToString();
			string s5 = selectedItem.ExtraFieldValues["jmmPurchaseLocationID"].ToString();
			string s6 = selectedItem.ExtraFieldValues["ItemTypeDesc"].ToString();
			DataRow[] array = dataTable.Select("jmmPartID = " + s2.ToLinq() + " And jmmPartRevisionID = " + s3.ToLinq() + " And jmmSupplierOrganizationID = " + s4.ToLinq() + " And jmmPurchaseLocationID = " + s5.ToLinq() + " And jmmJobID = " + s.ToLinq() + " And jmmJobAssemblyID = " + num.ToLinq() + " And jmmJobMaterialID = " + num2.ToLinq() + " And ItemTypeDesc = " + s6.ToLinq());
			foreach (DataRow item in array)
			{
				list.Add(item);
			}
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("RFQLines");
		DataTable dataTable2 = childBindingSource.GetDataTable();
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("RFQSuppliers");
		DataTable dataTable3 = childBindingSource2.GetDataTable();
		M1BindingSource childBindingSource3 = childBindingSource2.PrimaryTable.GetChildBindingSource("RFQQuantities");
		foreach (DataRow item2 in list.OrderBy((DataRow d) => d.Field<string>("jmmJobID").PadRight(30) + d.Field<int>("jmmJobAssemblyID").ToString().PadRight(10) + d.Field<int>("jmmJobMaterialID").ToString().PadRight(10)))
		{
			addRFQLine(childBindingSource, item2, currentAsDataRow, matchingFieldsInfo, dataTable2, childBindingSource2, matchingFieldsInfo2, dataTable3, childBindingSource3, quantityMatches, messages);
		}
	}

	private void addRFQLine(M1BindingSource bsRFQLines, DataRow jobLineRow, DataRow rfqRow, MatchingFieldsInfo lineMatches, DataTable dtRFQLines, M1BindingSource bsRFQSuppliers, MatchingFieldsInfo supplierMatches, DataTable dtRFQSuppliers, M1BindingSource bsRFQQuantities, MatchingFieldsInfo quantityMatches, List<string> messages)
	{
		DataRow[] array = ((jobLineRow.Field<int>("ItemType") != 2) ? dtRFQLines.Select("rqlJobID = " + jobLineRow.Field<string>("jmmJobID").ToLinq() + " And rqlJobAssemblyID = " + jobLineRow.Field<int>("jmmJobAssemblyID").ToLinq() + " And rqlRFQType = 1 And rqlJobMaterialID = " + jobLineRow.Field<int>("jmmJobMaterialID").ToLinq() + " And rqlPartID = " + jobLineRow.Field<string>("jmmPartID").ToLinq() + " And rqlPartRevisionID = " + jobLineRow.Field<string>("jmmPartRevisionID").ToLinq()) : dtRFQLines.Select("rqlJobID = " + jobLineRow.Field<string>("jmmJobID").ToLinq() + " And rqlJobAssemblyID = " + jobLineRow.Field<int>("jmmJobAssemblyID").ToLinq() + " And rqlRFQType = 2 And rqlJobOperationID = " + jobLineRow.Field<int>("jmmJobMaterialID").ToLinq() + " And rqlPartID = " + jobLineRow.Field<string>("jmmPartID").ToLinq() + " And rqlPartRevisionID = " + jobLineRow.Field<string>("jmmPartRevisionID").ToLinq()));
		DataRow dataRow;
		if (array.Length == 0)
		{
			dataRow = TransferLineInfo(this, jobLineRow, bsRFQLines, lineMatches, rfqRow);
			if (jobLineRow.Field<int>("ItemType") == 1)
			{
				dataRow["rqlRFQType"] = 1;
				dataRow["rqlJobMaterialID"] = jobLineRow["jmmJobMaterialID"];
			}
			else
			{
				dataRow["rqlRFQType"] = 2;
				dataRow["rqlJobOperationID"] = jobLineRow["jmmJobMaterialID"];
			}
		}
		else
		{
			dataRow = array[0];
		}
		if (string.IsNullOrWhiteSpace(jobLineRow.Field<string>("jmmSupplierOrganizationID")))
		{
			return;
		}
		if (dtRFQSuppliers.Select("rqsRFQLineID = " + dataRow.Field<short>("rqlRFQLineID").ToLinq() + " And rqsSupplierOrganizationID = " + jobLineRow.Field<string>("jmmSupplierOrganizationID").ToLinq() + " And rqsPurchaseLocationID = " + jobLineRow.Field<string>("jmmPurchaseLocationID").ToLinq()).Length == 0)
		{
			DataRow dataRow2 = TransferLineInfo(this, jobLineRow, bsRFQSuppliers, supplierMatches, dataRow);
			dataRow2["rqsDueDate"] = rfqRow["rqpDueDate"];
			if (!string.IsNullOrWhiteSpace(jobLineRow.Field<string>("cmlCurrencyRateID")))
			{
				dataRow2["rqsCurrencyRateID"] = jobLineRow["cmlCurrencyRateID"];
			}
			decimal num = jobLineRow.Field<decimal>("jmmEstimatedQuantity");
			if (jobLineRow.Field<decimal>("imrConversionFactor") != 0m)
			{
				num *= jobLineRow.Field<decimal>("imrConversionFactor");
			}
			if (!(num > 0m))
			{
				return;
			}
			DataRow dataRow3 = TransferLineInfo(this, jobLineRow, bsRFQQuantities, quantityMatches, dataRow2);
			dataRow3["rqqRFQQuantityID"] = 1;
			dataRow3["rqqQuantity"] = num;
			SqlCommand sqlCommand = BindingSource.Database.NewSqlCommand("select IsNull(imxLotSize,imzLotSize) As imxLotSize, IsNull(imxMinimumPurchaseQuantity,imzMinimumPurchaseQuantity) As imxMinimumPurchaseQuantity from PartOrgReferences left outer join PartCrossReferences on imzPartID=imxPartID and imzPartRevisionID=imxPartRevisionID and imzOrganizationID=imxOrganizationID and imxLocationId = @LocationID where imzPartID = @PartID and imzPartRevisionID = @RevisionID and imzOrganizationID = @OrgID");
			sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = jobLineRow.Field<string>("jmmPurchaseLocationID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = jobLineRow.Field<string>("jmmPartID");
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = jobLineRow.Field<string>("jmmPartRevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = jobLineRow.Field<string>("jmmSupplierOrganizationID");
			DataTable dataTable = BindingSource.Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				decimal num2 = dataTable.Rows[0].Field<decimal>("imxMinimumPurchaseQuantity");
				decimal num3 = dataTable.Rows[0].Field<decimal>("imxLotSize");
				if (dataRow3.Field<decimal>("rqqQuantity") < num2)
				{
					dataRow3["rqqQuantity"] = num2;
				}
				decimal num4 = dataRow3.Field<decimal>("rqqQuantity");
				if (num3 > 0m && num4 % num3 > 0m)
				{
					decimal num5 = (decimal)Convert.ToInt16(num4 / num3) * num3 + num3;
					dataRow3["rqqQuantity"] = num5;
				}
			}
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("Supplier Detail {1} / {2} already exists for RFQ Line {0} and was not added. ", dataRow.Field<short>("rqlRFQLineID"), jobLineRow.Field<string>("jmmSupplierOrganizationID"), jobLineRow.Field<string>("jmmPurchaseLocationID")));
			messages.Add(stringBuilder.ToString());
		}
	}
}
