using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class QuoteQuantity
{
	public class QuoteAsmData
	{
		public M1Database Database;

		public string QuoteID;

		public short QuoteLineID;

		public DataTable QuoteAssemblies;

		public DataTable QuoteMaterials;

		public DataTable QuoteOperationsInside;

		public DataTable QuoteOperationsOutside;

		public QuoteAsmData(M1Database database, string quoteID, short quoteLineID, SqlTransaction transaction)
		{
			Database = database;
			QuoteID = quoteID;
			QuoteLineID = quoteLineID;
			SqlCommand sqlCommand = database.NewSqlCommand("select qmaQuoteAssemblyID,qmaParentAssemblyID,qmaLevel,qmaQuantityPerParent,qmaPartID,qmaPartRevisionID,qmaPullAllFromStock" + database.Props("IM").Field<byte>("xapIMCostingMethod") switch
			{
				1 => ",imrAverageLaborCost+imrAverageOverheadCost+imrAverageMaterialCost+imrAverageSubcontractCost+imrAverageDutyCost+imrAverageFreightCost+imrAverageMiscCost As PartCost", 
				2 => ",imrLastLaborCost+imrLastOverheadCost+imrLastMaterialCost+imrLastSubcontractCost+imrLastDutyCost+imrLastFreightCost+imrLastMiscCost As PartCost", 
				_ => ",imrStandardLaborCost+imrStandardOverheadCost+imrStandardMaterialCost+imrStandardSubcontractCost+imrStandardDutyCost+imrStandardFreightCost+imrStandardMiscCost As PartCost", 
			} + " from QuoteAssemblies Left Outer Join PartRevisions On qmaPartID = imrPartID And qmaPartRevisionID = imrPartRevisionID where qmaQuoteID = @QuoteID and qmaQuoteLineID = @QuoteLineID");
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.SmallInt)).Value = quoteLineID;
			QuoteAssemblies = database.GetDataTable(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("select qmoQuoteAssemblyID,qmoQuantityPerAssembly,qmoAdditionalSetupHours,qmoAdditionalSetupQuantity,qmoProductionStandard,qmoStandardFactor,qmoWorkCenterID,qmoQuotingRate,qmoSetupRate,qmoOverheadRate,qmoProductionRate,qmoSetupHours,qmoOperationType from QuoteOperations where qmoQuoteID = @QuoteID and qmoQuoteLineID = @QuoteLineID and qmoOperationType = 1");
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.SmallInt)).Value = quoteLineID;
			QuoteOperationsInside = database.GetDataTable(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("select qmoQuoteAssemblyID,qmoQuantityPerAssembly,qmoEstimatedUnitCost,qmoMinimumCharge,qmoSetupCharge,qmoQuantityBreak1,qmoUnitCost1,qmoQuantityBreak2,qmoUnitCost2,qmoQuantityBreak3,qmoUnitCost3,qmoQuantityBreak4,qmoUnitCost4,qmoQuantityBreak5,qmoUnitCost5,qmoQuantityBreak6,qmoUnitCost6,qmoQuantityBreak7,qmoUnitCost7,qmoQuantityBreak8,qmoUnitCost8,qmoQuantityBreak9,qmoUnitCost9,qmoOperationType from QuoteOperations where qmoQuoteID = @QuoteID and qmoQuoteLineID = @QuoteLineID and qmoOperationType = 2");
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.SmallInt)).Value = quoteLineID;
			QuoteOperationsOutside = database.GetDataTable(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("select qmmQuoteAssemblyID,qmmQuantityPerAssembly,qmmScrapPercent,qmmScrapQuantity,qmmEstimatedUnitCost,qmmMinimumCharge,qmmQuantityBreak1,qmmUnitCost1,qmmQuantityBreak2,qmmUnitCost2,qmmQuantityBreak3,qmmUnitCost3,qmmQuantityBreak4,qmmUnitCost4,qmmQuantityBreak5,qmmUnitCost5,qmmQuantityBreak6,qmmUnitCost6,qmmQuantityBreak7,qmmUnitCost7,qmmQuantityBreak8,qmmUnitCost8,qmmQuantityBreak9,qmmUnitCost9 from QuoteMaterials where qmmQuoteID = @QuoteID and qmmQuoteLineID = @QuoteLineID");
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = quoteID;
			sqlCommand.Parameters.Add(new SqlParameter("@QuoteLineID", SqlDbType.SmallInt)).Value = quoteLineID;
			QuoteMaterials = database.GetDataTable(sqlCommand, transaction);
		}
	}

	public DataTable GenerateCostsPerAssemblyTable(M1Database database, DataRow quoteQuantityRow, QuoteAsmData quoteParm, bool costRollUp = false)
	{
		DataTable dataTable = database.GetDataTable("Select qmaQuoteID,qmaQuoteLineID,qmaQuoteAssemblyID,qmaParentAssemblyID,qmaLevel,qmaQuantityPerParent,qmaPartID,qmaPartRevisionID,qmqMaterialCost,qmqSubcontractCost,qmqLaborCost,qmqOverheadCost,qmqQuotingCost,qmqPurchaseToOrderCost,qmqSetupHours,qmqProductionHours,qmqProductionHours As TotalHours From QuoteAssemblies,QuoteQuantities Where 0=1");
		List<QuoteAssemblyTotals> list = CalculateUsingCurrentQty(database, quoteQuantityRow, quoteParm, costRollUp);
		if (list != null)
		{
			foreach (QuoteAssemblyTotals item in list)
			{
				DataRow dataRow = dataTable.AddBlankRow();
				dataRow["qmaQuoteID"] = quoteQuantityRow["qmqQuoteID"];
				dataRow["qmaQuoteLineID"] = quoteQuantityRow["qmqQuoteLineID"];
				dataRow["qmaQuoteAssemblyID"] = item.AssemblyID;
				dataRow["qmaParentAssemblyID"] = item.ParentAssemblyID;
				dataRow["qmaLevel"] = item.Level;
				dataRow["qmaQuantityPerParent"] = item.QuantityPerParent;
				dataRow["qmaPartID"] = item.PartID;
				dataRow["qmaPartRevisionID"] = item.PartRevisionID;
				dataRow["qmqMaterialCost"] = item.MaterialCost;
				dataRow["qmqSubcontractCost"] = item.SubcontractCost;
				dataRow["qmqLaborCost"] = item.LaborCost;
				dataRow["qmqOverheadCost"] = item.OverheadCost;
				dataRow["qmqQuotingCost"] = item.QuotingCost;
				dataRow["qmqSetupHours"] = item.SetupHours;
				dataRow["qmqProductionHours"] = item.ProductionHours;
				dataRow["TotalHours"] = M1Math.Round(item.SetupHours + item.ProductionHours, 2);
			}
		}
		return dataTable;
	}

	public List<QuoteAssemblyTotals> CalculateUsingCurrentQty(M1Database database, DataRow quoteQuantityRow, SqlTransaction transaction, bool costRollUp = false)
	{
		string quoteID = quoteQuantityRow.Field<string>("qmqQuoteID");
		short quoteLineID = quoteQuantityRow.Field<short>("qmqQuoteLineID");
		QuoteAsmData quoteParm = new QuoteAsmData(database, quoteID, quoteLineID, transaction);
		return CalculateUsingCurrentQty(database, quoteQuantityRow, quoteParm);
	}

	public List<QuoteAssemblyTotals> CalculateUsingCurrentQty(M1Database database, DataRow quoteQuantityRow, QuoteAsmData quoteParm, bool costRollUp = false)
	{
		QuoteAssemblyTotals quoteAssemblyTotals = new QuoteAssemblyTotals();
		List<QuoteAssemblyTotals> list = new List<QuoteAssemblyTotals>();
		if (quoteQuantityRow.Field<decimal>("qmqQuoteQuantity") != 0m)
		{
			Job jobRef = new Job();
			if (quoteQuantityRow.Field<bool>("qmqPurchaseToOrder"))
			{
				CalculateInfoForAsm(database, jobRef, 0, quoteQuantityRow.Field<decimal>("qmqTotalRunQuantity"), quoteQuantityRow.Field<decimal>("qmqQuoteQuantity"), quoteAssemblyTotals, list, null, quoteParm, costRollUp);
			}
			else
			{
				DataRow[] array = quoteParm.QuoteAssemblies.Select("qmaQuoteAssemblyID = 0");
				if (array.Length != 0)
				{
					CalculateInfoForAsm(database, jobRef, 0, quoteQuantityRow.Field<decimal>("qmqTotalRunQuantity") * array[0].Field<decimal>("qmaQuantityPerParent"), quoteQuantityRow.Field<decimal>("qmqQuoteQuantity"), quoteAssemblyTotals, list, array[0], quoteParm, costRollUp);
				}
			}
			jobRef = null;
		}
		else
		{
			quoteQuantityRow["qmqCalculatedUnitPrice"] = 0;
			quoteQuantityRow["qmqDiscountPercent"] = 0;
			quoteQuantityRow["qmqUnitDiscountBase"] = 0;
			quoteQuantityRow["qmqUnitDiscountForeign"] = 0;
			quoteQuantityRow["qmqFullRevisedUnitPriceBase"] = 0;
			quoteQuantityRow["qmqFullRevisedUnitPriceForeign"] = 0;
			quoteQuantityRow["qmqRevisedUnitPriceBase"] = 0;
			quoteQuantityRow["qmqRevisedUnitPriceForeign"] = 0;
		}
		quoteQuantityRow["qmqLaborCost"] = quoteAssemblyTotals.LaborCost;
		quoteQuantityRow["qmqOverheadCost"] = quoteAssemblyTotals.OverheadCost;
		quoteQuantityRow["qmqQuotingCost"] = quoteAssemblyTotals.QuotingCost;
		quoteQuantityRow["qmqSubcontractCost"] = quoteAssemblyTotals.SubcontractCost;
		quoteQuantityRow["qmqMaterialCost"] = quoteAssemblyTotals.MaterialCost;
		quoteQuantityRow["qmqSetupHours"] = quoteAssemblyTotals.SetupHours;
		quoteQuantityRow["qmqProductionHours"] = quoteAssemblyTotals.ProductionHours;
		return list;
	}

	public decimal SetPriceForQuantity(M1BindingSource bindingSource, DataRow quoteQuantityRow)
	{
		M1BindingSource parentBindingSource = bindingSource.PrimaryTable.GetParentBindingSource(quoteQuantityRow);
		DataRow currentAsDataRow = parentBindingSource.CurrentAsDataRow;
		if ((!string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("qmlPartID")) || !string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("qmlPartGroupID"))) && bindingSource.Count != 0)
		{
			DataRow row = parentBindingSource.Fields["qmlQuoteID"].RelatedTableGetDataRow("qmpCustomerOrganizationID,qmpARInvoiceLocationID,qmpCurrencyRateID,qmpQuoteDate");
			PriceCalculation sellingPrice = new Part().GetSellingPrice(bindingSource.CurrentDatabase, currentAsDataRow.Field<string>("qmlPartID"), currentAsDataRow.Field<string>("qmlPartRevisionID"), currentAsDataRow.Field<string>("qmlPartGroupID"), row.Field<string>("qmpCustomerOrganizationID"), row.Field<string>("qmpARInvoiceLocationID"), quoteQuantityRow.Field<decimal>("qmqQuoteQuantity"), row.Field<string>("qmpCurrencyRateID"), row.Field<DateTime?>("qmpQuoteDate"));
			if (sellingPrice.CalculationType != PriceCalculationType.NoPrice && sellingPrice.FullPrice != 0m)
			{
				if (quoteQuantityRow != null)
				{
					quoteQuantityRow["qmqLeadTime"] = string.Empty;
					PriceLineData priceLineData = null;
					decimal num;
					decimal num2;
					if (sellingPrice.PartPrice != null)
					{
						priceLineData = sellingPrice.PartPrice.GetLineForQuantity(quoteQuantityRow.Field<decimal>("qmqQuoteQuantity"));
						if (priceLineData != null)
						{
							if (priceLineData.UnitPrice == 0m)
							{
								num = sellingPrice.FullPrice;
								num2 = M1Math.Round(sellingPrice.FullPrice - priceLineData.Discount / 100.0m * sellingPrice.FullPrice, 5);
							}
							else if (sellingPrice.FullPrice == 0m)
							{
								num = priceLineData.UnitPrice;
								num2 = priceLineData.UnitPrice;
							}
							else
							{
								num = sellingPrice.FullPrice;
								num2 = priceLineData.UnitPrice;
							}
						}
						else
						{
							num = sellingPrice.FullPrice;
							num2 = sellingPrice.DiscountedPrice;
						}
					}
					else
					{
						num = sellingPrice.FullPrice;
						num2 = sellingPrice.DiscountedPrice;
					}
					if (priceLineData != null && priceLineData.LeadTime != 0)
					{
						quoteQuantityRow["qmqLeadTime"] = priceLineData.LeadTime.ToString();
					}
					else
					{
						quoteQuantityRow["qmqLeadTime"] = string.Empty;
					}
					if (num == 0m && priceLineData != null)
					{
						quoteQuantityRow["qmqDiscountPercent"] = priceLineData.Discount;
					}
					else if (sellingPrice.IsForeignCurrency)
					{
						quoteQuantityRow["qmqFullRevisedUnitPriceForeign"] = num;
						quoteQuantityRow["qmqRevisedUnitPriceForeign"] = num2;
					}
					else
					{
						quoteQuantityRow["qmqFullRevisedUnitPriceBase"] = num;
						quoteQuantityRow["qmqRevisedUnitPriceBase"] = num2;
					}
				}
			}
		}
		return 0m;
	}

	private void CalculateInfoForAsm(M1Database database, Job jobRef, int parentAsm, decimal totalRunQty, decimal quoteQuantity, QuoteAssemblyTotals parentAsmTotals, List<QuoteAssemblyTotals> asmTotals, DataRow quoteAsmRow, QuoteAsmData quoteParm, bool costRollUp = false)
	{
		byte decimals = 5;
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		decimal num3 = default(decimal);
		decimal num4 = default(decimal);
		decimal num5 = default(decimal);
		decimal num6 = default(decimal);
		decimal num7 = default(decimal);
		bool flag = false;
		QuoteAssemblyTotals quoteAssemblyTotals = new QuoteAssemblyTotals();
		if (quoteAsmRow != null)
		{
			quoteAssemblyTotals.AssemblyID = quoteAsmRow.Field<int>("qmaQuoteAssemblyID");
			quoteAssemblyTotals.ParentAssemblyID = quoteAsmRow.Field<int>("qmaParentAssemblyID");
			quoteAssemblyTotals.Level = quoteAsmRow.Field<short>("qmaLevel");
			quoteAssemblyTotals.QuantityPerParent = quoteAsmRow.Field<decimal>("qmaQuantityPerParent");
			quoteAssemblyTotals.PartID = quoteAsmRow.Field<string>("qmaPartID");
			quoteAssemblyTotals.PartRevisionID = quoteAsmRow.Field<string>("qmaPartRevisionID");
		}
		asmTotals.Add(quoteAssemblyTotals);
		if (quoteAsmRow != null && quoteAsmRow.Field<bool>("qmaPullAllFromStock") && quoteAsmRow["PartCost"] != DBNull.Value)
		{
			flag = true;
			quoteAssemblyTotals.MaterialCost = M1Math.Round(quoteQuantity * quoteAsmRow.Field<decimal>("qmaQuantityPerParent") * quoteAsmRow.Field<decimal>("PartCost"), decimals);
		}
		if (!flag)
		{
			DataRow[] array = quoteParm.QuoteOperationsInside.Select("qmoQuoteAssemblyID = " + parentAsm.ToLinq() + " And qmoOperationType = 1");
			foreach (DataRow row in array)
			{
				num7 = ((!(row.Field<decimal>("qmoAdditionalSetupQuantity") == 0m)) ? (row.Field<decimal>("qmoSetupHours") + row.Field<decimal>("qmoAdditionalSetupHours") * Math.Floor(quoteQuantity / row.Field<decimal>("qmoAdditionalSetupQuantity"))) : row.Field<decimal>("qmoSetupHours"));
				num5 = totalRunQty * row.Field<decimal>("qmoQuantityPerAssembly");
				num6 = (decimal)jobRef.CalculateProductionHours(database, (double)num5, (double)row.Field<decimal>("qmoProductionStandard"), row.Field<string>("qmoStandardFactor"), row.Field<string>("qmoWorkCenterID"), 5);
				quoteAssemblyTotals.SetupHours += num7;
				quoteAssemblyTotals.ProductionHours += num6;
				quoteAssemblyTotals.LaborCost += M1Math.Round(num7 * row.Field<decimal>("qmoSetupRate"), decimals);
				quoteAssemblyTotals.LaborCost += M1Math.Round(num6 * row.Field<decimal>("qmoProductionRate"), decimals);
				quoteAssemblyTotals.OverheadCost += M1Math.Round((num7 + num6) * row.Field<decimal>("qmoOverheadRate"), decimals);
				quoteAssemblyTotals.QuotingCost += M1Math.Round((num7 + num6) * row.Field<decimal>("qmoQuotingRate"), decimals);
			}
			quoteAssemblyTotals.SubcontractCost = default(decimal);
			num4 = default(decimal);
			array = quoteParm.QuoteOperationsOutside.Select("qmoQuoteAssemblyID = " + parentAsm.ToLinq() + " And qmoOperationType = 2");
			foreach (DataRow row2 in array)
			{
				num3 = totalRunQty * row2.Field<decimal>("qmoQuantityPerAssembly");
				num4 = row2.Field<decimal>("qmoEstimatedUnitCost");
				for (int num8 = 9; num8 >= 1; num8--)
				{
					if (row2.Field<decimal>("qmoQuantityBreak" + num8) != 0m && num3 >= row2.Field<decimal>("qmoQuantityBreak" + num8))
					{
						num4 = row2.Field<decimal>("qmoUnitCost" + num8);
						break;
					}
				}
				num4 = M1Math.Round(num3 * num4, decimals);
				if (row2.Field<decimal>("qmoMinimumCharge") != 0m && num4 < row2.Field<decimal>("qmoMinimumCharge"))
				{
					num4 = row2.Field<decimal>("qmoMinimumCharge");
				}
				num4 += row2.Field<decimal>("qmoSetupCharge");
				quoteAssemblyTotals.SubcontractCost += num4;
			}
			quoteAssemblyTotals.MaterialCost = default(decimal);
			num2 = default(decimal);
			array = quoteParm.QuoteMaterials.Select("qmmQuoteAssemblyID = " + parentAsm.ToLinq());
			foreach (DataRow row3 in array)
			{
				num = (decimal)jobRef.CalculateQtyWithScrap(database, (double)(totalRunQty * row3.Field<decimal>("qmmQuantityPerAssembly")), (double)row3.Field<decimal>("qmmScrapPercent"), (double)row3.Field<decimal>("qmmScrapQuantity"), 5);
				num2 = row3.Field<decimal>("qmmEstimatedUnitCost");
				if (!costRollUp)
				{
					for (int num9 = 9; num9 >= 1; num9--)
					{
						if (row3.Field<decimal>("qmmQuantityBreak" + num9) != 0m && num >= row3.Field<decimal>("qmmQuantityBreak" + num9))
						{
							num2 = row3.Field<decimal>("qmmUnitCost" + num9);
							break;
						}
					}
				}
				num2 = M1Math.Round(num * num2, decimals);
				if (row3.Field<decimal>("qmmMinimumCharge") != 0m && num2 < row3.Field<decimal>("qmmMinimumCharge"))
				{
					num2 = row3.Field<decimal>("qmmMinimumCharge");
				}
				quoteAssemblyTotals.MaterialCost += num2;
			}
		}
		if (quoteAsmRow != null && !flag)
		{
			DataRow[] array = quoteParm.QuoteAssemblies.Select("qmaParentAssemblyID = " + parentAsm.ToLinq() + " And qmaQuoteAssemblyID <> 0");
			foreach (DataRow dataRow in array)
			{
				CalculateInfoForAsm(database, jobRef, dataRow.Field<int>("qmaQuoteAssemblyID"), totalRunQty * dataRow.Field<decimal>("qmaQuantityPerParent"), quoteQuantity, quoteAssemblyTotals, asmTotals, dataRow, quoteParm, costRollUp);
			}
		}
		parentAsmTotals.LaborCost += quoteAssemblyTotals.LaborCost;
		parentAsmTotals.OverheadCost += quoteAssemblyTotals.OverheadCost;
		parentAsmTotals.QuotingCost += quoteAssemblyTotals.QuotingCost;
		parentAsmTotals.SubcontractCost += quoteAssemblyTotals.SubcontractCost;
		parentAsmTotals.MaterialCost += quoteAssemblyTotals.MaterialCost;
		parentAsmTotals.SetupHours += quoteAssemblyTotals.SetupHours;
		parentAsmTotals.ProductionHours += quoteAssemblyTotals.ProductionHours;
	}

	public decimal GetQuoteCommissionRate(M1BindingSource bindingSource)
	{
		M1Database currentDatabase = bindingSource.CurrentDatabase;
		SqlCommand sqlCommand;
		if (bindingSource.PrimaryTable.GetHighestLoadedTopLevelTable().TableName.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase))
		{
			decimal result = default(decimal);
			M1BindingSource childBindingSource = bindingSource.PrimaryTable.ParentBindingSource.PrimaryTable.ParentBindingSource.PrimaryTable.GetChildBindingSource("QuoteSalespeople");
			if (childBindingSource.Count != 0)
			{
				decimal num = default(decimal);
				sqlCommand = currentDatabase.NewSqlCommand("Select IsNull(lmeCommissionRate,0) From Employees Where lmeEmployeeID = @EmployeeID");
				sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar));
				foreach (DataRow row in childBindingSource.GetDataTable().Rows)
				{
					if (row.RowState != DataRowState.Deleted)
					{
						sqlCommand.Parameters["@EmployeeID"].Value = row.Field<string>("qmjSalesEmployeeID");
						result += (decimal)currentDatabase.ExecuteScalar(sqlCommand) * (row.Field<decimal>("qmjPercent") / 100.0m);
						++num;
					}
				}
			}
			return result;
		}
		sqlCommand = currentDatabase.NewSqlCommand("Select IsNull(Round(Sum((qmjPercent / 100) * lmeCommissionRate),2),0) From QuoteSalespeople Inner Join Employees On qmjSalesEmployeeID = lmeEmployeeID Where qmjQuoteID = @QuoteID");
		sqlCommand.Parameters.Add(new SqlParameter("@QuoteID", SqlDbType.NVarChar)).Value = bindingSource.CurrentAsDataRow.Field<string>("qmqQuoteID");
		return (decimal)currentDatabase.ExecuteScalar(sqlCommand);
	}

	public void SetGridSequence(M1BindingSource quantityBindingSource)
	{
		if (quantityBindingSource.Count == 0)
		{
			return;
		}
		byte b = 1;
		DataTable dataTable = quantityBindingSource.GetDataTable();
		DataTable source = quantityBindingSource.GetDataView().ToTable();
		List<DataRow> list = (from r in dataTable.AsEnumerable()
			where r.RowState != DataRowState.Deleted && r.Field<decimal>("qmqQuoteQuantity") > 0m
			select r).ToList();
		OrderedEnumerableRowCollection<DataRow> orderedEnumerableRowCollection = from s in source.AsEnumerable()
			where s.RowState != DataRowState.Deleted && s.Field<decimal>("qmqQuoteQuantity") > 0m
			orderby s.Field<decimal>("qmqQuoteQuantity")
			select s;
		if (IsGridAlreadySorted(orderedEnumerableRowCollection))
		{
			return;
		}
		bool flag = false;
		foreach (DataRow item in orderedEnumerableRowCollection)
		{
			if (item.Field<byte>("qmqQuoteQuantityID") != b)
			{
				item["qmqQuoteQuantityID"] = b;
				flag = true;
			}
			b++;
		}
		if (!flag)
		{
			return;
		}
		foreach (DataRow item2 in list)
		{
			item2.Delete();
		}
		orderedEnumerableRowCollection.CopyToDataTable(dataTable, LoadOption.Upsert);
	}

	private bool IsGridAlreadySorted(IEnumerable<DataRow> dataRows)
	{
		bool result = true;
		byte b = 0;
		foreach (DataRow dataRow in dataRows)
		{
			if (dataRow.Field<byte>("qmqQuoteQuantityID") - b != 1)
			{
				result = false;
				break;
			}
			b = dataRow.Field<byte>("qmqQuoteQuantityID");
		}
		return result;
	}
}
