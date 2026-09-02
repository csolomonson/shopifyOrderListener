using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1.ServiceCore.AxScript;

namespace M1.Ax.Erp;

[AxScript("Part")]
[ComVisible(true)]
public class AppAxPart : IWebAxPart
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxPart(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public bool GetPartClassReadOnly(string fieldName)
	{
		return new Part().GetPartClassReadOnly(_Database, fieldName);
	}

	public bool CanSetPreferredSupplier(object purchased, string partID, string partRevisionID, string orgID, string locID)
	{
		if (purchased == null || purchased == DBNull.Value)
		{
			return false;
		}
		return new Part().CanSetPreferredSupplier(_Database, Convert.ToBoolean(purchased), partID, partRevisionID, orgID, locID);
	}

	public void SetPreferredSupplier(string partID, string partRevisionID, string orgID, string locID, string purUoM, decimal conversionFactor)
	{
		new Part().SetPreferredSupplier(_Database, partID, partRevisionID, orgID, locID, purUoM, conversionFactor);
	}

	public void ChangeAllocations(object oTransaction, string cOldPartID, string cOldRevisionID, string cOldWarehouseID, string cOldBin, double nOldQty, string cNewPartID, string cNewRevisionID, string cNewWarehouseID, string cNewBin, double nNewQty)
	{
		if (oTransaction == DBNull.Value)
		{
			oTransaction = null;
		}
		new Part().ChangeAllocations(_Database, (SqlTransaction)oTransaction, cOldPartID, cOldRevisionID, cOldWarehouseID, cOldBin, nOldQty, cNewPartID, cNewRevisionID, cNewWarehouseID, cNewBin, nNewQty);
	}

	public PartGroupMarkup GetPartGroupMarkups(string partGroupID)
	{
		return new Part().GetPartGroupMarkups(_Database, partGroupID);
	}

	public void DeletePartAssembly(object transaction, string partID, string revisionID, int asmID)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		new Part().DeletePartAssembly(_Database, (SqlTransaction)transaction, partID, revisionID, asmID);
	}

	public int GetPartAlternateCount(string partID, string partRevisionID)
	{
		return new Part().GetPartAlternateCount(_Database, partID, partRevisionID);
	}

	public bool RefreshPartAllocations(object transaction = null, string partID = "", string revisionID = "")
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().RefreshPartAllocations(_Database, (SqlTransaction)transaction, partID, revisionID);
	}

	public bool RefreshOnOrderQuantitesPurchases(object transaction = null, string partID = "", string revisionID = "")
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().RefreshOnOrderQuantitesPurchases(_Database, (SqlTransaction)transaction, partID, revisionID);
	}

	public bool RefreshOnOrderQuantitesSales(object transaction = null, string partID = "", string revisionID = "")
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().RefreshOnOrderQuantitesSales(_Database, (SqlTransaction)transaction, partID, revisionID);
	}

	public PriceData GetPrice(string partID, string partRevisionID, string partGroupID, string orgID, string locationID, string currencyID, byte priceType, DateTime? priceDate, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().GetPrice(_Database, partID, partRevisionID, partGroupID, orgID, locationID, currencyID, priceType, checkDate(priceDate), (SqlTransaction)transaction);
	}

	public PriceCalculation GetPurchasePrice(string partID, string partRevisionID, string orgID, string locationID, decimal quantity, string costType, string currencyID, object priceDate, decimal purchaseQuantity, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().GetPurchasePrice(_Database, partID, partRevisionID, orgID, locationID, quantity, costType, currencyID, checkDate(priceDate), purchaseQuantity, (SqlTransaction)transaction);
	}

	public PriceCalculation GetSellingPrice(string partID, string partRevisionID, string partGroupID, string orgID, string locationID, object quantity, string currencyID, object priceDate)
	{
		return new Part().GetSellingPrice(_Database, partID, partRevisionID, partGroupID, orgID, locationID, Convert.ToDecimal(quantity), currencyID, checkDate(priceDate));
	}

	public IWebPriceCalculation GetWebSellingPrice(string partID, string partRevisionID, string partGroupID, string orgID, string locationID, object quantity, string currencyID, object priceDate)
	{
		return new Part().GetSellingPrice(_Database, partID, partRevisionID, partGroupID, orgID, locationID, Convert.ToDecimal(quantity), currencyID, checkDate(priceDate));
	}

	private DateTime? checkDate(object date)
	{
		return (!M1Util.IsNullOrEmpty(date)) ? new DateTime?(Convert.ToDateTime(date)) : ((DateTime?)null);
	}

	public bool IsSerialOrLotTracked(string partID, object transaction)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().IsSerialOrLotTracked(_Database, partID, (SqlTransaction)transaction);
	}

	public void PartRevisionForeignKeyCheck(object revisionFieldDefinition, object validationInfo, object dateToCheck, object revisionEffectiveStartDate, object revisionEffectiveEndDate, object revisionInactive)
	{
		DateTime obj = (DateTime)dateToCheck;
		FieldDefinition fieldDefinition = (FieldDefinition)revisionFieldDefinition;
		ValidationInfo validationInfo2 = ((ValidEventArgs)validationInfo).ValidationInfo;
		DateTime dateTime = ((revisionEffectiveStartDate != null && revisionEffectiveStartDate != DBNull.Value) ? ((DateTime)revisionEffectiveStartDate) : DateTime.MinValue);
		DateTime dateTime2 = ((revisionEffectiveEndDate != null && revisionEffectiveEndDate != DBNull.Value) ? ((DateTime)revisionEffectiveEndDate) : DateTime.MaxValue);
		if (obj < dateTime)
		{
			validationInfo2.AddError(fieldDefinition.RelatedFieldsFormatCaptionAndCurrentValues(validationInfo2.Row) + " does not become effective until " + dateTime.ToString("d"));
		}
		if (obj > dateTime2)
		{
			validationInfo2.AddError(fieldDefinition.RelatedFieldsFormatCaptionAndCurrentValues(validationInfo2.Row) + " expired " + dateTime2.ToString("d"));
		}
		if (Convert.ToBoolean(revisionInactive))
		{
			validationInfo2.AddError(fieldDefinition.RelatedFieldsFormatCaptionAndCurrentValues(validationInfo2.Row) + " is inactive");
		}
	}

	public PartCost GetPartCosts(object transaction, string partID, string partRevisionID)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().GetPartCosts(_Database, (SqlTransaction)transaction, partID, partRevisionID);
	}

	public void NewPartRevCheck(string partID, string revisionID, string plantID, SqlTransaction transaction)
	{
		new Part().NewPartRevCheck(_Database, partID, revisionID, plantID, transaction);
	}

	public decimal CalculateQtyAvailableFromPart(M1Database database, string partID, string revisionID)
	{
		return new Part().CalculateQtyAvailable(_Database, partID, revisionID);
	}

	public decimal CalculateQtyAvailableFromBindingSource(M1BindingSource bindingSource)
	{
		return new Part().CalculateQtyAvailable(bindingSource);
	}

	public void CreatePartAndRevision(string partID, string partRevisionID, string description, string longDescriptionRTF, string longDescriptionText, byte partType, string orgID, string locID, string invUoM, string purUoM, decimal conversionFactor, decimal leadTime, string plantID, object transaction)
	{
		new Part().CreatePartAndRevision(_Database, partID, partRevisionID, description, longDescriptionRTF, longDescriptionText, partType, orgID, locID, invUoM, purUoM, conversionFactor, leadTime, plantID, (SqlTransaction)transaction);
	}

	public void CreatePartCrossRef(string partID, string partRevisionID, string orgPartID, string orgID, string locID, string orgDescription, string purUoM, decimal conversionFactor, object transaction)
	{
		new Part().CreatePartCrossRef(_Database, partID, partRevisionID, orgPartID, orgID, locID, orgDescription, purUoM, conversionFactor, (SqlTransaction)transaction);
	}

	public bool IsKitPart(string partID)
	{
		return new Part().IsKitPart(_Database, partID);
	}

	public bool IsBinActive(string warehouseID, string binID)
	{
		return new Part().IsBinActive(_Database, warehouseID, binID);
	}

	public bool GetLatestPartRevision(string partID, ref string partRevisionID, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().GetLatestPartRevision(_Database, (SqlTransaction)transaction, partID, ref partRevisionID);
	}

	public decimal GetConversionFactor(string partID, string partRevisionID, string supplierID, string locationID)
	{
		return new Part().GetConversionFactor(_Database, partID, partRevisionID, supplierID, locationID);
	}

	public void AddCostsUpdatesTransaction(M1BindingSource bindingSource, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		new Part().AddCostsUpdatesTransaction(bindingSource, (SqlTransaction)transaction);
	}

	public void CreatePartClassJournals(M1BindingSource bindingSource, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		new Part().CreatePartClassJournals(bindingSource, (SqlTransaction)transaction);
	}

	public void CreatePartCostsJournals(M1BindingSource bindingSource, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		new Part().CreatePartCostsJournals(bindingSource, (SqlTransaction)transaction);
	}

	public void StandardCostRollupUpdate(string partID, string partRevisionID, decimal laborCost, decimal overheadCost, decimal materialCost, decimal subcontractCost, bool updateUnitSalePrice, decimal unitSalePrice, string currencyID)
	{
		new Part().StandardCostRollupUpdate(_Database, partID, partRevisionID, laborCost, overheadCost, materialCost, subcontractCost, updateUnitSalePrice, unitSalePrice, currencyID);
	}

	public decimal GetDiscountedPrice(string partID, string partRevisionID, string partGroupID, string orgID, string locationID, object quantity, string currencyID, object priceDate)
	{
		return new Part().GetSellingPrice(_Database, partID, partRevisionID, partGroupID, orgID, locationID, Convert.ToDecimal(quantity), currencyID, checkDate(priceDate)).DiscountedPrice;
	}

	public bool GetFutureAdjustmentTransactionStatus(object transaction, string partID, string revisionID, string warehouseID, string binID, object tranDate)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().GetFutureAdjustmentTransactionStatus(_Database, (SqlTransaction)transaction, partID, revisionID, warehouseID, binID, checkDate(tranDate));
	}

	public bool PartRevisionQuantitiesConcurrencyCheck(M1BindingSource bindingSource)
	{
		return new Part().PartRevisionQuantitiesConcurrencyCheck(bindingSource);
	}

	public void CopyRevision(string partID, string sourcePartRevisionID, string destPartRevisionID, object startDate, bool copyPrices, bool copyMemos, bool copyRules, bool copyAlternates, bool copyOrgReferences)
	{
		new Part().CopyRevision(_Database, partID, sourcePartRevisionID, destPartRevisionID, checkDate(startDate), copyPrices, copyMemos, copyRules, copyAlternates, copyOrgReferences);
	}

	public string GetDefaultWarehouse(string plantID, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().GetDefaultWarehouseByPlant(_Database, (SqlTransaction)transaction, plantID);
	}

	public string GetWebDefaultWarehouse(string partID, string partRevisionID, string plantID)
	{
		return new Part().GetPreferredWarehouse(_Database, partID, partRevisionID, plantID);
	}

	public string GetDefaultWarehouseBin(string warehouseID, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Part().GetDefaultBinOfGivenWarehouse(_Database, (SqlTransaction)transaction, warehouseID);
	}

	public string GetDefaultBin(string partID, string partRevisionID, string warehouseID, string plantID)
	{
		return new Part().GetPreferredWarehouseBin(_Database, partID, partRevisionID, warehouseID, plantID);
	}

	public bool InitializeWarehouseBinForOutboundPart(M1BindingSource bindingSource, string partID, string partRevisionID, string plantID, ref string returnWarehouseID, ref string returnWarehouseBinID, ref string returnMessage)
	{
		return new Part().InitializeWarehouseBinForPartRev(bindingSource.Database, partID, partRevisionID, plantID, ref returnWarehouseID, ref returnWarehouseBinID, ref returnMessage);
	}

	public bool IsPartBinInactiveAndEmpty(string partID, string partRevisionID, string warehouseID, string binID, object transaction = null)
	{
		return new Part().IsPartBinInactiveAndEmpty(_Database, partID, partRevisionID, warehouseID, binID, (SqlTransaction)transaction);
	}

	public bool IsPartNonStockedOrKit(string partID, object transaction = null)
	{
		return new Part().IsPartNonStockedOrKit(_Database, partID, (SqlTransaction)transaction);
	}

	public bool GetWarehouseBinForWarehouseTransferLine(M1BindingSource bindingSource, string partId, string partRevisionId, string warehouseId, ref string returnWarehouseBinId, string parentPartId = "", string parentPartRevisionId = "", object transaction = null)
	{
		return new Part().GetWarehouseBinForWarehouseTransferLine(bindingSource, partId, partRevisionId, warehouseId, ref returnWarehouseBinId, parentPartId, parentPartRevisionId, (SqlTransaction)transaction);
	}

	public bool IsPartBinValid(string partId, string partRevisionId, string warehouseId, string partBinId, object transaction = null)
	{
		return new Part().IsPartBinValid(_Database, partId, partRevisionId, warehouseId, partBinId, (SqlTransaction)transaction);
	}

	public bool IsPartBinInactive(string partId, string partRevisionId, string warehouseId, string partBinId, object transaction = null)
	{
		return new Part().IsPartBinInactive(_Database, partId, partRevisionId, warehouseId, partBinId, (SqlTransaction)transaction);
	}

	public bool IsWarehouseBinValid(string warehouseId, string warehouseBinId, object transaction = null)
	{
		return new Part().IsWarehouseBinValid(_Database, warehouseId, warehouseBinId, (SqlTransaction)transaction);
	}

	public bool IsWarehouseBinInactive(string warehouseId, string warehouseBinId, object transaction = null)
	{
		return new Part().IsWarehouseBinInactive(_Database, warehouseId, warehouseBinId, (SqlTransaction)transaction);
	}

	public int GetDefaultWarehouseCount(M1BindingSource bindingSource)
	{
		return new Part().DefaultWarehouseCount(bindingSource.Database, bindingSource.Transaction);
	}
}
