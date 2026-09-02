using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class CreateSerialLotNumberBase
{
	public DataTable NumbersTable;

	public string PartID = string.Empty;

	public string PartRevisionID = string.Empty;

	public string PartWarehouseLocationID = string.Empty;

	public string PartBinID = string.Empty;

	public DateTime? TransactionDate;

	public DateTime? ExpirationDate;

	public bool RequireSystemWideUniqueSerialNumbers;

	private string TrPrefix;

	private string TPrefix;

	private string HPrefix;

	private bool _IsChanged;

	public bool IsChanged
	{
		get
		{
			return _IsChanged;
		}
		private set
		{
			_IsChanged = value;
		}
	}

	public CreateSerialLotNumberBase(char type)
	{
		ProcessPrefixCodes(type);
	}

	public virtual void Clear()
	{
		NumbersTable.Clear();
	}

	public virtual void Add(M1User user, string NumberID, DateTime? expirationDate)
	{
		DataRow row = NumbersTable.NewRow().BlankRow();
		IsChanged = true;
		row.SetField($"{HPrefix}AddedByUserID", user.ID);
		row.SetField($"{HPrefix}AddedDate", DateTime.Now);
		row.SetField($"{HPrefix}PartID", PartID);
		row.SetField($"{HPrefix}PartRevisioniD", PartRevisionID);
		row.SetField($"{HPrefix}CreatedBy", user.ID);
		row.SetField($"{HPrefix}CreatedDate", DateTime.Now);
		row.SetField($"{HPrefix}{TPrefix}NumberID", NumberID);
		row.SetField($"{HPrefix}ExpirationDate", expirationDate);
		NumbersTable.Rows.Add(row);
	}

	public virtual void CreateStatusAndTransactionRecords(M1User user, M1Database database, SqlTransaction sqlTransaction)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(" INSERT INTO {0}NumberTransactions ", TPrefix);
		stringBuilder.AppendFormat("({0}{1}NumberID, {0}TransactionDate, ", TrPrefix, TPrefix);
		stringBuilder.AppendFormat("{0}PartID, {0}PartRevisionID, {0}PartWarehouseLocationID, {0}PartBinID, ", TrPrefix);
		stringBuilder.AppendFormat("{0}TransactionType, {0}Status, {0}CreatedBy, {0}CreatedDate, {0}Quantity)", TrPrefix);
		stringBuilder.Append("Values (@NumberID,@TransactionDate,@PartID,@PartRevisionID,@PartWarehouseLocationID,@PartBinID,");
		stringBuilder.Append("@TransactionType,@Status,@CreatedBy,@CreatedDate,@Quantity)");
		SqlCommand sqlCommand = database.NewSqlCommand(stringBuilder.ToString());
		sqlCommand.Parameters.Add(new SqlParameter("@NumberID", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime));
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@PartWarehouseLocationID", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@PartBinID", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@TransactionType", SqlDbType.Decimal));
		sqlCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.SmallInt));
		sqlCommand.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime));
		sqlCommand.Parameters.Add(new SqlParameter("@Quantity", SqlDbType.Decimal));
		foreach (DataRow row in NumbersTable.Rows)
		{
			sqlCommand.Parameters["@NumberID"].Value = row.Field<string>($"{HPrefix}{TPrefix}NumberID");
			if (TransactionDate.HasValue)
			{
				sqlCommand.Parameters["@TransactionDate"].Value = TransactionDate;
			}
			else
			{
				sqlCommand.Parameters["@TransactionDate"].Value = DateTime.Now;
			}
			sqlCommand.Parameters["@PartID"].Value = row.Field<string>($"{HPrefix}PartID");
			sqlCommand.Parameters["@PartRevisionID"].Value = row.Field<string>($"{HPrefix}PartRevisionID");
			sqlCommand.Parameters["@PartWarehouseLocationID"].Value = PartWarehouseLocationID;
			sqlCommand.Parameters["@PartBinID"].Value = PartBinID;
			sqlCommand.Parameters["@TransactionType"].Value = 19;
			sqlCommand.Parameters["@Status"].Value = 0;
			sqlCommand.Parameters["@CreatedBy"].Value = user.ID;
			sqlCommand.Parameters["@CreatedDate"].Value = DateTime.Now;
			if (TPrefix.Equals("LOT"))
			{
				sqlCommand.Parameters["@Quantity"].Value = 0;
			}
			else
			{
				sqlCommand.Parameters["@Quantity"].Value = 0;
			}
			database.ExecuteCommand(sqlCommand, sqlTransaction);
		}
	}

	public virtual void Load(FieldDefinition partBinField, M1Database database, DataRow row)
	{
		if (partBinField != null)
		{
			PartID = row.Field<string>(partBinField.RelatedFieldsAndCurrentFieldArray[0]).Trim();
			PartRevisionID = row.Field<string>(partBinField.RelatedFieldsAndCurrentFieldArray[1]).Trim();
			PartWarehouseLocationID = row.Field<string>(partBinField.RelatedFieldsAndCurrentFieldArray[2]).Trim();
			PartBinID = row.Field<string>(partBinField.RelatedFieldsAndCurrentFieldArray[3]).Trim();
		}
	}

	public virtual bool IsDataValid(M1Database database)
	{
		bool flag = true;
		string text = string.Empty;
		string text2 = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		SqlCommand sqlCommand = database.NewSqlCommand(string.Format("Select IsNull(Count(*),0) From {0}Numbers Where {1}{0}NumberID = @NumberID", TPrefix, HPrefix));
		sqlCommand.Parameters.Add(new SqlParameter("@NumberID", SqlDbType.NVarChar));
		SqlCommand sqlCommand2 = database.NewSqlCommand(string.Format("Select IsNull(Count(*),0) From {0}Numbers Where {1}PartID = @PartID And {1}PartRevisionID = @PartRevisionID And {1}{0}NumberID = @NumberID", TPrefix, HPrefix));
		sqlCommand2.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar));
		sqlCommand2.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar));
		sqlCommand2.Parameters.Add(new SqlParameter("@NumberID", SqlDbType.NVarChar));
		foreach (DataRow row in NumbersTable.Rows)
		{
			if (row.Field<string>($"{HPrefix}{TPrefix}NumberID").Trim().Length == 0)
			{
				text = $"\r\n{TPrefix} Number is required.";
				flag = false;
				continue;
			}
			if (row.Field<string>($"{HPrefix}PartID").Trim().Length == 0)
			{
				text2 = "\r\nPart ID is required.";
				flag = false;
				continue;
			}
			if (RequireSystemWideUniqueSerialNumbers)
			{
				sqlCommand.Parameters["@SerialNumberID"].Value = row.Field<string>("imsSerialNumberID");
				if (Convert.ToInt32(database.ExecuteScalar(sqlCommand)) != 0)
				{
					stringBuilder.AppendFormat("\r\nSerial Number {0} already exists on another part. Please enter a different serial number.", row.Field<string>("imsSerialNumberID").Trim());
					flag = false;
				}
				continue;
			}
			sqlCommand2.Parameters["@PartID"].Value = row.Field<string>($"{HPrefix}PartID");
			sqlCommand2.Parameters["@PartRevisionID"].Value = row.Field<string>($"{HPrefix}PartRevisionID");
			sqlCommand2.Parameters["@NumberID"].Value = row.Field<string>($"{HPrefix}{TPrefix}NumberID");
			if (Convert.ToInt32(database.ExecuteScalar(sqlCommand2)) != 0)
			{
				stringBuilder.AppendFormat("\r\n{1} Number {0} already exists for this part. Please enter a different {1} number.", row.Field<string>($"{HPrefix}{TPrefix}NumberID").Trim(), TPrefix);
				flag = false;
			}
		}
		if (!flag)
		{
			throw new M1Exception(string.Format("The following information was returned while validating the {3} numbers: {0}{1}{2}", text, text2, stringBuilder.ToString(), TPrefix));
		}
		return flag;
	}

	private void ProcessPrefixCodes(char type)
	{
		if (type.ToString().ToUpper().Equals("S"))
		{
			TrPrefix = "snt";
			TPrefix = "Serial";
			HPrefix = "ims";
		}
		else
		{
			TrPrefix = "abt";
			TPrefix = "Lot";
			HPrefix = "abl";
		}
	}
}
