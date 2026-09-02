using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Ax.Erp.IntegrationService;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class DMRShipment
{
	private class PartBinRecordKey
	{
		public string PartID { get; set; }

		public string PartRevision { get; set; }

		public string PartWHouse { get; set; }

		public string PartWHBin { get; set; }
	}

	private class DMRQuantity
	{
		public decimal QuantityAccepted { get; set; }

		public decimal QuantityReturned { get; set; }

		public decimal QuantityReturnedJob { get; set; }
	}

	private class PartBinRecordKeyEqualityComparer : IEqualityComparer<PartBinRecordKey>
	{
		public bool Equals(PartBinRecordKey x, PartBinRecordKey y)
		{
			if (x.PartID.Equals(y.PartID, StringComparison.CurrentCultureIgnoreCase) && x.PartRevision.Equals(y.PartRevision, StringComparison.CurrentCultureIgnoreCase) && x.PartWHouse.Equals(y.PartWHouse, StringComparison.CurrentCultureIgnoreCase))
			{
				return x.PartWHBin.Equals(y.PartWHBin, StringComparison.CurrentCultureIgnoreCase);
			}
			return false;
		}

		public int GetHashCode(PartBinRecordKey obj)
		{
			return (obj.PartID.Trim() + "|" + obj.PartRevision.Trim() + "|" + obj.PartWHouse.Trim() + "|" + obj.PartWHBin.Trim()).GetHashCode();
		}
	}

	public const string QTY_ON_HAND = "qtyOnHand";

	public const string QTY_TO_RETURN = "qtyToReturn";

	public const string QTY_TO_RETURN_JOB = "qtyToReturnJob";

	private PartBinRecordKey CreatePartQuantityKey(DataRow lineRow, DataRow componentRow)
	{
		if (lineRow != null)
		{
			return new PartBinRecordKey
			{
				PartID = lineRow.Field<string>("dslPartID").Trim(),
				PartRevision = lineRow.Field<string>("dslPartRevisionID").Trim(),
				PartWHouse = lineRow.Field<string>("dslPartWarehouseLocationID").Trim(),
				PartWHBin = lineRow.Field<string>("dslPartBinID").Trim()
			};
		}
		if (componentRow != null)
		{
			return new PartBinRecordKey
			{
				PartID = componentRow.Field<string>("dsoPartID").Trim(),
				PartRevision = componentRow.Field<string>("dsoPartRevisionID").Trim(),
				PartWHouse = componentRow.Field<string>("dsoPartWarehouseLocationID").Trim(),
				PartWHBin = componentRow.Field<string>("dsoPartBinID").Trim()
			};
		}
		return null;
	}

	private void PopulatePartQuantityDictionary(M1BindingSource lineBindingsource, IDictionary<PartBinRecordKey, DMRQuantity> dicPartQuantities, DataRow lineRow)
	{
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		decimal num3 = default(decimal);
		PartBinRecordKey partBinRecordKey = null;
		if (lineRow.Field<bool>("dslKitPart"))
		{
			DataTable dataTable = lineBindingsource.PrimaryTable.GetChildBindingSource("DMRShipmentComponents").GetDataView(lineRow).ToTable();
			if (dataTable == null || dataTable.Rows.Count == 0)
			{
				return;
			}
			{
				foreach (DataRow row in dataTable.Rows)
				{
					partBinRecordKey = CreatePartQuantityKey(null, row);
					num = row.Field<decimal>("dsoInvQuantityShipped") + row.Field<decimal>("dsoAdditionalQuantity");
					num2 = row.Field<decimal>("dsoReturnQuantityShipped") + row.Field<decimal>("dsoAdditionalQuantity");
					num3 = row.Field<decimal>("dsoJobMatQuantityShipped") + row.Field<decimal>("dsoAdditionalQuantity");
					if (dicPartQuantities.ContainsKey(partBinRecordKey))
					{
						dicPartQuantities[partBinRecordKey].QuantityAccepted += num;
						dicPartQuantities[partBinRecordKey].QuantityReturned += num2;
						dicPartQuantities[partBinRecordKey].QuantityReturnedJob += num3;
					}
					else
					{
						dicPartQuantities.Add(partBinRecordKey, new DMRQuantity
						{
							QuantityAccepted = num,
							QuantityReturned = num2,
							QuantityReturnedJob = num3
						});
					}
				}
				return;
			}
		}
		partBinRecordKey = CreatePartQuantityKey(lineRow, null);
		num = lineRow.Field<decimal>("dslInventoryQuantityShipped");
		num2 = lineRow.Field<decimal>("dslReturnQuantityShipped");
		num3 = lineRow.Field<decimal>("dslJobMatQuantityShipped");
		if (dicPartQuantities.ContainsKey(partBinRecordKey))
		{
			dicPartQuantities[partBinRecordKey].QuantityAccepted += num;
			dicPartQuantities[partBinRecordKey].QuantityReturned += num2;
			dicPartQuantities[partBinRecordKey].QuantityReturnedJob += num3;
		}
		else
		{
			dicPartQuantities.Add(partBinRecordKey, new DMRQuantity
			{
				QuantityAccepted = num,
				QuantityReturned = num2,
				QuantityReturnedJob = num3
			});
		}
	}

	private IDictionary<string, IList<string>> VerifyQuantityAgainstInventory(M1BindingSource bindingsource, IDictionary<PartBinRecordKey, DMRQuantity> dicPartQuantities)
	{
		IDictionary<string, IList<string>> dictionary = new Dictionary<string, IList<string>>();
		IList<string> list = new List<string>();
		IList<string> list2 = new List<string>();
		IList<string> list3 = new List<string>();
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		decimal num3 = default(decimal);
		bool flag = true;
		bool flag2 = false;
		foreach (KeyValuePair<PartBinRecordKey, DMRQuantity> dicPartQuantity in dicPartQuantities)
		{
			num = default(decimal);
			num2 = default(decimal);
			num3 = default(decimal);
			string partID = dicPartQuantity.Key.PartID;
			string partRevision = dicPartQuantity.Key.PartRevision;
			string partWHouse = dicPartQuantity.Key.PartWHouse;
			string partWHBin = dicPartQuantity.Key.PartWHBin;
			using (SqlCommand sqlCommand = new SqlCommand("SELECT ISNULL(imbQuantityOnHand, 0) AS imbQuantityOnHand, ISNULL(imbQuantityToReturn,0) AS imbQuantityToReturn, ISNULL(imbQuantityToReturnJob,0) AS imbQuantityToReturnJob, ISNULL(impNonStockedItem,1) AS impNonStockedItem, ISNULL(impPhantomOrKitPart,0) AS impPhantomOrKitPart FROM PartBins left outer join Parts on imbPartID = impPartID WHERE (imbPartID = @PartID) AND (imbPartRevisionID = @PartRevisionID) AND (imbWarehouseID = @WarehouseID) AND (imbPartBinID = @PartBinID) "))
			{
				sqlCommand.Parameters.AddWithValue("@PartID", partID);
				sqlCommand.Parameters.AddWithValue("@PartRevisionID", partRevision);
				sqlCommand.Parameters.AddWithValue("@WarehouseID", partWHouse);
				sqlCommand.Parameters.AddWithValue("@PartBinID", partWHBin);
				DataRow dataRow = bindingsource.Database.GetDataTable(sqlCommand).AsEnumerable().FirstOrDefault();
				if (dataRow != null)
				{
					num = dataRow.Field<decimal>("imbQuantityOnHand");
					num2 = dataRow.Field<decimal>("imbQuantityToReturn");
					num3 = dataRow.Field<decimal>("imbQuantityToReturnJob");
					flag = dataRow.Field<bool>("impNonStockedItem");
					flag2 = dataRow.Field<bool>("impPhantomOrKitPart");
				}
			}
			if (!flag && !flag2)
			{
				decimal quantityAccepted = dicPartQuantity.Value.QuantityAccepted;
				decimal quantityReturned = dicPartQuantity.Value.QuantityReturned;
				decimal quantityReturnedJob = dicPartQuantity.Value.QuantityReturnedJob;
				if (num - quantityAccepted < 0m && quantityAccepted != 0m)
				{
					string item = $"Quantity to Ship [{quantityAccepted}] is greater than Quantity on Hand [{num}] [Part: '{partID}', Revision: '{partRevision}', Warehouse: '{partWHouse}', Bin: '{partWHBin}'].";
					list.Add(item);
				}
				if (num2 - quantityReturned < 0m && quantityReturned != 0m)
				{
					string item2 = $"Return Quantity to Ship [{quantityReturned}] is greater than Quantity to Return [{num2}] [Part: '{partID}', Revision: '{partRevision}', Warehouse: '{partWHouse}', Bin: '{partWHBin}'].";
					list2.Add(item2);
				}
				if (num3 - quantityReturnedJob < 0m && quantityReturnedJob != 0m)
				{
					string item3 = $"Job Material Quantity Shipped [{quantityReturnedJob}] is greater than Quantity to Return (Job) [{num3}] [Part: '{partID}', Revision: '{partRevision}', Warehouse: '{partWHouse}', Bin: '{partWHBin}'].";
					list3.Add(item3);
				}
			}
		}
		dictionary["qtyOnHand"] = list;
		dictionary["qtyToReturn"] = list2;
		dictionary["qtyToReturnJob"] = list3;
		return dictionary;
	}

	public bool CheckSumOfShipQtyForDelivery(M1BindingSource m1BindingSource)
	{
		DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
		if (currentAsDataRow == null)
		{
			return true;
		}
		DataTable dataTable = m1BindingSource.GetDataView().ToTable();
		decimal num = currentAsDataRow.Field<decimal>("dslDMROpenQuantity");
		decimal num2 = default(decimal);
		int num3 = 0;
		DataRow[] array = dataTable.Select("dslDMRClaimID = " + currentAsDataRow.Field<string>("dslDMRClaimID").Trim().ToLinq() + " and dslDMRClaimLineID = " + Convert.ToInt32(currentAsDataRow["dslDMRClaimLineID"]).ToLinq());
		for (int i = 0; i < array.Length; i++)
		{
			_ = array[i];
			if (!string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("dslDMRClaimID")))
			{
				num2 += currentAsDataRow.Field<decimal>("dslQuantityShipped");
				num3++;
			}
		}
		if (num2 > num && num3 > 1)
		{
			return false;
		}
		return true;
	}

	public void PostDMRShipment(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		SqlTransaction sqlTransaction = bindingSource.Transaction ?? database.BeginTransaction();
		try
		{
			if (bindingSource.CurrentAsDataRow == null)
			{
				return;
			}
			bindingSource.CurrentAsDataRow.SetField("dspPosted", value: true);
			string value = bindingSource.CurrentAsDataRow.Field<string>("dspDMRShipmentID");
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) AS sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, dslUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntNegativeTransaction from DMRShipmentLines inner join SerialNumberTransactions on dslUniqueID = sntTableUniqueID where dslDMRShipmentID = @ID and dslPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
				foreach (DataRow row in dataTable.Rows)
				{
					byte status = 0;
					byte transType = 0;
					serialNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row.Field<string>("sntSerialNumberID"));
					bool flag = row.Field<bool>("sntNegativeTransaction");
					switch (row.Field<byte>("sntTransactionType"))
					{
					case 61:
						status = (byte)(flag ? 2 : 4);
						transType = 5;
						break;
					case 62:
						status = (byte)(flag ? 7 : 4);
						transType = 42;
						break;
					case 63:
						status = (byte)(flag ? 7 : 4);
						transType = 40;
						break;
					}
					serialNumberDefinition.AddSerialTransaction(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), status, transType, "DMRShipmentLines", row.Field<Guid>("dslUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), 0, row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) AS sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, dsoUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from DMRShipmentComponents inner join SerialNumberTransactions on dsoUniqueID = sntTableUniqueID where dsoDMRShipmentID = @ID and dsoPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition2 = new SerialNumberDefinition();
				foreach (DataRow row2 in dataTable.Rows)
				{
					byte status2 = 0;
					byte transType2 = 0;
					serialNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row2.Field<string>("sntSerialNumberID"));
					bool flag2 = row2.Field<bool>("sntNegativeTransaction");
					switch (row2.Field<byte>("sntTransactionType"))
					{
					case 61:
						status2 = (byte)(flag2 ? 2 : 4);
						transType2 = 5;
						break;
					case 62:
						status2 = (byte)(flag2 ? 7 : 4);
						transType2 = 42;
						break;
					case 63:
						status2 = (byte)(flag2 ? 7 : 4);
						transType2 = 40;
						break;
					}
					serialNumberDefinition2.AddSerialTransaction(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntPartWarehouseLocationID"), row2.Field<string>("sntPartBinID"), row2.Field<string>("sntSerialNumberID"), row2.Field<decimal>("sntQuantity"), status2, transType2, "DMRShipmentComponents", row2.Field<Guid>("dsoUniqueID"), row2.Field<string>("sntJobID"), Convert.ToInt32(row2["sntJobAssemblyID"]), Convert.ToInt32(row2["sntJobMaterialID"]), Convert.ToInt32(row2["sntJobMaterialComponentID"]), row2.Field<bool>("sntNegativeTransaction"), row2.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) AS abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, dslUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from DMRShipmentLines inner join LotNumberTransactions on dslUniqueID = abtTableUniqueID where dslDMRShipmentID = @ID and dslPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
				foreach (DataRow row3 in dataTable.Rows)
				{
					byte status3 = 0;
					byte transType3 = 0;
					lotNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row3.Field<string>("abtLotNumberID"));
					bool flag3 = row3.Field<bool>("abtNegativeTransaction");
					switch (row3.Field<byte>("abtTransactionType"))
					{
					case 61:
						status3 = (byte)(flag3 ? 2 : 4);
						transType3 = 5;
						break;
					case 62:
						status3 = (byte)(flag3 ? 7 : 4);
						transType3 = 42;
						break;
					case 63:
						status3 = (byte)(flag3 ? 7 : 4);
						transType3 = 40;
						break;
					}
					lotNumberDefinition.AddLotTransaction(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtPartWarehouseLocationID"), row3.Field<string>("abtPartBinID"), row3.Field<string>("abtLotNumberID"), row3.Field<decimal>("abtQuantity"), status3, transType3, "DMRShipmentLines", row3.Field<Guid>("dslUniqueID"), row3.Field<string>("abtJobID"), Convert.ToInt32(row3["abtJobAssemblyID"]), Convert.ToInt32(row3["abtJobMaterialID"]), Convert.ToInt32(row3["abtJobMaterialComponentID"]), row3.Field<bool>("abtNegativeTransaction"), row3.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtLotNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) AS abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, dsoUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from DMRShipmentComponents inner join LotNumberTransactions on dsoUniqueID = abtTableUniqueID where dsoDMRShipmentID = @ID and dsoPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
				foreach (DataRow row4 in dataTable.Rows)
				{
					byte status4 = 0;
					byte transType4 = 0;
					lotNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row4.Field<string>("abtLotNumberID"));
					bool flag4 = row4.Field<bool>("abtNegativeTransaction");
					switch (row4.Field<byte>("abtTransactionType"))
					{
					case 61:
						status4 = (byte)(flag4 ? 2 : 4);
						transType4 = 5;
						break;
					case 62:
						status4 = (byte)(flag4 ? 7 : 4);
						transType4 = 42;
						break;
					case 63:
						status4 = (byte)(flag4 ? 7 : 4);
						transType4 = 40;
						break;
					}
					lotNumberDefinition2.AddLotTransaction(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtPartWarehouseLocationID"), row4.Field<string>("abtPartBinID"), row4.Field<string>("abtLotNumberID"), row4.Field<decimal>("abtQuantity"), status4, transType4, "DMRShipmentComponents", row4.Field<Guid>("dsoUniqueID"), row4.Field<string>("abtJobID"), Convert.ToInt32(row4["abtJobAssemblyID"]), Convert.ToInt32(row4["abtJobMaterialID"]), Convert.ToInt32(row4["abtJobMaterialComponentID"]), row4.Field<bool>("abtNegativeTransaction"), row4.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition2.RefreshStatuses(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtLotNumberID"));
				}
			}
			M1BindingSource m1BindingSource = bindingSource.PrimaryTable?.GetChildBindingSource("DMRShipmentLines");
			if (m1BindingSource != null && m1BindingSource.Count > 0)
			{
				IntegrationServiceConstants.EntityType entityType = ((!bindingSource.CurrentAsDataRow.Field<bool>("dspReversalEntry")) ? IntegrationServiceConstants.EntityType.VendorCredit : IntegrationServiceConstants.EntityType.Bill);
				new M1.Ax.Erp.IntegrationService.IntegrationService().CreateTransactionQueueRecord(database, sqlTransaction, IntegrationServiceConstants.IntegrationType.Financial, IntegrationServiceConstants.ApiAction.Create, entityType, IntegrationServiceConstants.Status.Pending, "DMRShipments", bindingSource.CurrentAsDataRow.Field<Guid>("dspUniqueId"), 13);
			}
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public bool DMRShipmentPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool result = false;
		if (currentAsDataRow != null)
		{
			if (currentAsDataRow.Table.Columns.Contains("dspShipDate"))
			{
				dateTime = currentAsDataRow.Field<DateTime>("dspShipDate");
			}
			if (new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
			{
				result = true;
			}
		}
		return result;
	}

	private bool CheckDRMShipmentFutureDatePost(DataRow dmrShipment)
	{
		return dmrShipment.Field<DateTime>("dspShipDate") > DateTime.Now;
	}

	public bool PostDMRShipmentCheck(M1BindingSource bindingsource)
	{
		IDictionary<PartBinRecordKey, DMRQuantity> dicPartQuantities = new Dictionary<PartBinRecordKey, DMRQuantity>(new PartBinRecordKeyEqualityComparer());
		DataRow currentAsDataRow = bindingsource.CurrentAsDataRow;
		if (currentAsDataRow != null && !currentAsDataRow.Field<bool>("dspReversalEntry"))
		{
			string table = "DMRShipmentLines";
			DataTable dataTable = bindingsource.PrimaryTable.GetChildBindingSource(table).GetDataTable();
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				M1BindingSource childBindingSource = bindingsource.PrimaryTable.GetChildBindingSource(table);
				foreach (DataRow row in dataTable.Rows)
				{
					PopulatePartQuantityDictionary(childBindingSource, dicPartQuantities, row);
				}
				IDictionary<string, IList<string>> dictionary = VerifyQuantityAgainstInventory(bindingsource, dicPartQuantities);
				bool flag = (bool)bindingsource.Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
				if (dictionary["qtyToReturn"].Any())
				{
					MessageBox.Show("This transaction CAN NOT be posted because Return Qty to Ship CANNOT BE GREATER THAN Quantity to Return.\n\n" + string.Join("\n", dictionary["qtyToReturn"]), "DMR Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
				if (dictionary["qtyToReturnJob"].Any())
				{
					MessageBox.Show("This transaction CAN NOT be posted because Job Material Quantity Shipped CANNOT BE GREATER THAN Quantity to Return (Job).\n\n" + string.Join("\n", dictionary["qtyToReturnJob"]), "DMR Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
				if (flag)
				{
					if (CheckDRMShipmentFutureDatePost(currentAsDataRow) && dictionary["qtyOnHand"].Any())
					{
						MessageBox.Show("This transaction CAN NOT be posted because future dating is not supported when the transaction will result in a negative quantity on hand.\n\n" + string.Join("\n", dictionary["qtyOnHand"]), "DMR Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
					bool flag2 = (bool)bindingsource.Database.Props("IM")["xapIMEnableWarningWhenNegative"];
					string text = VerifyQuantityAndInactiveBin(bindingsource.Database, currentAsDataRow.Field<string>("dspDMRShipmentID"));
					if (!string.IsNullOrEmpty(text))
					{
						MessageBox.Show("This transaction CAN NOT be posted because it will result in a negative quantity on hand for an INACTIVE bin for the part(s) indicated." + "\n\n" + text, "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
					if (flag2 && dictionary["qtyOnHand"].Any() && MessageBox.Show("This transaction WILL RESULT in a negative quantity on hand for the part(s) indicated. Are you sure?\n\n" + string.Join("\n", dictionary["qtyOnHand"]), "DMR Shipments", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
					{
						return false;
					}
				}
				else if (dictionary["qtyOnHand"].Any())
				{
					MessageBox.Show("This transaction CAN NOT be posted because it will result in a negative quantity on hand for the part(s) indicated.\n\n" + string.Join("\n", dictionary["qtyOnHand"]), "DMR Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
			}
		}
		return true;
	}

	private string VerifyQuantityAndInactiveBin(M1Database database, string dmrShipmentID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (SqlCommand sqlCommand = new SqlCommand("select imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID,imbInactiveBin,dslQuantityShipped as quantityShipped,imbQuantityOnHand from PartBins inner join DMRShipmentLines on imbPartID=dslPartID and imbPartRevisionID=dslPartRevisionID and imbWarehouseID=dslPartWarehouseLocationID and imbPartBinID=dslPartBinID inner join Parts on impPartID=imbPartID where dslDMRShipmentID=" + dmrShipmentID.ToSql() + " and imbQuantityOnHand<dslQuantityShipped and imbInactiveBin=1 and impPhantomOrKitPart=0 \r\nunion\r\nselect imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbInactiveBin, dsoInvQuantityShipped as quantityShipped, imbQuantityOnHand from PartBins inner join DMRShipmentComponents on imbPartID = dsoPartID and imbPartRevisionID = dsoPartRevisionID and imbWarehouseID = dsoPartWarehouseLocationID and imbPartBinID = dsoPartBinID inner join Parts on impPartID=imbPartID where dsoDMRShipmentID = " + dmrShipmentID.ToSql() + " and imbQuantityOnHand<dsoInvQuantityShipped and imbInactiveBin=1 and impPhantomOrKitPart=0"))
		{
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					decimal num = row.Field<decimal>("quantityShipped");
					decimal num2 = row.Field<decimal>("imbQuantityOnHand");
					string text = row.Field<string>("imbPartID");
					string text2 = row.Field<string>("imbPartRevisionID");
					string text3 = row.Field<string>("imbWarehouseID");
					string text4 = row.Field<string>("imbPartBinID");
					stringBuilder.AppendLine($"[Quantity to Ship [{num}] IS GREATER THAN Quantity on Hand [{num2}]");
					stringBuilder.AppendLine("[Part: '" + text + "', Revision: '" + text2 + "', Warehouse: '" + text3 + "', Bin: '" + text4 + "']");
					stringBuilder.AppendLine();
				}
				return stringBuilder.ToString();
			}
		}
		return string.Empty;
	}

	public decimal GetDMRComponentUnitCostsFromReceipt(M1Database database, SqlTransaction transaction, string DMRShipmentID, int DMRShipmentLineID, int DMRShipmentComID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select rmoPurchaseUnitCost From DMRShipmentComponents Inner Join InspectionComponents On dsoInspectionID = qamInspectionID And dsoInspectionLineID = qamInspectionLineID And dsoInspectionComponentID = qamInspectionComponentID Inner Join ReceiptComponents On qamSourceTableUniqueID = rmoUniqueID Where dsoDMRShipmentID = @dmrShipID And dsoDMRShipmentLineID = @lineID And dsoDMRShipmentComponentID = @comID");
		sqlCommand.Parameters.Add(new SqlParameter("@dmrShipID", SqlDbType.NVarChar)).Value = DMRShipmentID;
		sqlCommand.Parameters.Add(new SqlParameter("@lineID", SqlDbType.Int)).Value = DMRShipmentLineID;
		sqlCommand.Parameters.Add(new SqlParameter("@comID", SqlDbType.Int)).Value = DMRShipmentComID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count > 0)
		{
			return dataTable.Rows[0].Field<decimal>("rmoPurchaseUnitCost");
		}
		return 0m;
	}

	public string CheckDMRShipmentForZeroDollarTotals(M1BindingSource bindingSource)
	{
		if (bindingSource.CurrentAsDataRow == null)
		{
			return string.Empty;
		}
		if (bindingSource.CurrentAsDataRow.Field<bool>("dspReversalEntry"))
		{
			return string.Empty;
		}
		DataTable dataTable = bindingSource.PrimaryTable.GetChildBindingSource("DMRShipmentLines").GetDataTable();
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return string.Empty;
		}
		if (dataTable.Rows.Cast<DataRow>().Any((DataRow lineRow) => lineRow.Field<decimal>("dslUnitPriceForeign").Equals(0m)))
		{
			return "There are dmr shipment lines that have zero dollar total amounts. If you continue, this will result in a zero dollar vendor credit line in your financial package.\n\nDo you wish to continue posting?";
		}
		return string.Empty;
	}
}
