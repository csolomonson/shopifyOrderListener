using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferQuoteToRFQProcess : ProcessParameters
{
	public TransferQuoteToRFQProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[2] { "qmmQuoteID", "qmmQuoteLineID" };
		PromptFieldAllowMultiples = true;
		ExtraFieldNames = new string[5] { "qmmPartID", "qmmPartRevisionID", "qmmSupplierOrganizationID", "qmmPurchaseLocationID", "ItemTypeDesc" };
		KeyValueFieldNames = new string[4] { "qmmQuoteID", "qmmQuoteLineID", "qmmQuoteAssemblyID", "qmmQuoteMaterialID" };
		KeyValueTableName = "QuoteMaterials";
		Description = "Use this tool to pull information from a quote into this RFQ. Alternate suppliers and parts will be shown in the grid and will be included in the RFQ if checked.";
		GridID = "M1ADDFROMRFQQUOTE";
		BindingSourceTable = "RFQs";
		HelpLink = "RQ_TransferQuoteToRFQ.htm";
		ContinueMessage = "This will add to an RFQ from the {0} selected quote detail(s). Are you sure you want to continue?";
		CreatedBindingSourceCaption = "Create RFQ from Quote";
		PromptFieldValidations.Add(new PromptFieldValidationBool("qmmClosed", fieldValue: false, "Quote is closed."));
		DefaultValueFieldNames = new string[1] { "ProductionProperties.xapRQIncludeAlternateParts" };
		DefaultValueFilterExpression = "(AltPart <= (CASE WHEN xapRQIncludeAlternateParts = 1 THEN 1 ELSE 0 END))";
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.DefaultFieldValues;
		List<string> messages = arg.Messages;
		Job jobObj = new Job();
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("QuoteMaterials", "RFQLines", new string[12]
		{
			"qmmQuoteID", "qmmQuoteLineID", "qmmQuoteAssemblyID", "AltPart", "qmmPartID", "qmmPartRevisionID", "qmmPartShortDescription", "qmmPartLongDescriptionRTF", "qmmPartLongDescriptionText", "qmmUnitOfMeasure",
			"qmmPurchaseUnitOfMeasure", "qmoDocuments"
		}, new string[12]
		{
			"rqlQuoteID", "rqlQuoteLineID", "rqlQuoteAssemblyID", "rqlAlternatePart", "rqlPartID", "rqlPartRevisionID", "rqlPartShortDescription", "rqlPartLongDescriptionRTF", "rqlPartLongDescriptionText", "rqlInventoryUnitOfMeasure",
			"rqlPurchaseUnitOfMeasure", "rqlDocuments"
		});
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("QuoteMaterials", "RFQSuppliers", new string[4] { "qmmSupplierOrganizationID", "qmmPurchaseLocationID", "cmlPurchaseContactID", "imxOrgPartID" }, new string[4] { "rqsSupplierOrganizationID", "rqsPurchaseLocationID", "rqsPurchaseContactID", "rqsOrgPartID" });
		MatchingFieldsInfo quantityMatches = m1DataDictionary.FindMatchingFields("QuoteMaterials", "RFQQuantities", new string[0], new string[0]);
		DataTable dataTable = databaseForRow.GetDataTable("select qmaQuoteID,qmaQuoteLineID,qmaQuoteAssemblyID,qmaParentAssemblyID,qmaQuantityPerParent from QuoteMaterials Inner Join QuoteAssemblies on qmmQuoteID=qmaQuoteID and qmmQuoteLineID=qmaQuoteLineID where " + text + " order by qmaQuoteID,qmaQuoteLineID,qmaQuoteAssemblyID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select ItemType,ItemTypeDesc,PartType,PartTypeDesc,cmlCurrencyRateID,imrConversionFactor,qmmQuoteMaterialID,qmmQuantityPerAssembly,qmmScrapPercent,qmmScrapQuantity " + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + " from (select 1 as ItemType,'Material' as ItemTypeDesc,1 As PartType, '' As PartTypeDesc,0 as AltPart,qmmQuoteID,qmmQuoteLineID,qmmQuoteAssemblyID,qmmQuoteMaterialID,qmmPartID,qmmPartRevisionID,CASE WHEN imxOrgPartID IS NULL THEN CASE WHEN imzOrgPartID IS NULL THEN '' ELSE imzOrgPartID END ELSE imxOrgPartID END AS imxOrgPartID,qmmPartShortDescription,qmmPartLongDescriptionRTF,qmmPartLongDescriptionText,qmmUnitOfMeasure,CASE WHEN imrPurchaseUnitOfMeasure IS NULL OR imrPurchaseUnitOfMeasure = '' THEN qmmUnitOfMeasure ELSE imrPurchaseUnitOfMeasure END AS qmmPurchaseUnitOfMeasure,qmmQuantityPerAssembly,qmmSupplierOrganizationID,qmmPurchaseLocationID,Isnull(cmlPurchaseContactID,'') As cmlPurchaseContactID,imrQuantityOnHand,imrQuantityAllocated,imrLastMaterialCost,qmmScrapPercent,qmmScrapQuantity,CASE WHEN IsNull(imxConversionFactor,IsNull(imzConversionFactor,IsNull(imrConversionFactor,1))) = 0 THEN 1 ELSE IsNull(imxConversionFactor,IsNull(imzConversionFactor,IsNull(imrConversionFactor,1))) END As imrConversionFactor,'' as qmoDocuments,cmlCurrencyRateID from QuoteMaterials left outer join PartRevisions on qmmPartID = imrPartID And qmmPartRevisionID = imrPartRevisionID left outer join OrganizationLocations on qmmSupplierOrganizationID = cmlOrganizationID and qmmPurchaseLocationID=cmlLocationID left outer join PartCrossReferences on qmmPartID = imxPartID And qmmPartRevisionID = imxPartRevisionID and qmmSupplierOrganizationID = imxOrganizationID and qmmPurchaseLocationID = imxLocationID Left Outer Join PartOrgReferences On qmmPartID = imzPartID And qmmPartRevisionID = imzPartRevisionID and qmmSupplierOrganizationID = imzOrganizationID Union All (select 1 as ItemType,'Material' as ItemTypeDesc,2 As PartType,'Alt-Supp' As PartTypeDesc,0 as AltPart,qmmQuoteID,qmmQuoteLineID,qmmQuoteAssemblyID,qmmQuoteMaterialID,qmmPartID,qmmPartRevisionID,imxOrgPartID,qmmPartShortDescription,qmmPartLongDescriptionRTF,qmmPartLongDescriptionText,qmmUnitOfMeasure,CASE WHEN imrPurchaseUnitOfMeasure IS NULL OR imrPurchaseUnitOfMeasure = '' THEN qmmUnitOfMeasure ELSE imrPurchaseUnitOfMeasure END AS qmmPurchaseUnitOfMeasure,qmmQuantityPerAssembly,imxOrganizationID As qmmSupplierOrganizationID,imxLocationID As qmmPurchaseLocationID,Isnull(cmlPurchaseContactID,'') As cmlPurchaseContactID,imrQuantityOnHand,imrQuantityAllocated,imrLastMaterialCost,qmmScrapPercent,qmmScrapQuantity,CASE WHEN IsNull(imxConversionFactor,IsNull(imrConversionFactor,1)) = 0 THEN 1 ELSE IsNull(imxConversionFactor,IsNull(imrConversionFactor,1)) END As imrConversionFactor,'' as qmoDocuments,cmlCurrencyRateID from QuoteMaterials a inner join PartRevisions on qmmPartID = imrPartID And qmmPartRevisionID = imrPartRevisionID inner join PartCrossReferences on qmmPartID = imxPartID And qmmPartRevisionID = imxPartRevisionID And imxPurchased = 1 and imxInactive = 0 and imxOrganizationID <> '' left outer join OrganizationLocations on imxOrganizationID = cmlOrganizationID and imxLocationID = cmlLocationID left outer join Organizations on imxOrganizationID = cmoOrganizationID where (cmoSupplierStatus = 1 or cmoSupplierStatus = 2) And imxOrganizationID+imxLocationID Not In (Select qmmSupplierOrganizationID+qmmPurchaseLocationID From QuoteMaterials b Where b.qmmQuoteID = a.qmmQuoteID And b.qmmQuoteLineID = a.qmmQuoteLineID And b.qmmQuoteAssemblyID = a.qmmQuoteAssemblyID And b.qmmQuoteMaterialID = a.qmmQuoteMaterialID)) Union All (select 1 as ItemType,'Material' as ItemTypeDesc,3 As PartType,'Alt-Part' As PartTypeDesc,1 as AltPart,qmmQuoteID,qmmQuoteLineID,qmmQuoteAssemblyID,qmmQuoteMaterialID,imeAlternatePartID As qmmPartID,imeAlternatePartRevisionID As qmmPartRevisionID,CASE WHEN imxOrgPartID IS NULL THEN CASE WHEN imzOrgPartID IS NULL THEN '' ELSE imzOrgPartID END ELSE imxOrgPartID END AS imxOrgPartID,imrShortDescription As qmmPartShortDescription,imrLongDescriptionRTF As qmmPartLongDescriptionRTF,imrLongDescriptionText As qmmPartLongDescriptionText,imrInventoryUnitOfMeasure As qmmUnitOfMeasure,CASE WHEN imrPurchaseUnitOfMeasure IS NULL OR imrPurchaseUnitOfMeasure = '' THEN qmmUnitOfMeasure ELSE imrPurchaseUnitOfMeasure END AS qmmPurchaseUnitOfMeasure,qmmQuantityPerAssembly,IsNull(imxOrganizationID,'') As qmmSupplierOrganizationID,IsNull(imxLocationID,'') As qmmPurchaseLocationID,Isnull(cmlPurchaseContactID,'') As cmlPurchaseContactID,imrQuantityOnHand,imrQuantityAllocated,imrLastMaterialCost,qmmScrapPercent,qmmScrapQuantity,CASE WHEN IsNull(imxConversionFactor,IsNull(imzConversionFactor,IsNull(imrConversionFactor,1))) = 0 THEN 1 ELSE IsNull(imxConversionFactor,IsNull(imzConversionFactor,IsNull(imrConversionFactor,1))) END As imrConversionFactor,'' as qmoDocuments,cmlCurrencyRateID from QuoteMaterials inner join PartAlternates On qmmPartID = imePartID And qmmPartRevisionID = imePartRevisionID Inner join PartRevisions on imeAlternatePartID = imrPartID And imeAlternatePartRevisionID = imrPartRevisionID Left outer join (Select imxPartID,imxPartRevisionID,imzOrgPartID,imxOrgPartID,imxOrganizationID,imxLocationID,cmlPurchaseContactID,imxPurchaseUnitOfMeasure,imzPurchaseUnitOfMeasure,imxConversionFactor,imzConversionFactor,cmlCurrencyRateID     From PartCrossReferences     Inner join PartOrgReferences on imxPartID = imzPartID And imxPartRevisionID = imzPartRevisionID And imzPurchased = 1 and imzInactive = 0 and imzOrganizationID <> ''     Inner join OrganizationLocations on imxOrganizationID = cmlOrganizationID and imxLocationID = cmlLocationID     Inner join Organizations on imxOrganizationID = cmoOrganizationID Where imxPurchased = 1 and imxInactive = 0 and imxOrganizationID <> '' And (cmoSupplierStatus = 1 or cmoSupplierStatus = 2) ) As AltTest On imeAlternatePartID = imxPartID And imeAlternatePartRevisionID = imxPartRevisionID  ) Union All (select 1 as ItemType,'Material' as ItemTypeDesc,4 As PartType,'Quote Line' As PartTypeDesc,0 As AltPart,qmlQuoteID AS qmmQuoteID, qmlQuoteLineID AS qmmQuoteLineID, 0 AS qmmQuoteAssemblyID, 0 AS qmmQuoteMaterialID,qmlPartID AS qmmPartID,qmlPartRevisionID AS qmmPartRevisionID,imxOrgPartID,qmlPartShortDescription AS qmmPartShortDescription,qmlPartLongDescriptionRTF AS qmmPartLongDescriptionRTF,qmlPartLongDescriptionText AS qmmPartLongDescriptionText, qmlUnitOfMeasure As qmmUnitOfMeasure,CASE WHEN imrPurchaseUnitOfMeasure IS NULL OR imrPurchaseUnitOfMeasure = '' THEN qmlUnitOfMeasure ELSE imrPurchaseUnitOfMeasure END AS qmmPurchaseUnitOfMeasure,1 AS qmmQuantityPerAssembly,ISNULL(imxOrganizationID,'') As qmmSupplierOrganizationID,ISNULL(imxLocationID,'') As qmmPurchaseLocationID,cmlPurchaseContactID,imrQuantityOnHand,imrQuantityAllocated,imrLastMaterialCost,0 AS qmmScrapPercent,0 AS qmmScrapQuantity,CASE WHEN IsNull(imxConversionFactor,IsNull(imrConversionFactor,1)) = 0 THEN 1 ELSE IsNull(imxConversionFactor,IsNull(imrConversionFactor,1)) END As imrConversionFactor,'',cmlCurrencyRateID from QuoteLines LEFT OUTER join PartRevisions on qmlPartID=imrPartID And qmlPartRevisionID = imrPartRevisionID left outer join PartCrossReferences on qmlPartID = imxPartID And qmlPartRevisionID = imxPartRevisionID And imxPurchased = 1 and imxInactive = 0 and imxOrganizationID <> '' left outer join OrganizationLocations on imxOrganizationID=cmlOrganizationID and imxLocationID=cmlLocationID left outer join Organizations on imxOrganizationID=cmoOrganizationID where qmlPurchaseToOrder <> 0) Union All (select 2 as ItemType,'Subcontract' as ItemTypeDesc,0 As PartType,'' as PartTypeDesc,0 as AltPart,qmoQuoteID,qmoQuoteLineID,qmoQuoteAssemblyID,qmoQuoteOperationID,qmoPartID,qmoPartRevisionID,CASE WHEN imxOrgPartID IS NULL THEN CASE WHEN imzOrgPartID IS NULL THEN '' ELSE imzOrgPartID END ELSE imxOrgPartID END AS imxOrgPartID,qmoProcessShortDescription,qmoProcessLongDescriptionRTF,qmoProcessLongDescriptionText,qmoUnitOfMeasure,CASE WHEN imrPurchaseUnitOfMeasure IS NULL OR imrPurchaseUnitOfMeasure = '' THEN qmoUnitOfMeasure ELSE imrPurchaseUnitOfMeasure END AS qmoPurchaseUnitOfMeasure,qmoQuantityPerAssembly As qmmQuantityPerAssembly,qmoSupplierOrganizationID,qmoPurchaseLocationID,cmlPurchaseContactID,0,0,0,0,0,IsNull(imrConversionFactor,1) As imrConversionFactor,qmoDocuments,cmlCurrencyRateID from QuoteOperations left outer join PartRevisions on qmoPartID=imrPartID And qmoPartRevisionID = imrPartRevisionID left outer join OrganizationLocations on qmoSupplierOrganizationID=cmlOrganizationID and qmoPurchaseLocationID=cmlLocationID left outer join PartCrossReferences on qmoPartID = imxPartID And qmoPartRevisionID = imxPartRevisionID and qmoSupplierOrganizationID = imxOrganizationID and qmoPurchaseLocationID = imxLocationID left outer join PartOrgReferences on qmoPartID = imzPartID And qmoPartRevisionID = imzPartRevisionID and qmoSupplierOrganizationID = imzOrganizationID where qmoOperationType = 2) ) X, ProductionProperties where " + text + " and " + DefaultValueFilterExpression + "order by qmmQuoteID,qmmQuoteLineID,qmmQuoteAssemblyID,PartType,qmmQuoteMaterialID");
		List<DataRow> list = new List<DataRow>();
		foreach (ProcessSelectedItemValues selectedItem in arg.SelectedItems)
		{
			string s = selectedItem.KeyValues[0].ToString();
			int num = Convert.ToInt32(selectedItem.KeyValues[1]);
			int num2 = Convert.ToInt32(selectedItem.KeyValues[2]);
			int num3 = Convert.ToInt32(selectedItem.KeyValues[3]);
			string s2 = selectedItem.ExtraFieldValues["qmmPartID"].ToString();
			string s3 = selectedItem.ExtraFieldValues["qmmPartRevisionID"].ToString();
			string s4 = selectedItem.ExtraFieldValues["qmmSupplierOrganizationID"].ToString();
			string s5 = selectedItem.ExtraFieldValues["qmmPurchaseLocationID"].ToString();
			string s6 = selectedItem.ExtraFieldValues["ItemTypeDesc"].ToString();
			DataRow[] array = dataTable2.Select("qmmPartID = " + s2.ToLinq() + " And qmmPartRevisionID = " + s3.ToLinq() + " And qmmSupplierOrganizationID = " + s4.ToLinq() + " And qmmPurchaseLocationID = " + s5.ToLinq() + " And qmmQuoteID = " + s.ToLinq() + " And qmmQuoteLineID = " + num.ToLinq() + " And qmmQuoteAssemblyID = " + num2.ToLinq() + " And qmmQuoteMaterialID = " + num3.ToLinq() + " And ItemTypeDesc = " + s6.ToLinq());
			foreach (DataRow item in array)
			{
				list.Add(item);
			}
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("RFQLines");
		DataTable dataTable3 = childBindingSource.GetDataTable();
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("RFQSuppliers");
		DataTable dataTable4 = childBindingSource2.GetDataTable();
		M1BindingSource childBindingSource3 = childBindingSource2.PrimaryTable.GetChildBindingSource("RFQQuantities");
		foreach (DataRow item2 in list.OrderBy((DataRow d) => d.Field<string>("qmmQuoteID").PadRight(30) + d.Field<short>("qmmQuoteLineID").ToString().PadRight(10) + d.Field<int>("qmmQuoteAssemblyID").ToString().PadRight(10) + d.Field<int>("qmmQuoteMaterialID").ToString().PadRight(10)))
		{
			addRFQLine(childBindingSource, item2, currentAsDataRow, matchingFieldsInfo, dataTable3, childBindingSource2, matchingFieldsInfo2, dataTable4, childBindingSource3, quantityMatches, dataTable, messages, jobObj);
		}
		BindingSource.OnDataChanged(3);
	}

	private void addRFQLine(M1BindingSource bsRFQLines, DataRow QuoteLineRow, DataRow rfqRow, MatchingFieldsInfo lineMatches, DataTable dtRFQLines, M1BindingSource bsRFQSuppliers, MatchingFieldsInfo supplierMatches, DataTable dtRFQSuppliers, M1BindingSource bsRFQQuantities, MatchingFieldsInfo quantityMatches, DataTable dtQuoteAssemblies, List<string> messages, Job jobObj)
	{
		DataRow[] array = ((QuoteLineRow.Field<int>("ItemType") != 2) ? dtRFQLines.Select("rqlQuoteID = " + QuoteLineRow.Field<string>("qmmQuoteID").ToLinq() + " And rqlQuoteLineID = " + QuoteLineRow.Field<short>("qmmQuoteLineID").ToLinq() + " And rqlQuoteAssemblyID = " + QuoteLineRow.Field<int>("qmmQuoteAssemblyID").ToLinq() + " And rqlRFQType = 1 And rqlQuoteMaterialID = " + QuoteLineRow.Field<int>("qmmQuoteMaterialID").ToLinq() + " And rqlPartID = " + QuoteLineRow.Field<string>("qmmPartID").ToLinq() + " And rqlPartRevisionID = " + QuoteLineRow.Field<string>("qmmPartRevisionID").ToLinq()) : dtRFQLines.Select("rqlQuoteID = " + QuoteLineRow.Field<string>("qmmQuoteID").ToLinq() + " And rqlQuoteLineID = " + QuoteLineRow.Field<short>("qmmQuoteLineID").ToLinq() + " And rqlQuoteAssemblyID = " + QuoteLineRow.Field<int>("qmmQuoteAssemblyID").ToLinq() + " And rqlRFQType = 2 And rqlQuoteOperationID = " + QuoteLineRow.Field<int>("qmmQuoteMaterialID").ToLinq() + " And rqlPartID = " + QuoteLineRow.Field<string>("qmmPartID").ToLinq() + " And rqlPartRevisionID = " + QuoteLineRow.Field<string>("qmmPartRevisionID").ToLinq()));
		DataRow dataRow;
		if (array.Length == 0)
		{
			dataRow = TransferLineInfo(this, QuoteLineRow, bsRFQLines, lineMatches, rfqRow);
			if (QuoteLineRow.Field<int>("ItemType") == 1)
			{
				dataRow["rqlRFQType"] = 1;
				dataRow["rqlQuoteMaterialID"] = QuoteLineRow["qmmQuoteMaterialID"];
			}
			else
			{
				dataRow["rqlRFQType"] = 2;
				dataRow["rqlQuoteOperationID"] = QuoteLineRow["qmmQuoteMaterialID"];
			}
		}
		else
		{
			dataRow = array[0];
		}
		DataTable dataTable = bsRFQLines.Database.GetDataTable("select qmqQuoteID,qmqQuoteLineID,qmqQuoteQuantity from QuoteQuantities Where qmqQuoteID = " + dataRow.Field<string>("rqlQuoteID").ToSql() + " And qmqQuoteLineID = " + dataRow.Field<short>("rqlQuoteLineID").ToSql() + " order by qmqQuoteID,qmqQuoteLineID,qmqQuoteQuantityID");
		if (string.IsNullOrWhiteSpace(QuoteLineRow.Field<string>("qmmSupplierOrganizationID")))
		{
			return;
		}
		if (dtRFQSuppliers.Select("rqsRFQLineID = " + dataRow.Field<short>("rqlRFQLineID").ToLinq() + " And rqsSupplierOrganizationID = " + QuoteLineRow.Field<string>("qmmSupplierOrganizationID").ToLinq() + " And rqsPurchaseLocationID = " + QuoteLineRow.Field<string>("qmmPurchaseLocationID").ToLinq()).Length == 0)
		{
			DataRow dataRow2 = TransferLineInfo(this, QuoteLineRow, bsRFQSuppliers, supplierMatches, dataRow);
			dataRow2["rqsDueDate"] = rfqRow["rqpDueDate"];
			if (!string.IsNullOrWhiteSpace(QuoteLineRow.Field<string>("cmlCurrencyRateID")))
			{
				dataRow2["rqsCurrencyRateID"] = QuoteLineRow["cmlCurrencyRateID"];
			}
			if (bsRFQQuantities.Count != 0)
			{
				bsRFQQuantities.RemoveWhere(string.Empty, dataRow2);
			}
			{
				foreach (DataRow row in dataTable.Rows)
				{
					DataRow dataRow4 = TransferLineInfo(this, row, bsRFQQuantities, quantityMatches, dataRow2);
					decimal qtyMultiplier = getQtyMultiplier(dtQuoteAssemblies, QuoteLineRow.Field<string>("qmmQuoteID"), QuoteLineRow.Field<short>("qmmQuoteLineID"), QuoteLineRow.Field<int>("qmmQuoteAssemblyID"));
					dataRow4["rqqQuantity"] = row.Field<decimal>("qmqQuoteQuantity") * QuoteLineRow.Field<decimal>("qmmQuantityPerAssembly") * qtyMultiplier;
					dataRow4["rqqQuantity"] = jobObj.CalculateQtyWithScrap(bsRFQQuantities.Database, Convert.ToDouble(dataRow4["rqqQuantity"]), Convert.ToDouble(QuoteLineRow["qmmScrapPercent"]), Convert.ToDouble(QuoteLineRow["qmmScrapQuantity"]), bsRFQQuantities.Database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					if (QuoteLineRow.Field<decimal>("imrConversionFactor") != 0m)
					{
						dataRow4["rqqQuantity"] = dataRow4.Field<decimal>("rqqQuantity") * QuoteLineRow.Field<decimal>("imrConversionFactor");
					}
					SqlCommand sqlCommand = BindingSource.Database.NewSqlCommand("select IsNull(imxLotSize,imzLotSize) As imxLotSize, IsNull(imxMinimumPurchaseQuantity,imzMinimumPurchaseQuantity) As imxMinimumPurchaseQuantity from PartOrgReferences left outer join PartCrossReferences on imzPartID=imxPartID and imzPartRevisionID=imxPartRevisionID and imzOrganizationID=imxOrganizationID and imxLocationId = @LocationID where imzPartID = @PartID and imzPartRevisionID = @RevisionID and imzOrganizationID = @OrgID");
					sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = QuoteLineRow.Field<string>("qmmPurchaseLocationID");
					sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = QuoteLineRow.Field<string>("qmmPartID");
					sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = QuoteLineRow.Field<string>("qmmPartRevisionID");
					sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = QuoteLineRow.Field<string>("qmmSupplierOrganizationID");
					DataTable dataTable2 = BindingSource.Database.GetDataTable(sqlCommand);
					if (dataTable2.Rows.Count != 0)
					{
						decimal num = dataTable2.Rows[0].Field<decimal>("imxMinimumPurchaseQuantity");
						decimal num2 = dataTable2.Rows[0].Field<decimal>("imxLotSize");
						if (dataRow4.Field<decimal>("rqqQuantity") < num)
						{
							dataRow4["rqqQuantity"] = num;
						}
						decimal num3 = dataRow4.Field<decimal>("rqqQuantity");
						if (num2 > 0m && num3 % num2 > 0m)
						{
							decimal num4 = (decimal)Convert.ToInt16(num3 / num2) * num2 + num2;
							dataRow4["rqqQuantity"] = num4;
						}
					}
				}
				return;
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(string.Format("Supplier Detail {1} / {2} already exists for RFQ Line {0} and was not added. ", dataRow.Field<short>("rqlRFQLineID"), QuoteLineRow.Field<string>("qmmSupplierOrganizationID"), QuoteLineRow.Field<string>("qmmPurchaseLocationID")));
		messages.Add(stringBuilder.ToString());
	}

	private decimal getQtyMultiplier(DataTable dtQuoteAssemblies, string quoteID, short quoteLineID, int asmID)
	{
		decimal result = 1m;
		DataRow[] array = dtQuoteAssemblies.Select("qmaQuoteID = " + quoteID.ToLinq() + " And qmaQuoteLineID = " + quoteLineID.ToLinq() + " And qmaQuoteAssemblyID = " + asmID.ToLinq(), "qmaQuoteID,qmaQuoteLineID,qmaQuoteAssemblyID");
		if (array.Length != 0)
		{
			result *= array[0].Field<decimal>("qmaQuantityPerParent");
			if (array[0].Field<int>("qmaQuoteAssemblyID") != 0)
			{
				result *= getQtyMultiplier(dtQuoteAssemblies, quoteID, quoteLineID, array[0].Field<int>("qmaParentAssemblyID"));
			}
		}
		return result;
	}
}
