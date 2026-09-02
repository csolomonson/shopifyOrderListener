using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Core;
using M1.Core.Database;
using M1.Extensions;
using M1.Forms.Controls;

namespace M1.Ax.Erp;

public class Part
{
	public class SupplierRequirement
	{
		public string PartID;

		public string RevisionID;

		public string OrgID;

		public string LocID;

		public decimal MinPurQty;

		public decimal LotSize;
	}

	public bool GetPartClassReadOnly(M1Database database, string fieldName)
	{
		if (database.Props("FN").Field<bool>("xafGLCreateStockJournals"))
		{
			byte b = database.Props("FN").Field<byte>("xafCOGSUseAccounts");
			if (fieldName.Equals("IMCINVENTORYGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase) || fieldName.Equals("XARREASONGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase) || fieldName.Equals("IMCINVININSPECTIONGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase) || fieldName.Equals("IMCINVTORETURNGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase) || fieldName.Equals("IMCINVINTRANSFERGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase))
			{
				if (b != 1)
				{
					return true;
				}
			}
			else if ((fieldName.Equals("IMFINVENTORYGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase) || fieldName.Equals("XAJREASONGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase) || fieldName.Equals("IMFINVININSPECTIONGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase) || fieldName.Equals("IMFINVTORETURNGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase) || fieldName.Equals("IMFINVINTRANSFERGLACCOUNTID", StringComparison.CurrentCultureIgnoreCase)) && b != 2 && b != 3)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public bool CanSetPreferredSupplier(M1Database database, bool purchased, string partID, string partRevisionID, string orgID, string locID)
	{
		if (!purchased || string.IsNullOrWhiteSpace(partID) || string.IsNullOrWhiteSpace(orgID))
		{
			return false;
		}
		if (!IsOrganizationActiveSupplier(database, orgID, locID))
		{
			return false;
		}
		return !IsPreferredSupplier(database, partID, partRevisionID, orgID, locID);
	}

	private bool IsPreferredSupplier(M1Database database, string partID, string partRevisionID, string orgID, string locID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select imrPartID From PartRevisions Where imrPartID = @PartID And imrPartRevisionID = @RevisionID And imrSupplierOrganizationID = @OrgID And imrPurchaseLocationID = @LocID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = orgID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = locID;
		return !string.IsNullOrWhiteSpace(Convert.ToString(database.ExecuteScalar(sqlCommand)));
	}

	private bool IsOrganizationActiveSupplier(M1Database database, string organizationID, string locationID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select cmlOrganizationID From OrganizationLocations Inner Join Organizations ON cmoOrganizationID = cmlOrganizationID Where cmlOrganizationID = @OrganizationID and cmlLocationID = @LocationID and cmlPurchaseLocation = 1 And cmlInactive = 0 and cmoSupplierStatus = 2");
		sqlCommand.Parameters.Add(new SqlParameter("@OrganizationID", SqlDbType.NVarChar)).Value = organizationID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locationID;
		return !string.IsNullOrWhiteSpace(Convert.ToString(database.ExecuteScalar(sqlCommand)));
	}

	public void SetPreferredSupplier(M1Database database, string partID, string partRevisionID, string orgID, string locID, string purUoM, decimal conversionFactor)
	{
		using M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.DataSourceTable = "PARTREVISIONS";
		m1BindingSource.NavigateTo(database, "imrPartID = " + M1Util.ConvertToSql(partID) + " And imrPartRevisionID = " + M1Util.ConvertToSql(partRevisionID));
		if (m1BindingSource.Count != 0)
		{
			DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
			currentAsDataRow["imrPreferredRefExists"] = true;
			currentAsDataRow["imrSupplierOrganizationID"] = orgID;
			currentAsDataRow["imrPurchaseLocationID"] = locID;
			currentAsDataRow["imrPurchaseUnitOfMeasure"] = purUoM;
			currentAsDataRow["imrConversionFactor"] = conversionFactor;
			m1BindingSource.SaveData();
		}
	}

	public bool CanRevisionBeAdded(M1Database database, string partID, string partRevisionID)
	{
		if (!(database.GetService(typeof(M1DataDictionary)) as M1DataDictionary).ProductCode.IsModulePurchased("AB", database))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select Count(*) From PartRevisions Where imrPartID = @PartID And imrPartRevisionID <> @RevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
			return Convert.ToInt32(database.ExecuteScalar(sqlCommand)) == 0;
		}
		return true;
	}

	public bool DoesPartExist(M1Database database, string partID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select Count(*) From Parts Where impPartID = @PartID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand)) != 0;
	}

	public bool DoesPartRevisionExist(M1Database database, string partID, string partRevisionID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select Count(*) From PartRevisions Where imrPartID = @PartID And imrPartRevisionID = @PartRevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand)) != 0;
	}

	public int GetPartAlternateCount(M1Database database, string partID, string partRevisionID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select Count(*) From PartAlternates Where imePartID = @PartID And imePartRevisionID = @PartRevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand));
	}

	public void RefreshUnitSalePriceInRow(M1Database database, DataRow row, object priceDate = null)
	{
		if (row != null)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select " + GetUnitSalePriceQuery(database, string.Empty, string.Empty, priceDate) + " From PartRevisions Where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = row.Field<string>("imrPartID");
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = row.Field<string>("imrPartRevisionID");
			row["imhUnitSalePrice"] = database.ExecuteScalar(sqlCommand);
		}
	}

	public string GetUnitSalePriceQuery(M1Database database, string currencyID, string prefix = "", object oPriceDate = null)
	{
		if (string.IsNullOrWhiteSpace(prefix))
		{
			prefix = "imr";
		}
		DateTime d = DateTime.Today;
		if (oPriceDate != null && oPriceDate != DBNull.Value)
		{
			d = Convert.ToDateTime(oPriceDate);
		}
		string text = ((!string.IsNullOrWhiteSpace(currencyID)) ? (" And (imhCurrencyRateID = " + currencyID.ToSql() + ")") : (" And imhCurrencyRateID = " + database.HomeCurrencyID.ToSql()));
		return " IsNull((Select Top 1 IsNull(imhUnitSalePrice,0) From PartUnitSalePrices Where imhPartID = " + prefix + "PartID And imhPartRevisionID = " + prefix + "PartRevisionID AND {fn IFNULL(imhStartDate,'19000101')} <= " + d.ToSql() + " AND {fn IFNULL(imhEndDate,'20790606')} >= " + d.ToSql() + " " + text + " Order By imhCurrencyRateID Desc, IsNull(imhStartDate,'19000101') Desc),0) ";
	}

	public PartGroupMarkup GetPartGroupMarkups(M1Database database, string partGroupID)
	{
		bool flag = true;
		PartGroupMarkup partGroupMarkup = new PartGroupMarkup();
		DataRow row = database.Props("PN");
		partGroupMarkup.PartGroupID = partGroupID;
		if (!string.IsNullOrWhiteSpace(partGroupID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select imuQMMaterialMarkup,imuQMSubcontractMarkup,imuQMlaborMarkup,imuQMOverHeadMarkup,imuQMQuotingMarkup,imuQMQuoteMarkuptype,imuQMMarkupOption,imuQMPurchaseToOrderMarkup from partgroups where imuPartGroupid = @PartGroupID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartGroupID", SqlDbType.NVarChar)).Value = partGroupID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row2 = dataTable.Rows[0];
				if (row2.Field<byte>("imuQMMarkupOption") != 0)
				{
					flag = false;
					partGroupMarkup.MarkupOption = 1;
					partGroupMarkup.MaterialMarkup = row2.Field<decimal>("imuQMMaterialMarkup");
					partGroupMarkup.SubcontractMarkup = row2.Field<decimal>("imuQMSubContractMarkup");
					partGroupMarkup.LaborMarkup = row2.Field<decimal>("imuQMLaborMarkup");
					partGroupMarkup.OverheadMarkup = row2.Field<decimal>("imuQMOverHeadMarkup");
					partGroupMarkup.PurchaseToOrderMarkup = row2.Field<decimal>("imuQMPurchaseToOrderMarkup");
					partGroupMarkup.QuoteMarkup = row2.Field<decimal>("imuQMQuotingMarkup");
					if (row2.Field<byte>("imuQMQuoteMarkuptype") != 0)
					{
						partGroupMarkup.MarkupType = row2.Field<byte>("imuQMQuoteMarkupType");
					}
					else
					{
						partGroupMarkup.MarkupType = row.Field<byte>("xapQMQuoteMarkupType");
					}
				}
			}
		}
		if (flag)
		{
			partGroupMarkup.MarkupOption = 0;
			partGroupMarkup.MaterialMarkup = row.Field<decimal>("xapQMMaterialMarkup");
			partGroupMarkup.SubcontractMarkup = row.Field<decimal>("xapQMSubcontractMarkup");
			partGroupMarkup.LaborMarkup = row.Field<decimal>("xapQMLaborMarkup");
			partGroupMarkup.OverheadMarkup = row.Field<decimal>("xapQMOverheadMarkup");
			partGroupMarkup.PurchaseToOrderMarkup = row.Field<decimal>("xapQMPurchaseToOrderMarkup");
			partGroupMarkup.QuoteMarkup = row.Field<decimal>("xapQMQuotingMarkup");
			partGroupMarkup.MarkupType = row.Field<byte>("xapQMQuoteMarkupType");
		}
		return partGroupMarkup;
	}

	public PriceCalculation GetSellingPrice(M1Database database, string partID, string partRevisionID, string partGroupID, string orgID, string locationID, decimal quantity, string currencyID, DateTime? priceDate)
	{
		if (!priceDate.HasValue)
		{
			priceDate = DateTime.Today;
		}
		if (quantity < 0m)
		{
			quantity = -quantity;
		}
		PriceCalculation priceCalculation = new PriceCalculation();
		string text = " And (imhStartDate Is Null Or imhStartDate <= @PriceDate) AND (imhEndDate Is Null Or imhEndDate >= @PriceDate) And (imhCurrencyRateID = @HomeCurrencyID Or imhCurrencyRateID = @CurrencyID)";
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT imrLeadTime,imrConversionFactor,imhUnitSalePrice,IsNull(imhCurrencyRateID,'') As imhCurrencyRateID, (CASE WHEN imhCurrencyRateID = @HomeCurrencyID THEN 1 WHEN imhCurrencyRateID = '' THEN 1 ELSE 2 END) As CurrencyType FROM PartRevisions Left Outer Join PartUnitSalePrices On imhPartID = imrPartID And imhPartRevisionID = imrPartRevisionID " + text + " WHERE imrPartID = @PartID And imrPartRevisionID = @PartRevisionID Order By CurrencyType Desc, imhCurrencyRateID Desc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@HomeCurrencyID", SqlDbType.NVarChar)).Value = database.HomeCurrencyID;
		sqlCommand.Parameters.Add(new SqlParameter("@CurrencyID", SqlDbType.NVarChar)).Value = currencyID;
		sqlCommand.Parameters.Add(new SqlParameter("@PriceDate", SqlDbType.DateTime)).Value = priceDate.Value;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			PriceLineData priceLineData = null;
			priceCalculation.ConversionFactor = dataRow.Field<decimal>("imrConversionFactor");
			if (priceCalculation.ConversionFactor <= 0m)
			{
				priceCalculation.ConversionFactor = 1m;
			}
			priceCalculation.PartPrice = GetPrice(database, partID, partRevisionID, partGroupID, orgID, locationID, currencyID, 2, priceDate, null);
			if (priceCalculation.PartPrice != null && quantity != 0m)
			{
				priceLineData = priceCalculation.PartPrice.GetLineForQuantity(quantity);
			}
			if (priceLineData != null && priceLineData.UnitPrice > 0m)
			{
				if (dataRow["imhUnitSalePrice"] == DBNull.Value || dataRow.Field<decimal>("imhUnitSalePrice") < priceLineData.UnitPrice)
				{
					priceCalculation.FullPrice = priceLineData.UnitPrice;
				}
				else if (dataRow.Field<string>("imhCurrencyRateID").Equals(priceCalculation.PartPrice.CurrencyID, StringComparison.CurrentCultureIgnoreCase))
				{
					priceCalculation.FullPrice = dataRow.Field<decimal>("imhUnitSalePrice");
				}
				else
				{
					priceCalculation.FullPrice = priceLineData.UnitPrice;
				}
				priceCalculation.DiscountedPrice = priceLineData.UnitPrice;
				priceCalculation.LeadTime = priceLineData.LeadTime;
				priceCalculation.CalculationType = PriceCalculationType.PartPriceUnitPrice;
				priceCalculation.CurrencyID = priceCalculation.PartPrice.CurrencyID;
			}
			else if (priceLineData != null && priceLineData.Discount != 0m)
			{
				if (dataRow["imhUnitSalePrice"] == DBNull.Value)
				{
					priceCalculation.FullPrice = 0m;
					priceCalculation.DiscountedPrice = 0m;
					priceCalculation.CalculationType = PriceCalculationType.NoPrice;
				}
				else
				{
					priceCalculation.FullPrice = dataRow.Field<decimal>("imhUnitSalePrice");
					priceCalculation.DiscountedPrice = priceCalculation.FullPrice - priceLineData.Discount / 100.0m * priceCalculation.FullPrice;
					priceCalculation.LeadTime = priceLineData.LeadTime;
					priceCalculation.Discount = priceLineData.Discount;
					priceCalculation.CalculationType = PriceCalculationType.PartPriceDiscount;
				}
				priceCalculation.CurrencyID = dataRow.Field<string>("imhCurrencyRateID");
			}
			else
			{
				if (dataRow["imhUnitSalePrice"] == DBNull.Value)
				{
					priceCalculation.FullPrice = 0m;
					priceCalculation.DiscountedPrice = 0m;
					priceCalculation.CalculationType = PriceCalculationType.NoPrice;
				}
				else
				{
					priceCalculation.FullPrice = dataRow.Field<decimal>("imhUnitSalePrice");
					priceCalculation.DiscountedPrice = priceCalculation.FullPrice;
					priceCalculation.CalculationType = PriceCalculationType.PartUnitSalePrice;
				}
				priceCalculation.LeadTime = dataRow.Field<short>("imrLeadTime");
				priceCalculation.CurrencyID = dataRow.Field<string>("imhCurrencyRateID");
			}
		}
		priceCalculation.IsForeignCurrency = !string.IsNullOrWhiteSpace(priceCalculation.CurrencyID) && !priceCalculation.CurrencyID.Equals(database.HomeCurrencyID, StringComparison.CurrentCultureIgnoreCase);
		return priceCalculation;
	}

	public PriceCalculation GetPurchasePrice(M1Database database, string partID, string partRevisionID, string orgID, string locationID, decimal quantity, string costType, string currencyID, DateTime? priceDate, decimal purchaseQuantity, SqlTransaction transaction)
	{
		if (!priceDate.HasValue)
		{
			priceDate = DateTime.Today;
		}
		if (quantity < 0m)
		{
			quantity *= -1m;
		}
		if (purchaseQuantity < 0m)
		{
			purchaseQuantity *= -1m;
		}
		PriceCalculation priceCalculation = new PriceCalculation();
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT IsNull(Case When IsNull(imxConversionFactor, 0) = 0 Then imzConversionFactor Else imxConversionFactor End,imrConversionFactor) As ConversionFactor,imrLeadTime" + database.Props("PM").Field<byte>("xapPMCostingMethod") switch
		{
			2 => (!costType.Equals("Subcontract", StringComparison.CurrentCultureIgnoreCase)) ? ",imrLastMaterialCost as unitprice" : ",imrLastSubcontractCost as unitprice", 
			3 => (!costType.Equals("Subcontract", StringComparison.CurrentCultureIgnoreCase)) ? ",imrStandardMaterialCost as unitprice" : ",imrStandardSubcontractCost as unitprice", 
			_ => (!costType.Equals("Subcontract", StringComparison.CurrentCultureIgnoreCase)) ? ",imrAverageMaterialCost as unitprice" : ",imrAverageSubcontractCost as unitprice", 
		} + " From PartRevisions Left Outer Join PartOrgReferences On imzPartID = imrPartID And imzPartRevisionID = imrPartRevisionID AND imzOrganizationID = @OrgID Left Outer Join PartCrossReferences on imzOrganizationID = imxOrganizationID And imxLocationID = @LocID And imzPartID = imxPartID And imzPartRevisionID = imxPartRevisionID WHERE imrPartID = @PartID And imrPartRevisionID = @PartRevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = orgID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = locationID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			DataRow row = dataTable.Rows[0];
			priceCalculation.ConversionFactor = row.Field<decimal>("ConversionFactor");
			if (priceCalculation.ConversionFactor <= 0m)
			{
				priceCalculation.ConversionFactor = 1m;
			}
			decimal num = row.Field<decimal>("ConversionFactor");
			PriceLineData priceLineData = null;
			priceCalculation.PartPrice = GetPrice(database, partID, partRevisionID, null, orgID, locationID, currencyID, 1, priceDate, transaction);
			if (priceCalculation.PartPrice != null)
			{
				if (!priceCalculation.PartPrice.InventoryPrice)
				{
					quantity = purchaseQuantity;
					num = 1m;
				}
				if (quantity != 0m)
				{
					priceLineData = priceCalculation.PartPrice.GetLineForQuantity(quantity);
				}
			}
			if (priceLineData != null && priceLineData.UnitPrice > 0m)
			{
				priceCalculation.FullPrice = priceLineData.UnitPrice;
				priceCalculation.LeadTime = priceLineData.LeadTime;
				priceCalculation.CalculationType = PriceCalculationType.PartPriceUnitPrice;
				priceCalculation.CurrencyID = priceCalculation.PartPrice.CurrencyID;
				if (num != 0m)
				{
					priceCalculation.FullPrice = M1Math.Round(priceCalculation.FullPrice / num, 5);
				}
			}
			else
			{
				priceCalculation.FullPrice = M1Math.Round(row.Field<decimal>("unitPrice") / priceCalculation.ConversionFactor, 5);
				priceCalculation.LeadTime = row.Field<short>("imrLeadTime");
				priceCalculation.CalculationType = PriceCalculationType.PartCost;
				priceCalculation.CurrencyID = string.Empty;
			}
			priceCalculation.DiscountedPrice = priceCalculation.FullPrice;
		}
		priceCalculation.IsForeignCurrency = !string.IsNullOrWhiteSpace(priceCalculation.CurrencyID) && !priceCalculation.CurrencyID.Equals(database.HomeCurrencyID, StringComparison.CurrentCultureIgnoreCase);
		return priceCalculation;
	}

	public PriceData GetPrice(M1Database database, string partID, string partRevisionID, string partGroupID, string orgID, string locationID, string currencyID, byte priceType, DateTime? priceDate, SqlTransaction transaction)
	{
		if (!priceDate.HasValue)
		{
			priceDate = DateTime.Today;
		}
		string text = string.Empty;
		SqlCommand sqlCommand;
		if (!string.IsNullOrWhiteSpace(orgID))
		{
			sqlCommand = database.NewSqlCommand("select IsNull(cmoCustomerGroupID,'') from Organizations where cmoOrganizationID = @OrgID");
			sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = orgID;
			text = (string)database.ExecuteScalar(sqlCommand, transaction);
			if (text == null)
			{
				text = string.Empty;
			}
		}
		if (partGroupID == null)
		{
			partGroupID = string.Empty;
			if (!string.IsNullOrWhiteSpace(partID))
			{
				sqlCommand = database.NewSqlCommand("select IsNull(impPartGroupID,'') from Parts where impPartID = @PartID");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				partGroupID = (string)database.ExecuteScalar(sqlCommand, transaction);
				if (partGroupID == null)
				{
					partGroupID = string.Empty;
				}
			}
		}
		if (currencyID == null)
		{
			currencyID = string.Empty;
		}
		string text2 = "(imiStartDate Is Null Or imiStartDate <= @PriceDate) AND (imiEndDate Is Null Or imiEndDate >= @PriceDate) AND imiPriceType = @PriceType And (imiCurrencyRateID = @HomeCurrencyID Or imiCurrencyRateID = @CurrencyID)";
		sqlCommand = database.NewSqlCommand("Select Top 1 imiPartPriceID,imiInventoryPrice,imiCurrencyRateID,(CASE WHEN imiCurrencyRateID = @HomeCurrencyID THEN 1 WHEN imiCurrencyRateID = '' THEN 1 ELSE 2 END) As CurrencyType, GroupType From (Select imiPartPriceID,imiInventoryPrice,imiCurrencyRateID,1 As GroupType From PartPrices Where imiPartID = @PartID And imiPartRevisionID = @PartRevisionID AND imiOrganizationID = @OrgID AND imiLocationID = @LocationID And " + text2 + " Union All Select imiPartPriceID,imiInventoryPrice,imiCurrencyRateID,2 As GroupType From PartPrices Where imiOrganizationID = '' And imiLocationID = '' And imiPartID = @PartID And imiPartRevisionID = @PartRevisionID AND imiCustomerGroupID <> '' And imiCustomerGroupID = @CustomerGroupID And " + text2 + " Union All Select imiPartPriceID,imiInventoryPrice,imiCurrencyRateID,3 As GroupType From PartPrices Where imiPartID = '' And imiPartGroupID <> '' And imiPartGroupID = @PartGroupID  AND imiOrganizationID = @OrgID AND imiLocationID = @LocationID And " + text2 + " Union All Select imiPartPriceID,imiInventoryPrice,imiCurrencyRateID,4 As GroupType From PartPrices Where imiOrganizationID = '' And imiLocationID = '' And imiPartGroupID <> '' And imiPartGroupID = @PartGroupID AND imiCustomerGroupID <> '' And imiCustomerGroupID = @CustomerGroupID And " + text2 + " Union All Select imiPartPriceID,imiInventoryPrice,imiCurrencyRateID,5 As GroupType From PartPrices Where imiPartID = @PartID And imiPartRevisionID = @PartRevisionID AND imiOrganizationID = '' And imiLocationID = '' AND imiCustomerGroupID = '' And " + text2 + " Union All Select imiPartPriceID,imiInventoryPrice,imiCurrencyRateID,6 As GroupType From PartPrices Where imiPartID = '' And imiPartGroupID <> '' And imiPartGroupID = @PartGroupID AND imiOrganizationID = '' AND imiLocationID = '' AND imiCustomerGroupID = '' And " + text2 + " Union All Select imiPartPriceID,imiInventoryPrice,imiCurrencyRateID,7 As GroupType From PartPrices Where imiOrganizationID = @OrgID AND imiOrganizationID <> '' AND imiLocationID = @LocationID AND imiPartID = '' AND imiPartGroupID = '' And " + text2 + " Union All Select imiPartPriceID,imiInventoryPrice,imiCurrencyRateID,8 As GroupType From PartPrices Where imiOrganizationID = '' And imiLocationID = '' And imiCustomerGroupID <> '' And imiCustomerGroupID = @CustomerGroupID AND imiPartID = '' AND imiPartGroupID = '' And " + text2 + ") As data Order By GroupType Asc, CurrencyType Desc, imiCurrencyRateID Desc, imiPartPriceID Asc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@HomeCurrencyID", SqlDbType.NVarChar)).Value = database.HomeCurrencyID;
		sqlCommand.Parameters.Add(new SqlParameter("@CurrencyID", SqlDbType.NVarChar)).Value = currencyID;
		sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = orgID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locationID;
		sqlCommand.Parameters.Add(new SqlParameter("@CustomerGroupID", SqlDbType.NVarChar)).Value = text;
		sqlCommand.Parameters.Add(new SqlParameter("@PartGroupID", SqlDbType.NVarChar)).Value = partGroupID;
		sqlCommand.Parameters.Add(new SqlParameter("@PriceDate", SqlDbType.DateTime)).Value = priceDate.Value;
		sqlCommand.Parameters.Add(new SqlParameter("@PriceType", SqlDbType.TinyInt)).Value = priceType;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			PriceData priceData = new PriceData(dataRow.Field<int>("imiPartPriceID"), dataRow.Field<string>("imiCurrencyRateID"), dataRow.Field<bool>("imiInventoryPrice"), (PartPriceMatchType)Convert.ToByte(dataRow["GroupType"]));
			SqlCommand sqlCommand2 = database.NewSqlCommand("Select imjPartPriceBreakID,imjUnitPrice,imjQuantity,imjDiscount,imjLeadTime From PartPriceBreaks WHERE imjPartPriceID = @PartPriceID Order By imjPartPriceBreakID");
			sqlCommand2.Parameters.Add(new SqlParameter("@PartPriceID", SqlDbType.Int)).Value = priceData.ID;
			{
				foreach (DataRow row in database.GetDataTable(sqlCommand2, transaction).Rows)
				{
					priceData.Lines.Add(new PriceLineData(row.Field<short>("imjPartPriceBreakID"), row.Field<decimal>("imjQuantity"), row.Field<decimal>("imjUnitPrice"), row.Field<decimal>("imjDiscount"), row.Field<short>("imjLeadTime")));
				}
				return priceData;
			}
		}
		return null;
	}

	public void RenameRevision(M1Database database, string cOldPartID, string cOldPartRevisionID, string cNewPartID, string cNewPartRevisionID)
	{
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Update PartRevisions Set imrPartID = @NewPartID, imrPartRevisionID = @NewRevisionID Where imrPartID = @OldPartID And imrPartRevisionID = @OldRevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@NewPartID", SqlDbType.NVarChar)).Value = cNewPartID;
			sqlCommand.Parameters.Add(new SqlParameter("@NewRevisionID", SqlDbType.NVarChar)).Value = cNewPartRevisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@OldPartID", SqlDbType.NVarChar)).Value = cOldPartID;
			sqlCommand.Parameters.Add(new SqlParameter("@OldRevisionID", SqlDbType.NVarChar)).Value = cOldPartRevisionID;
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartAssemblies Set imaPartID = @NewPartID, imaPartRevisionID = @NewRevisionID Where imaPartID = @OldPartID And imaPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartAssemblies Set imaPartID = @NewPartID, imaPartRevisionID = @NewRevisionID Where imaMethodID = @OldPartID And imaMethodRevisionID = @OldRevisionID And imaMethodAssemblyID = 0";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartAssemblies Set imaMethodID = @NewPartID, imaMethodRevisionID = @NewRevisionID Where imaMethodID = @OldPartID And imaMethodRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartMaterials Set immPartID = @NewPartID, immPartRevisionID = @NewRevisionID Where immPartID = @OldPartID And immPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartMaterials Set immMethodID = @NewPartID, immMethodRevisionID = @NewRevisionID Where immMethodID = @OldPartID And immMethodRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartOperations Set imoPartID = @NewPartID, imoPartRevisionID = @NewRevisionID Where imoPartID = @OldPartID And imoPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartOperations Set imoMethodID = @NewPartID, imoMethodRevisionID = @NewRevisionID Where imoMethodID = @OldPartID And imoMethodRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartMemos Set imkPartID = @NewPartID, imkPartRevisionID = @NewRevisionID Where imkPartID = @OldPartID And imkPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartOrgReferences Set imzPartID = @NewPartID, imzPartRevisionID = @NewRevisionID Where imzPartID = @OldPartID And imzPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartAlternates Set imePartID = @NewPartID, imePartRevisionID = @NewRevisionID Where imePartID = @OldPartID And imePartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartBins Set imbPartID = @NewPartID, imbPartRevisionID = @NewRevisionID Where imbPartID = @OldPartID And imbPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartBinDetails Set imgPartID = @NewPartID, imgPartRevisionID = @NewRevisionID Where imgPartID = @OldPartID And imgPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartCrossReferences Set imxPartID = @NewPartID, imxPartRevisionID = @NewRevisionID Where imxPartID = @OldPartID And imxPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartPrices Set imiPartID = @NewPartID, imiPartRevisionID = @NewRevisionID Where imiPartID = @OldPartID And imiPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartWarehouseLocations Set imlPartID = @NewPartID, imlPartRevisionID = @NewRevisionID Where imlPartID = @OldPartID And imlPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartUnitSalePrices Set imhPartID = @NewPartID, imhPartRevisionID = @NewRevisionID Where imhPartID = @OldPartID And imhPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update SerialNumbers Set imsPartID = @NewPartID, imsPartRevisionID = @NewRevisionID Where imsPartID = @OldPartID And imsPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartRules Set pcrMethodID = @NewPartID, pcrMethodRevisionID = @NewRevisionID Where pcrMethodID = @OldPartID And pcrMethodRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartForecasts Set inpPartID = @NewPartID, inpPartRevisionID = @NewRevisionID Where inpPartID = @OldPartID And inpPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartForecastLines Set inlPartID = @NewPartID, inlPartRevisionID = @NewRevisionID Where inlPartID = @OldPartID And inlPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update LotNumbers Set ablPartID = @NewPartID, ablPartRevisionID = @NewRevisionID Where ablPartID = @OldPartID And ablPartRevisionID = @OldRevisionID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			string value = ProductConfigurator.GenerateFormIDForPart(cOldPartID, cOldPartRevisionID);
			string value2 = ProductConfigurator.GenerateFormIDForPart(cNewPartID, cNewPartRevisionID);
			sqlCommand.CommandText = "Update PartRevisions Set imrFormID = @NewFormID Where imrPartID = @NewPartID And imrPartRevisionID = @NewRevisionID";
			sqlCommand.Parameters.Clear();
			sqlCommand.Parameters.Add(new SqlParameter("@NewFormID", SqlDbType.NVarChar)).Value = value2;
			sqlCommand.Parameters.Add(new SqlParameter("@NewPartID", SqlDbType.NVarChar)).Value = cNewPartID;
			sqlCommand.Parameters.Add(new SqlParameter("@NewRevisionID", SqlDbType.NVarChar)).Value = cNewPartRevisionID;
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update FormDefinitions Set xaoFormID = @NewFormID Where xaoFormID = @OldFormID";
			sqlCommand.Parameters.Clear();
			sqlCommand.Parameters.Add(new SqlParameter("@NewFormID", SqlDbType.NVarChar)).Value = value2;
			sqlCommand.Parameters.Add(new SqlParameter("@OldFormID", SqlDbType.NVarChar)).Value = value;
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update FormInputValues Set xaiFormID = @NewFormID Where xaiFormID = @OldFormID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update FormInputValues Set xaiTopLevelFormID = @NewFormID Where xaiTopLevelFormID = @OldFormID";
			database.ExecuteCommand(sqlCommand, sqlTransaction);
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public void RenameBin(M1BindingSource bindingSource, string cOldPartID, string cOldPartRevisionID, string cOldWarehouseID, string cOldPartBinID, string cNewPartID, string cNewPartRevisionID, string cNewWarehouseID, string cNewPartBinID)
	{
		M1Database currentDatabase = bindingSource.CurrentDatabase;
		SqlTransaction sqlTransaction = currentDatabase.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = currentDatabase.NewSqlCommand("select * from PartWarehouseLocations where imlPartID = @PartID And imlPartRevisionID = @RevisionID And imlPartWarehouseID = @WarehouseID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = cNewPartID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = cNewPartRevisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = cNewWarehouseID;
			SqlDataAdapter adapter;
			DataTable dataTable = currentDatabase.GetDataTable(sqlCommand, fillSchema: false, out adapter, sqlTransaction);
			DataRow row;
			if (dataTable.Rows.Count == 0)
			{
				row = dataTable.AddBlankRow();
				row.SetField("imlPartID", cNewPartID);
				row.SetField("imlPartRevisionID", cNewPartRevisionID);
				row.SetField("imlPartWarehouseID", cNewWarehouseID);
				row.SetField("imlCreatedBy", currentDatabase.User.ID);
				row.SetField("imlCreatedDate", DateTime.Now);
				currentDatabase.UpdateData(dataTable, adapter, sqlTransaction);
			}
			row = bindingSource.CurrentAsDataRow;
			PartTransactionDefinition partTransactionDefinition = new PartTransactionDefinition(bindingSource, "imbQuantityOnHand", "imbPartBinID");
			partTransactionDefinition.TransactionType = 2;
			partTransactionDefinition.Source = 13;
			partTransactionDefinition.PartTransactionQuantityField = "imtInventoryQuantityReceived";
			partTransactionDefinition.AddTransaction(currentDatabase, row, DataRowVersion.Current, sqlTransaction, backoutQty: true);
			row["imbPartID"] = cNewPartID;
			row["imbPartRevisionID"] = cNewPartRevisionID;
			row["imbWarehouseID"] = cNewWarehouseID;
			row["imbPartBinID"] = cNewPartBinID;
			partTransactionDefinition.TransactionType = 1;
			partTransactionDefinition.Source = 13;
			partTransactionDefinition.AddTransaction(currentDatabase, row, DataRowVersion.Current, sqlTransaction, backoutQty: false);
			currentDatabase.UpdateData(new DataRow[1] { row }, bindingSource.Query.DataAdapter, sqlTransaction);
			sqlCommand = currentDatabase.NewSqlCommand("Update SalesOrderDeliveries Set omdPartID = @NewPartID, omdPartRevisionID = @NewRevisionID, omdPartWarehouseLocationID = @NewWarehouseID, omdPartBinID = @NewBinID Where omdPartID = @OldPartID And omdPartRevisionID = @OldRevisionID And omdPartWarehouseLocationID = @OldWarehouseID And omdPartBinID = @OldBinID And omdClosed = 0 And omdDeliveryType <> 3 And omdShippedComplete = 0");
			sqlCommand.Parameters.Add(new SqlParameter("@NewPartID", SqlDbType.NVarChar)).Value = cNewPartID;
			sqlCommand.Parameters.Add(new SqlParameter("@NewRevisionID", SqlDbType.NVarChar)).Value = cNewPartRevisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@NewWarehouseID", SqlDbType.NVarChar)).Value = cNewWarehouseID;
			sqlCommand.Parameters.Add(new SqlParameter("@NewBinID", SqlDbType.NVarChar)).Value = cNewPartBinID;
			sqlCommand.Parameters.Add(new SqlParameter("@OldPartID", SqlDbType.NVarChar)).Value = cOldPartID;
			sqlCommand.Parameters.Add(new SqlParameter("@OldRevisionID", SqlDbType.NVarChar)).Value = cOldPartRevisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@OldWarehouseID", SqlDbType.NVarChar)).Value = cOldWarehouseID;
			sqlCommand.Parameters.Add(new SqlParameter("@OldBinID", SqlDbType.NVarChar)).Value = cOldPartBinID;
			currentDatabase.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update JobMaterials Set jmmPartID = @NewPartID, jmmPartRevisionID = @NewRevisionID, jmmPartWarehouseLocationID = @NewWarehouseID, jmmPartBinID = @NewBinID Where jmmPartID = @OldPartID And jmmPartRevisionID = @OldRevisionID And jmmPartWarehouseLocationID = @OldWarehouseID And jmmPartBinID = @OldBinID And jmmClosed = 0 And jmmReceivedComplete = 0";
			currentDatabase.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update InventoryCountLines Set imqPartID = @NewPartID, imqPartRevisionID = @NewRevisionID, imqPartWarehouseLocationID = @NewWarehouseID, imqPartBinID = @NewBinID From InventoryCountLines Inner Join InventoryCounts On imqInventoryCountID = imnInventoryCountID Where imqPartID = @OldPartID And imqPartRevisionID = @OldRevisionID And imqPartWarehouseLocationID = @OldWarehouseID And imqPartBinID = @OldBinID And imnPostedToInventory = 0";
			currentDatabase.ExecuteCommand(sqlCommand, sqlTransaction);
			sqlCommand.CommandText = "Update PartMaterials Set immPartID = @NewPartID, immPartRevisionID = @NewRevisionID, immPartWarehouseLocationID = @NewWarehouseID, immPartBinID = @NewBinID Where immPartID = @OldPartID And immPartRevisionID = @OldRevisionID And immPartWarehouseLocationID = @OldWarehouseID And immPartBinID = @OldBinID And immUseDefaultWarehouseAndBin = 0";
			currentDatabase.ExecuteCommand(sqlCommand, sqlTransaction);
			currentDatabase.CommitTransaction(sqlTransaction);
			bindingSource.OnDataChanged(new DataChangedEventArgs(DataChangedFlag.CurrentRow));
		}
		catch
		{
			currentDatabase.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public void ChangeAllocations(M1Database database, SqlTransaction transaction, string oldPartID, string oldRevisionID, string oldWarehouseID, string oldBin, double oldQty, string newPartID, string newRevisionID, string newWarehouseID, string newBin, double newQty)
	{
		oldPartID = oldPartID.Trim();
		oldRevisionID = oldRevisionID.Trim();
		oldWarehouseID = oldWarehouseID.Trim();
		oldBin = oldBin.Trim();
		newPartID = newPartID.Trim();
		newRevisionID = newRevisionID.Trim();
		newWarehouseID = newWarehouseID.Trim();
		newBin = newBin.Trim();
		double num = newQty;
		bool flag = false;
		if (transaction == null)
		{
			flag = true;
			transaction = database.BeginTransaction();
		}
		try
		{
			if (oldPartID.Length != 0 && oldQty != 0.0)
			{
				if (oldPartID.Equals(newPartID, StringComparison.CurrentCultureIgnoreCase) && oldRevisionID.Equals(newRevisionID, StringComparison.CurrentCultureIgnoreCase) && oldWarehouseID.Equals(newWarehouseID, StringComparison.CurrentCultureIgnoreCase) && oldBin.Equals(newBin, StringComparison.CurrentCultureIgnoreCase))
				{
					num = ((num == oldQty) ? num : (num - oldQty));
				}
				else
				{
					doAllocBinUpdate(database, transaction, oldPartID, oldRevisionID, oldWarehouseID, oldBin, 0.0 - oldQty);
				}
				if (oldPartID.Equals(newPartID, StringComparison.CurrentCultureIgnoreCase) && oldRevisionID.Equals(newRevisionID, StringComparison.CurrentCultureIgnoreCase))
				{
					newQty = ((newQty == oldQty) ? newQty : (newQty - oldQty));
					oldPartID = string.Empty;
					oldQty = 0.0;
				}
				else
				{
					doAllocRevUpdate(database, transaction, oldPartID, oldRevisionID, 0.0 - oldQty);
				}
			}
			if (newPartID.Length != 0 && num != 0.0)
			{
				doAllocBinUpdate(database, transaction, newPartID, newRevisionID, newWarehouseID, newBin, num);
			}
			if (newPartID.Length != 0 && newQty != 0.0)
			{
				doAllocRevUpdate(database, transaction, newPartID, newRevisionID, newQty);
			}
			if (flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	private void doAllocBinUpdate(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, double qtyChange)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartBins SET imbQuantityAllocated = imbQuantityAllocated + @QtyChange WHERE imbPartID = @PartID And imbPartRevisionID = @RevisionID AND imbWarehouseID = @WarehouseID AND imbPartBinID = @BinID");
		sqlCommand.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		if (database.ExecuteCommand(sqlCommand, transaction) != 0)
		{
			return;
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("Select imrPartRevisionID From PartRevisions Where imrPartID = @Part And imrPartRevisionID = @Revision");
		sqlCommand2.Parameters.Add(new SqlParameter("@Part", SqlDbType.NVarChar)).Value = partID;
		sqlCommand2.Parameters.Add(new SqlParameter("@Revision", SqlDbType.NVarChar)).Value = revisionID;
		if (database.ExecuteScalar(sqlCommand2, transaction) == null)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(warehouseID))
		{
			SqlCommand sqlCommand3 = database.NewSqlCommand("INSERT INTO PartWarehouseLocations (imlPartID,imlPartRevisionID,imlPartWarehouseID) SELECT imrPartID,imrPartRevisionID,@WarehouseID FROM PartRevisions WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID And imrPartID+imrPartRevisionID NOT IN (SELECT imlPartID+imlPartRevisionID FROM PartWarehouseLocations WHERE imlPartID = @PartID And imlPartRevisionID = @RevisionID And imlPartWarehouseID = @WarehouseID)");
			sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand3.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
			sqlCommand3.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
			database.ExecuteCommand(sqlCommand3, transaction);
			if (!string.IsNullOrWhiteSpace(binID))
			{
				sqlCommand3 = database.NewSqlCommand("INSERT INTO PartBins (imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID,imbConversionFactor) VALUES (@PartID,@RevisionID,@WarehouseID,@BinID,1)");
				sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand3.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				sqlCommand3.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
				sqlCommand3.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
				database.ExecuteCommand(sqlCommand3, transaction);
			}
		}
		database.ExecuteCommand(sqlCommand, transaction);
	}

	private void doAllocRevUpdate(M1Database database, SqlTransaction transaction, string partID, string revisionID, double qtyChange)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE PartRevisions SET imrQuantityAllocated = imrQuantityAllocated + @QtyChange WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@QtyChange", SqlDbType.Decimal)).Value = qtyChange;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public void DeletePartAssembly(M1Database database, SqlTransaction transaction, string partID, string revisionID, int asmID, bool deleteInitAsm)
	{
		deleteMethodAssembly(database, transaction, partID, revisionID, asmID, deleteInitAsm);
	}

	public void DeletePartAssembly(M1Database database, SqlTransaction transaction, string partID, string revisionID, int asmID)
	{
		deleteMethodAssembly(database, transaction, partID, revisionID, asmID, deleteAssembly: false);
	}

	private void deleteMethodAssembly(M1Database database, SqlTransaction transaction, string partID, string revisionID, int asmID, bool deleteAssembly)
	{
		if (string.IsNullOrWhiteSpace(partID))
		{
			throw new M1Exception("Part ID is required.");
		}
		bool flag = false;
		if (transaction == null)
		{
			flag = true;
			transaction = database.BeginTransaction();
		}
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select imaMethodAssemblyID,imaParentAssemblyID from PartAssemblies where imaMethodID = @PartID and imaMethodRevisionID = @RevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
			SqlDataAdapter adapter;
			DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, transaction);
			if (dataTable.Rows.Count != 0)
			{
				deleteNextAsmLevel(database, transaction, dataTable, partID, revisionID, asmID);
				deleteAsm(database, transaction, partID, revisionID, asmID, deleteAssembly);
			}
			if (flag)
			{
				database.CommitTransaction(transaction);
			}
		}
		catch
		{
			if (flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	private void deleteAsm(M1Database database, SqlTransaction transaction, string partID, string revisionID, int asmID, bool deleteAsm)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("DELETE FormInputValues FROM FormInputValues INNER JOIN PartAssemblies On xaiSourceUniqueID = imaUniqueID WHERE imaMethodID = @PartID And imaMethodRevisionID = @RevisionID And imaMethodAssemblyID = @AsmID And xaiSourceTable = 'PARTASSEMBLIES'");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("DELETE FROM PartMaterials WHERE immMethodID = @PartID And immMethodRevisionID = @RevisionID AND immMethodAssemblyID = @AsmID\rDELETE FROM PartOperations WHERE imoMethodID = @PartID And imoMethodRevisionID = @RevisionID AND imoMethodAssemblyID = @AsmID\rDELETE FROM PartRules WHERE pcrMethodID = @PartID And pcrMethodRevisionID = @RevisionID AND pcrMethodAssemblyID = @AsmID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
		database.ExecuteCommand(sqlCommand, transaction);
		if (deleteAsm)
		{
			sqlCommand = database.NewSqlCommand("DELETE FROM PartAssemblies WHERE imaMethodID = @PartID And imaMethodRevisionID = @RevisionID AND imaMethodAssemblyID = @AsmID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = asmID;
			database.ExecuteCommand(sqlCommand, transaction);
		}
	}

	private void deleteNextAsmLevel(M1Database database, SqlTransaction transaction, DataTable assembliesTable, string partID, string revisionID, int parentAsm)
	{
		DataRow[] array = assembliesTable.Select("imaParentAssemblyID = " + M1Util.ConvertToLinq(parentAsm) + " and imaMethodAssemblyID <> 0");
		foreach (DataRow dataRow in array)
		{
			deleteNextAsmLevel(database, transaction, assembliesTable, partID, revisionID, Convert.ToInt32(dataRow["imaMethodAssemblyID"]));
			deleteAsm(database, transaction, partID, revisionID, Convert.ToInt32(dataRow["imaMethodAssemblyID"]), deleteAsm: true);
		}
	}

	public decimal GetCurrentForecastQty(M1Database database, string partID, string partRevisionID, DateTime? date)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select Top 1 IsNull(inlForecastQuantity - inlActualQuantity,0) from partforecastlines where inlPartID = @PartID and inlPartRevisionID = @RevisionID and @CheckDate >= inlStartDate and @CheckDate < inlEndDate");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@CheckDate", SqlDbType.DateTime)).Value = (date.HasValue ? date.Value : DateTime.Today);
		return Convert.ToDecimal(database.ExecuteScalar(sqlCommand));
	}

	public void CopyRevision(M1Database database, string partID, string sourcePartRevisionID, string destPartRevisionID, DateTime? startDate, bool copyPrices, bool copyMemos, bool copyRules, bool copyAlternates, bool copyOrgReferences)
	{
		List<InsertKeys> list = new List<InsertKeys>();
		StringBuilder stringBuilder = new StringBuilder();
		list.Add(new InsertKeys("imrPartID", partID, partID));
		list.Add(new InsertKeys("imrPartRevisionID", sourcePartRevisionID, destPartRevisionID));
		genInsert(database, "PartRevisions", list, stringBuilder);
		list.Clear();
		list.Add(new InsertKeys("imaMethodID", partID, partID));
		list.Add(new InsertKeys("imaMethodRevisionID", sourcePartRevisionID, destPartRevisionID));
		genInsert(database, "PartAssemblies", list, stringBuilder);
		list.Clear();
		list.Add(new InsertKeys("immMethodID", partID, partID));
		list.Add(new InsertKeys("immMethodRevisionID", sourcePartRevisionID, destPartRevisionID));
		genInsert(database, "PartMaterials", list, stringBuilder);
		list.Clear();
		list.Add(new InsertKeys("imoMethodID", partID, partID));
		list.Add(new InsertKeys("imoMethodRevisionID", sourcePartRevisionID, destPartRevisionID));
		genInsert(database, "PartOperations", list, stringBuilder);
		list.Clear();
		if (copyOrgReferences)
		{
			list.Add(new InsertKeys("imzPartID", partID, partID));
			list.Add(new InsertKeys("imzPartRevisionID", sourcePartRevisionID, destPartRevisionID));
			genInsert(database, "PartOrgReferences", list, stringBuilder);
			list.Clear();
			list.Add(new InsertKeys("imxPartID", partID, partID));
			list.Add(new InsertKeys("imxPartRevisionID", sourcePartRevisionID, destPartRevisionID));
			genInsert(database, "PartCrossReferences", list, stringBuilder);
			list.Clear();
		}
		list.Add(new InsertKeys("imhPartID", partID, partID));
		list.Add(new InsertKeys("imhPartRevisionID", sourcePartRevisionID, destPartRevisionID));
		genInsert(database, "PartUnitSalePrices", list, stringBuilder);
		list.Clear();
		list.Add(new InsertKeys("imlPartID", partID, partID));
		list.Add(new InsertKeys("imlPartRevisionID", sourcePartRevisionID, destPartRevisionID));
		genInsert(database, "PartWarehouseLocations", list, stringBuilder);
		list.Clear();
		list.Add(new InsertKeys("imbPartID", partID, partID));
		list.Add(new InsertKeys("imbPartRevisionID", sourcePartRevisionID, destPartRevisionID));
		genInsert(database, "PartBins", list, stringBuilder);
		list.Clear();
		list.Add(new InsertKeys("xazPartID", partID, partID));
		list.Add(new InsertKeys("xazPartRevisionID", sourcePartRevisionID, destPartRevisionID));
		genInsert(database, "ExpenseAccountSplits", list, stringBuilder);
		list.Clear();
		if (copyMemos)
		{
			list.Add(new InsertKeys("imkPartID", partID, partID));
			list.Add(new InsertKeys("imkPartRevisionID", sourcePartRevisionID, destPartRevisionID));
			genInsert(database, "PartMemos", list, stringBuilder);
			list.Clear();
		}
		if (copyAlternates)
		{
			list.Add(new InsertKeys("imePartID", partID, partID));
			list.Add(new InsertKeys("imePartRevisionID", sourcePartRevisionID, destPartRevisionID));
			genInsert(database, "PartAlternates", list, stringBuilder);
			list.Clear();
		}
		if (copyRules)
		{
			list.Add(new InsertKeys("pcrMethodID", partID, partID));
			list.Add(new InsertKeys("pcrMethodRevisionID", sourcePartRevisionID, destPartRevisionID));
			genInsert(database, "PartRules", list, stringBuilder);
			list.Clear();
			list.Add(new InsertKeys("xaoFormID", ProductConfigurator.GenerateFormIDForPart(partID, sourcePartRevisionID), ProductConfigurator.GenerateFormIDForPart(partID, destPartRevisionID)));
			genInsert(database, "FormDefinitions", list, stringBuilder);
			list.Clear();
		}
		DataTable dataTable = null;
		SqlDataAdapter adapter = null;
		DataTable dataTable2 = null;
		SqlDataAdapter adapter2 = null;
		if (copyPrices)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select * From PartPrices Where imiPartID = @PartID And imiPartRevisionID = @RevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = sourcePartRevisionID;
			dataTable = database.GetDataTable(sqlCommand, fillSchema: false, out adapter);
			sqlCommand = database.NewSqlCommand("select * from PartPriceBreaks Where imjPartPriceID In (Select imiPartPriceID From PartPrices Where imiPartID = @PartID And imiPartRevisionID = @RevisionID) Order By imjPartPriceID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = sourcePartRevisionID;
			dataTable2 = database.GetDataTable(sqlCommand, fillSchema: false, out adapter2);
			int num = Convert.ToInt32(database.NextIDs.GetNextIDForTable("PartPrices"));
			foreach (DataRow row in dataTable.Rows)
			{
				DataRow[] array = dataTable2.Select("imjPartPriceID = " + row.Field<int>("imiPartPriceID").ToLinq());
				foreach (DataRow obj in array)
				{
					obj.SetAdded();
					obj["imjPartPriceID"] = num;
					obj["imjUniqueID"] = Guid.NewGuid();
				}
				row.SetAdded();
				row["imiPartPriceID"] = num;
				row["imiUniqueID"] = Guid.NewGuid();
				row["imiPartRevisionID"] = destPartRevisionID;
				num++;
			}
		}
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			database.ExecuteCommand(stringBuilder.ToString(), sqlTransaction);
			database.ExecuteCommand("UPDATE PartRevisions SET imrQuantityOnHand = 0, imrQuantityAllocated = 0, imrEffectiveStartDate = " + M1Util.ConvertToSql(startDate) + ", imrEffectiveEndDate = Null, imrQuantityToInspect = 0, imrQuantityOnOrderPurchases = 0, imrQuantityOnOrderSales = 0 Where imrPartID = " + M1Util.ConvertToSql(partID) + " And imrPartRevisionID = " + M1Util.ConvertToSql(destPartRevisionID), sqlTransaction);
			database.ExecuteCommand("UPDATE PartAssemblies SET imaPartID = " + M1Util.ConvertToSql(partID) + ", imaPartRevisionID = " + M1Util.ConvertToSql(destPartRevisionID) + " Where imaMethodID = " + M1Util.ConvertToSql(partID) + " And imaMethodRevisionID = " + M1Util.ConvertToSql(destPartRevisionID) + " And imaMethodAssemblyID = 0", sqlTransaction);
			database.ExecuteCommand("UPDATE PartOperations SET imoPartID = " + M1Util.ConvertToSql(partID) + ", imoPartRevisionID = " + M1Util.ConvertToSql(destPartRevisionID) + " Where imoMethodID = " + M1Util.ConvertToSql(partID) + " And imoMethodRevisionID = " + M1Util.ConvertToSql(destPartRevisionID) + " And imoPartID = " + M1Util.ConvertToSql(partID) + " And imoPartRevisionID = " + M1Util.ConvertToSql(sourcePartRevisionID), sqlTransaction);
			database.ExecuteCommand("UPDATE PartWarehouseLocations SET imlQuantityInTransit = 0 Where imlPartID = " + M1Util.ConvertToSql(partID) + " And imlPartRevisionID = " + M1Util.ConvertToSql(destPartRevisionID), sqlTransaction);
			database.ExecuteCommand("UPDATE PartBins SET imbQuantityOnHand = 0, imbBinQuantityOnHand = 0, imbQuantityAllocated = 0, imbQuantityToInspect = 0, imbQuantityToReturn = 0, imbQuantityOnOrderPurchases = 0, imbQuantityOnOrderSales = 0 Where imbPartID = " + M1Util.ConvertToSql(partID) + " And imbPartRevisionID = " + M1Util.ConvertToSql(destPartRevisionID), sqlTransaction);
			AppAxProduction appAxProduction = new AppAxProduction(database);
			NewPartRevCheck(database, partID, destPartRevisionID, appAxProduction.PlantID, sqlTransaction);
			if (copyPrices && dataTable.Rows.Count != 0)
			{
				database.UpdateData(dataTable, adapter, sqlTransaction);
				database.UpdateData(dataTable2, adapter2, sqlTransaction);
			}
			CopyCalculatorBarEx(database, "PartRevisions", partID, sourcePartRevisionID, partID, destPartRevisionID, sqlTransaction);
			CopyCalculatorPunchEx(database, "PartRevisions", partID, sourcePartRevisionID, partID, destPartRevisionID, sqlTransaction);
			CopyCalculatorSheetEx(database, "PartRevisions", partID, sourcePartRevisionID, partID, destPartRevisionID, sqlTransaction);
			CopyCalculatorLaserEx(database, "PartRevisions", partID, sourcePartRevisionID, partID, destPartRevisionID, sqlTransaction);
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	private void genInsert(M1Database database, string table, List<InsertKeys> keys, StringBuilder builder)
	{
		string insertStatement = SaveAsProcessing.GetInsertStatement(database, table, keys);
		if (insertStatement.Length == 0)
		{
			throw new M1Exception("Unable to generate insert statement.");
		}
		builder.AppendLine(insertStatement);
	}

	public void NewPartRevCheck(M1Database database, string partID, string revisionID, string plantID, SqlTransaction transaction)
	{
		string defaultWarehouseByPlant = GetDefaultWarehouseByPlant(database, transaction, plantID);
		string defaultBinOfGivenWarehouse = GetDefaultBinOfGivenWarehouse(database, transaction, defaultWarehouseByPlant);
		SqlCommand sqlCommand = database.NewSqlCommand("INSERT INTO PartBins (imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID,imbQuantityOnHand,imbBinQuantityOnHand,imbConversionFactor,imbQuantityAllocated,imbCreatedBy,imbCreatedDate) SELECT imrPartID as imbPartID,imrPartRevisionID as imbPartRevisionID, @WarehouseID As imbWarehouseID, @BinID As imbPartBinID,imrQuantityOnHand as imbQuantityOnHand,imrQuantityOnHand as imbBinQuantityOnHand,1 as imbConversionFactor,imrQuantityAllocated as imbQuantityAllocated,@UserID as imbCreatedBy,GetDate() as imbCreatedDate FROM PartRevisions WHERE imrPartID = @PartID And imrPartRevisionID = @RevisionID And imrPartID+imrPartRevisionID NOT IN (SELECT imbPartID+imbPartRevisionID FROM PartBins WHERE imbPartID = @PartID And imbPartRevisionID = @RevisionID)");
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = defaultWarehouseByPlant;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = defaultBinOfGivenWarehouse;
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = database.User.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("INSERT INTO PartWarehouseLocations (imlPartID,imlPartRevisionID,imlPartWarehouseID,imlCreatedBy,imlCreatedDate) SELECT imbPartID,imbPartRevisionID,imbWarehouseID,@UserID as imlCreatedBy,GetDate() as imlCreatedDate FROM PartBins WHERE imbPartID = @PartID And imbPartRevisionID = @RevisionID And imbPartID+imbPartRevisionID+imbWarehouseID NOT IN (SELECT imlPartID+imlPartRevisionID+imlPartWarehouseID FROM PartWarehouseLocations WHERE imlPartID = @PartID And imlPartRevisionID = @RevisionID)");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = database.User.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public void CreatePartAndRevision(M1Database database, string partID, string partRevisionID, string description, string longDescriptionRTF, string longDescriptionText, byte partType, string orgID, string locID, string invUoM, string purUoM, decimal conversionFactor, decimal leadTime, string plantID, SqlTransaction transaction)
	{
		if (string.IsNullOrWhiteSpace(description))
		{
			description = partID;
		}
		M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
		m1BindingSource.DataSourceTable = "PARTS";
		m1BindingSource.NavigateTo(database, "impPartID = " + M1Util.ConvertToSql(partID));
		if (m1BindingSource.Count == 0)
		{
			DataRow obj = m1BindingSource.AddNew() as DataRow;
			obj["impPartID"] = partID;
			obj["impShortDescription"] = description;
			obj["impLongDescriptionRTF"] = longDescriptionRTF;
			obj["impLongDescriptionText"] = longDescriptionText;
			obj["impPartType"] = partType;
			m1BindingSource.SaveData();
		}
		M1BindingSource childBindingSource = m1BindingSource.PrimaryTable.GetChildBindingSource("PartRevisions");
		childBindingSource.NavigateTo(database, "imrPartID = " + M1Util.ConvertToSql(partID) + " And imrPartRevisionID = " + M1Util.ConvertToSql(partRevisionID));
		if (childBindingSource.Count == 0)
		{
			DataRow obj2 = childBindingSource.AddNew() as DataRow;
			obj2["imrPartID"] = partID;
			obj2["imrPartRevisionID"] = partRevisionID;
			obj2["imrShortDescription"] = description;
			obj2["imrLongDescriptionRTF"] = longDescriptionRTF;
			obj2["imrLongDescriptionText"] = longDescriptionText;
			obj2["imrEffectiveStartDate"] = DateTime.Today;
			if (conversionFactor == 0m)
			{
				conversionFactor = 1m;
			}
			obj2["imrConversionFactor"] = conversionFactor;
			obj2["imrInventoryUnitOfMeasure"] = invUoM;
			obj2["imrPurchaseUnitOfMeasure"] = purUoM;
			obj2["imrLeadTime"] = leadTime;
			obj2["imrSupplierOrganizationID"] = orgID;
			obj2["imrPurchaseLocationID"] = locID;
			childBindingSource.SaveData();
		}
	}

	public void CreatePartCrossRef(M1Database database, string partID, string partRevisionID, string orgPartID, string orgID, string locID, string orgDescription, string purUoM, decimal conversionFactor, SqlTransaction transaction)
	{
		if (string.IsNullOrWhiteSpace(orgID))
		{
			return;
		}
		M1BindingSource m1BindingSource = new M1BindingSource(database, transaction);
		m1BindingSource.DataSourceTable = "PARTORGREFERENCES";
		m1BindingSource.NavigateTo(database, "imzPartID = " + M1Util.ConvertToSql(partID) + " And imzPartRevisionID = " + M1Util.ConvertToSql(partRevisionID) + " And imzOrganizationID = " + M1Util.ConvertToSql(orgID));
		if (m1BindingSource.Count == 0)
		{
			DataRow dataRow = m1BindingSource.AddNew(database, null, new object[3] { partID, partRevisionID, orgID }, null) as DataRow;
			dataRow["imzOrgPartID"] = orgPartID;
			if (!string.IsNullOrWhiteSpace(orgDescription))
			{
				dataRow["imzOrgPartShortDescription"] = orgDescription;
			}
			if (!string.IsNullOrWhiteSpace(purUoM))
			{
				dataRow["imzPurchaseUnitOfMeasure"] = purUoM;
			}
			if (conversionFactor != 0m)
			{
				dataRow["imzConversionFactor"] = conversionFactor;
			}
			m1BindingSource.SaveData();
		}
		if (string.IsNullOrWhiteSpace(locID))
		{
			return;
		}
		M1BindingSource m1BindingSource2 = new M1BindingSource(database, transaction);
		m1BindingSource2.DataSourceTable = "PARTCROSSREFERENCES";
		m1BindingSource2.NavigateTo(database, "imxPartID = " + M1Util.ConvertToSql(partID) + " And imxPartRevisionID = " + M1Util.ConvertToSql(partRevisionID) + " And imxOrganizationID = " + M1Util.ConvertToSql(orgID) + " And imxLocationID = " + M1Util.ConvertToSql(locID));
		if (m1BindingSource2.Count == 0)
		{
			DataRow dataRow = m1BindingSource2.AddNew(database, null, new object[4] { partID, partRevisionID, orgID, locID }, null) as DataRow;
			dataRow["imxOrgPartID"] = orgPartID;
			if (!string.IsNullOrWhiteSpace(orgDescription))
			{
				dataRow["imxOrgPartShortDescription"] = orgDescription;
			}
			if (!string.IsNullOrWhiteSpace(purUoM))
			{
				dataRow["imxPurchaseUnitOfMeasure"] = purUoM;
			}
			if (conversionFactor != 0m)
			{
				dataRow["imxConversionFactor"] = conversionFactor;
			}
			m1BindingSource2.SaveData();
		}
	}

	public bool IsBinActive(M1Database database, string warehouseID, string binID)
	{
		if (binID == string.Empty)
		{
			return true;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select inbWarehouseBinID From WarehouseBins Where inbInactive = 0 and inbWarehouseID = @WarehouseID And inbWarehouseBinID = @BinID");
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = binID;
		return !string.IsNullOrWhiteSpace(Convert.ToString(database.ExecuteScalar(sqlCommand)));
	}

	private void CopyCalculatorBarEx(M1Database database, string table, string sourceID, object source2, string destID, object dest2, SqlTransaction transaction)
	{
		string calculatorFields = getCalculatorFields(database, "BarCalculators");
		string text = "ccbBarCalculatorID" + calculatorFields;
		if (table.Equals("QuoteLines", StringComparison.CurrentCultureIgnoreCase))
		{
			string text2 = "DestTable.qmmUniqueID As ccbBarCalculatorID" + calculatorFields;
			database.ExecuteCommand("Delete BarCalculators From BarCalculators Inner Join QuoteMaterials On ccbBarCalculatorID = qmmUniqueID Where qmmQuoteID = " + M1Util.ConvertToSql(destID) + " And qmmQuoteLineID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into BarCalculators (" + text + ") Select " + text2 + " From BarCalculators Inner Join QuoteMaterials SourceTable On ccbBarCalculatorID = SourceTable.qmmUniqueID Inner Join QuoteMaterials DestTable On DestTable.qmmQuoteID = " + M1Util.ConvertToSql(destID) + " And DestTable.qmmQuoteLineID = " + M1Util.ConvertToSql(dest2) + " And DestTable.qmmQuoteAssemblyID = SourceTable.qmmQuoteAssemblyID And DestTable.qmmQuoteMaterialID = SourceTable.qmmQuoteMaterialID Where SourceTable.qmmQuoteID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.qmmQuoteLineID = " + M1Util.ConvertToSql(source2), transaction);
		}
		else if (table.Equals("PartRevisions", StringComparison.CurrentCultureIgnoreCase))
		{
			string text2 = "DestTable.immUniqueID As ccbBarCalculatorID" + calculatorFields;
			database.ExecuteCommand("Delete BarCalculators From BarCalculators Inner Join PartMaterials On ccbBarCalculatorID = immUniqueID Where immMethodID = " + M1Util.ConvertToSql(destID) + " And immMethodRevisionID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into BarCalculators (" + text + ") Select " + text2 + " From BarCalculators Inner Join PartMaterials SourceTable On ccbBarCalculatorID = SourceTable.immUniqueID Inner Join PartMaterials DestTable On DestTable.immMethodID = " + M1Util.ConvertToSql(destID) + " And DestTable.immMethodRevisionID = " + M1Util.ConvertToSql(dest2) + " And DestTable.immMethodAssemblyID = SourceTable.immMethodAssemblyID And DestTable.immMethodMaterialID = SourceTable.immMethodMaterialID Where SourceTable.immMethodID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.immMethodRevisionID = " + M1Util.ConvertToSql(source2), transaction);
		}
	}

	private void CopyCalculatorPunchEx(M1Database database, string table, string sourceID, object source2, string destID, object dest2, SqlTransaction transaction)
	{
		string calculatorFields = getCalculatorFields(database, "PunchCalculators");
		string text = "ccuPunchCalculatorID" + calculatorFields;
		if (table.Equals("QuoteLines", StringComparison.CurrentCultureIgnoreCase))
		{
			string text2 = "DestTable.qmoUniqueID As ccuPunchCalculatorID" + calculatorFields;
			database.ExecuteCommand("Delete PunchCalculators From PunchCalculators Inner Join QuoteOperations On ccuPunchCalculatorID = qmoUniqueID Where qmoQuoteID = " + M1Util.ConvertToSql(destID) + " And qmoQuoteLineID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into PunchCalculators (" + text + ") Select " + text2 + " From PunchCalculators Inner Join QuoteOperations SourceTable On ccuPunchCalculatorID = SourceTable.qmoUniqueID Inner Join QuoteOperations DestTable On DestTable.qmoQuoteID = " + M1Util.ConvertToSql(destID) + " And DestTable.qmoQuoteLineID = " + M1Util.ConvertToSql(dest2) + " And DestTable.qmoQuoteAssemblyID = SourceTable.qmoQuoteAssemblyID And DestTable.qmoQuoteOperationID = SourceTable.qmoQuoteOperationID Where SourceTable.qmoQuoteID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.qmoQuoteLineID = " + M1Util.ConvertToSql(source2), transaction);
		}
		else if (table.Equals("PartRevisions", StringComparison.CurrentCultureIgnoreCase))
		{
			string text2 = "DestTable.imoUniqueID As ccuPunchCalculatorID" + calculatorFields;
			database.ExecuteCommand("Delete PunchCalculators From PunchCalculators Inner Join PartOperations On ccuPunchCalculatorID = imoUniqueID Where imoMethodID = " + M1Util.ConvertToSql(destID) + " And imoMethodRevisionID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into PunchCalculators (" + text + ") Select " + text2 + " From PunchCalculators Inner Join PartOperations SourceTable On ccuPunchCalculatorID = SourceTable.imoUniqueID Inner Join PartOperations DestTable On DestTable.imoMethodID = " + M1Util.ConvertToSql(destID) + " And DestTable.imoMethodRevisionID = " + M1Util.ConvertToSql(dest2) + " And DestTable.imoMethodAssemblyID = SourceTable.imoMethodAssemblyID And DestTable.imoMethodOperationID = SourceTable.imoMethodOperationID Where SourceTable.imoMethodID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.imoMethodRevisionID = " + M1Util.ConvertToSql(source2), transaction);
		}
	}

	private void CopyCalculatorSheetEx(M1Database database, string table, string sourceID, object source2, string destID, object dest2, SqlTransaction transaction)
	{
		string calculatorFields = getCalculatorFields(database, "SheetCalculators");
		string text = "ccsSheetCalculatorID" + calculatorFields;
		if (table.Equals("QuoteLines", StringComparison.CurrentCultureIgnoreCase))
		{
			string text2 = "DestTable.qmmUniqueID As ccsSheetCalculatorID" + calculatorFields;
			database.ExecuteCommand("Delete SheetCalculators From SheetCalculators Inner Join QuoteMaterials On ccsSheetCalculatorID = qmmUniqueID Where qmmQuoteID = " + M1Util.ConvertToSql(destID) + " And qmmQuoteLineID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into SheetCalculators (" + text + ") Select " + text2 + " From SheetCalculators Inner Join QuoteMaterials SourceTable On ccsSheetCalculatorID = SourceTable.qmmUniqueID Inner Join QuoteMaterials DestTable On DestTable.qmmQuoteID = " + M1Util.ConvertToSql(destID) + " And DestTable.qmmQuoteLineID = " + M1Util.ConvertToSql(dest2) + " And DestTable.qmmQuoteAssemblyID = SourceTable.qmmQuoteAssemblyID And DestTable.qmmQuoteMaterialID = SourceTable.qmmQuoteMaterialID Where SourceTable.qmmQuoteID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.qmmQuoteLineID = " + M1Util.ConvertToSql(source2), transaction);
		}
		else if (table.Equals("PartRevisions", StringComparison.CurrentCultureIgnoreCase))
		{
			string text2 = "DestTable.immUniqueID As ccsSheetCalculatorID" + calculatorFields;
			database.ExecuteCommand("Delete SheetCalculators From SheetCalculators Inner Join PartMaterials On ccsSheetCalculatorID = immUniqueID Where immMethodID = " + M1Util.ConvertToSql(destID) + " And immMethodRevisionID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into SheetCalculators (" + text + ") Select " + text2 + " From SheetCalculators Inner Join PartMaterials SourceTable On ccsSheetCalculatorID = SourceTable.immUniqueID Inner Join PartMaterials DestTable On DestTable.immMethodID = " + M1Util.ConvertToSql(destID) + " And DestTable.immMethodRevisionID = " + M1Util.ConvertToSql(dest2) + " And DestTable.immMethodAssemblyID = SourceTable.immMethodAssemblyID And DestTable.immMethodMaterialID = SourceTable.immMethodMaterialID Where SourceTable.immMethodID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.immMethodRevisionID = " + M1Util.ConvertToSql(source2), transaction);
		}
	}

	private void CopyCalculatorLaserEx(M1Database database, string table, string sourceID, object source2, string destID, object dest2, SqlTransaction transaction)
	{
		string calculatorFields = getCalculatorFields(database, "LaserCalculators");
		string text = "ccpLaserCalculatorId" + calculatorFields;
		string calculatorFields2 = getCalculatorFields(database, "LaserCalculatorLines");
		string text2 = "cclLaserCalculatorID" + calculatorFields2;
		if (table.Equals("QuoteLines", StringComparison.CurrentCultureIgnoreCase))
		{
			string text3 = "DestTable.qmoUniqueID As ccpLaserCalculatorId" + calculatorFields;
			database.ExecuteCommand("Delete LaserCalculators From LaserCalculators Inner Join QuoteOperations On ccpLaserCalculatorId = qmoUniqueID Where qmoQuoteID = " + M1Util.ConvertToSql(destID) + " And qmoQuoteLineID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into LaserCalculators (" + text + ") Select " + text3 + " From LaserCalculators Inner Join QuoteOperations SourceTable On ccpLaserCalculatorId = SourceTable.qmoUniqueID Inner Join QuoteOperations DestTable On DestTable.qmoQuoteID = " + M1Util.ConvertToSql(destID) + " And DestTable.qmoQuoteLineID = " + M1Util.ConvertToSql(dest2) + " And DestTable.qmoQuoteAssemblyID = SourceTable.qmoQuoteAssemblyID And DestTable.qmoQuoteOperationID = SourceTable.qmoQuoteOperationID Where SourceTable.qmoQuoteID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.qmoQuoteLineID = " + M1Util.ConvertToSql(source2), transaction);
			string text4 = "DestTable.qmoUniqueID As cclLaserCalculatorID" + calculatorFields2;
			database.ExecuteCommand("Delete LaserCalculatorLines From LaserCalculatorLines Inner Join QuoteOperations On cclLaserCalculatorID = qmoUniqueID Where qmoQuoteID = " + M1Util.ConvertToSql(destID) + " And qmoQuoteLineID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into LaserCalculatorLines (" + text2 + ") Select " + text4 + " From LaserCalculatorLines Inner Join QuoteOperations SourceTable On cclLaserCalculatorID = SourceTable.qmoUniqueID Inner Join QuoteOperations DestTable On DestTable.qmoQuoteID = " + M1Util.ConvertToSql(destID) + " And DestTable.qmoQuoteLineID = " + M1Util.ConvertToSql(dest2) + " And DestTable.qmoQuoteAssemblyID = SourceTable.qmoQuoteAssemblyID And DestTable.qmoQuoteOperationID = SourceTable.qmoQuoteOperationID Where SourceTable.qmoQuoteID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.qmoQuoteLineID = " + M1Util.ConvertToSql(source2), transaction);
		}
		else if (table.Equals("PartRevisions", StringComparison.CurrentCultureIgnoreCase))
		{
			string text3 = "DestTable.imoUniqueID As ccpLaserCalculatorId" + calculatorFields;
			database.ExecuteCommand("Delete LaserCalculators From LaserCalculators Inner Join PartOperations On ccpLaserCalculatorId = imoUniqueID Where imoMethodID = " + M1Util.ConvertToSql(destID) + " And imoMethodRevisionID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into LaserCalculators (" + text + ") Select " + text3 + " From LaserCalculators Inner Join PartOperations SourceTable On ccpLaserCalculatorId = SourceTable.imoUniqueID Inner Join PartOperations DestTable On DestTable.imoMethodID = " + M1Util.ConvertToSql(destID) + " And DestTable.imoMethodRevisionID = " + M1Util.ConvertToSql(dest2) + " And DestTable.imoMethodAssemblyID = SourceTable.imoMethodAssemblyID And DestTable.imoMethodOperationID = SourceTable.imoMethodOperationID Where SourceTable.imoMethodID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.imoMethodRevisionID = " + M1Util.ConvertToSql(source2), transaction);
			string text4 = "DestTable.imoUniqueID As cclLaserCalculatorID" + calculatorFields2;
			database.ExecuteCommand("Delete LaserCalculatorLines From LaserCalculatorLines Inner Join PartOperations On cclLaserCalculatorID = imoUniqueID Where imoMethodID = " + M1Util.ConvertToSql(destID) + " And imoMethodRevisionID = " + M1Util.ConvertToSql(dest2), transaction);
			database.ExecuteCommand("Insert Into LaserCalculatorLines (" + text2 + ") Select " + text4 + " From LaserCalculatorLines Inner Join PartOperations SourceTable On cclLaserCalculatorID = SourceTable.imoUniqueID Inner Join PartOperations DestTable On DestTable.imoMethodID = " + M1Util.ConvertToSql(destID) + " And DestTable.imoMethodRevisionID = " + M1Util.ConvertToSql(dest2) + " And DestTable.imoMethodAssemblyID = SourceTable.imoMethodAssemblyID And DestTable.imoMethodOperationID = SourceTable.imoMethodOperationID Where SourceTable.imoMethodID = " + M1Util.ConvertToSql(sourceID) + " And SourceTable.imoMethodRevisionID = " + M1Util.ConvertToSql(source2), transaction);
		}
	}

	private string getCalculatorFields(M1Database database, string table)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataColumn column in database.GetDataTable("Select * From " + table + " Where 0=1").Columns)
		{
			if (!SystemGeneratedFields.IsGenerated(column.ColumnName) && !column.ColumnName.EndsWith("CalculatorID", StringComparison.CurrentCultureIgnoreCase))
			{
				stringBuilder.Append("," + column.ColumnName);
			}
		}
		return stringBuilder.ToString();
	}

	public bool IsSerialOrLotTracked(M1Database database, string partID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(Case When impTrackLotNumbers = 1 Or impTrackSerialNumbers = 1 Then Convert(bit,1) Else Convert(bit,0) End,0) From Parts Where impPartID = @PartID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public bool IsKitPart(M1Database database, string partID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(impPhantomOrKitPart,0) From Parts Where impPartID = @PartID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand));
	}

	public PartCost GetPartCosts(M1Database database, SqlTransaction transaction, string partID, string partRevisionID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select imrAverageLaborCost,imrAverageOverheadCost,imrAverageMaterialCost,imrAverageSubcontractCost,imrAverageDutyCost,imrAverageFreightCost,imrAverageMiscCost,imrLastLaborCost,imrLastOverheadCost,imrLastMaterialCost,imrLastSubcontractCost,imrLastDutyCost,imrLastFreightCost,imrLastMiscCost,imrStandardLaborCost,imrStandardOverheadCost,imrStandardMaterialCost,imrStandardSubcontractCost,imrStandardDutyCost,imrStandardFreightCost,imrStandardMiscCost from PartRevisions inner join parts on imppartid = imrpartid where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			DataRow row = dataTable.Rows[0];
			PartCost partCost = new PartCost();
			switch (database.Props("IM").Field<byte>("xapIMCostingMethod"))
			{
			case 2:
				partCost.LaborCost = row.Field<decimal>("imrLastLaborCost");
				partCost.MaterialCost = row.Field<decimal>("imrLastMaterialCost");
				partCost.OverheadCost = row.Field<decimal>("imrLastOverheadCost");
				partCost.SubcontractCost = row.Field<decimal>("imrLastSubcontractCost");
				partCost.DutyCost = row.Field<decimal>("imrLastDutyCost");
				partCost.FreightCost = row.Field<decimal>("imrLastFreightCost");
				partCost.MiscCost = row.Field<decimal>("imrLastMiscCost");
				break;
			case 3:
				partCost.LaborCost = row.Field<decimal>("imrStandardLaborCost");
				partCost.MaterialCost = row.Field<decimal>("imrStandardMaterialCost");
				partCost.OverheadCost = row.Field<decimal>("imrStandardOverheadCost");
				partCost.SubcontractCost = row.Field<decimal>("imrStandardSubcontractCost");
				partCost.DutyCost = row.Field<decimal>("imrStandardDutyCost");
				partCost.FreightCost = row.Field<decimal>("imrStandardFreightCost");
				partCost.MiscCost = row.Field<decimal>("imrStandardMiscCost");
				break;
			default:
				partCost.LaborCost = row.Field<decimal>("imrAverageLaborCost");
				partCost.MaterialCost = row.Field<decimal>("imrAverageMaterialCost");
				partCost.OverheadCost = row.Field<decimal>("imrAverageOverheadCost");
				partCost.SubcontractCost = row.Field<decimal>("imrAverageSubcontractCost");
				partCost.DutyCost = row.Field<decimal>("imrAverageDutyCost");
				partCost.FreightCost = row.Field<decimal>("imrAverageFreightCost");
				partCost.MiscCost = row.Field<decimal>("imrAverageMiscCost");
				break;
			}
			return partCost;
		}
		return null;
	}

	public bool RefreshOnOrderQuantitesSales(M1Database database, SqlTransaction transaction, string partID = "", string revisionID = "")
	{
		bool flag;
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
			flag = false;
		}
		else
		{
			flag = true;
		}
		try
		{
			if (!string.IsNullOrWhiteSpace(partID))
			{
				SqlCommand sqlCommand = database.NewSqlCommand("Update PartRevisions set imrQuantityOnOrderSales = isnull(qty,0) from PartRevisions left outer join (select partid,revisionid,sum(isnull(qty,0)) as qty from (select omdPartID as partid,omdPartRevisionID as revisionid,omdDeliveryQuantity - omdQuantityShipped as qty from SalesOrderDeliveries where omdPartID = @PartID And omdPartRevisionID = @RevisionID And omdClosed = 0 And omdDeliveryType <> 3 And omdShippedComplete = 0 and ((omdDeliveryQuantity >= 0 And omdQuantityShipped < omdDeliveryQuantity) Or (omdDeliveryQuantity < 0 And omdQuantityShipped > omdDeliveryQuantity)) ) as test group by partid,revisionid) as test2 on imrPartID = isnull(partid,'') and imrPartRevisionID = isnull(revisionid,'') Where imrPartID = @PartID And imrPartRevisionID = @RevisionID And imrQuantityOnOrderSales <> isnull(qty,0)");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				database.ExecuteCommand(sqlCommand, transaction);
				sqlCommand = database.NewSqlCommand("UPDATE PartBins SET imbQuantityOnOrderSales = IsNull(qty,0)  FROM PartBins LEFT OUTER JOIN (SELECT partid, revisionid, warehouseid, binid, sum(isnull(qty,0)) as qty  from (SELECT omdPartID as partid, omdPartRevisionID as revisionid, omdPartWarehouseLocationID as warehouseid, omdPartBinID as binid, omdDeliveryQuantity - omdQuantityShipped as qty FROM SalesOrderDeliveries WHERE omdPartID = @PartID And omdPartRevisionID = @RevisionID And omdClosed = 0 And omdDeliveryType <> 3 And omdShippedComplete = 0 and ((omdDeliveryQuantity >= 0 And omdQuantityShipped < omdDeliveryQuantity) Or (omdDeliveryQuantity < 0 And omdQuantityShipped > omdDeliveryQuantity)) ) as test group by partid, revisionid, warehouseid, binid) as test2 on imbPartID = isnull(partid,'') And imbPartRevisionID = isnull(revisionid,'') AND imbWarehouseID = isnull(warehouseid,'') AND imbPartBinID = isnull(BinID,'') Where imbPartID = @PartID And imbPartRevisionID = @RevisionID And imbQuantityOnOrderSales <> isnull(qty,0)");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				database.ExecuteCommand(sqlCommand, transaction);
			}
			else
			{
				SqlCommand sqlCommand2 = database.NewSqlCommand("Update PartRevisions set imrQuantityOnOrderSales = isnull(qty,0) from PartRevisions left outer join (select partid,revisionid,sum(isnull(qty,0)) as qty from (select omdPartID as partid,omdPartRevisionID as revisionid,omdDeliveryQuantity - omdQuantityShipped as qty from SalesOrderDeliveries where omdClosed = 0 And omdDeliveryType <> 3 And omdShippedComplete = 0 and ((omdDeliveryQuantity >= 0 And omdQuantityShipped < omdDeliveryQuantity) Or (omdDeliveryQuantity < 0 And omdQuantityShipped > omdDeliveryQuantity)) ) as test group by partid,revisionid) as test2 on imrPartID = isnull(partid,'') and imrPartRevisionID = isnull(revisionid,'') Where imrQuantityOnOrderSales <> isnull(qty,0)");
				database.ExecuteCommand(sqlCommand2, transaction);
				sqlCommand2 = database.NewSqlCommand("UPDATE PartBins SET imbQuantityOnOrderSales = IsNull(qty,0)  FROM PartBins LEFT OUTER JOIN (SELECT partid, revisionid, warehouseid, binid, sum(isnull(qty,0)) as qty  from (SELECT omdPartID as partid, omdPartRevisionID as revisionid, omdPartWarehouseLocationID as warehouseid, omdPartBinID as binid, omdDeliveryQuantity - omdQuantityShipped as qty FROM SalesOrderDeliveries WHERE omdClosed = 0 And omdDeliveryType <> 3 And omdShippedComplete = 0 and ((omdDeliveryQuantity >= 0 And omdQuantityShipped < omdDeliveryQuantity) Or (omdDeliveryQuantity < 0 And omdQuantityShipped > omdDeliveryQuantity)) ) as test group by partid, revisionid, warehouseid, binid) as test2 on imbPartID = isnull(partid,'') And imbPartRevisionID = isnull(revisionid,'') AND imbWarehouseID = isnull(warehouseid,'') AND imbPartBinID = isnull(BinID,'') Where imbQuantityOnOrderSales <> isnull(qty,0)");
				database.ExecuteCommand(sqlCommand2, transaction);
			}
			if (!flag)
			{
				database.CommitTransaction(transaction);
			}
			return true;
		}
		catch
		{
			if (!flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	public bool RefreshOnOrderQuantitesPurchases(M1Database database, SqlTransaction transaction, string partID = "", string revisionID = "")
	{
		bool flag;
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
			flag = false;
		}
		else
		{
			flag = true;
		}
		try
		{
			if (!string.IsNullOrWhiteSpace(partID))
			{
				SqlCommand sqlCommand = database.NewSqlCommand("Update PartRevisions set imrQuantityOnOrderPurchases = isnull(qty,0) from PartRevisions left outer join (select partid,revisionid,sum(isnull(qty,0)) as qty from (select pmlPartID as partid,pmlPartRevisionID as revisionid,pmlInventoryQuantity - pmlInventoryQuantityReceived as qty from PurchaseOrderLines inner join PurchaseOrders on pmpPurchaseOrderID=pmlPurchaseOrderID where pmlPartID = @PartID And pmlPartRevisionID = @RevisionID And pmpClosed = 0 And pmlReceivedComplete = 0 And ((pmlInventoryQuantity >= 0 And pmlInventoryQuantityReceived < pmlInventoryQuantity) Or (pmlInventoryQuantity < 0 And pmlInventoryQuantityReceived > pmlInventoryQuantity)) ) as test group by partid,revisionid) as test2 on imrPartID = isnull(partid,'') and imrPartRevisionID = isnull(revisionid,'') Where imrPartID = @PartID And imrPartRevisionID = @RevisionID And imrQuantityOnOrderPurchases <> isnull(qty,0)");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				database.ExecuteCommand(sqlCommand, transaction);
				sqlCommand = database.NewSqlCommand("UPDATE PartBins SET imbQuantityOnOrderPurchases = IsNull(qty,0)  FROM PartBins LEFT OUTER JOIN (SELECT partid, revisionid, warehouseid, binid, sum(isnull(qty,0)) as qty  from (SELECT pmlPartID as partid, pmlPartRevisionID as revisionid, pmlPartWarehouseLocationID as warehouseid, pmlPartBinID as binid, pmlInventoryQuantity - pmlInventoryQuantityReceived as qty FROM PurchaseOrderLines inner join PurchaseOrders on pmpPurchaseOrderID=pmlPurchaseOrderID WHERE pmlPartID = @PartID And pmlPartRevisionID = @RevisionID And pmpClosed = 0 And pmlReceivedComplete = 0 And pmlInvoicedComplete = 0 and ((pmlInventoryQuantity >= 0 And pmlInventoryQuantityReceived < pmlInventoryQuantity) Or (pmlInventoryQuantity < 0 And pmlInventoryQuantityReceived > pmlInventoryQuantity)) ) as test group by partid, revisionid, warehouseid, binid) as test2 on imbPartID = isnull(partid,'') And imbPartRevisionID = isnull(revisionid,'') AND imbWarehouseID = isnull(warehouseid,'') AND imbPartBinID = isnull(BinID,'') Where imbPartID = @PartID And imbPartRevisionID = @RevisionID And imbQuantityOnOrderPurchases <> isnull(qty,0)");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				database.ExecuteCommand(sqlCommand, transaction);
			}
			else
			{
				SqlCommand sqlCommand2 = database.NewSqlCommand("Update PartRevisions set imrQuantityOnOrderPurchases = isnull(qty,0) from PartRevisions left outer join (select partid,revisionid,sum(isnull(qty,0)) as qty from (select pmlPartID as partid,pmlPartRevisionID as revisionid,pmlInventoryQuantity - pmlInventoryQuantityReceived as qty from PurchaseOrderLines inner join PurchaseOrders on pmpPurchaseOrderID=pmlPurchaseOrderID Where pmpClosed = 0 And pmlReceivedComplete = 0 And((pmlInventoryQuantity >= 0 And pmlInventoryQuantityReceived < pmlInventoryQuantity) Or (pmlInventoryQuantity < 0 And pmlInventoryQuantityReceived > pmlInventoryQuantity)) ) as test group by partid,revisionid) as test2 on imrPartID = isnull(partid,'') and imrPartRevisionID = isnull(revisionid,'') Where imrQuantityOnOrderPurchases <> isnull(qty,0)");
				database.ExecuteCommand(sqlCommand2, transaction);
				sqlCommand2 = database.NewSqlCommand("UPDATE PartBins SET imbQuantityOnOrderPurchases = IsNull(qty,0)  FROM PartBins LEFT OUTER JOIN (SELECT partid, revisionid, warehouseid, binid, sum(isnull(qty,0)) as qty  from (SELECT pmlPartID as partid, pmlPartRevisionID as revisionid, pmlPartWarehouseLocationID as warehouseid, pmlPartBinID as binid, pmlInventoryQuantity - pmlInventoryQuantityReceived as qty FROM PurchaseOrderLines inner join PurchaseOrders on pmpPurchaseOrderID=pmlPurchaseOrderID WHERE pmpClosed = 0 And pmlReceivedComplete = 0 And pmlInvoicedComplete = 0 and ((pmlInventoryQuantity >= 0 And pmlInventoryQuantityReceived < pmlInventoryQuantity) Or (pmlInventoryQuantity < 0 And pmlInventoryQuantityReceived > pmlInventoryQuantity)) ) as test group by partid, revisionid, warehouseid, binid) as test2 on imbPartID = isnull(partid,'') And imbPartRevisionID = isnull(revisionid,'') AND imbWarehouseID = isnull(warehouseid,'') AND imbPartBinID = isnull(BinID,'') Where imbQuantityOnOrderPurchases <> isnull(qty,0)");
				database.ExecuteCommand(sqlCommand2, transaction);
			}
			if (!flag)
			{
				database.CommitTransaction(transaction);
			}
			return true;
		}
		catch
		{
			if (!flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	public bool RefreshPartAllocations(M1Database database, SqlTransaction transaction, string partID = "", string revisionID = "")
	{
		bool flag;
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
			flag = false;
		}
		else
		{
			flag = true;
		}
		try
		{
			if (!string.IsNullOrWhiteSpace(partID))
			{
				SqlCommand sqlCommand = database.NewSqlCommand("Update PartRevisions set imrQuantityAllocated = isnull(qty,0) from PartRevisions left outer join (select partid,revisionid,sum(isnull(qty,0)) as qty from (select jmmPartID as partid,jmmPartRevisionID as revisionid,jmmEstimatedQuantity - jmmQuantityReceived as qty from JobMaterials where jmmPartID = @PartID And jmmPartRevisionID = @RevisionID And jmmClosed = 0 And jmmPullAllFromStock = 1 And jmmReceivedComplete = 0 and jmmKitPart = 0 and ((jmmEstimatedQuantity >= 0 And jmmQuantityReceived < jmmEstimatedQuantity) Or (jmmEstimatedQuantity < 0 And jmmQuantityReceived > jmmEstimatedQuantity)) union all (select jmtPartID as partid,jmtPartRevisionID as revisionid,jmtMaterialQuantity - jmtQuantityReceived as qty from JobMaterialComponents Inner Join JobMaterials On jmtJobID = jmmJobID And jmtJobAssemblyID = jmmJobAssemblyID And jmtJobMaterialID = jmmJobMaterialID where jmtPartID = @PartID And jmtPartRevisionID = @RevisionID And jmtPullAllFromStock = 1 And jmtClosed = 0 And jmtReceivedComplete = 0 and jmmKitPart <> 0 and ((jmtMaterialQuantity >= 0 And jmtQuantityReceived < jmtMaterialQuantity) Or (jmtMaterialQuantity < 0 And jmtQuantityReceived > jmtMaterialQuantity))) union all (select jmaPartID as partid,jmaPartRevisionID as revisionid,jmaQuantityToPull - jmaQuantityIssued as qty from JobAssemblies where jmaPartID = @PartID And jmaPartRevisionID = @RevisionID And jmaClosed = 0 And jmaIssuedComplete = 0 and ((jmaQuantityToPull >= 0 And jmaQuantityIssued < jmaQuantityToPull) Or (jmaQuantityToPull < 0 And jmaQuantityIssued > jmaQuantityToPull))) union all (select omdPartID as partid,omdPartRevisionID as revisionid,omdDeliveryQuantity - omdQuantityShipped as qty from SalesOrderDeliveries where omdPartID = @PartID And omdPartRevisionID = @RevisionID And omdClosed = 0 And omdDeliveryType in (2) And omdShippedComplete = 0 and ((omdDeliveryQuantity >= 0 And omdQuantityShipped < omdDeliveryQuantity) Or (omdDeliveryQuantity < 0 And omdQuantityShipped > omdDeliveryQuantity))) union all (select omoPartID as partid,omoPartRevisionID as revisionid,omoDeliveryQuantity - omoQuantityShipped as qty from SalesOrderComponents where omoPartID = @PartID And omoPartRevisionID = @RevisionID And omoClosed = 0 And omoShippedComplete = 0 and ((omoDeliveryQuantity >= 0 And omoQuantityShipped < omoDeliveryQuantity) Or (omoDeliveryQuantity < 0 And omoQuantityShipped > omoDeliveryQuantity)))) as test group by partid,revisionid) as test2 on imrPartID = isnull(partid,'') and imrPartRevisionID = isnull(revisionid,'') Where imrPartID = @PartID And imrPartRevisionID = @RevisionID And imrQuantityAllocated <> isnull(qty,0)");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				database.ExecuteCommand(sqlCommand, transaction);
				sqlCommand = database.NewSqlCommand("UPDATE PartBins SET imbQuantityAllocated = IsNull(qty,0)  FROM PartBins LEFT OUTER JOIN (SELECT partid, revisionid, warehouseid, binid, sum(isnull(qty,0)) as qty  from (SELECT jmmPartID as partid, jmmPartRevisionID as revisionid, jmmPartWarehouseLocationID as warehouseid, jmmPartBinID as binid, jmmEstimatedQuantity - jmmQuantityReceived as qty from JobMaterials where jmmPartID = @PartID And jmmPartRevisionID = @RevisionID And jmmClosed = 0 And jmmPullAllFromStock = 1 And jmmReceivedComplete = 0 and jmmKitPart = 0 and ((jmmEstimatedQuantity >= 0 And jmmQuantityReceived < jmmEstimatedQuantity) Or (jmmEstimatedQuantity < 0 And jmmQuantityReceived > jmmEstimatedQuantity))  union all (SELECT jmtPartID as partid, jmtPartRevisionID as revisionid, jmtPartWarehouseLocationID as warehouseid, jmtPartBinID as binid, jmtMaterialQuantity - jmtQuantityReceived as qty from JobMaterialComponents Inner Join JobMaterials On jmtJobID = jmmJobID And jmtJobAssemblyID = jmmJobAssemblyID And jmtJobMaterialID = jmmJobMaterialID where jmtPartID = @PartID And jmtPartRevisionID = @RevisionID And jmtPullAllFromStock = 1 And jmtClosed = 0 And jmtReceivedComplete = 0 and jmmKitPart <> 0 and ((jmtMaterialQuantity >= 0 And jmtQuantityReceived < jmtMaterialQuantity) Or (jmtMaterialQuantity < 0 And jmtQuantityReceived > jmtMaterialQuantity)))  union all (SELECT jmaPartID as partid, jmaPartRevisionID as revisionid, jmaPartWarehouseLocationID as warehouseid, jmaPartBinID as binid, jmaQuantityToPull - jmaQuantityIssued as qty from JobAssemblies where jmaPartID = @PartID And jmaPartRevisionID = @RevisionID And jmaClosed = 0 And jmaIssuedComplete = 0 and ((jmaQuantityToPull >= 0 And jmaQuantityIssued < jmaQuantityToPull) Or (jmaQuantityToPull < 0 And jmaQuantityIssued > jmaQuantityToPull)))  union all (SELECT omdPartID as partid, omdPartRevisionID as revisionid, omdPartWarehouseLocationID as warehouseid, omdPartBinID as binid, omdDeliveryQuantity - omdQuantityShipped as qty FROM SalesOrderDeliveries WHERE omdPartID = @PartID And omdPartRevisionID = @RevisionID And omdClosed = 0 And omdDeliveryType in (2) And omdShippedComplete = 0 and ((omdDeliveryQuantity >= 0 And omdQuantityShipped < omdDeliveryQuantity) Or (omdDeliveryQuantity < 0 And omdQuantityShipped > omdDeliveryQuantity)))  union all (SELECT omoPartID as partid, omoPartRevisionID as revisionid, omoPartWarehouseLocationID as warehouseid, omoPartBinID as binid, omoDeliveryQuantity - omoQuantityShipped as qty FROM SalesOrderComponents WHERE omoPartID = @PartID And omoPartRevisionID = @RevisionID And omoClosed = 0 And omoShippedComplete = 0 and ((omoDeliveryQuantity >= 0 And omoQuantityShipped < omoDeliveryQuantity) Or (omoDeliveryQuantity < 0 And omoQuantityShipped > omoDeliveryQuantity))))  as test group by partid, revisionid, warehouseid, binid) as test2 on imbPartID = isnull(partid,'') And imbPartRevisionID = isnull(revisionid,'') AND imbWarehouseID = isnull(warehouseid,'') AND imbPartBinID = isnull(BinID,'') Where imbPartID = @PartID And imbPartRevisionID = @RevisionID And imbQuantityAllocated <> isnull(qty,0)");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
				database.ExecuteCommand(sqlCommand, transaction);
			}
			else
			{
				SqlCommand sqlCommand2 = database.NewSqlCommand("Update PartRevisions set imrQuantityAllocated = isnull(qty,0) from PartRevisions left outer join (select partid,revisionid,sum(isnull(qty,0)) as qty from (select jmmPartID as partid,jmmPartRevisionID as revisionid,jmmEstimatedQuantity - jmmQuantityReceived as qty from JobMaterials where jmmClosed = 0 And jmmPullAllFromStock = 1 And jmmReceivedComplete = 0 and jmmKitPart = 0 and ((jmmEstimatedQuantity >= 0 And jmmQuantityReceived < jmmEstimatedQuantity) Or (jmmEstimatedQuantity < 0 And jmmQuantityReceived > jmmEstimatedQuantity)) union all (select jmtPartID as partid,jmtPartRevisionID as revisionid,jmtMaterialQuantity - jmtQuantityReceived as qty from JobMaterialComponents Inner Join JobMaterials On jmtJobID = jmmJobID And jmtJobAssemblyID = jmmJobAssemblyID And jmtJobMaterialID = jmmJobMaterialID where jmtPullAllFromStock = 1 And jmtClosed = 0 And jmtReceivedComplete = 0 and jmmKitPart <> 0 and ((jmtMaterialQuantity >= 0 And jmtQuantityReceived < jmtMaterialQuantity) Or (jmtMaterialQuantity < 0 And jmtQuantityReceived > jmtMaterialQuantity))) union all (select jmaPartID as partid,jmaPartRevisionID as revisionid,jmaQuantityToPull - jmaQuantityIssued as qty from JobAssemblies where jmaClosed = 0 And jmaIssuedComplete = 0 and ((jmaQuantityToPull >= 0 And jmaQuantityIssued < jmaQuantityToPull) Or (jmaQuantityToPull < 0 And jmaQuantityIssued > jmaQuantityToPull))) union all (select omdPartID as partid,omdPartRevisionID as revisionid,omdDeliveryQuantity - omdQuantityShipped as qty from SalesOrderDeliveries where omdClosed = 0 And omdDeliveryType in (2) And omdShippedComplete = 0 and ((omdDeliveryQuantity >= 0 And omdQuantityShipped < omdDeliveryQuantity) Or (omdDeliveryQuantity < 0 And omdQuantityShipped > omdDeliveryQuantity))) union all (select omoPartID as partid,omoPartRevisionID as revisionid,omoDeliveryQuantity - omoQuantityShipped as qty from SalesOrderComponents where omoClosed = 0 And omoShippedComplete = 0 and ((omoDeliveryQuantity >= 0 And omoQuantityShipped < omoDeliveryQuantity) Or (omoDeliveryQuantity < 0 And omoQuantityShipped > omoDeliveryQuantity)))) as test group by partid,revisionid) as test2 on imrPartID = isnull(partid,'') and imrPartRevisionID = isnull(revisionid,'') Where imrQuantityAllocated <> isnull(qty,0)");
				database.ExecuteCommand(sqlCommand2, transaction);
				sqlCommand2 = database.NewSqlCommand("UPDATE PartBins SET imbQuantityAllocated = IsNull(qty,0)  FROM PartBins LEFT OUTER JOIN (SELECT partid, revisionid, warehouseid, binid, sum(isnull(qty,0)) as qty  from (SELECT jmmPartID as partid, jmmPartRevisionID as revisionid, jmmPartWarehouseLocationID as warehouseid, jmmPartBinID as binid, jmmEstimatedQuantity - jmmQuantityReceived as qty from JobMaterials where jmmClosed = 0 And jmmPullAllFromStock = 1 And jmmReceivedComplete = 0 and jmmKitPart = 0 and ((jmmEstimatedQuantity >= 0 And jmmQuantityReceived < jmmEstimatedQuantity) Or (jmmEstimatedQuantity < 0 And jmmQuantityReceived > jmmEstimatedQuantity))  union all (SELECT jmtPartID as partid, jmtPartRevisionID as revisionid, jmtPartWarehouseLocationID as warehouseid, jmtPartBinID as binid, jmtMaterialQuantity - jmtQuantityReceived as qty from JobMaterialComponents Inner Join JobMaterials On jmtJobID = jmmJobID And jmtJobAssemblyID = jmmJobAssemblyID And jmtJobMaterialID = jmmJobMaterialID where jmtPullAllFromStock = 1 And jmtClosed = 0 And jmtReceivedComplete = 0 and jmmKitPart <> 0 and ((jmtMaterialQuantity >= 0 And jmtQuantityReceived < jmtMaterialQuantity) Or (jmtMaterialQuantity < 0 And jmtQuantityReceived > jmtMaterialQuantity)))  union all (SELECT jmaPartID as partid, jmaPartRevisionID as revisionid, jmaPartWarehouseLocationID as warehouseid, jmaPartBinID as binid, jmaQuantityToPull - jmaQuantityIssued as qty from JobAssemblies where jmaClosed = 0 And jmaIssuedComplete = 0 and ((jmaQuantityToPull >= 0 And jmaQuantityIssued < jmaQuantityToPull) Or (jmaQuantityToPull < 0 And jmaQuantityIssued > jmaQuantityToPull)))  union all (SELECT omdPartID as partid, omdPartRevisionID as revisionid, omdPartWarehouseLocationID as warehouseid, omdPartBinID as binid, omdDeliveryQuantity - omdQuantityShipped as qty FROM SalesOrderDeliveries WHERE omdClosed = 0 And omdDeliveryType in (2) And omdShippedComplete = 0 and ((omdDeliveryQuantity >= 0 And omdQuantityShipped < omdDeliveryQuantity) Or (omdDeliveryQuantity < 0 And omdQuantityShipped > omdDeliveryQuantity)))  union all (SELECT omoPartID as partid, omoPartRevisionID as revisionid, omoPartWarehouseLocationID as warehouseid, omoPartBinID as binid, omoDeliveryQuantity - omoQuantityShipped as qty FROM SalesOrderComponents WHERE omoClosed = 0 And omoShippedComplete = 0 and ((omoDeliveryQuantity >= 0 And omoQuantityShipped < omoDeliveryQuantity) Or (omoDeliveryQuantity < 0 And omoQuantityShipped > omoDeliveryQuantity))))  as test group by partid, revisionid, warehouseid, binid) as test2 on imbPartID = isnull(partid,'') And imbPartRevisionID = isnull(revisionid,'') AND imbWarehouseID = isnull(warehouseid,'') AND imbPartBinID = isnull(BinID,'') Where imbQuantityAllocated <> isnull(qty,0)");
				database.ExecuteCommand(sqlCommand2, transaction);
			}
			if (!flag)
			{
				database.CommitTransaction(transaction);
			}
			return true;
		}
		catch
		{
			if (!flag)
			{
				database.RollbackTransaction(transaction);
			}
			throw;
		}
	}

	public decimal GetConversionFactor(M1Database database, string partID, string partRevisionID, string supplierID, string locationID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull((SELECT IsNull(Case When IsNull(imxConversionFactor, 0) = 0 Then imzConversionFactor Else imxConversionFactor End,imrConversionFactor) As imrConversionFactor From PartRevisions Left Outer Join PartOrgReferences On imzPartID = imrPartID And imzPartRevisionID = imrPartRevisionID And imzOrganizationID = @OrgID Left Outer Join PartCrossReferences on imzOrganizationID = imxOrganizationID And imxLocationID = @LocID And imzPartID = imxPartID And imzPartRevisionID = imxPartRevisionID Where imrPartID = @PartID And imrPartRevisionID = @PartRevisionID),1)");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = supplierID;
		sqlCommand.Parameters.Add(new SqlParameter("@LocID", SqlDbType.NVarChar)).Value = locationID;
		return Convert.ToDecimal(database.ExecuteScalar(sqlCommand));
	}

	public decimal CalculateQtyAvailable(M1Database database, string partID, string revisionID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select immPartID as PartID,immPartRevisionID as PartRevisionID,immQuantityPerAssembly as QuantityPerParent,imrQuantityOnHand,imrQuantityAllocated,0.00 as AdditionalQuantity,1.00 As ParentQuantity From PartMaterials Inner Join Parts On immMethodID = impPartID Inner Join PartRevisions On immPartID = imrPartID And immPartRevisionID = imrPartRevisionID Where immMethodID = @PartID And immMethodRevisionID = @RevisionID And impPhantomOrKitPart <> 0");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		return findLowestMultiple(database, dataTable, "", partID, revisionID, "");
	}

	public decimal CalculateQtyAvailable(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		if (currentAsDataRow == null)
		{
			return -1m;
		}
		string text = "";
		text = ((!bindingSource.PrimaryTable.TopLevelTable.Equals("Jobs", StringComparison.CurrentCultureIgnoreCase)) ? (M1Util.GetSingularOfTableName(bindingSource.PrimaryTable.TopLevelTable) + "Components") : (M1Util.GetSingularOfTableName(bindingSource.PrimaryTable.TableName) + "Components"));
		if (!DoesTableExist(bindingSource.Database, bindingSource.Transaction, text))
		{
			return -1m;
		}
		DataTable componentsDT = new DataTable();
		string componentsPrefix = string.Empty;
		if (bindingSource.PrimaryTable.ChildBindingSourceExists(text))
		{
			componentsDT = bindingSource.PrimaryTable.GetChildBindingSource(text).GetDataTable();
			componentsPrefix = bindingSource.PrimaryTable.GetChildBindingSource(text).PrimaryTable.FieldPrefix;
		}
		return findLowestMultiple(database, componentsDT, componentsPrefix, currentAsDataRow.Field<string>(bindingSource.PrimaryTable.FieldPrefix + "PartID"), currentAsDataRow.Field<string>(bindingSource.PrimaryTable.FieldPrefix + "PartRevisionID"), "omoSalesOrderDeliveryID = " + currentAsDataRow.Field<short>("omdSalesOrderDeliveryID").ToSql());
	}

	private decimal findLowestMultiple(M1Database database, DataTable componentsDT, string componentsPrefix, string finalPartID, string finalRevisionID, string filterExpression)
	{
		decimal num = default(decimal);
		string value = string.Empty;
		string value2 = string.Empty;
		string value3 = string.Empty;
		int num2 = 0;
		if (componentsDT.Rows.Count > 0)
		{
			bool flag = false;
			DataRow[] array = componentsDT.Select(filterExpression);
			foreach (DataRow dataRow in array)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("Select imrQuantityOnHand,imrQuantityAllocated From PartRevisions Where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
				if (dataRow.RowState == DataRowState.Added)
				{
					sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = dataRow.Field<string>(componentsPrefix + "PartID", DataRowVersion.Current);
					sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = dataRow.Field<string>(componentsPrefix + "PartRevisionID", DataRowVersion.Current);
				}
				else
				{
					sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = dataRow.Field<string>(componentsPrefix + "PartID", DataRowVersion.Original);
					sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = dataRow.Field<string>(componentsPrefix + "PartRevisionID", DataRowVersion.Original);
				}
				if (!string.IsNullOrWhiteSpace(filterExpression))
				{
					value = dataRow.Field<string>(componentsPrefix + "SalesOrderID", DataRowVersion.Current);
					num2 = dataRow.Field<short>(componentsPrefix + "SalesOrderLineID", DataRowVersion.Current);
					value2 = dataRow.Field<string>(componentsPrefix + "PartID", DataRowVersion.Current);
					value3 = dataRow.Field<string>(componentsPrefix + "PartRevisionID", DataRowVersion.Current);
				}
				DataTable dataTable = database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count <= 0)
				{
					continue;
				}
				decimal num3 = default(decimal);
				if (!string.IsNullOrWhiteSpace(filterExpression))
				{
					SqlCommand sqlCommand2 = database.NewSqlCommand("Select omoPartID, omoPartRevisionID, omoPartWarehouseLocationID, omoPartBinID, omoDeliveryQuantity - omoQuantityShipped As imrQuantityAllocated, omosalesorderid, omosalesOrderLineID  from SalesOrderComponents where omoClosed = 0 And omoShippedComplete = 0 and ((omoDeliveryQuantity >= 0 And omoQuantityShipped < omoDeliveryQuantity) Or (omoDeliveryQuantity < 0 And omoQuantityShipped > omoDeliveryQuantity))  and omoPartID = @PartID and omoPartRevisionID = @RevisionID and omoSalesOrderID = @SalesOrderID and omoSalesOrderLineID = @SalesOrderLineID");
					sqlCommand2.Parameters.AddWithValue("@PartID", value2);
					sqlCommand2.Parameters.AddWithValue("@RevisionID", value3);
					sqlCommand2.Parameters.AddWithValue("@SalesOrderID", value);
					sqlCommand2.Parameters.AddWithValue("@SalesOrderLineID", num2);
					DataTable dataTable2 = database.GetDataTable(sqlCommand2);
					if (dataTable2 != null && dataTable2.Rows.Count != 0)
					{
						num3 = dataTable2.AsEnumerable().Sum((DataRow x) => x.Field<decimal>("imrQuantityAllocated"));
					}
				}
				DataRow row = dataTable.Rows[0];
				decimal num4 = ((dataRow.RowState != DataRowState.Added) ? (row.Field<decimal>("imrQuantityOnHand") - row.Field<decimal>("imrQuantityAllocated") + num3 - (dataRow.Field<decimal>(componentsPrefix + "ParentQuantity", DataRowVersion.Current) * dataRow.Field<decimal>(componentsPrefix + "QuantityPerParent", DataRowVersion.Current) + dataRow.Field<decimal>(componentsPrefix + "AdditionalQuantity", DataRowVersion.Current))) : (row.Field<decimal>("imrQuantityOnHand") - row.Field<decimal>("imrQuantityAllocated") + num3));
				num4 = Math.Round(num4 / dataRow.Field<decimal>(componentsPrefix + "QuantityPerParent", DataRowVersion.Current), 5);
				if (!flag)
				{
					num = num4;
					flag = true;
				}
				else if (num4 < num)
				{
					num = num4;
				}
			}
			if (num < 0m)
			{
				num = default(decimal);
			}
		}
		else
		{
			SqlCommand sqlCommand3 = database.NewSqlCommand("Select imrQuantityOnHand,imrQuantityAllocated From PartRevisions Where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
			sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = finalPartID;
			sqlCommand3.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = finalRevisionID;
			DataTable dataTable3 = database.GetDataTable(sqlCommand3);
			if (dataTable3.Rows.Count > 0)
			{
				DataRow row2 = dataTable3.Rows[0];
				decimal num5 = row2.Field<decimal>("imrQuantityOnHand") - row2.Field<decimal>("imrQuantityAllocated");
				num = ((!(num5 < 0m)) ? num5 : default(decimal));
			}
		}
		return num;
	}

	public bool DoesTableExist(M1Database database, SqlTransaction transaction, string tableName)
	{
		SqlCommand sqlCommand = new SqlCommand("SELECT Table_name FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = @TableName");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = tableName;
		return database.ExecuteScalar(sqlCommand, transaction) != null;
	}

	public void RefreshPreviousQOH(M1Database database, SqlTransaction transaction, string where)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("DECLARE @cPartID varchar(30), @cRevID varchar(15), @cWarehouseID varchar(5), @cBinID varchar(15), @nQtyOnHand numeric(16,5)\r");
		stringBuilder.Append("DECLARE @nTransID numeric(20), @nQtyReceived numeric(16,5), @nCalcQOH numeric(16,5)\r");
		stringBuilder.Append("DECLARE @cPartIDPrev varchar(30), @cRevIDPrev varchar(15), @cWarehouseIDPrev varchar(5), @cBinIDPrev varchar(15)\r");
		stringBuilder.Append("DECLARE @nimtPrevQOH numeric(16,5), @nimtQtyRec numeric(16,5),@nTempPrevQOH numeric(16,5), @nSource integer, @nTempQtyRec numeric(16,5)\r");
		stringBuilder.Append("DECLARE @nPreviousQtyOnHand numeric(16,5), @nInitialQtyAssigned numeric(16,5), @nLastQtyAssignedReceived numeric(16,5), @nRegistrationNumberCreated integer\r");
		stringBuilder.Append("SET @cPartIDPrev=''\r");
		stringBuilder.Append("SET @cRevIDPrev=''\r");
		stringBuilder.Append("SET @cWarehouseIDPrev=''\r");
		stringBuilder.Append("SET @cBinIDPrev=''\r");
		stringBuilder.Append("SET @nRegistrationNumberCreated = 0\r");
		stringBuilder.Append("SELECT imtPartTransactionID,imtInventoryQuantityReceived INTO #TempTrans FROM PartTransactions WHERE 0=1\r");
		stringBuilder.Append("DECLARE TransCursor CURSOR FAST_FORWARD FOR SELECT imtPartTransactionID,imtInventoryQuantityReceived = case when imtNonInventoryTransaction <> 0 then 0 else (case when (imtTransactionType = 2) or (imtTransactionType = 1 and imtSource in (1,2)) then imtInventoryQuantityReceived else -1*imtInventoryQuantityReceived end) end,imtPartID,imtPartRevisionID,imtPartWarehouseLocationID,imtPartBinID,imbQuantityOnHand,imtPreviousQuantityOnHand,imtInventoryQuantityReceived,imtSource FROM PartTransactions INNER JOIN PartBins ON imbPartID=imtPartID AND imbPartRevisionID=imtPartRevisionID AND imbWarehouseID=imtPartWarehouseLocationID AND imbPartBinID=imtPartBinID WHERE imtSource <> 7 " + where + " ORDER BY imtPartID,imtPartRevisionID,imtPartWarehouseLocationID,imtPartBinID,imtTransactionDate, imtPartTransactionID\r");
		stringBuilder.Append("OPEN TransCursor\r");
		stringBuilder.Append("FETCH NEXT FROM TransCursor INTO @nTransID,@nQtyReceived,@cPartID,@cRevID,@cWarehouseID,@cBinID,@nQtyOnHand,@nimtPrevQOH,@nimtQtyRec,@nSource\r");
		stringBuilder.Append("WHILE @@FETCH_STATUS = 0\r");
		stringBuilder.Append("BEGIN\r");
		stringBuilder.Append(" IF @cPartIDPrev <> @cPartID OR @cRevIDPrev <> @cRevID OR @cWarehouseIDPrev <> @cWarehouseID OR @cBinIDPrev <> @cBinID\r");
		stringBuilder.Append(" BEGIN\r");
		stringBuilder.Append("     SET @nCalcQOH=@nQtyOnHand\r");
		stringBuilder.Append("     SET @cPartIDPrev=@cPartID\r");
		stringBuilder.Append("     SET @cRevIDPrev=@cRevID\r");
		stringBuilder.Append("     SET @cWarehouseIDPrev=@cWarehouseID\r");
		stringBuilder.Append("     SET @cBinIDPrev=@cBinID\r");
		stringBuilder.Append(" END\r");
		stringBuilder.Append(" SET @nCalcQOH = @nCalcQOH + @nQtyReceived\r");
		stringBuilder.Append(" IF @nSource = 7\r");
		stringBuilder.Append(" BEGIN\r");
		stringBuilder.Append("     SET @nTempPrevQOH=@nimtPrevQOH\r");
		stringBuilder.Append("     SET @nTempQtyRec=@nCalcQOH\r");
		stringBuilder.Append(" END\r");
		stringBuilder.Append(" ELSE\r");
		stringBuilder.Append(" BEGIN\r");
		stringBuilder.Append("     SET @nTempPrevQOH=@nCalcQOH\r");
		stringBuilder.Append("     SET @nTempQtyRec=@nimtQtyRec\r");
		stringBuilder.Append(" END\r");
		stringBuilder.Append(" IF @nTempPrevQOH <> @nimtPrevQOH OR @nTempQtyRec <> @nimtQtyRec\r");
		stringBuilder.Append(" INSERT INTO #TempTrans (imtPartTransactionID,imtInventoryQuantityReceived) VALUES (@nTransID,@nTempQtyRec)\r");
		stringBuilder.Append(" FETCH NEXT FROM TransCursor INTO @nTransID,@nQtyReceived,@cPartID,@cRevID,@cWarehouseID,@cBinID,@nQtyOnHand,@nimtPrevQOH,@nimtQtyRec,@nSource\r");
		stringBuilder.Append("END\r");
		stringBuilder.Append("CLOSE TransCursor\r");
		stringBuilder.Append("DEALLOCATE TransCursor\r");
		stringBuilder.Append("SET @nInitialQtyAssigned = (SELECT TOP 1 imtInventoryQuantityReceived FROM PartTransactions INNER JOIN PartBins ON imbPartID=imtPartID AND imbPartRevisionID=imtPartRevisionID AND imbWarehouseID=imtPartWarehouseLocationID AND imbPartBinID=imtPartBinID WHERE imtSource <> 7 " + where + " ORDER BY imtPartTransactionID ASC)\r");
		stringBuilder.Append("SELECT imtPartTransactionID,imtPreviousQuantityOnHand,imtInventoryQuantityReceived INTO #TempTransactions FROM PartTransactions WHERE 0=1\r");
		stringBuilder.Append("DECLARE TransCursor CURSOR FAST_FORWARD FOR SELECT imtPartTransactionID, imtInventoryQuantityReceived FROM #TempTrans ORDER BY imtPartTransactionID\r");
		stringBuilder.Append("OPEN TransCursor\r");
		stringBuilder.Append("FETCH NEXT FROM TransCursor INTO @nTransID,@nQtyReceived\r");
		stringBuilder.Append("WHILE @@FETCH_STATUS = 0\r");
		stringBuilder.Append("BEGIN\r");
		stringBuilder.Append(" IF @nRegistrationNumberCreated = 0\r");
		stringBuilder.Append(" BEGIN\r");
		stringBuilder.Append("     SET @nPreviousQtyOnHand = 0\r");
		stringBuilder.Append(" END\r");
		stringBuilder.Append(" ELSE IF @nRegistrationNumberCreated = 1\r");
		stringBuilder.Append(" BEGIN\r");
		stringBuilder.Append("     SET @nPreviousQtyOnHand = @nInitialQtyAssigned\r");
		stringBuilder.Append(" END\r");
		stringBuilder.Append(" ELSE\r");
		stringBuilder.Append(" BEGIN\r");
		stringBuilder.Append("     SET @nLastQtyAssignedReceived = (SELECT TOP 1 imtInventoryQuantityReceived FROM #TempTransactions ORDER BY imtPartTransactionID DESC)\r");
		stringBuilder.Append("     SET @nInitialQtyAssigned = (SELECT TOP 1 imtPreviousQuantityOnHand FROM #TempTransactions ORDER BY imtPartTransactionID DESC)\r");
		stringBuilder.Append("     SET @nPreviousQtyOnHand = @nInitialQtyAssigned + @nLastQtyAssignedReceived\r");
		stringBuilder.Append(" END\r");
		stringBuilder.Append(" INSERT INTO #TempTransactions(imtPartTransactionID,imtPreviousQuantityOnHand,imtInventoryQuantityReceived)VALUES(@nTransID, @nPreviousQtyOnHand, @nQtyReceived)\r");
		stringBuilder.Append(" SET @nRegistrationNumberCreated = @nRegistrationNumberCreated + 1\r");
		stringBuilder.Append("FETCH NEXT FROM TransCursor INTO @nTransID,@nQtyReceived\r");
		stringBuilder.Append("END\r");
		stringBuilder.Append("CLOSE TransCursor\r");
		stringBuilder.Append("DEALLOCATE TransCursor\r");
		stringBuilder.Append("UPDATE PartTransactions SET imtPreviousQuantityOnHand=#TempTransactions.imtPreviousQuantityOnHand, imtInventoryQuantityReceived = #TempTransactions.imtInventoryQuantityReceived FROM PartTransactions INNER JOIN #TempTransactions ON PartTransactions.imtPartTransactionID=#TempTransactions.imtPartTransactionID\r");
		stringBuilder.Append("DROP TABLE #TempTrans\r");
		stringBuilder.Append("DROP TABLE #TempTransactions");
		database.ExecuteCommand(stringBuilder.ToString(), transaction);
	}

	public bool GetLatestPartRevision(M1Database database, SqlTransaction transaction, string partID, ref string partRevisionID)
	{
		partID = partID.Trim();
		if (partID.Length != 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select Top 1 imrPartRevisionID from PartRevisions Where imrPartID = @PartID and imrEffectiveStartDate <= @RevDate and (imrEffectiveEndDate is Null Or imrEffectiveEndDate >= @RevDate) Order By imrEffectiveStartDate Desc");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevDate", SqlDbType.DateTime)).Value = DateTime.Today;
			object obj = database.ExecuteScalar(sqlCommand, transaction);
			if (obj != null)
			{
				partRevisionID = obj.ToString();
				return true;
			}
		}
		return false;
	}

	public void AddCostsUpdatesTransaction(M1BindingSource bindingSource, SqlTransaction transaction)
	{
		M1Database database = bindingSource.Database;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		DataRowVersion version = DataRowVersion.Current;
		decimal num = default(decimal);
		DateTime now = DateTime.Now;
		string value = currentAsDataRow.Field<string>("imrPartID", version);
		string value2 = currentAsDataRow.Field<string>("imrPartRevisionID", version);
		SqlCommand sqlCommand = database.NewSqlCommand("Select imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbQuantityOnHand From PartBins WITH(NOLOCK)Where imbPartID = @PartID And imbPartRevisionID = @RevisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = value2;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			dataTable = bindingSource.PrimaryTable.GetChildBindingSource("PartBins").GetDataTable().GetChanges(DataRowState.Added);
		}
		foreach (DataRow row in dataTable.Rows)
		{
			num = row.Field<decimal>("imbQuantityOnHand");
			addPartBinTransaction(bindingSource, row, num, transaction, now);
		}
		currentAsDataRow["imrLastTransactionDate"] = now;
		bindingSource.OnDataChanged(new DataChangedEventArgs(DataChangedFlag.CurrentRow));
	}

	private void addPartBinTransaction(M1BindingSource bindingSource, DataRow binSourceRow, decimal qtyOnHand, SqlTransaction transaction, DateTime tranDate)
	{
		M1Database database = bindingSource.Database;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		DataRowVersion dataRowVersion = DataRowVersion.Current;
		string text = binSourceRow.Field<string>("imbPartID");
		string value = binSourceRow.Field<string>("imbPartRevisionID");
		string value2 = binSourceRow.Field<string>("imbWarehouseID");
		string value3 = binSourceRow.Field<string>("imbPartBinID");
		List<PartCost> partRevisionCosts = getPartRevisionCosts(currentAsDataRow, qtyOnHand);
		if (partRevisionCosts == null)
		{
			return;
		}
		FieldDefinition fieldDefinition = bindingSource.Fields["imrQuantityOnHand"];
		SqlDataAdapter adapter = null;
		DataTable dataTable = database.GetDataTable("Select * From PartTransactions Where 0=1", fillSchema: false, out adapter, transaction);
		DataRow dataRow = dataTable.NewRow().BlankRow();
		dataRow["imtPartID"] = text;
		dataRow["imtPartRevisionID"] = value;
		dataRow["imtPartWarehouseLocationID"] = value2;
		dataRow["imtPartBinID"] = value3;
		dataRow["imtTableName"] = fieldDefinition.Table.TableName;
		dataRow["imtTableUniqueID"] = currentAsDataRow.Field<Guid>(fieldDefinition.Table.UniqueField, dataRowVersion);
		dataRow["imtTransactionDate"] = tranDate;
		dataRow["imtTransactionType"] = 3;
		dataRow["imtSource"] = 7;
		dataRow["imtInventoryQuantityReceived"] = qtyOnHand;
		dataRow["imtJobType"] = 1;
		dataRow["imtReceiptType"] = 2;
		dataRow["imtIssueType"] = 2;
		dataRow["imtCOGSCalculatedDate"] = tranDate;
		dataRow["imtCOGSPostedToGL"] = true;
		dataRow["imtPreviousQuantityOnHand"] = (currentAsDataRow.HasVersion(DataRowVersion.Original) ? currentAsDataRow.Field<decimal>("imrQuantityOnHand", DataRowVersion.Original) : 0m);
		dataRow["imtInventoryUnitOfMeasure"] = currentAsDataRow.Field<string>("imrInventoryUnitOfMeasure");
		dataRow["imtReference"] = "COST REPLACE";
		dataRow["imtUserID"] = database.User.ID;
		if (Convert.ToBoolean(database.Props("DatasetProperties")["xadEnableNonNettable"]))
		{
			text = text.Trim();
			bool flag = false;
			bool flag2 = false;
			byte b = database.Props("PN").Field<byte>("xapIMCostingMethod");
			if (text.Length != 0)
			{
				SqlCommand sqlCommand = database.NewSqlCommand("Select Isnull(imlNonNettable,0) As imlNonNettable, impNonStockedItem From Parts Inner Join PartWarehouseLocations on impPartID = imlPartID Where imlPartID = @PartID and imlPartRevisionID = @RevisionID and imlPartWarehouseID = @WarehouseID");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = text;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = value;
				sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = value2;
				DataTable dataTable2 = database.GetDataTable(sqlCommand, transaction);
				if (dataTable2 != null && dataTable2.Rows.Count != 0)
				{
					flag = dataTable2.Rows[0].Field<bool>("imlNonNettable");
					flag2 = dataTable2.Rows[0].Field<bool>("impNonStockedItem");
				}
			}
			dataRow["imtNonNettable"] = flag;
			dataRow["imtNonInventoryTransaction"] = b == 3 || flag2;
		}
		else
		{
			dataRow["imtNonInventoryTransaction"] = true;
		}
		if (fieldDefinition.Table.GetDocumentPlantID(database, currentAsDataRow, transaction, dataRowVersion) != null)
		{
			dataRow["imtPlantID"] = fieldDefinition.Table.GetDocumentPlantID(database, currentAsDataRow, transaction, dataRowVersion);
		}
		dataRow["imtCreatedBy"] = database.User.ID;
		dataRow["imtCreatedDate"] = tranDate;
		dataRow["imtPartTransactionID"] = database.ExecuteScalar("Select IsNull(Max(imtPartTransactionID),0)+1 From PartTransactions", transaction);
		dataTable.Rows.Add(dataRow);
		database.UpdateData(new DataRow[1] { dataRow }, adapter, transaction);
		DataTable dataTable3 = null;
		SqlDataAdapter adapter2 = null;
		int num = 1;
		foreach (PartCost item in partRevisionCosts)
		{
			if (dataTable3 == null)
			{
				dataTable3 = database.GetDataTable("Select * From PartTransactionCosts Where 0=1", fillSchema: false, out adapter2, transaction);
			}
			DataRow dataRow2 = AddPartTransactionCostsRecord(database.User.ID, tranDate, item, dataTable3, dataRow.Field<int>("imtPartTransactionID"), num);
			dataTable3.Rows.Add(dataRow2);
			database.UpdateData(new DataRow[1] { dataRow2 }, adapter2, transaction);
			num++;
		}
	}

	private List<PartCost> getPartRevisionCosts(DataRow sourceRow, decimal quantity)
	{
		List<PartCost> list = new List<PartCost>();
		if (sourceRow != null)
		{
			PartCost partCost = new PartCost();
			partCost.CostType = PartTransactionDefinition.CostType.Actual;
			partCost.Quantity = quantity;
			list.Add(setPartCostFields(PartTransactionDefinition.CostType.Average, quantity, sourceRow));
			list.Add(setPartCostFields(PartTransactionDefinition.CostType.Last, quantity, sourceRow));
			list.Add(setPartCostFields(PartTransactionDefinition.CostType.Standard, quantity, sourceRow));
			list.Add(partCost);
		}
		return list;
	}

	private PartCost setPartCostFields(PartTransactionDefinition.CostType costType, decimal quantity, DataRow partRevRow)
	{
		PartCost partCost = new PartCost();
		partCost.CostType = costType;
		partCost.Quantity = quantity;
		string empty = string.Empty;
		empty = costType switch
		{
			PartTransactionDefinition.CostType.Average => "Average", 
			PartTransactionDefinition.CostType.Last => "Last", 
			PartTransactionDefinition.CostType.Standard => "Standard", 
			_ => "Average", 
		};
		string columnName = $"imr{empty}LaborCost";
		string columnName2 = $"imr{empty}OverheadCost";
		string columnName3 = $"imr{empty}MaterialCost";
		string columnName4 = $"imr{empty}SubcontractCost";
		string columnName5 = $"imr{empty}DutyCost";
		string columnName6 = $"imr{empty}FreightCost";
		string columnName7 = $"imr{empty}MiscCost";
		decimal num = (partRevRow.HasVersion(DataRowVersion.Original) ? partRevRow.Field<decimal>(columnName, DataRowVersion.Original) : 0m);
		decimal num2 = (partRevRow.HasVersion(DataRowVersion.Original) ? partRevRow.Field<decimal>(columnName2, DataRowVersion.Original) : 0m);
		decimal num3 = (partRevRow.HasVersion(DataRowVersion.Original) ? partRevRow.Field<decimal>(columnName3, DataRowVersion.Original) : 0m);
		decimal num4 = (partRevRow.HasVersion(DataRowVersion.Original) ? partRevRow.Field<decimal>(columnName4, DataRowVersion.Original) : 0m);
		decimal num5 = (partRevRow.HasVersion(DataRowVersion.Original) ? partRevRow.Field<decimal>(columnName5, DataRowVersion.Original) : 0m);
		decimal num6 = (partRevRow.HasVersion(DataRowVersion.Original) ? partRevRow.Field<decimal>(columnName6, DataRowVersion.Original) : 0m);
		decimal num7 = (partRevRow.HasVersion(DataRowVersion.Original) ? partRevRow.Field<decimal>(columnName7, DataRowVersion.Original) : 0m);
		partCost.LaborCost = partRevRow.Field<decimal>(columnName, DataRowVersion.Current) - num;
		partCost.OverheadCost = partRevRow.Field<decimal>(columnName2, DataRowVersion.Current) - num2;
		partCost.MaterialCost = partRevRow.Field<decimal>(columnName3, DataRowVersion.Current) - num3;
		partCost.SubcontractCost = partRevRow.Field<decimal>(columnName4, DataRowVersion.Current) - num4;
		partCost.DutyCost = partRevRow.Field<decimal>(columnName5, DataRowVersion.Current) - num5;
		partCost.FreightCost = partRevRow.Field<decimal>(columnName6, DataRowVersion.Current) - num6;
		partCost.MiscCost = partRevRow.Field<decimal>(columnName7, DataRowVersion.Current) - num7;
		partCost.ActualUnitLaborCost = partRevRow.Field<decimal>(columnName, DataRowVersion.Current);
		partCost.ActualUnitOverheadCost = partRevRow.Field<decimal>(columnName2, DataRowVersion.Current);
		partCost.ActualUnitMaterialCost = partRevRow.Field<decimal>(columnName3, DataRowVersion.Current);
		partCost.ActualUnitSubcontractCost = partRevRow.Field<decimal>(columnName4, DataRowVersion.Current);
		partCost.ActualUnitDutyCost = partRevRow.Field<decimal>(columnName5, DataRowVersion.Current);
		partCost.ActualUnitFreightCost = partRevRow.Field<decimal>(columnName6, DataRowVersion.Current);
		partCost.ActualUnitMiscCost = partRevRow.Field<decimal>(columnName7, DataRowVersion.Current);
		partCost.PrevUnitLaborCost = num;
		partCost.PrevUnitOverheadCost = num2;
		partCost.PrevUnitMaterialCost = num3;
		partCost.PrevUnitSubcontractCost = num4;
		partCost.PrevUnitDutyCost = num5;
		partCost.PrevUnitFreightCost = num6;
		partCost.PrevUnitMiscCost = num7;
		return partCost;
	}

	private DataRow AddPartTransactionCostsRecord(string userID, DateTime tranDate, PartCost costs, DataTable partTransactionCosts, int partTransactionID, int partTransactionCostID)
	{
		DataRow dataRow = partTransactionCosts.NewRow().BlankRow();
		dataRow["intPartTransactionID"] = partTransactionID;
		dataRow["intPartTransactionCostID"] = partTransactionCostID;
		dataRow["intCostType"] = costs.CostType;
		dataRow["intQuantity"] = costs.Quantity;
		dataRow["intUnitLaborCost"] = costs.LaborCost;
		dataRow["intUnitOverheadCost"] = costs.OverheadCost;
		dataRow["intUnitMaterialCost"] = costs.MaterialCost;
		dataRow["intUnitSubcontractCost"] = costs.SubcontractCost;
		dataRow["intUnitDutyCost"] = costs.DutyCost;
		dataRow["intUnitFreightCost"] = costs.FreightCost;
		dataRow["intUnitMiscCost"] = costs.MiscCost;
		dataRow["intPrevUnitLaborCost"] = costs.PrevUnitLaborCost;
		dataRow["intPrevUnitOverheadCost"] = costs.PrevUnitOverheadCost;
		dataRow["intPrevUnitMaterialCost"] = costs.PrevUnitMaterialCost;
		dataRow["intPrevUnitSubcontractCost"] = costs.PrevUnitSubcontractCost;
		dataRow["intPrevUnitDutyCost"] = costs.PrevUnitDutyCost;
		dataRow["intPrevUnitFreightCost"] = costs.PrevUnitFreightCost;
		dataRow["intPrevUnitMiscCost"] = costs.PrevUnitMiscCost;
		dataRow["intActualUnitLaborCost"] = costs.ActualUnitLaborCost;
		dataRow["intActualUnitOverheadCost"] = costs.ActualUnitOverheadCost;
		dataRow["intActualUnitMaterialCost"] = costs.ActualUnitMaterialCost;
		dataRow["intActualUnitSubcontractCost"] = costs.ActualUnitSubcontractCost;
		dataRow["intActualUnitDutyCost"] = costs.ActualUnitDutyCost;
		dataRow["intActualUnitFreightCost"] = costs.ActualUnitFreightCost;
		dataRow["intActualUnitMiscCost"] = costs.ActualUnitMiscCost;
		dataRow["intCreatedBy"] = userID;
		dataRow["intCreatedDate"] = tranDate;
		return dataRow;
	}

	public void CreatePartClassJournals(M1BindingSource bindingSource, SqlTransaction transaction)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		string text = currentAsDataRow.Field<string>("impPartClassID", DataRowVersion.Original);
		string text2 = currentAsDataRow.Field<string>("impPartClassID", DataRowVersion.Current);
		if (string.IsNullOrWhiteSpace(text) || text2.Equals(text, StringComparison.CurrentCultureIgnoreCase))
		{
			return;
		}
		M1Database database = bindingSource.Database;
		string text3 = currentAsDataRow.Field<string>("impPartID", DataRowVersion.Current);
		_ = string.Empty;
		string oldClassDescription = string.Empty;
		byte costingMethod = database.Props("PN").Field<byte>("xapIMCostingMethod");
		byte b = database.Props("FN").Field<byte>("xafCOGSUseAccounts");
		DateTime now = DateTime.Now;
		short year = new Financial().GetYearAndPeriod(database, now, "GL", IgnoreClosed: true, transaction).Year;
		byte period = new Financial().GetYearAndPeriod(database, now, "GL", IgnoreClosed: true, transaction).Period;
		CostOfGoodSoldDefinition.JournalSource headerSource = (CostOfGoodSoldDefinition.JournalSource)new CostOfGoodSoldDefinition(CostOfGoodSoldDefinition.JournalLineTransactionType.PartClassChange).getHeaderSource();
		CostOfGoodSoldDefinition.DetailSource detailSource = new CostOfGoodSoldDefinition(CostOfGoodSoldDefinition.JournalLineTransactionType.PartClassChange).getDetailSource();
		string text4 = "---1---";
		int num = 1;
		string glAccountID = string.Empty;
		string glAccountID2 = string.Empty;
		CostOfGoodSoldDefinition.Journal journal = new CostOfGoodSoldDefinition.Journal();
		CostOfGoodSoldDefinition.JournalLine journalLine = new CostOfGoodSoldDefinition.JournalLine();
		string text5 = string.Empty;
		COGSAccounts cOGSAccounts = null;
		COGSAccounts cOGSAccounts2 = null;
		using (SqlCommand sqlCommand = new SqlCommand("SELECT imcDescription FROM PartClasses WHERE  imcPartClassID = @oldPartClassID"))
		{
			sqlCommand.Parameters.Add(new SqlParameter("@oldPartClassID", SqlDbType.NVarChar)).Value = text;
			object obj = database.ExecuteScalar(sqlCommand, transaction);
			oldClassDescription = ((obj == null) ? string.Empty : obj.ToString().Trim());
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("SELECT * FROM ((SELECT * FROM (        SELECT  imgPartID, imgPartRevisionID, imgWarehouseID, imgQuantityType, imgRemainingQuantity, imgUnitLaborCost, imgUnitOverheadCost, imgUnitMaterialCost,imgUnitSubcontractCost, imgUnitDutyCost, imgUnitFreightCost, imgUnitMiscCost,imgUniqueID FROM    PartBinDetails WHERE imgPartID = @partID AND PartBinDetails.imgRemainingQuantity > 0 ) AS PB INNER JOIN ( SELECT imrPartID, imrPartRevisionID, imrAverageLaborCost, imrAverageOverheadCost, imrAverageMaterialCost, imrAverageSubcontractCost, imrLastLaborCost, imrLastOverheadCost,imrLastMaterialCost, imrLastSubcontractCost, imrStandardOverheadCost, imrStandardLaborCost, imrStandardMaterialCost, imrStandardSubcontractCost,imrAverageDutyCost, imrAverageFreightCost, imrAverageMiscCost, imrLastDutyCost, imrLastFreightCost, imrLastMiscCost, imrStandardDutyCost,imrStandardFreightCost, imrStandardMiscCost FROM PartRevisions WHERE imrPartID = @partID ) AS PR ON PB.imgPartID = PR.imrPartID AND PB.imgPartRevisionID = PR.imrPartRevisionID ) AS PBPR INNER JOIN (SELECT imwWarehouseID, imwPlantID FROM   Warehouses ) AS WH ON PBPR.imgWarehouseID = WH.imwWarehouseID INNER JOIN (SELECT  impPartID, impPartClassID FROM  Parts WHERE impPartClassID = @newPartClassID) AS PT ON PBPR.imgPartID = PT.impPartID INNER JOIN (SELECT imcPartClassID, imcDescription FROM   PartClasses WHERE  imcPartClassID =  @newPartClassID ) AS PC ON PT.impPartClassID = PC.imcPartClassID) ORDER BY PBPR.imgPartRevisionID,PBPR.imgQuantityType");
		sqlCommand2.Parameters.Add(new SqlParameter("@partID", SqlDbType.NVarChar)).Value = text3;
		sqlCommand2.Parameters.Add(new SqlParameter("@newPartClassID", SqlDbType.NVarChar)).Value = text2;
		DataTable dataTable = database.GetDataTable(sqlCommand2, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		if (b == 1)
		{
			cOGSAccounts = GetPartAccounts(database, transaction, text, string.Empty, b);
			cOGSAccounts2 = GetPartAccounts(database, transaction, text2, string.Empty, b);
		}
		string newClassDescription = dataTable.Rows[0].Field<string>("imcDescription");
		IList<CostOfGoodSoldDefinition.Journal> list = new List<CostOfGoodSoldDefinition.Journal>();
		IList<CostOfGoodSoldDefinition.JournalLine> list2 = new List<CostOfGoodSoldDefinition.JournalLine>();
		foreach (DataRow row in dataTable.Rows)
		{
			text5 = row.Field<string>("imrPartRevisionID");
			byte b2 = row.Field<byte>("imgQuantityType");
			string plantId = row.Field<string>("imwPlantID");
			Guid sourceUniqueId = row.Field<Guid>("imgUniqueID");
			decimal num2 = CalculateJournalValue(row, costingMethod);
			if (b != 1)
			{
				cOGSAccounts = GetPartAccounts(database, transaction, text, plantId, b);
				cOGSAccounts2 = GetPartAccounts(database, transaction, text2, plantId, b);
			}
			switch (b2)
			{
			case 1:
				glAccountID = cOGSAccounts2.InventoryGLAccountID;
				glAccountID2 = cOGSAccounts.InventoryGLAccountID;
				break;
			case 2:
				glAccountID = cOGSAccounts2.InventoryInInspectionGLAccountID;
				glAccountID2 = cOGSAccounts.InventoryInInspectionGLAccountID;
				break;
			case 3:
				glAccountID = cOGSAccounts2.InventoryToReturnGLAccountID;
				glAccountID2 = cOGSAccounts.InventoryToReturnGLAccountID;
				break;
			}
			if (!text4.Equals(text5, StringComparison.CurrentCultureIgnoreCase))
			{
				if (list2.Count > 0)
				{
					journal.JournalLines = list2.ToList();
					journal.TotalDebits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.DebitAmount);
					journal.TotalCredits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.CreditAmount);
					journal.LongDescriptionText = GetLongDescriptionText(text3, text4.Equals("---1---", StringComparison.CurrentCultureIgnoreCase) ? text5 : text4, oldClassDescription, newClassDescription);
					list.Add(journal);
					list2 = new List<CostOfGoodSoldDefinition.JournalLine>();
				}
				journal = new COGS().BuildJournalObject(database, transaction, now, year, period, headerSource, detailSource, "Part Class Change Transaction");
				text4 = text5;
				num = 1;
			}
			journalLine = new COGS().BuildJournalLineObject(database, transaction, journal, num, num2, glAccountID, sourceUniqueId, "Part Class Change Transaction", CostOfGoodSoldDefinition.JournalLineTransactionType.PartClassChange, null);
			list2.Add(journalLine);
			num++;
			journalLine = new COGS().BuildJournalLineObject(database, transaction, journal, num, -num2, glAccountID2, sourceUniqueId, "Part Class Change Transaction", CostOfGoodSoldDefinition.JournalLineTransactionType.PartClassChange, null);
			list2.Add(journalLine);
			num++;
		}
		if (list2.Count > 0)
		{
			journal.JournalLines = list2.ToList();
			journal.TotalDebits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.DebitAmount);
			journal.TotalCredits = journal.JournalLines.Sum((CostOfGoodSoldDefinition.JournalLine x) => x.CreditAmount);
			journal.LongDescriptionText = GetLongDescriptionText(text3, text5, oldClassDescription, newClassDescription);
			list.Add(journal);
			list2 = new List<CostOfGoodSoldDefinition.JournalLine>();
		}
		if (list.Count <= 0)
		{
			return;
		}
		foreach (CostOfGoodSoldDefinition.Journal item in list)
		{
			new COGS().AddJournal(database, transaction, item, "PartBinDetails", currentAsDataRow, bindingSource.PrimaryTable.FieldPrefix);
		}
	}

	private string GetLongDescriptionText(string part, string revision, string oldClassDescription, string newClassDescription)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Part:");
		stringBuilder.AppendLine(part);
		stringBuilder.Append("Revision:");
		stringBuilder.AppendLine(revision);
		stringBuilder.Append("Original Part Class:");
		stringBuilder.AppendLine(oldClassDescription);
		stringBuilder.Append("New Part Class:");
		stringBuilder.AppendLine(newClassDescription);
		return stringBuilder.ToString().Trim();
	}

	private decimal CalculateJournalValue(DataRow binDetailRow, byte costingMethod)
	{
		decimal num = default(decimal);
		switch (costingMethod)
		{
		case 1:
			num = Math.Round(binDetailRow.Field<decimal>("imrAverageLaborCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrAverageOverheadCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrAverageMaterialCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrAverageSubcontractCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrAverageDutyCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrAverageFreightCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrAverageMiscCost"), 2);
			break;
		case 2:
			num = default(decimal);
			break;
		case 3:
			num = Math.Round(binDetailRow.Field<decimal>("imrStandardOverheadCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrStandardLaborCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrStandardMaterialCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrStandardSubcontractCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrStandardDutyCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrStandardFreightCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imrStandardMiscCost"), 2);
			break;
		case 4:
		case 5:
			num = Math.Round(binDetailRow.Field<decimal>("imgUnitOverheadCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imgUnitLaborCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imgUnitMaterialCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imgUnitSubcontractCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imgUnitDutyCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imgUnitFreightCost"), 2) + Math.Round(binDetailRow.Field<decimal>("imgUnitMiscCost"), 2);
			break;
		}
		return Math.Round(binDetailRow.Field<decimal>("imgRemainingQuantity") * num, 2);
	}

	private COGSAccounts GetPartAccounts(M1Database database, SqlTransaction transaction, string partClassId, string plantId, byte accountFrom)
	{
		SqlCommand sqlCommand = new SqlCommand();
		COGSAccounts cOGSAccounts = new COGSAccounts();
		if (string.IsNullOrWhiteSpace(plantId))
		{
			sqlCommand = database.NewSqlCommand("SELECT imcInventoryGLAccountID,imCInvToReturnGLAccountID,imcInvInInspectionGLAccountID FROM PartClasses WHERE(imcPartClassID = @partClassID) ");
			sqlCommand.Parameters.Add(new SqlParameter("@partClassID", SqlDbType.NVarChar)).Value = partClassId;
		}
		else
		{
			sqlCommand = database.NewSqlCommand("SELECT imfInventoryGLAccountID,imfInvToReturnGLAccountID,imfInvInInspectionGLAccountID FROM PartClasses Left Outer Join PartClassPlants On imcPartClassID = imfPartClassID WHERE imcPartClassID=@partClassID And imfPartClassPlantID =@partClassPlantID");
			sqlCommand.Parameters.Add(new SqlParameter("@partClassID", SqlDbType.NVarChar)).Value = partClassId;
			sqlCommand.Parameters.Add(new SqlParameter("@partClassPlantID", SqlDbType.NVarChar)).Value = plantId;
		}
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			cOGSAccounts.PartClassID = partClassId;
			if (dataRow.Table.Columns.Contains("imcInventoryGLAccountID") && dataRow["imcInventoryGLAccountID"] != DBNull.Value)
			{
				cOGSAccounts.InventoryGLAccountID = dataRow.Field<string>("imcInventoryGLAccountID");
			}
			if (dataRow.Table.Columns.Contains("imfInventoryGLAccountID") && dataRow["imfInventoryGLAccountID"] != DBNull.Value)
			{
				cOGSAccounts.InventoryGLAccountID = dataRow.Field<string>("imfInventoryGLAccountID");
			}
			if (dataRow.Table.Columns.Contains("imcInvToReturnGLAccountID") && dataRow["imcInvToReturnGLAccountID"] != DBNull.Value)
			{
				cOGSAccounts.InventoryToReturnGLAccountID = dataRow.Field<string>("imcInvToReturnGLAccountID");
			}
			if (dataRow.Table.Columns.Contains("imfInvToReturnGLAccountID") && dataRow["imfInvToReturnGLAccountID"] != DBNull.Value)
			{
				cOGSAccounts.InventoryToReturnGLAccountID = dataRow.Field<string>("imfInvToReturnGLAccountID");
			}
			if (dataRow.Table.Columns.Contains("imcInvInInspectionGLAccountID") && dataRow["imcInvInInspectionGLAccountID"] != DBNull.Value)
			{
				cOGSAccounts.InventoryInInspectionGLAccountID = dataRow.Field<string>("imcInvInInspectionGLAccountID");
			}
			if (dataRow.Table.Columns.Contains("imfInvInInspectionGLAccountID") && dataRow["imfInvInInspectionGLAccountID"] != DBNull.Value)
			{
				cOGSAccounts.InventoryInInspectionGLAccountID = dataRow.Field<string>("imfInvInInspectionGLAccountID");
			}
		}
		return cOGSAccounts;
	}

	public void CreatePartCostsJournals(M1BindingSource bindingSource, SqlTransaction transaction)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		new CostOfGoodSoldDefinition(bindingSource, "imrQuantityOnHand", "imrPartRevisionID", DateTime.Now, 35, 3, reverseSign: false, currentAsDataRow.Field<decimal>("imrQuantityOnHand"), "ManualJournalCreation").AddJournal(bindingSource.Database, currentAsDataRow, DataRowVersion.Current, bindingSource.Transaction);
	}

	public void StandardCostRollupUpdate(M1Database database, string partID, string partRevisionID, decimal laborCost, decimal overheadCost, decimal materialCost, decimal subcontractCost, bool updateUnitSalePrice, decimal unitSalePrice, string currencyID)
	{
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			SqlCommand sqlCommand = database.NewSqlCommand("Select imrPartID, imrPartRevisionID, imrStandardLaborCost, imrStandardOverheadCost, imrStandardMaterialCost, imrStandardSubcontractCost From PartRevisions WITH(NOLOCK) Where imrPartID = @PartID And imrPartRevisionID = @RevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
			using (DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: false, out adapter, sqlTransaction))
			{
				if (dataTable.Rows.Count != 0)
				{
					DataRow dataRow = dataTable.Rows[0];
					if (dataRow != null && (dataRow.Field<decimal>("imrStandardLaborCost") != laborCost || dataRow.Field<decimal>("imrStandardOverheadCost") != overheadCost || dataRow.Field<decimal>("imrStandardMaterialCost") != materialCost || dataRow.Field<decimal>("imrStandardSubcontractCost") != subcontractCost))
					{
						dataRow["imrStandardLaborCost"] = laborCost;
						dataRow["imrStandardOverheadCost"] = overheadCost;
						dataRow["imrStandardMaterialCost"] = materialCost;
						dataRow["imrStandardSubcontractCost"] = subcontractCost;
						using (M1BindingSource m1BindingSource = new M1BindingSource(database, sqlTransaction))
						{
							m1BindingSource.DataSourceTable = "PARTREVISIONS";
							m1BindingSource.NavigateTo(database, "imrPartID = " + M1Util.ConvertToSql(partID) + " And imrPartRevisionID = " + M1Util.ConvertToSql(partRevisionID));
							if (m1BindingSource.Count != 0)
							{
								DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
								currentAsDataRow["imrStandardLaborCost"] = laborCost;
								currentAsDataRow["imrStandardOverheadCost"] = overheadCost;
								currentAsDataRow["imrStandardMaterialCost"] = materialCost;
								currentAsDataRow["imrStandardSubcontractCost"] = subcontractCost;
								new Part().AddCostsUpdatesTransaction(m1BindingSource, sqlTransaction);
								if (database.Props("FinancialProperties").Field<bool>("xafGLCreateStockJournals") && database.Props("ProductionProperties").Field<byte>("xapIMCostingMethod").Equals(3))
								{
									new CostOfGoodSoldDefinition(m1BindingSource, "imrQuantityOnHand", "imrPartRevisionID", DateTime.Now, 37, 3, reverseSign: false, currentAsDataRow.Field<decimal>("imrQuantityOnHand"), "ManualJournalCreation").AddJournal(m1BindingSource.Database, currentAsDataRow, DataRowVersion.Current, sqlTransaction);
								}
							}
						}
						database.UpdateData(new DataRow[1] { dataRow }, adapter, sqlTransaction);
					}
				}
			}
			if (updateUnitSalePrice)
			{
				sqlCommand = database.NewSqlCommand("Select * From PartUnitSalePrices WITH(NOLOCK) Where imhPartID = @PartID And imhPartRevisionID = @RevisionID And imhCurrencyRateID = @CurrencyID And ISNULL(imhStartDate,'19000101') <= GETDATE() AND ISNULL(imhEndDate,'20790606') >= GETDATE() Order By imhCurrencyRateID Desc");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
				sqlCommand.Parameters.Add(new SqlParameter("@CurrencyID", SqlDbType.NVarChar)).Value = currencyID;
				using DataTable dataTable2 = database.GetDataTable(sqlCommand, fillSchema: false, out adapter, sqlTransaction);
				if (dataTable2.Rows.Count == 0)
				{
					if (unitSalePrice != 0m)
					{
						DataRow dataRow2 = dataTable2.AddBlankRow();
						dataRow2["imhPartID"] = partID;
						dataRow2["imhPartRevisionID"] = partRevisionID;
						dataRow2["imhStartDate"] = DateTime.Now;
						dataRow2["imhUnitSalePrice"] = unitSalePrice;
						dataRow2["imhCurrencyRateID"] = currencyID;
						dataRow2["imhCreatedBy"] = database.User.ID;
						dataRow2["imhCreatedDate"] = DateTime.Now;
						database.UpdateData(new DataRow[1] { dataRow2 }, adapter, sqlTransaction);
					}
				}
				else
				{
					DataRow dataRow3 = dataTable2.Rows[0];
					if (dataRow3 != null)
					{
						dataRow3["imhUnitSalePrice"] = unitSalePrice;
						database.UpdateData(new DataRow[1] { dataRow3 }, adapter, sqlTransaction);
					}
				}
			}
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
		}
	}

	public SupplierRequirement GetSupplierRequirements(M1Database database, SqlTransaction transaction, string partID, string revisionID, string orgID, string locID)
	{
		SupplierRequirement supplierRequirement = new SupplierRequirement();
		if (!string.IsNullOrWhiteSpace(partID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("SELECT imxMinimumPurchaseQuantity, imxLotSize FROM PartCrossReferences WHERE imxPartID = @PartID and imxPartRevisionID = @RevisionID and imxOrganizationID = @OrgID and imxLocationID = @LocationID and imxInactive = 0 and imxPurchased = 1");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.NVarChar)).Value = revisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@OrgID", SqlDbType.NVarChar)).Value = orgID;
			sqlCommand.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.NVarChar)).Value = locID;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				supplierRequirement.PartID = partID;
				supplierRequirement.RevisionID = revisionID;
				supplierRequirement.OrgID = orgID;
				supplierRequirement.LocID = locID;
				supplierRequirement.MinPurQty = row.Field<decimal>("imxMinimumPurchaseQuantity");
				supplierRequirement.LotSize = row.Field<decimal>("imxLotSize");
			}
		}
		return supplierRequirement;
	}

	public decimal GetInventoryQuantityInProduction(M1Database database, string partID, string partRevisionID, DateTime cutoffDate, string plants = null, string warehouses = null)
	{
		bool flag = !string.IsNullOrWhiteSpace(plants);
		bool flag2 = !string.IsNullOrWhiteSpace(warehouses);
		SqlCommand sqlCommand = database.NewSqlCommand("Select isnull(sum(Case When isnull(jmaQuantityReceivedToInventory,0) = 0 Then IsNull(jmaInventoryQuantity, 0) When isnull(jmaQuantityReceivedToInventory,0) > 0 AND jmaProductionComplete = 0 Then IsNull(jmaInventoryQuantity - jmaQuantityReceivedToInventory, 0) When isnull(jmaQuantityReceivedToInventory,0) > 0 AND jmaProductionComplete = 1 Then 0 Else 0 End),0) As InvQtyInProduction\r\n                                From JobAssemblies \r\n                                left outer Join Jobs on jmpJobID = jmaJobID \r\n                                " + (flag ? " Left Outer Join Warehouses on imwWarehouseID = jmaPartWareHouseLocationID " : string.Empty) + "\r\n                                Where jmaPartID = @PartId And jmaPartRevisionID = @PartRevisionId \r\n                                " + (flag ? (" and imwPlantID in (" + plants + ") ") : string.Empty) + "\r\n                                " + ((!flag && flag2) ? (" and jmaPartWarehouseLocationID in (" + warehouses + ") ") : string.Empty) + "\r\n                                and IsNull(jmaScheduledDueDate, jmpProductionDueDate) < @CutOffDate\r\n                                and jmaClosed = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@PartId", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionId", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@CutOffDate", SqlDbType.DateTime)).Value = cutoffDate;
		return (decimal)database.ExecuteScalar(sqlCommand);
	}

	public bool GetFutureAdjustmentTransactionStatus(M1Database database, SqlTransaction transaction, string partID, string revisionID, string warehouseID, string binID, DateTime? tranDate)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT ISNULL(COUNT(*),0) FROM PartTransactions Where imtPartID = @PartID and imtPartRevisionID = @RevisionID and imtPartWarehouseLocationID = @WarehouseID and imtPartBinID = @BinID and imtTransactionDate > @TransDate AND imtTransactionType = 3 AND imtSource <> 7");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.VarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.VarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.VarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.VarChar)).Value = binID;
		sqlCommand.Parameters.Add(new SqlParameter("@TransDate", SqlDbType.DateTime)).Value = tranDate;
		return (int)database.ExecuteScalar(sqlCommand, transaction) != 0;
	}

	public bool PartRevisionQuantitiesConcurrencyCheck(M1BindingSource bindingSource)
	{
		if (bindingSource != null)
		{
			DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
			if (currentAsDataRow != null)
			{
				SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand("SELECT ISNULL(COUNT(*),0) FROM PartRevisions WHERE imrPartID = @PartID AND imrPartRevisionID = @RevisionID        AND (imrQuantityOnHand <> @QtyOnHand OR imrQuantityAllocated <> @QtyAllocated        OR imrQuantityOnOrderPurchases <> @QtyOnOrderPurchases OR imrQuantityOnOrderSales <> @QtyOnOrderSales        OR imrQuantityToInspect <> @QtyToInspect OR imrQuantityToReturn <> @QtyToReturn OR imrQuantityToReturnJob <> @QtyToReturnJob)");
				sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.VarChar)).Value = currentAsDataRow.Field<string>("imrPartID");
				sqlCommand.Parameters.Add(new SqlParameter("@RevisionID", SqlDbType.VarChar)).Value = currentAsDataRow.Field<string>("imrPartRevisionID");
				sqlCommand.Parameters.Add(new SqlParameter("@QtyOnHand", SqlDbType.Decimal)).Value = currentAsDataRow.Field<decimal>("imrQuantityOnHand");
				sqlCommand.Parameters.Add(new SqlParameter("@QtyAllocated", SqlDbType.Decimal)).Value = currentAsDataRow.Field<decimal>("imrQuantityAllocated");
				sqlCommand.Parameters.Add(new SqlParameter("@QtyOnOrderPurchases", SqlDbType.Decimal)).Value = currentAsDataRow.Field<decimal>("imrQuantityOnOrderPurchases");
				sqlCommand.Parameters.Add(new SqlParameter("@QtyOnOrderSales", SqlDbType.Decimal)).Value = currentAsDataRow.Field<decimal>("imrQuantityOnOrderSales");
				sqlCommand.Parameters.Add(new SqlParameter("@QtyToInspect", SqlDbType.Decimal)).Value = currentAsDataRow.Field<decimal>("imrQuantityToInspect");
				sqlCommand.Parameters.Add(new SqlParameter("@QtyToReturn", SqlDbType.Decimal)).Value = currentAsDataRow.Field<decimal>("imrQuantityToReturn");
				sqlCommand.Parameters.Add(new SqlParameter("@QtyToReturnJob", SqlDbType.Decimal)).Value = currentAsDataRow.Field<decimal>("imrQuantityToReturnJob");
				return (int)bindingSource.Database.ExecuteScalar(sqlCommand, bindingSource.Transaction) == 0;
			}
		}
		return true;
	}

	public string ChangeAllowNegativeQuantityOnHandSetting(M1BindingSource bindingSource)
	{
		string result = string.Empty;
		if (bindingSource.CurrentAsDataRow == null)
		{
			return string.Empty;
		}
		try
		{
			DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
			bool flag = currentAsDataRow.Field<bool>("xapIMAllowNegativeQtyOnHand");
			string text = currentAsDataRow["xapAllowNegQtyOnHandHistory"].ToString();
			string text2 = DateTime.Now.ToString() + "," + bindingSource.CurrentDatabase.User.ID + ",";
			if (flag)
			{
				text2 += "Enabled,Disabled";
				currentAsDataRow["xapIMAllowNegativeQtyOnHand"] = false;
				currentAsDataRow["xapIMEnableWarningWhenNegative"] = false;
			}
			else
			{
				text2 += "Disabled,Enabled";
				currentAsDataRow["xapIMAllowNegativeQtyOnHand"] = true;
				currentAsDataRow["xapIMEnableWarningWhenNegative"] = true;
			}
			if (!string.IsNullOrEmpty(text))
			{
				text2 = text + Environment.NewLine + text2;
			}
			currentAsDataRow["xapAllowNegQtyOnHandHistory"] = text2;
			bindingSource.SaveData();
		}
		catch (Exception ex)
		{
			result = ex.Message;
		}
		return result;
	}

	public string ValidateChangeAllowNegativeQuantityOnHandSetting(M1BindingSource bindingSource, ref bool passValidation, bool enableNegativeQtyOH)
	{
		passValidation = true;
		if (bindingSource.CurrentAsDataRow == null)
		{
			return string.Empty;
		}
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		StringBuilder stringBuilder = new StringBuilder();
		string text = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbBinQuantityOnHand FROM PartBins ";
		if (enableNegativeQtyOH)
		{
			if (currentAsDataRow.Field<bool>("xapGLCreateStockJournals"))
			{
				stringBuilder.AppendLine("COGS is ENABLED." + Environment.NewLine);
				passValidation = false;
			}
			string text2 = currentAsDataRow["xapIMCostingMethod"].ToString();
			if (string.IsNullOrEmpty(text2) || text2 == "4" || text2 == "5")
			{
				stringBuilder.AppendLine("FIFO or LIFO is ENABLED." + Environment.NewLine);
				passValidation = false;
			}
			SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand(text + " WHERE imbBinQuantityOnHand <> 0 ORDER BY imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID ");
			DataTable dataTable = bindingSource.Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				passValidation = false;
				stringBuilder.AppendLine("All Parts must have 0 (ZERO) Quantity on Hand before Negative Quantity on Hand is enabled. The following parts have Quantity on Hand GREATER or LESS THAN 0:");
				foreach (DataRow row3 in dataTable.Rows)
				{
					stringBuilder.AppendLine(GeneratePartValidationLine(row3.Field<string>("imbPartID"), row3.Field<string>("imbPartRevisionID"), row3.Field<string>("imbWarehouseID"), row3.Field<string>("imbPartBinID"), row3.Field<decimal>("imbBinQuantityOnHand").ToString()));
				}
			}
		}
		else
		{
			SqlCommand sqlCommand2 = bindingSource.Database.NewSqlCommand(text + " WHERE imbBinQuantityOnHand < 0 ORDER BY imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID ");
			DataTable dataTable2 = bindingSource.Database.GetDataTable(sqlCommand2);
			if (dataTable2.Rows.Count > 0)
			{
				stringBuilder.AppendLine("Parts exist which have Negative QOH and that those Parts CANNOT be used in Transactions until there is not a Negative QOH:");
				foreach (DataRow row4 in dataTable2.Rows)
				{
					stringBuilder.AppendLine(GeneratePartValidationLine(row4.Field<string>("imbPartID"), row4.Field<string>("imbPartRevisionID"), row4.Field<string>("imbWarehouseID"), row4.Field<string>("imbPartBinID"), row4.Field<decimal>("imbBinQuantityOnHand").ToString()));
				}
			}
		}
		return stringBuilder.ToString();
	}

	private string GeneratePartValidationLine(string part, string rev, string warehouse, string bin, string QOH)
	{
		string text = part + (string.IsNullOrEmpty(rev) ? string.Empty : (" / Rev " + rev));
		if (text.Length <= 16)
		{
			text += "\t";
		}
		if (text.Length <= 8)
		{
			text += "\t";
		}
		return "Part/Rev: " + text + "\tWarehouse/Bin: " + warehouse + "/" + bin + "\tQty on Hand: " + QOH;
	}

	public DataTable GetQuantityAllocations(M1Database database, string partId = "", string partRevisionId = "", string warehouseId = "", string warehouseBinId = "", bool includePartRevision = false)
	{
		using SqlCommand sqlCommand = database.NewSqlCommand(string.Empty);
		bool flag = true;
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(partId))
		{
			bool flag2 = !flag;
			if (flag)
			{
				flag = false;
			}
			stringBuilder.Append((flag2 ? " AND" : string.Empty) + " imrPartID = @PartID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partId;
		}
		if (!string.IsNullOrEmpty(partRevisionId) || includePartRevision)
		{
			bool flag3 = !flag;
			if (flag)
			{
				flag = false;
			}
			stringBuilder.Append((flag3 ? " AND" : string.Empty) + " imrPartRevisionID = @PartRevision");
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevision", SqlDbType.NVarChar)).Value = partRevisionId;
		}
		if (!string.IsNullOrEmpty(warehouseId))
		{
			bool flag4 = !flag;
			if (flag)
			{
				flag = false;
			}
			stringBuilder.Append((flag4 ? " AND" : string.Empty) + " jmmPartWarehouseLocationID = @Warehouse");
			sqlCommand.Parameters.Add(new SqlParameter("@Warehouse", SqlDbType.NVarChar)).Value = warehouseId;
		}
		if (!string.IsNullOrEmpty(warehouseBinId))
		{
			bool flag5 = !flag;
			stringBuilder.Append((flag5 ? " AND" : string.Empty) + " jmmPartBinID = @BinID");
			sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = warehouseBinId;
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Insert(0, " WHERE ");
		}
		sqlCommand.CommandText = $"SELECT * FROM (\r\n                    SELECT jmmPartID as imrPartID, jmmPartRevisionID as imrPartRevisionID, jmmPartWarehouseLocationID as jmmPartWarehouseLocationID, jmmPartBinID as jmmPartBinID, jmmEstimatedQuantity - jmmQuantityReceived as imrQuantityAllocated, '' as omdSalesOrderID, jmmJobID as jmmJobID \r\n                    FROM JobMaterials \r\n                    WHERE jmmPullAllFromStock = 1 AND jmmClosed = 0 AND jmmReceivedComplete = 0 AND jmmKitPart = 0 AND ((jmmEstimatedQuantity >= 0 And jmmQuantityReceived < jmmEstimatedQuantity) OR (jmmEstimatedQuantity < 0 AND jmmQuantityReceived > jmmEstimatedQuantity))\r\n                UNION ALL (\r\n                    SELECT jmtPartID as imrPartID, jmtPartRevisionID as imrPartRevisionID, jmtPartWarehouseLocationID as jmmPartWarehouseLocationID, jmtPartBinID as jmmPartBinID, jmtMaterialQuantity - jmtQuantityReceived As imrQuantityAllocated, '' As omdSalesOrderID, jmtjobid As jmmJobID \r\n                    FROM JobMaterialComponents \r\n                    INNER JOIN JobMaterials ON jmtJobID = jmmJobID And jmtJobAssemblyID = jmmJobAssemblyID And jmtJobMaterialID = jmmJobMaterialID \r\n                    WHERE jmtPullAllFromStock = 1 And jmtClosed = 0 And jmtReceivedComplete = 0 And jmmKitPart <> 0 And ((jmtMaterialQuantity >= 0 And jmtQuantityReceived < jmtMaterialQuantity) Or (jmtMaterialQuantity < 0 And jmtQuantityReceived > jmtMaterialQuantity))\r\n                    )\r\n                UNION ALL (\r\n                    SELECT jmaPartID as imrPartID, jmaPartRevisionID as imrPartRevisionID, jmaPartWarehouseLocationID as jmmPartWarehouseLocationID, jmaPartBinID as jmmPartBinID, jmaQuantityToPull - jmaQuantityIssued As imrQuantityAllocated, '' As omdSalesOrderID, jmajobid As jmmJobID \r\n                    FROM JobAssemblies\r\n                    WHERE jmaClosed = 0 And jmaIssuedComplete = 0 And ((jmaQuantityToPull >= 0 And jmaQuantityIssued < jmaQuantityToPull) Or (jmaQuantityToPull < 0 And jmaQuantityIssued > jmaQuantityToPull))\r\n                    ) \r\n                UNION ALL (\r\n                    SELECT omdPartID as imrPartID, omdPartRevisionID as imrPartRevisionID, omdPartWarehouseLocationID as jmmPartWarehouseLocationID, omdPartBinID as jmmPartBinID, omdDeliveryQuantity - omdQuantityShipped As imrQuantityAllocated, omdsalesorderid As omdSalesOrderID, '' as jmmJobID \r\n                    FROM SalesOrderDeliveries\r\n                    WHERE omdClosed = 0 And omdDeliveryType in (2) And omdShippedComplete = 0 and ((omdDeliveryQuantity >= 0 And omdQuantityShipped < omdDeliveryQuantity) Or (omdDeliveryQuantity < 0 And omdQuantityShipped > omdDeliveryQuantity))\r\n                    )\r\n                UNION ALL (\r\n                    SELECT omoPartID as imrPartID, omoPartRevisionID as imrPartRevisionID, omoPartWarehouseLocationID as jmmPartWarehouseLocationID, omoPartBinID as jmmPartBinID, omoDeliveryQuantity - omoQuantityShipped As imrQuantityAllocated, omosalesorderid As omdSalesOrderID, '' as jmmJobID \r\n                    FROM SalesOrderComponents \r\n                    WHERE omoClosed = 0 And omoShippedComplete = 0 and ((omoDeliveryQuantity >= 0 And omoQuantityShipped < omoDeliveryQuantity) Or (omoDeliveryQuantity < 0 And omoQuantityShipped > omoDeliveryQuantity))\r\n                    )\r\n                ) AS test\r\n                INNER JOIN PartBins pb ON imrPartID = pb.imbPartID AND imrPartRevisionID = pb.imbPartRevisionID AND jmmPartWarehouseLocationID = pb.imbWarehouseID AND jmmPartBinID = pb.imbPartBinID \r\n                {stringBuilder}";
		return database.GetDataTable(sqlCommand);
	}

	public DataTable GetQuantityToInspect(M1Database database, string partId = "", string partRevisionId = "", string warehouseId = "", string warehouseBinId = "", bool includePartRevision = false)
	{
		using SqlCommand sqlCommand = database.NewSqlCommand(string.Empty);
		bool flag = true;
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(partId))
		{
			bool flag2 = !flag;
			if (flag)
			{
				flag = false;
			}
			stringBuilder.Append((flag2 ? " AND" : string.Empty) + " qalPartID = @PartID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partId;
		}
		if (!string.IsNullOrEmpty(partRevisionId) || includePartRevision)
		{
			bool flag3 = !flag;
			if (flag)
			{
				flag = false;
			}
			stringBuilder.Append((flag3 ? " AND" : string.Empty) + " qalPartRevisionID = @PartRevision");
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevision", SqlDbType.NVarChar)).Value = partRevisionId;
		}
		if (!string.IsNullOrEmpty(warehouseId))
		{
			bool flag4 = !flag;
			if (flag)
			{
				flag = false;
			}
			stringBuilder.Append((flag4 ? " AND" : string.Empty) + " qalPartWarehouseLocationID = @Warehouse");
			sqlCommand.Parameters.Add(new SqlParameter("@Warehouse", SqlDbType.NVarChar)).Value = warehouseId;
		}
		if (!string.IsNullOrEmpty(warehouseBinId))
		{
			bool flag5 = !flag;
			stringBuilder.Append((flag5 ? " AND" : string.Empty) + " qalPartBinID = @BinID");
			sqlCommand.Parameters.Add(new SqlParameter("@BinID", SqlDbType.NVarChar)).Value = warehouseBinId;
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Insert(0, " WHERE ");
		}
		sqlCommand.CommandText = $"SELECT qalInspectionID, qalInspectionLineID, qalPartID, qalPartRevisionID, qalPartWarehouseLocationID, qalPartBinID, qalQuantityToInspect, imbInactiveBin FROM (\r\n                    (\r\n                        SELECT il.qalInspectionID AS qalInspectionID, il.qalInspectionLineID AS qalInspectionLineID, il.qalPartID AS qalPartID, il.qalPartRevisionID AS qalPartRevisionID, il.qalPartWarehouseLocationID AS qalPartWarehouseLocationID, il.qalPartBinID AS qalPartBinID, il.qalQuantityToInspect AS qalQuantityToInspect\r\n                        FROM InspectionLines il\r\n                        WHERE il.qalStatus IN ('P', 'O') AND (il.qalManualInspectionFinalized = 1 OR il.qalSourceTableName in ('ReceiptLines', 'MfgReceipts'))\r\n                    )\r\n                    UNION ALL (\r\n                        SELECT ic.qamInspectionID AS qalInspectionID, ic.qamInspectionLineID AS qalInspectionLineID, ic.qamPartID AS qalPartID, ic.qamPartRevisionID AS qalPartRevisionID, ic.qamPartWarehouseLocationID AS qalPartWarehouseLocationID, ic.qamPartBinID AS qalPartBinID, ic.qamComponentQtyToInspect AS qalQuantityToInspect\r\n                        FROM InspectionComponents ic\r\n                        INNER JOIN InspectionLines il2 ON ic.qamInspectionID = il2.qalInspectionID\r\n                        WHERE il2.qalStatus IN ('P', 'O') AND (ic.qamManualInspectionFinalized = 1 OR ic.qamSourceTableName in ('ReceiptLines', 'MfgReceipts'))\r\n                    )\r\n                    ) AS PartQTI\r\n                    INNER JOIN PartBins pb ON qalPartID = pb.imbPartID AND qalPartRevisionID = pb.imbPartRevisionID AND qalPartWarehouseLocationID = pb.imbWarehouseID AND qalPartBinID = pb.imbPartBinID\r\n                    {stringBuilder}";
		return database.GetDataTable(sqlCommand);
	}

	public bool IsPartBinInactive(M1Database database, string partId, string partRevision, string warehouseId, string binId, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT imbInactiveBin FROM PartBins WHERE imbPartID = @PartID AND imbPartRevisionID = @PartRevisionID AND imbWarehouseID = @WarehouseID AND imbPartBinID = @PartBinID");
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		sqlCommand.Parameters.AddWithValue("@PartRevisionID", partRevision);
		sqlCommand.Parameters.AddWithValue("@WarehouseID", warehouseId);
		sqlCommand.Parameters.AddWithValue("@PartBinID", binId);
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public decimal GetQuantityOnHand(M1Database database, string partId, string partRevision, string warehouseId, string binId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT imbQuantityOnHand FROM PartBins WITH (NOLOCK) WHERE imbPartID = @PartID AND imbPartRevisionID = @PartRevisionID AND imbWarehouseID = @WarehouseID AND imbPartBinID = @PartBinID");
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		sqlCommand.Parameters.AddWithValue("@PartRevisionID", partRevision);
		sqlCommand.Parameters.AddWithValue("@WarehouseID", warehouseId);
		sqlCommand.Parameters.AddWithValue("@PartBinID", binId);
		return Convert.ToDecimal(database.ExecuteScalar(sqlCommand));
	}

	public int GetPartCountForWarehouseBin(M1Database database, string warehouseId, string binId, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT count(*) FROM PartBins WITH (NOLOCK) WHERE imbWarehouseID = @WarehouseID AND imbPartBinID = @PartBinID");
		sqlCommand.Parameters.AddWithValue("@WarehouseID", warehouseId);
		sqlCommand.Parameters.AddWithValue("@PartBinID", binId);
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand, transaction));
	}

	public bool HasTheSameDefaultBin(M1Database database, string partId1, string partRevId1, string partId2, string partRevId2, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT imbWarehouseID, imbPartBinID FROM PartBins WITH (NOLOCK) WHERE imbDefaultBin = 1 and imbPartID = @PartID AND imbPartRevisionID = @PartRevID");
		sqlCommand.Parameters.AddWithValue("@PartID", partId1);
		sqlCommand.Parameters.AddWithValue("@PartRevID", partRevId1);
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return false;
		}
		string value = dataTable.Rows[0].Field<string>("imbWarehouseID");
		string value2 = dataTable.Rows[0].Field<string>("imbPartBinID");
		SqlCommand sqlCommand2 = database.NewSqlCommand("SELECT Count(*) FROM PartBins WITH (NOLOCK) WHERE imbDefaultBin = 1 and imbPartID = @PartID AND imbPartRevisionID = @PartRevID AND imbWarehouseID = @WarehouseID AND imbPartBinID = @PartBinID ");
		sqlCommand2.Parameters.AddWithValue("@PartID", partId2);
		sqlCommand2.Parameters.AddWithValue("@PartRevID", partRevId2);
		sqlCommand2.Parameters.AddWithValue("@WarehouseID", value);
		sqlCommand2.Parameters.AddWithValue("@PartBinID", value2);
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand2, transaction));
	}

	public string GetDefaultWarehouseByPlant(M1Database database, SqlTransaction transaction, string plantID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select Top 1 IsNull(imwWarehouseID,'') From Warehouses With (NoLock) Where imwDefaultWarehouse <> 0 And imwInactive = 0 And (imwPlantID = @PlantID Or imwPlantID = '') Order By imwPlantID Desc,imwWarehouseID");
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj == null)
		{
			return string.Empty;
		}
		return obj.ToString();
	}

	public string GetDefaultBinOfGivenWarehouse(M1Database database, string warehouseID)
	{
		return GetDefaultBinOfGivenWarehouse(database, null, warehouseID);
	}

	public string GetDefaultBinOfGivenWarehouse(M1Database database, SqlTransaction transaction, string warehouseID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT inbWarehouseBinID FROM  WarehouseBins WHERE (inbWarehouseID = @WarehouseID) AND (inbDefaultBin = 1) AND (inbInactive = 0)");
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		object obj = ((transaction != null) ? database.ExecuteScalar(sqlCommand, transaction) : database.ExecuteScalar(sqlCommand));
		if (obj == null)
		{
			return string.Empty;
		}
		return obj.ToString();
	}

	public string GetPreferredWarehouse(M1Database database, string partID, string partRevisionID, string plantID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select Top 1 IsNull(imbWarehouseID,'') From (Select 1 As BinType,imbWarehouseID,imbPartBinID,imbInactiveBin,imwDefaultWarehouse,imbQuantityOnHand,imbQuantityToInspect,imwPlantID From PartBins With(NoLock)  Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID  Where imbDefaultBin = 1 And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID Union All Select 2 As BinType,imbWarehouseID,imbPartBinID,imbInactiveBin,imwDefaultWarehouse,imbQuantityOnHand,imbQuantityToInspect,imwPlantID From PartBins With(NoLock)  Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID  Where imbDefaultBin = 0 And imwDefaultWarehouse = 1 And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID And imwPlantID = @PlantID Union All Select 3 As BinType,imbWarehouseID,imbPartBinID,imbInactiveBin,imwDefaultWarehouse,imbQuantityOnHand,imbQuantityToInspect,imwPlantID From PartBins With(NoLock)  Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID  Where imbDefaultBin = 0 And imwDefaultWarehouse = 0 And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID And imwPlantID = @PlantID Union All Select 4 As BinType,imbWarehouseID,imbPartBinID,imbInactiveBin,0 As imwDefaultWarehouse,imbQuantityOnHand,imbQuantityToInspect,'' As imwPlantID From PartBins With(NoLock)  Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID  Where imbDefaultBin = 0 And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID ) As data Order By imbInactiveBin, BinType, imbQuantityOnHand Desc, imbQuantityToInspect Desc, imbWarehouseID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
		object obj = database.ExecuteScalar(sqlCommand);
		if (obj == null)
		{
			return string.Empty;
		}
		return obj.ToString();
	}

	public string GetPreferredWarehouseBin(M1Database database, string partID, string partRevisionID, string warehouseID, string plantID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select Top 1 IsNull(imbPartBinID,'') From (Select 1 As BinType,imbWarehouseID,imbPartBinID,imbInactiveBin,imwDefaultWarehouse,imbQuantityOnHand,imbQuantityToInspect,imwPlantID From PartBins With(NoLock)  Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID  Where imbDefaultBin = 1 And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID And imbWarehouseID = @WarehouseID Union All Select 2 As BinType,imbWarehouseID,imbPartBinID,imbInactiveBin,imwDefaultWarehouse,imbQuantityOnHand,imbQuantityToInspect,imwPlantID From PartBins With(NoLock)  Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID  Where imbDefaultBin = 0 And imwDefaultWarehouse = 1 And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID And imbWarehouseID = @WarehouseID And imwPlantID = @PlantID Union All Select 3 As BinType,imbWarehouseID,imbPartBinID,imbInactiveBin,imwDefaultWarehouse,imbQuantityOnHand,imbQuantityToInspect,imwPlantID From PartBins With(NoLock)  Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID  Where imbDefaultBin = 0 And imwDefaultWarehouse = 0 And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID And imbWarehouseID = @WarehouseID And imwPlantID = @PlantID Union All Select 4 As BinType,imbWarehouseID,imbPartBinID,imbInactiveBin,0 As imwDefaultWarehouse,imbQuantityOnHand,imbQuantityToInspect,'' As imwPlantID From PartBins With(NoLock)  Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID  Where imbDefaultBin = 0 And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID And imbWarehouseID = @WarehouseID ) As data Order By imbInactiveBin, BinType, imbQuantityOnHand Desc, imbQuantityToInspect Desc, imbWarehouseID Asc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
		object obj = database.ExecuteScalar(sqlCommand);
		if (obj == null)
		{
			return string.Empty;
		}
		return obj.ToString();
	}

	public bool SearchProperWarehouseAndBinForAPart(M1Database database, string partID, string partRevisionID, string salesOrderID, ref string returnWarehouseID, ref string returnWarehouseBinID, ref string returnMessage)
	{
		try
		{
			string plantID = SearchPlantOfSalesOrder(database, salesOrderID, ref returnMessage);
			return InitializeWarehouseBinForPartRev(database, partID, partRevisionID, plantID, ref returnWarehouseID, ref returnWarehouseBinID, ref returnMessage);
		}
		catch (Exception ex)
		{
			returnMessage = ex.Message;
			return false;
		}
	}

	public bool InitializeWarehouseBinForPartRev(M1Database database, string partID, string partRevisionID, string plantID, ref string returnWarehouseID, ref string returnWarehouseBinID, ref string returnMessage)
	{
		returnWarehouseID = "";
		returnWarehouseBinID = "";
		returnMessage = "";
		try
		{
			if (string.IsNullOrEmpty(partID))
			{
				returnMessage = "Empty PartID.";
				return false;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("Select count(*) From PartBins Where imbPartID = @PartID and imbPartRevisionID = @PartRevisionID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
			if (Convert.ToInt32(database.ExecuteScalar(sqlCommand)) == 0)
			{
				returnMessage = "PartID " + partID + " PartRevision " + partRevisionID + " doesn't exist in the system.";
				return false;
			}
			SqlCommand sqlCommand2 = database.NewSqlCommand("Select imbWarehouseID, imbPartBinID From PartBins Where imbPartID = @PartID and imbPartRevisionID = @PartRevisionID and imbDefaultBin = 1 ");
			sqlCommand2.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand2.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
			DataTable dataTable = database.GetDataTable(sqlCommand2);
			if (dataTable.Rows.Count == 1)
			{
				returnWarehouseID = dataTable.Rows[0].Field<string>("imbWarehouseID");
				returnWarehouseBinID = dataTable.Rows[0].Field<string>("imbPartBinID");
				returnMessage = "Found default warehouse " + returnWarehouseID + " and warehouseBin " + returnWarehouseBinID + " for PartID " + partID + " PartRevision " + partRevisionID + ".";
				return true;
			}
			if (dataTable.Rows.Count == 0)
			{
				string text = "";
				SqlCommand sqlCommand3 = database.NewSqlCommand("Select count(*) From PartBins Where imbPartID = @PartID and imbPartRevisionID = @PartRevisionID and imbInactiveBin = 0 ");
				sqlCommand3.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
				sqlCommand3.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
				bool flag = Convert.ToInt32(database.ExecuteScalar(sqlCommand3)) >= 1;
				text = SearchTheDefaultWarehouseOfAPlant(database, partID, partRevisionID, plantID, !flag, ref returnMessage);
				if (string.IsNullOrEmpty(text))
				{
					return SearchTheMaxQuantityPartBinsWithPlantReference(database, partID, partRevisionID, plantID, !flag, ref returnWarehouseID, ref returnWarehouseBinID, ref returnMessage);
				}
				return SearchTheMaxQuantityPartBinInOneWarehouse(database, partID, partRevisionID, text, !flag, ref returnWarehouseID, ref returnWarehouseBinID, ref returnMessage);
			}
			returnMessage = "PartID " + partID + " PartRevision " + partRevisionID + " has more than one default bin, please check your data integrity and try again.";
			return false;
		}
		catch (Exception ex)
		{
			returnMessage = ex.Message;
			return false;
		}
	}

	public bool GetWarehouseBinForWarehouseTransferLine(M1BindingSource bindingSource, string partId, string partRevisionId, string warehouseId, ref string returnWarehouseBinId, string parentPartId = "", string parentPartRevisionId = "", SqlTransaction transaction = null)
	{
		M1Database database = bindingSource.Database;
		SqlCommand sqlCommand = new SqlCommand("SELECT w.imwPlantID FROM Warehouses w where w.imwWarehouseID = @WarehouseId");
		sqlCommand.Parameters.Add(new SqlParameter("@WarehouseId", SqlDbType.NVarChar)).Value = warehouseId;
		string plantID = Convert.ToString(database.ExecuteScalar(sqlCommand, transaction));
		DataRow dataRow = null;
		if (!string.IsNullOrEmpty(parentPartId) && IsKitPart(database, parentPartId))
		{
			sqlCommand = new SqlCommand("SELECT immPartWarehouseLocationID, immPartBinID, immUseDefaultWarehouseAndBin FROM PartMaterials WHERE immMethodID = @MethodId AND immMethodRevisionID = @MethodRevisionId AND immMethodAssemblyID = 0 AND immPartID = @PartId AND immPartRevisionID = @PartRevisionId");
			sqlCommand.Parameters.Add(new SqlParameter("@MethodId", SqlDbType.NVarChar)).Value = parentPartId;
			sqlCommand.Parameters.Add(new SqlParameter("@MethodRevisionId", SqlDbType.NVarChar)).Value = parentPartRevisionId;
			sqlCommand.Parameters.Add(new SqlParameter("@PartId", SqlDbType.NVarChar)).Value = partId;
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionId", SqlDbType.NVarChar)).Value = partRevisionId;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			dataRow = ((dataTable.Rows.Count > 0) ? dataTable.Rows[0] : null);
		}
		string text = ((dataRow == null || dataRow.Field<bool>("immUseDefaultWarehouseAndBin") || !(dataRow.Field<string>("immPartWarehouseLocationID") == warehouseId)) ? GetPreferredWarehouseBin(database, partId, partRevisionId, warehouseId, plantID) : dataRow.Field<string>("immPartBinID"));
		if (IsPartBinInactiveAndEmpty(database, partId, partRevisionId, warehouseId, text, transaction) && !IsPartNonStockedOrKit(database, partId, transaction))
		{
			returnWarehouseBinId = string.Empty;
			return false;
		}
		returnWarehouseBinId = text;
		return true;
	}

	private bool SearchTheMaxQuantityPartBinInOneWarehouse(M1Database database, string partID, string partRevisionID, string withinGivenWarehouseID, bool inactiveBinsOnly, ref string returnWarehouseID, ref string returnWarehouseBinID, ref string returnMessage)
	{
		returnWarehouseID = "";
		returnWarehouseBinID = "";
		returnMessage = "";
		try
		{
			string queryString = "Select Top 1 IsNull(imbPartBinID,'') As imbPartBinID From PartBins Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID Where imbInactiveBin = @InactiveBin And imbWarehouseID = @WarehouseID And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID Order By imbQuantityOnHand Desc, imbQuantityToInspect Desc";
			SqlCommand sqlCommand = database.NewSqlCommand(queryString);
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@InactiveBin", SqlDbType.Bit)).Value = inactiveBinsOnly;
			sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = withinGivenWarehouseID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			returnWarehouseID = withinGivenWarehouseID;
			returnWarehouseBinID = dataTable.Rows[0].Field<string>("imbPartBinID");
			returnMessage = "Found proper warehouseBin " + returnWarehouseBinID + " by Max(Quantity) for PartID " + partID + " PartRevision " + partRevisionID + " in warehouse " + returnWarehouseID + ".";
			return true;
		}
		catch (Exception ex)
		{
			returnMessage = ex.Message;
			return false;
		}
	}

	private bool SearchTheMaxQuantityPartBinsWithPlantReference(M1Database database, string partID, string partRevisionID, string plantID, bool inactiveBinsOnly, ref string returnWarehouseID, ref string returnWarehouseBinID, ref string returnMessage)
	{
		returnWarehouseID = "";
		returnWarehouseBinID = "";
		returnMessage = "";
		try
		{
			string queryString = "Select Top 1 IsNull(imbWarehouseID,'') As imbWarehouseID, IsNull(imbPartBinID,'') As imbPartBinID From ( Select 3 As BinType, imwDefaultWarehouse, imwPlantID, imbWarehouseID, imbPartBinID, imbQuantityOnHand, imbQuantityToInspect From PartBins Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID Where imbInactiveBin = @InactiveBin And imwDefaultWarehouse = 0 And imwPlantID = @PlantID And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID Union All Select 4 As BinType, imwDefaultWarehouse, imwPlantID, imbWarehouseID, imbPartBinID, imbQuantityOnHand, imbQuantityToInspect From PartBins Inner Join Warehouses With(NoLock) On imbWarehouseID = imwWarehouseID Where imbInactiveBin = @InactiveBin And imwPlantID <> @PlantID And imbPartID = @PartID And imbPartRevisionID = @PartRevisionID ) As Data Order By BinType, imbQuantityOnHand Desc, imbQuantityToInspect Desc, imbWarehouseID";
			SqlCommand sqlCommand = database.NewSqlCommand(queryString);
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@InactiveBin", SqlDbType.Bit)).Value = inactiveBinsOnly;
			sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			returnWarehouseID = dataTable.Rows[0].Field<string>("imbWarehouseID");
			returnWarehouseBinID = dataTable.Rows[0].Field<string>("imbPartBinID");
			returnMessage = "Found proper warehouse " + returnWarehouseID + " and warehouseBin " + returnWarehouseBinID + " for PartID " + partID + " PartRevision " + partRevisionID + " via PlantID " + plantID + " and Max(Quantity).";
			return true;
		}
		catch (Exception ex)
		{
			returnMessage = ex.Message;
			return false;
		}
	}

	private string SearchTheDefaultWarehouseOfAPlant(M1Database database, string partID, string partRevisionID, string plantID, bool inactiveBinsOnly, ref string returnMessage)
	{
		string result = "";
		returnMessage = "";
		try
		{
			string text = "";
			text = "Select imwWarehouseID From Warehouses inner join PartBins on imwWarehouseID = imbWarehouseID Where imwDefaultWarehouse = 1 And imwPlantID = @PlantID And imbPartID = @PartID and imbPartRevisionID = @PartRevisionID and imbInactiveBin = @InactiveBin Group by imwWarehouseID";
			SqlCommand sqlCommand = database.NewSqlCommand(text);
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@InactiveBin", SqlDbType.Bit)).Value = inactiveBinsOnly;
			sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count >= 1)
			{
				result = dataTable.Rows[0].Field<string>("imwWarehouseID");
			}
		}
		catch (Exception ex)
		{
			returnMessage = ex.Message;
		}
		return result;
	}

	private string SearchPlantOfSalesOrder(M1Database database, string salesOrderID, ref string returnMessage)
	{
		string result = "";
		returnMessage = "";
		try
		{
			string text = "";
			if (!string.IsNullOrEmpty(salesOrderID))
			{
				text = "Select ompPlantID From SalesOrders Where ompSalesOrderID = @SalesOrderID";
				SqlCommand sqlCommand = database.NewSqlCommand(text);
				sqlCommand.Parameters.Add(new SqlParameter("@SalesOrderID", SqlDbType.NVarChar)).Value = salesOrderID;
				DataTable dataTable = database.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count >= 1)
				{
					result = dataTable.Rows[0].Field<string>("ompPlantID");
				}
			}
		}
		catch (Exception ex)
		{
			returnMessage = ex.Message;
		}
		return result;
	}

	public bool IsPartBinInactiveAndEmpty(M1Database database, string partId, string partRevisionId, string warehouseId, string binId, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT count(*) FROM PartBins WHERE imbInactiveBin = 1 and imbQuantityOnHand = 0 and imbQuantityToInspect = 0 and imbPartID = @PartID AND imbPartRevisionID = @PartRevisionID AND imbWarehouseID = @WarehouseID AND imbPartBinID = @PartBinID");
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		sqlCommand.Parameters.AddWithValue("@PartRevisionID", partRevisionId);
		sqlCommand.Parameters.AddWithValue("@WarehouseID", warehouseId);
		sqlCommand.Parameters.AddWithValue("@PartBinID", binId);
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public bool IsPartNonStockedOrKit(M1Database database, string partID, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(impNonStockedItem,0) | IsNull(impPhantomOrKitPart,0) From Parts Where impPartID = @PartID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public bool IsPartBinValid(M1Database database, string partId, string partRevisionId, string warehouseId, string binId, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT count(*) FROM PartBins WHERE imbPartID = @PartID AND imbPartRevisionID = @PartRevisionID AND imbWarehouseID = @WarehouseID AND imbPartBinID = @PartBinID");
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		sqlCommand.Parameters.AddWithValue("@PartRevisionID", partRevisionId);
		sqlCommand.Parameters.AddWithValue("@WarehouseID", warehouseId);
		sqlCommand.Parameters.AddWithValue("@PartBinID", binId);
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public bool IsWarehouseBinValid(M1Database database, string warehouseId, string binId, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT count(*) FROM WarehouseBins WHERE inbWarehouseID = @WarehouseID and inbWarehouseBinID = @WarehouseBinID");
		sqlCommand.Parameters.AddWithValue("@WarehouseID", warehouseId);
		sqlCommand.Parameters.AddWithValue("@WarehouseBinID", binId);
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public bool IsWarehouseBinInactive(M1Database database, string warehouseId, string binId, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT inbInactive\u00a0FROM WarehouseBins WHERE inbWarehouseID = @WarehouseID AND inbWarehouseBinID = @BinID");
		sqlCommand.Parameters.AddWithValue("@WarehouseID", warehouseId);
		sqlCommand.Parameters.AddWithValue("@BinID", binId);
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public bool IsWarehouseBinDefault(M1Database database, string warehouseId, string binId, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT inbDefaultBin\u00a0FROM WarehouseBins WHERE inbWarehouseID = @WarehouseID AND inbWarehouseBinID = @BinID");
		sqlCommand.Parameters.AddWithValue("@WarehouseID", warehouseId);
		sqlCommand.Parameters.AddWithValue("@BinID", binId);
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public int DefaultWarehouseCount(M1Database database, SqlTransaction transaction = null)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT Count(*)\u00a0FROM Warehouses WHERE imwDefaultWarehouse = 1");
		return (int)database.ExecuteScalar(sqlCommand, transaction);
	}

	public bool IsPartBinDataValid(M1Database database, string partId, string partRevisionId, out string validationMsg, SqlTransaction transaction = null)
	{
		validationMsg = "";
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT count(*) FROM PartBins WHERE imbPartID = @PartID And imbPartRevisionID = @PartRevisionID");
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		sqlCommand.Parameters.AddWithValue("@PartRevisionID", partRevisionId);
		if (Convert.ToInt16(database.ExecuteScalar(sqlCommand, transaction)) == 0)
		{
			validationMsg = "Part " + partId + " Revision " + partRevisionId + " doesn't have any PartBin data.";
			return false;
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("SELECT count(*) FROM PartBins WHERE imbPartID = @PartID And imbPartRevisionID = @PartRevisionID And (imbWarehouseID = '' or imbPartBinID = '')");
		sqlCommand2.Parameters.AddWithValue("@PartID", partId);
		sqlCommand2.Parameters.AddWithValue("@PartRevisionID", partRevisionId);
		if (Convert.ToInt16(database.ExecuteScalar(sqlCommand2, transaction)) == 0)
		{
			return true;
		}
		validationMsg = "Part " + partId + " Revision " + partRevisionId + " has invalid PartBin data, either the WarehouseID or the PartBinID is empty.";
		return false;
	}

	public bool QuoteMaterialsHaveValidPartBinData(M1Database database, string quoteId, short quoteLineID, int quoteAssemblyID, out string validationMsg, SqlTransaction transaction = null)
	{
		validationMsg = "";
		string validationMsg2 = "";
		StringBuilder stringBuilder = new StringBuilder();
		SqlCommand sqlCommand = database.NewSqlCommand("select qmaPartID, qmaPartRevisionID from QuoteAssemblies where qmaQuoteID = @QuoteID and qmaQuoteLineID = @QuoteLineID and qmaQuoteAssemblyID = @QuoteAssemblyID ");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteId;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.SmallInt)).Value = quoteLineID;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteAssemblyID", SqlDbType.Int)).Value = quoteAssemblyID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row3 in dataTable.Rows)
			{
				if (!IsPartBinDataValid(database, row3.Field<string>("qmaPartID").Trim(), row3.Field<string>("qmaPartRevisionID").Trim(), out validationMsg2, transaction))
				{
					stringBuilder.Append(validationMsg2 + Environment.NewLine);
				}
			}
		}
		sqlCommand = database.NewSqlCommand("SELECT qmmPartID, qmmPartRevisionID FROM QuoteMaterials where qmmQuoteID = @QuoteID  and qmmQuoteLineID = @QuoteLineID and qmmQuoteAssemblyID = @QuoteAssemblyID order by qmmQuoteMaterialID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteId;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.SmallInt)).Value = quoteLineID;
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteAssemblyID", SqlDbType.Int)).Value = quoteAssemblyID;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row4 in dataTable.Rows)
			{
				if (!IsPartBinDataValid(database, row4.Field<string>("qmmPartID").Trim(), row4.Field<string>("qmmPartRevisionID").Trim(), out validationMsg2, transaction))
				{
					stringBuilder.Append(validationMsg2 + Environment.NewLine);
				}
			}
		}
		if (stringBuilder.Length > 0)
		{
			validationMsg = stringBuilder.ToString();
			return false;
		}
		return true;
	}

	public bool JobMaterialsHaveValidPartBinData(M1Database database, string jobId, int jobAssemblyID, out string validationMsg, SqlTransaction transaction = null)
	{
		validationMsg = "";
		string validationMsg2 = "";
		StringBuilder stringBuilder = new StringBuilder();
		SqlCommand sqlCommand = database.NewSqlCommand("select jmaPartID, jmaPartRevisionID from JobAssemblies where jmaJobID = @JobID and jmaJobAssemblyID = @JobAssemblyID ");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobId;
		sqlCommand.Parameters.Add(new SqlParameter("@JobAssemblyID", SqlDbType.Int)).Value = jobAssemblyID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row3 in dataTable.Rows)
			{
				if (!IsPartBinDataValid(database, row3.Field<string>("jmaPartID").Trim(), row3.Field<string>("jmaPartRevisionID").Trim(), out validationMsg2, transaction))
				{
					stringBuilder.Append(validationMsg2 + Environment.NewLine);
				}
			}
		}
		sqlCommand = database.NewSqlCommand("SELECT jmmPartID, jmmPartRevisionID FROM JobMaterials where jmmJobID = @JobID and jmmJobAssemblyID = @JobAssemblyID order by jmmJobMaterialID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobId;
		sqlCommand.Parameters.Add(new SqlParameter("@JobAssemblyID", SqlDbType.Int)).Value = jobAssemblyID;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row4 in dataTable.Rows)
			{
				if (!IsPartBinDataValid(database, row4.Field<string>("jmmPartID").Trim(), row4.Field<string>("jmmPartRevisionID").Trim(), out validationMsg2, transaction))
				{
					stringBuilder.Append(validationMsg2 + Environment.NewLine);
				}
			}
		}
		if (stringBuilder.Length > 0)
		{
			validationMsg = stringBuilder.ToString();
			return false;
		}
		return true;
	}

	public bool CheckPendingTransactions(M1Database database, string partID, string partRevisionID, string currentTransactionID = "", SqlTransaction transaction = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = (bool)database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
		if (!string.IsNullOrEmpty(partID) && partRevisionID != null && !flag)
		{
			string queryString = "select imqPartID as PartID, imqPartRevisionID as PartRevisionID, imqPartWarehouseLocationID as WarehouseID, imqPartBinID as BinID, imqQuantityOnHand as Quantity, 'InventoryCounts' as Source, CAST(imnInventoryCountID AS nvarchar(15)) as SourceID from InventoryCountLines inner join InventoryCounts on imqInventoryCountID = imnInventoryCountID where imnPostedToInventory = 0 and imqPartID = @PartID and imqPartRevisionID = @PartRevisionID union select qalPartID, qalPartRevisionID, qalPartWarehouseLocationID, qalPartBinID, qalQuantityToInspect, 'Inspections' as Source, qapInspectionID as SourceID  from InspectionLines inner join Inspections on qalInspectionID = qapInspectionID where qapPosted = 0 and qalPartID = @PartID and qalPartRevisionID = @PartRevisionID union select injPartID, injPartRevisionID, injPartWarehouseLocationID, injPartBinID, injQuantityOnHand, 'MaterialIssues' as Source, iniMaterialIssueID as SourceID from MaterialIssueLines inner join MaterialIssues on injMaterialIssueID = iniMaterialIssueID where iniPosted = 0 and injPartID = @PartID and injPartRevisionID = @PartRevisionID union select smlPartID, smlPartRevisionID, smlPartWarehouseLocationID, smlPartBinID, smlQuantityShipped, 'Shipments' as Source, smlShipmentID as SourceID  from ShipmentLines inner join Shipments on smpShipmentID = smlShipmentID where smpPostedToGL = 0 and smlPartID = @PartID and smlPartRevisionID = @PartRevisionID union select rmlPartID, rmlPartRevisionID, rmlPartWarehouseLocationID, rmlPartBinID, rmlInventoryQuantityReceived, 'Receipts' as Source, rmpReceiptID as SourceID  from ReceiptLines inner join Receipts on rmlReceiptID = rmpReceiptID where rmpPostedToGL = 0 and rmlPartID = @PartID and rmlPartRevisionID = @PartRevisionID union select mwlPartID, mwlPartRevisionID, mwlSourceWarehouseID, mwlSourcePartBinID, mwlQuantityInTransit, 'WarehouseTransfer' as Source, mwpWarehouseTransferID as SourceID  from WarehouseTransferLines inner join WarehouseTransfers on mwlWarehouseTransferID = mwpWarehouseTransferID where mwpPosted = 0 and mwlPartID = @PartID and mwlPartRevisionID = @PartRevisionID union select wrlPartID, wrlPartRevisionID, wrlSourceWarehouseID, wrlSourcePartBinID, wrlQuantityReceived, 'WarehouseReceipt' as Source, wrpWarehouseReceiptID as SourceID from WarehouseReceiptLines inner join WarehouseReceipts on wrlWarehouseReceiptID = wrpWarehouseReceiptID where wrpPosted = 0 and wrlPartID = @PartID and wrlPartRevisionID = @PartRevisionID union select inqPartID, inqPartRevisionID, inqPartWarehouseLocationID, inqPartBinID, inqChangeQuantity, 'QuantityAdjustments' as Source, inqQuantityAdjustmentID as SourceID  from QuantityAdjustments where inqPosted = 0 and inqPartID = @PartID and inqQuantityAdjustmentID <> @CurrentTransactionID and inqPartRevisionID = @PartRevisionID union select rmmPartID, rmmPartRevisionID, rmmPartWarehouseLocationID, rmmPartBinID, rmmEstimatedQuantity, 'MfgReceipts' as Source, rmmMfgReceiptID as SourceID  from  MfgReceipts where rmmPosted = 0 and rmmPartID = @PartID and rmmPartRevisionID = @PartRevisionID union select rrlPartID, rrlPartRevisionID, rrlPartWarehouseLocationID, rrlPartBinID, rrlInventoryQuantityReceived, 'RMA Receipts' as Source, rrpRMAReceiptID as SourceID  from RMAReceiptLines inner join RMAReceipts on rrlRMAReceiptID = rrpRMAReceiptID where rrpPosted = 0 and rrlPartID = @PartID and rrlPartRevisionID = @PartRevisionID union select dslPartID, dslPartRevisionID, dslPartWarehouseLocationID, dslPartBinID, dslQuantityShipped, 'DMRShipments' as Source, dspDMRShipmentID as SourceID  from DMRShipmentLines  inner join DMRShipments on dslDMRShipmentID = dspDMRShipmentID where dspPosted = 0 and dslPartID = @PartID and dslPartRevisionID = @PartRevisionID union select qamPartID, qamPartRevisionID, qamPartWarehouseLocationID, qamPartBinID, qamQuantityPerParent, 'InspectionComponents' as Source, qapInspectionID as SourceID from InspectionComponents inner join Inspections on qamInspectionID = qapInspectionID where qamPosted = 0 and qamPartID = @PartID and qamPartRevisionID = @PartRevisionID union select inkPartID, inkPartRevisionID, inkPartWarehouseLocationID, inkPartBinID, inkQuantityPerParent, 'MaterialIssueComponents' as Source, iniMaterialIssueID as SourceID from MaterialIssueComponents inner join MaterialIssues on inkMaterialIssueID = iniMaterialIssueID where iniPosted = 0 and inkPartID = @PartID and inkPartRevisionID = @PartRevisionID union select smoPartID, smoPartRevisionID, smoPartWarehouseLocationID, smoPartBinID, smoQuantityShipped, 'ShipmentComponents' as Source, smoShipmentID as SourceID from ShipmentComponents inner join Shipments on smpShipmentID = smoShipmentID where smpPostedToGL = 0 and smoPartID = @PartID and smoPartRevisionID = @PartRevisionID union select rmoPartID, rmoPartRevisionID, rmoPartWarehouseLocationID, rmoPartBinID, rmoQuantityToInspect, 'ReceiptComponents' as Source, rmpReceiptID as SourceID from ReceiptComponents inner join Receipts on rmoReceiptID = rmpReceiptID where rmpPostedToGL = 0 and rmoPartID = @PartID and rmoPartRevisionID = @PartRevisionID union select mwoPartID, mwoPartRevisionID, mwoSourceWarehouseID, mwoSourcePartBinID, mwoQuantityInTransit, 'WarehouseTransferComponents' as Source, mwoWarehouseTransferID as SourceID from WarehouseTransferComponents inner join WarehouseTransfers on mwoWarehouseTransferID = mwpWarehouseTransferID where mwpPosted = 0 and mwoPartID = @PartID and mwoPartRevisionID = @PartRevisionID union select wroPartID, wroPartRevisionID, wroSourceWarehouseID, wroSourcePartBinID, wroQuantityReceived, 'WarehouseReceiptComponents' as Source, wrpWarehouseReceiptID as SourceID from WarehouseReceiptComponents inner join WarehouseReceipts on wroWarehouseReceiptID = wrpWarehouseReceiptID where wrpPosted = 0 and wroPartID = @PartID and wroPartRevisionID = @PartRevisionID union select rmnPartID, rmnPartRevisionID, rmnPartWarehouseLocationID, rmnPartBinID, rmnQuantityPerParent, 'MfgReceiptComponents' as Source, rmmMfgReceiptID as SourceID from MfgReceiptComponents inner join MfgReceipts on rmnMfgReceiptID = rmmMfgReceiptID  where rmmPosted = 0 and rmnPartID = @PartID and rmnPartRevisionID = @PartRevisionID union select dsoPartID, dsoPartRevisionID, dsoPartWarehouseLocationID, dsoPartBinID, dsoQuantityPerParent, 'DMRShipmentComponents' as Source, dspDMRShipmentID as SourceID from DMRShipmentComponents inner join DMRShipments on dsoDMRShipmentID = dspDMRShipmentID  where dspPosted = 0 and dsoPartID = @PartID and dsoPartRevisionID = @PartRevisionID union select rroPartID, rroPartRevisionID, rroPartWarehouseLocationID, rroPartBinID, rroQuantityPerParent, 'RMAReceiptComponents' as Source, rrpRMAReceiptID as SourceID  from RMAReceiptComponents inner join RMAReceipts on rroRMAReceiptID = rrpRMAReceiptID where rrpPosted = 0 and rroPartID = @PartID and rroPartRevisionID = @PartRevisionID ";
			SqlCommand sqlCommand = database.NewSqlCommand(queryString);
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = partRevisionID;
			sqlCommand.Parameters.Add(new SqlParameter("@CurrentTransactionID", SqlDbType.NVarChar)).Value = currentTransactionID;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count != 0)
			{
				stringBuilder.AppendLine("Part: " + partID + " Revision: " + partRevisionID + " is being used in the following transactions:" + Environment.NewLine);
				foreach (DataRow row in dataTable.Rows)
				{
					stringBuilder.AppendLine(row.Field<string>("Source") + " ID: " + row.Field<string>("SourceID") + " WH: " + row.Field<string>("WarehouseID") + " Bin: " + row.Field<string>("BinID") + Environment.NewLine);
				}
				using LongMsgDialog longMsgDialog = new LongMsgDialog(database);
				longMsgDialog.DefaultSaveFileName = DateTime.Now.ToString("yyyyddMMM_HH.mm.ss") + "_" + partID + "_" + partRevisionID + "_pendingTransactions";
				longMsgDialog.HeaderText = "Part " + partID + " has pending transactions. These transactions need to be completed before you can complete the current transaction.";
				longMsgDialog.MessageText = stringBuilder.ToString();
				if (longMsgDialog.ShowDialog() == DialogResult.OK)
				{
					return false;
				}
			}
		}
		return stringBuilder.Length <= 0;
	}
}
