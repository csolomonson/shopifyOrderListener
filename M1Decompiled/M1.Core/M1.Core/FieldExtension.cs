using System;
using System.ComponentModel;
using System.Data;

namespace M1.Core;

public class FieldExtension : IDisposable
{
	private string _Table = string.Empty;

	private string _FieldName = string.Empty;

	private string _ExtensionID = string.Empty;

	private string _ExtensionTypeID = string.Empty;

	private Guid _UniqueID = Guid.Empty;

	private short _Sequence;

	private bool _Custom = true;

	private string _AppExtensionID = string.Empty;

	private string _AvailableFilterNegativeExpression = string.Empty;

	private string _AvailableFilterPositiveExpression = string.Empty;

	private string _RelatedJobField = string.Empty;

	private string _RelatedJobStatusField = string.Empty;

	private string _RelatedPlantField = string.Empty;

	private byte _StatusNegative;

	private byte _StatusPositive;

	private string _TransactionDateField = string.Empty;

	private byte _TransactionType;

	private byte _Source;

	private bool _AllowMismatchedQuantity;

	private bool _ReverseSign;

	private string _Parameters = string.Empty;

	private string _PartBinField = string.Empty;

	private string _RequiredExpression = string.Empty;

	private string _RequiredExpressionUser = string.Empty;

	public string DisplayText = string.Empty;

	public string OpenWithID = string.Empty;

	protected FieldDefinition _Field;

	public virtual string Table
	{
		get
		{
			return _Table;
		}
		set
		{
			_Table = value;
		}
	}

	public virtual string FieldName
	{
		get
		{
			return _FieldName;
		}
		set
		{
			_FieldName = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates the id for this object.")]
	public virtual string ExtensionID
	{
		get
		{
			return _ExtensionID;
		}
		set
		{
			_ExtensionID = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates the field extension type for this object.")]
	public virtual string ExtensionTypeID
	{
		get
		{
			return _ExtensionTypeID;
		}
		set
		{
			_ExtensionTypeID = value;
		}
	}

	public virtual Guid UniqueID
	{
		get
		{
			return _UniqueID;
		}
		set
		{
			_UniqueID = value;
		}
	}

	public virtual short Sequence
	{
		get
		{
			return _Sequence;
		}
		set
		{
			_Sequence = value;
		}
	}

	public virtual bool Custom
	{
		get
		{
			return _Custom;
		}
		set
		{
			_Custom = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates the application extension for this object.")]
	public virtual string AppExtensionID
	{
		get
		{
			return _AppExtensionID;
		}
		set
		{
			_AppExtensionID = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Specifies the filter to use when generating the list of available lot/serial numbers when the quantity to enter is negative. The standard Fields object can be used in this expression, which references the current table. The default query will always include a filter on the current part, revision, warehouse and bin.")]
	public virtual string AvailableFilterNegativeExpression
	{
		get
		{
			return _AvailableFilterNegativeExpression;
		}
		set
		{
			_AvailableFilterNegativeExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Specifies the filter to use when generating the list of available lot/serial numbers when the quantity to enter is positive. The standard Fields object can be used in this expression, which references the current table. The default query will always include a filter on the current part, revision, warehouse and bin.")]
	public virtual string AvailableFilterPositiveExpression
	{
		get
		{
			return _AvailableFilterPositiveExpression;
		}
		set
		{
			_AvailableFilterPositiveExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("This field will be copied to the selected transactions job field if it is set. This can be a JobID, JobAssemblyID, JobMaterialID or JobOperationID field.")]
	public virtual string RelatedJobField
	{
		get
		{
			return _RelatedJobField;
		}
		set
		{
			_RelatedJobField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("This field will be copied to the selected transactions job status field if it is set.")]
	public virtual string RelatedJobStatusField
	{
		get
		{
			return _RelatedJobStatusField;
		}
		set
		{
			_RelatedJobStatusField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("This field will be copied to the selected transactions plant field if it is set.")]
	public virtual string RelatedPlantField
	{
		get
		{
			return _RelatedPlantField;
		}
		set
		{
			_RelatedPlantField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Specifies the lot/serial number status for a negative transaction.")]
	public virtual byte StatusNegative
	{
		get
		{
			return _StatusNegative;
		}
		set
		{
			_StatusNegative = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Specifies the lot/serial number status for a positive transaction.")]
	public virtual byte StatusPositive
	{
		get
		{
			return _StatusPositive;
		}
		set
		{
			_StatusPositive = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Specifies the TransactionDate field in the Lot / Serial NumberTransaction record that gets created. If this field is empty, the current date and time will be used.")]
	public virtual string TransactionDateField
	{
		get
		{
			return _TransactionDateField;
		}
		set
		{
			_TransactionDateField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("This is the number that will be put into the TransactionType field of the Lot / Serial NumberTransaction record that gets created. This is required for lot numbers to work, and must be a unique number for a quantity field on a given table (i.e. if there are multiple quantity fields on receipts, each field must have a different transaction type). This number is used in the query to load the selected lot numbers.")]
	public virtual byte TransactionType
	{
		get
		{
			return _TransactionType;
		}
		set
		{
			_TransactionType = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("This is the number that will be put into the Source field of the record that gets created.")]
	public virtual byte Source
	{
		get
		{
			return _Source;
		}
		set
		{
			_Source = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Specifies if the number of lot / serial numbers selected can be less than the quantity entered.")]
	public virtual bool AllowMismatchedQuantity
	{
		get
		{
			return _AllowMismatchedQuantity;
		}
		set
		{
			_AllowMismatchedQuantity = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Description("Specifies if the quantity is to be positive or negative.")]
	public virtual bool ReverseSign
	{
		get
		{
			return _ReverseSign;
		}
		set
		{
			_ReverseSign = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("A comma delimited string of parameters used to specify actions within the expressions code.")]
	public virtual string Parameters
	{
		get
		{
			return _Parameters;
		}
		set
		{
			_Parameters = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Related PartBin field is required for lot / serial numbers to work.")]
	public virtual string PartBinField
	{
		get
		{
			return _PartBinField;
		}
		set
		{
			_PartBinField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("When this expression returns true, lot / serial numbers for the field will be required. The Fields object is available in this expression. This allows you to turn on lot / serial number tracking at the row level, in addition to the track lot numbers at the part level.")]
	public virtual string RequiredExpression
	{
		get
		{
			return _RequiredExpression;
		}
		set
		{
			_RequiredExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("When this expression returns true, lot /serial numbers for the field will be required. The Fields object is available in this expression. This allows you to turn on lot / serial number tracking at the row level, in addition to the track lot numbers at the part level.")]
	public virtual string RequiredExpressionUser
	{
		get
		{
			return _RequiredExpressionUser;
		}
		set
		{
			_RequiredExpressionUser = value;
		}
	}

	public FieldDefinition Field
	{
		get
		{
			return _Field;
		}
		set
		{
			_Field = value;
		}
	}

	[Browsable(false)]
	public bool RequiredResolved { get; private set; }

	public event EventHandler RequiredChanged;

	public virtual void Dispose()
	{
		if (Field != null)
		{
			Field.Valid -= Field_Valid;
			if (Field.BindingSource != null)
			{
				Field.BindingSource.CurrentChanged -= BindingSource_CurrentChanged;
			}
		}
		Field = null;
		this.RequiredChanged = null;
	}

	public virtual void Load(DataRow row, ReferencedFieldsList validCodeReferencedFields, FieldDefinition field, string openWithID, string displayText)
	{
		Field = field;
		DisplayText = displayText;
		OpenWithID = openWithID;
		Table = row.Field<string>("dqTable");
		FieldName = row.Field<string>("dqField");
		ExtensionID = row.Field<string>("dqFieldExtensionID");
		ExtensionTypeID = row.Field<string>("dqFieldExtensionTypeID");
		Sequence = row.Field<short>("dqSequence");
		UniqueID = row.Field<Guid>("dqUniqueID");
		Custom = row.Field<bool>("dqCustom");
		AppExtensionID = row.Field<string>("dqAppExtensionID");
		AvailableFilterNegativeExpression = row.Field<string>("dqAvailableFilterNegativeExpression");
		if (AvailableFilterNegativeExpression == null)
		{
			AvailableFilterNegativeExpression = string.Empty;
		}
		AvailableFilterPositiveExpression = row.Field<string>("dqAvailableFilterPositiveExpression");
		if (AvailableFilterPositiveExpression == null)
		{
			AvailableFilterPositiveExpression = string.Empty;
		}
		RelatedJobField = row.Field<string>("dqRelatedJobField");
		RelatedJobStatusField = row.Field<string>("dqRelatedJobStatusField");
		RelatedPlantField = row.Field<string>("dqRelatedPlantField");
		ReverseSign = row.Field<bool>("dqReverseSign");
		Parameters = row.Field<string>("dqParameters");
		if (Parameters == null)
		{
			Parameters = string.Empty;
		}
		StatusNegative = row.Field<byte>("dqStatusNegative");
		StatusPositive = row.Field<byte>("dqStatusPositive");
		TransactionDateField = row.Field<string>("dqTransactionDateField");
		TransactionType = row.Field<byte>("dqTransactionType");
		Source = row.Field<byte>("dqSource");
		AllowMismatchedQuantity = row.Field<bool>("dqAllowMismatchedQuantity");
		PartBinField = row.Field<string>("dqPartBinField");
		RequiredExpression = row.Field<string>("dqRequiredExpression");
		if (RequiredExpression == null)
		{
			RequiredExpression = string.Empty;
		}
		RequiredExpressionUser = row.Field<string>("dqRequiredExpressionUser");
		if (RequiredExpressionUser == null)
		{
			RequiredExpressionUser = string.Empty;
		}
		if (validCodeReferencedFields != null)
		{
			if (RequiredExpression != null && RequiredExpression.Length != 0)
			{
				validCodeReferencedFields.ParseCodeForFields(RequiredExpression);
			}
			if (RequiredExpressionUser != null && RequiredExpressionUser.Length != 0)
			{
				validCodeReferencedFields.ParseCodeForFields(RequiredExpressionUser);
			}
		}
	}

	public virtual bool IsRequired(M1Database database, DataRow row)
	{
		bool flag = false;
		if (row != null && row.RowState != DataRowState.Detached && PartBinField.Length != 0)
		{
			string columnName = Field.BindingSource.Fields[PartBinField].RelatedFieldsAndCurrentFieldArray[0];
			if (row.Field<string>(columnName).Trim().Length != 0)
			{
				if (RequiredExpression.Length != 0 && Field.Table.EvaluateScriptExpressionBool(RequiredExpression, database, row))
				{
					flag = true;
				}
				if (!flag && RequiredExpressionUser.Length != 0 && Field.Table.EvaluateScriptExpressionBool(RequiredExpressionUser, database, row))
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	public virtual void LoadComplete(FieldCollection fields, bool add)
	{
		if (add)
		{
			if (PartBinField.Length != 0)
			{
				fields[PartBinField].ValueChanged -= PartBin_ValueChanged;
				fields[PartBinField].ValueChanged += PartBin_ValueChanged;
			}
			Field.BindingSource.CurrentChanged -= BindingSource_CurrentChanged;
			Field.BindingSource.CurrentChanged += BindingSource_CurrentChanged;
			Field.Valid -= Field_Valid;
			Field.Valid += Field_Valid;
		}
		else
		{
			if (PartBinField.Length != 0)
			{
				Field.BindingSource.Fields[PartBinField].ValueChanged -= PartBin_ValueChanged;
			}
			Field.BindingSource.CurrentChanged -= BindingSource_CurrentChanged;
			Field.Valid -= Field_Valid;
		}
	}

	private void Field_Valid(object sender, ValidEventArgs e)
	{
		Validate(e.Database, e.Row, e.ValidationInfo);
	}

	public virtual void Validate(M1Database database, DataRow row, ValidationInfo errorInfo)
	{
	}

	private void evaluateRequired(M1Database database, DataRow row)
	{
		bool flag = false;
		if (row != null)
		{
			flag = IsRequired(database, row);
		}
		if (flag != RequiredResolved)
		{
			RequiredResolved = flag;
			OnRequiredChanged(EventArgs.Empty);
		}
	}

	protected virtual void OnRequiredChanged(EventArgs e)
	{
		this.RequiredChanged?.Invoke(this, e);
	}

	protected virtual void PartBin_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			evaluateRequired(e.Database, e.Row);
		}
	}

	protected virtual void BindingSource_CurrentChanged(object sender, EventArgs e)
	{
		DataRow currentAsDataRow = Field.BindingSource.CurrentAsDataRow;
		M1Database currentDatabase = Field.BindingSource.CurrentDatabase;
		evaluateRequired(currentDatabase, currentAsDataRow);
	}
}
