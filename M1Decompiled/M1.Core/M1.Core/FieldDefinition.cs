using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using M1.Core.Script;
using M1.Extensions;

namespace M1.Core;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class FieldDefinition : IDisposable, IProcessCodeBindings
{
	public enum DefaultToPreviousUserEnum : byte
	{
		None,
		False,
		True
	}

	public enum CalculationExpressionTypeEnum : byte
	{
		Standard,
		RunningTotal,
		Single
	}

	public enum BoundParentFieldTypeEnum : byte
	{
		None,
		FromParent,
		ToParent,
		ToForeignParent
	}

	public class ColumnErrorChangedEventArgs : EventArgs
	{
		public M1BindingSource BindingSource;

		public DataRow Row;

		public FieldDefinition Field;

		public string ErrorText = string.Empty;

		public int ErrorNumber;
	}

	[ComVisible(true)]
	public class FieldValueChangedEventArgs : DbAndRowEventArgs
	{
		public bool IsCurrentRow;

		public string FocusField;

		public object PreviousValue;

		public object Cancel;

		public FieldValueChangedEventArgs(M1Database database, DataRow row, bool isCurrentRow, object previousValue, SqlTransaction transaction)
			: base(database, row, transaction)
		{
			IsCurrentRow = isCurrentRow;
			PreviousValue = previousValue;
			FocusField = string.Empty;
			Cancel = false;
		}

		public bool IsCancelled()
		{
			return !M1Util.IsNullOrEmpty(Cancel);
		}
	}

	public class ButtonCodeEventArgs : EventArgs
	{
		public object Forms;

		public ButtonCodeEventArgs(object forms)
		{
			Forms = forms;
		}
	}

	private List<FieldExtension> _FieldExtensions;

	public List<OpenWithDefinition> FieldActions;

	private bool _AllowEditing = true;

	private string _AppExtensionID = string.Empty;

	protected Guid? _UniqueID;

	private bool _VirtualField;

	private string _FieldName = string.Empty;

	[Browsable(false)]
	[DefaultValue("")]
	public string FieldNameFormatted = string.Empty;

	private FieldTypeEnum _FieldType;

	private bool _AllowNulls;

	private string _TableName = string.Empty;

	private string _Caption = string.Empty;

	private ReferencedFieldsList CaptionExpressionReferencedFields = new ReferencedFieldsList();

	private string _CaptionExpression = string.Empty;

	private string _CaptionExpressionUser = string.Empty;

	private string _SaveAsExpression = string.Empty;

	private string _SaveAsExpressionUser = string.Empty;

	private string _FieldGroup = string.Empty;

	private string _FieldGroupParameters = string.Empty;

	private short _Sequence;

	private short _SequenceUser;

	private string _RelatedFields = string.Empty;

	[Browsable(false)]
	public string[] RelatedFieldsArray = new string[0];

	private string _RelatedFieldsAndCurrentField = string.Empty;

	[Browsable(false)]
	public string[] RelatedFieldsAndCurrentFieldArray = new string[0];

	private string _RelatedTable = string.Empty;

	public string RelatedTableCaption = string.Empty;

	public string RelatedTableKeyFields = string.Empty;

	public string[] RelatedTableKeyFieldsArray;

	public string RelatedTableQuickSearchFields;

	public bool RelatedTableLastKeyCanBeEmpty;

	public string RelatedTableModule = string.Empty;

	public string RelatedTableCurrencyModeLocationRelatedFields = string.Empty;

	public string RelatedTableCurrencyModeLocationField = string.Empty;

	public string RelatedTableCurrencyModeLocationAndRelatedFields = string.Empty;

	public string[] RelatedTableCurrencyModeLocationAndRelatedFieldsArray;

	public string RelatedTableCurrencyRateIdField = string.Empty;

	public string RelatedTableCurrencyCustomRateField = string.Empty;

	public string RelatedTableCurrencyExchangeRateField = string.Empty;

	public string RelatedTableDocumentDateField = string.Empty;

	private bool _RelatedTableRequiredForeignRelation;

	private string _RelatedTableForeignFilter = string.Empty;

	private string _RelatedTableSearchGridID = string.Empty;

	private string _RelatedTableReturnField = string.Empty;

	[Browsable(false)]
	public ReferencedFieldsList RelatedTableFilterReferencedFields = new ReferencedFieldsList();

	private string _RelatedTableFilter = string.Empty;

	private string _RelatedTableDescriptionField = string.Empty;

	private string _RelatedTableOrderByField = string.Empty;

	private bool _RelatedTableShowMemos;

	private string _RelatedTableMemoDescription = string.Empty;

	private string _RelatedTableUniqueIDField = string.Empty;

	private string _RequiredExpression = string.Empty;

	[Browsable(false)]
	public ReferencedFieldsList RequiredExpressionReferencedFields = new ReferencedFieldsList();

	private string _RequiredExpressionUser = string.Empty;

	[Browsable(false)]
	protected ReferencedFieldsList VisibleExpressionReferencedFields = new ReferencedFieldsList();

	private string _VisibleExpression = string.Empty;

	private string _VisibleExpressionUser = string.Empty;

	private string _DefaultExpression = string.Empty;

	private string _DefaultExpressionUser = string.Empty;

	protected string DefaultExpressionUserSetting;

	private bool _DefaultToPrevious;

	private DefaultToPreviousUserEnum _DefaultToPreviousUser;

	private string _CalculationExpression = string.Empty;

	private CalculationExpressionTypeEnum _CalculationExpressionType;

	[Browsable(false)]
	public ReferencedFieldsList BoundParentFieldExpressionReferencedFields = new ReferencedFieldsList();

	[Browsable(false)]
	public ReferencedFieldsList CalculationExpressionReferencedFields = new ReferencedFieldsList();

	private string _ReadOnlyExpression = string.Empty;

	[Browsable(false)]
	public ReferencedFieldsList ReadOnlyExpressionReferencedFields = new ReferencedFieldsList();

	[Browsable(false)]
	public ReferencedFieldsList ReadOnlyExpressionRelatedTableReferencedFields = new ReferencedFieldsList();

	private string _ReadOnlyExpressionUser = string.Empty;

	public bool IsUpdatedFromChildBoundField;

	[Browsable(false)]
	public ReferencedFieldsList ValidCodeReferencedFields = new ReferencedFieldsList();

	[Browsable(false)]
	public ReferencedFieldsList ForeignKeyValidCodeReferencedFields = new ReferencedFieldsList();

	private string _RelatedTableForeignKeyRequiredExpression = string.Empty;

	private string _RelatedTableForeignKeyRequiredExpressionUser = string.Empty;

	protected bool _Custom;

	public bool CustomField;

	private string _Module = string.Empty;

	private string _MapFromPart = string.Empty;

	private bool _AlwaysHidden;

	private int _FieldLength;

	private byte _FieldDecimals;

	private bool _ShowAsDropdown;

	private string _ValueList = string.Empty;

	private string _Format = string.Empty;

	private bool _AllowLowerCaseOrNegative = true;

	private string _BoundParentField = string.Empty;

	public string BoundParentRelatedFields = string.Empty;

	public string BoundParentRelatedAndCurrentFields = string.Empty;

	public string[] BoundParentRelatedAndCurrentFieldsArray;

	private BoundParentFieldTypeEnum _BoundParentFieldType;

	private string _BoundParentFieldProxy = string.Empty;

	private string _BoundParentFieldExpression = string.Empty;

	public bool IsPartOfKey;

	public bool IsEditableKey;

	private M1CurrencyStyle _CurrencyType;

	private string _CurrencyRelatedField = string.Empty;

	private bool _CurrencyUpdateRelatedField;

	public AppContext Context;

	public M1User User;

	public M1Database Database;

	public M1DataDictionary DataDictionary;

	public M1DatabaseFieldSecurityCollection Databases = new M1DatabaseFieldSecurityCollection();

	private ValidationInfo errorList;

	private TableDefinition _Table;

	private bool needToProcessRequired;

	private bool needToProcessReadOnly;

	private bool _IsValid = true;

	private string _ErrorText = string.Empty;

	protected ForeignUpdateHandler foreignUpdateHandler;

	private RelatedTableField _RelatedFieldsWrapper;

	private M1BindingSource _BindingSource;

	[Browsable(false)]
	public bool ForceToValidate { get; set; }

	[Browsable(false)]
	public virtual List<FieldExtension> FieldExtensions
	{
		get
		{
			return _FieldExtensions;
		}
		set
		{
			_FieldExtensions = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public bool AllowEditing
	{
		get
		{
			return _AllowEditing;
		}
		set
		{
			_AllowEditing = value;
			if (!AllowEditing)
			{
				ReadOnlyResolved = true;
			}
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
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

	[Browsable(false)]
	[Category("Behavior")]
	[DefaultValue(null)]
	[Description("Indicates the unique id of this field.")]
	[ReadOnly(true)]
	public Guid? UniqueID => _UniqueID;

	[Browsable(false)]
	[DefaultValue(false)]
	public bool VirtualField
	{
		get
		{
			return _VirtualField;
		}
		set
		{
			_VirtualField = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	[ParenthesizePropertyName(true)]
	public virtual string FieldName
	{
		get
		{
			return _FieldName;
		}
		set
		{
			_FieldName = value;
			RelatedFieldsAndCurrentField = ((_RelatedFields.Length == 0) ? string.Empty : (_RelatedFields + ",")) + FieldName;
		}
	}

	[Browsable(true)]
	[DefaultValue(FieldTypeEnum.None)]
	[Category("Definition")]
	[ParenthesizePropertyName(true)]
	[Description("Indicates the type of this field as it will be stored in the database.")]
	public virtual FieldTypeEnum FieldType
	{
		get
		{
			return _FieldType;
		}
		set
		{
			_FieldType = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Definition")]
	[ParenthesizePropertyName(true)]
	[Description("Indicates if this field will allow null values. Memo fields that store null for an empty value take up less space and process queries faster.")]
	public bool AllowNulls
	{
		get
		{
			return _AllowNulls;
		}
		set
		{
			_AllowNulls = value;
		}
	}

	[Browsable(false)]
	public virtual string TableName
	{
		get
		{
			return _TableName;
		}
		set
		{
			_TableName = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[ParenthesizePropertyName(true)]
	[Description("Indicates a short description for this field that will be shown in grids and entry screens. For standard fields the CustomCaption property must be set to true to enable this property.")]
	public string Caption
	{
		get
		{
			return _Caption;
		}
		set
		{
			if (!_Caption.Equals(value))
			{
				_Caption = value;
				OnCaptionChanged(EventArgs.Empty);
			}
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("VBScript expression that is used to determine the caption of this field. This allows you to change a caption based on another value in the same table by accessing the Fields() collection.")]
	public virtual string CaptionExpression
	{
		get
		{
			return _CaptionExpression;
		}
		set
		{
			_CaptionExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("VBScript expression that is used to determine the caption of this field. This allows you to change a caption based on another value in the same table by accessing the Fields() collection. This overrides the CaptionExpression property.")]
	public virtual string CaptionExpressionUser
	{
		get
		{
			return _CaptionExpressionUser;
		}
		set
		{
			_CaptionExpressionUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("VBScript expression that is used to determine the value of this field after a save as is run. This allows you to change the value of this field after a save as is run by accessing the Fields() collection.")]
	public virtual string SaveAsExpression
	{
		get
		{
			return _SaveAsExpression;
		}
		set
		{
			_SaveAsExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("VBScript expression that is used to determine the value of this field after a save as is run. This allows you to change the value of this field after a save as is run by accessing the Fields() collection. This overrides the SaveAsExpression property.")]
	public virtual string SaveAsExpressionUser
	{
		get
		{
			return _SaveAsExpressionUser;
		}
		set
		{
			_SaveAsExpressionUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates a short category name for this field. This is used for address, time, document management, gl account, and quantity fields, and is a key into the DDFieldGroups table.")]
	public virtual string FieldGroup
	{
		get
		{
			return _FieldGroup;
		}
		set
		{
			_FieldGroup = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Defines optional parameters for the associated FieldGroup.")]
	public virtual string FieldGroupParameters
	{
		get
		{
			return _FieldGroupParameters;
		}
		set
		{
			_FieldGroupParameters = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Definition")]
	[Description("Indicates the order of the fields in the database when creating this table.")]
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

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Definition")]
	[Description("Indicates the order of the fields in the database when creating this table. This overrides the Sequence property if not zero.")]
	public virtual short SequenceUser
	{
		get
		{
			return _SequenceUser;
		}
		set
		{
			_SequenceUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("Indicates the fields, in addition to the current field, that make up a key into a foreign table.")]
	public virtual string RelatedFields
	{
		get
		{
			return _RelatedFields;
		}
		set
		{
			_RelatedFields = value;
			RelatedFieldsArray = _RelatedFields.Split(',');
			RelatedFieldsAndCurrentField = ((_RelatedFields.Length == 0) ? string.Empty : (_RelatedFields + ",")) + FieldName;
		}
	}

	[Browsable(false)]
	public string RelatedFieldsAndCurrentField
	{
		get
		{
			return _RelatedFieldsAndCurrentField;
		}
		set
		{
			_RelatedFieldsAndCurrentField = value;
			RelatedFieldsAndCurrentFieldArray = _RelatedFieldsAndCurrentField.Split(',');
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("Indicates the foreign table that relates to this field. Any value entered into this field must exist in the foreign table.")]
	public virtual string RelatedTable
	{
		get
		{
			return _RelatedTable;
		}
		set
		{
			_RelatedTable = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("RelatedTable")]
	[Description("Indicates if the value in the current field must exist in the related table.")]
	public virtual bool RelatedTableRequiredForeignRelation
	{
		get
		{
			return _RelatedTableRequiredForeignRelation;
		}
		set
		{
			_RelatedTableRequiredForeignRelation = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("Indicates a Sql filter expression that will be added to the query used to fill ComboBoxes that are bound to this field.")]
	public virtual string RelatedTableForeignFilter
	{
		get
		{
			return _RelatedTableForeignFilter;
		}
		set
		{
			_RelatedTableForeignFilter = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("Indicates a Grid Definition ID to be used for filling a search when prompting for a value for this field.")]
	public virtual string RelatedTableSearchGridID
	{
		get
		{
			return _RelatedTableSearchGridID;
		}
		set
		{
			_RelatedTableSearchGridID = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("Indicates the field name from the foreign table that is used to fill this field. This will generally be the primary key of the foreign table. This is used with the RelatedTableDescriptionField to fill ComboBoxes that are bound to this field.")]
	public virtual string RelatedTableReturnField
	{
		get
		{
			return _RelatedTableReturnField;
		}
		set
		{
			_RelatedTableReturnField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("Indicates a Sql filter clause that will be added to searches on controls bound to this field. This is a vbscript expression that will be evaluated before it is added to the query (put in double quotes).")]
	public virtual string RelatedTableFilter
	{
		get
		{
			return _RelatedTableFilter;
		}
		set
		{
			_RelatedTableFilter = value;
			RelatedTableFilterReferencedFields.Clear();
			if (_RelatedTableFilter != null && _RelatedTableFilter.Length != 0)
			{
				RelatedTableFilterReferencedFields.ParseCodeForFields(_RelatedTableFilter);
			}
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("Indicates the field name(s) from the foreign table that will show in ComboBoxes that are bound to this field. This is used with the RelatedTableReturnField for filling ComboBoxes that are bound to this field.")]
	public virtual string RelatedTableDescriptionField
	{
		get
		{
			return _RelatedTableDescriptionField;
		}
		set
		{
			_RelatedTableDescriptionField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("Indicates the field name from the foreign table that is used to order the results that are shown in ComboBoxes that are bound to this field.")]
	public virtual string RelatedTableOrderByField
	{
		get
		{
			return _RelatedTableOrderByField;
		}
		set
		{
			_RelatedTableOrderByField = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[Category("RelatedTable")]
	[Description("Indicates if show memo alerts for the foreign table for this field has been turned on.")]
	public virtual bool RelatedTableShowMemos
	{
		get
		{
			return _RelatedTableShowMemos;
		}
		set
		{
			_RelatedTableShowMemos = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("The memo description for the foreign table for this field.")]
	public virtual string RelatedTableMemoDescription
	{
		get
		{
			return _RelatedTableMemoDescription;
		}
		set
		{
			_RelatedTableMemoDescription = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	[Category("RelatedTable")]
	[Description("Indicates the unique id field name from the foreign table. This is used if the ShowMemos flag has been turned on.")]
	public virtual string RelatedTableUniqueIDField
	{
		get
		{
			return _RelatedTableUniqueIDField;
		}
		set
		{
			_RelatedTableUniqueIDField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that returns true if this field is required. This allows you to make a field required based on another value in the same table by accessing the Fields() collection. This will be evaluated and Or'd with the RequiredExpressionUser to determine a value.")]
	public virtual string RequiredExpression
	{
		get
		{
			return _RequiredExpression;
		}
		set
		{
			_RequiredExpression = value;
			needToProcessRequired = true;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that returns true if this field is required. This allows you to make a field required based on another value in the same table by accessing the Fields() collection. This will be evaluated and Or'd with the RequiredExpression to determine a value.")]
	public virtual string RequiredExpressionUser
	{
		get
		{
			return _RequiredExpressionUser;
		}
		set
		{
			_RequiredExpressionUser = value;
			needToProcessRequired = true;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that returns false if this field should be hidden. This allows you to make a field hidden based on another value in the same table by accessing the Fields() collection, on global properties or functions accessed through the App object. This will be evaluated and And'd with the VisibleExpressionUser to determine a value.")]
	[Editor("M1.Forms.Design.Editors.CodePropertyEditor, M1.Forms.Design", typeof(UITypeEditor))]
	public virtual string VisibleExpression
	{
		get
		{
			return _VisibleExpression;
		}
		set
		{
			_VisibleExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that returns false if this field should be hidden. This allows you to make a field hidden based on another value in the same table by accessing the Fields() collection, on global properties or functions accessed through the App object. This will be evaluated and And'd with the VisibleExpression to determine a value.")]
	[Editor("M1.Forms.Design.Editors.CodePropertyEditor, M1.Forms.Design", typeof(UITypeEditor))]
	public virtual string VisibleExpressionUser
	{
		get
		{
			return _VisibleExpressionUser;
		}
		set
		{
			_VisibleExpressionUser = value;
		}
	}

	[Browsable(false)]
	public bool RequiredResolved { get; private set; }

	[Browsable(false)]
	public bool NoAccessResolved { get; private set; }

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that is used to set the initial value of this field when a record is created.")]
	public virtual string DefaultExpression
	{
		get
		{
			return _DefaultExpression;
		}
		set
		{
			_DefaultExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that is used to set the initial value of this field when a record is created. Setting this will override the DefaultValueExpression.")]
	public virtual string DefaultExpressionUser
	{
		get
		{
			return _DefaultExpressionUser;
		}
		set
		{
			_DefaultExpressionUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if this field should be set to the value that was last entered in the current editing session in the entry screen when creating a new record.")]
	public bool DefaultToPrevious
	{
		get
		{
			return _DefaultToPrevious;
		}
		set
		{
			_DefaultToPrevious = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(DefaultToPreviousUserEnum.None)]
	[Category("Behavior")]
	[Description("Indicates if this field should be set to the value that was last entered in the current editing session in the entry screen when creating a new record. If set, this overrides the DefaultToPrevious property, which will be read only on built-in objects.")]
	public DefaultToPreviousUserEnum DefaultToPreviousUser
	{
		get
		{
			return _DefaultToPreviousUser;
		}
		set
		{
			_DefaultToPreviousUser = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression used to calculate the value of this field. Whenever any of the fields referenced in the expression changes, this field will be refreshed by reevaluating the calculation formula. This means that you do not have to add the same code to update this field in the change events of all the related fields, and allows you to easily see how a field value is calculated without having to look through all the change events.")]
	public virtual string CalculationExpression
	{
		get
		{
			return _CalculationExpression;
		}
		set
		{
			_CalculationExpression = value;
			CalculationExpressionReferencedFields.Clear();
			if (_CalculationExpression != null && _CalculationExpression.Length != 0)
			{
				CalculationExpressionReferencedFields.ParseCodeForFields(_CalculationExpression);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(CalculationExpressionTypeEnum.Standard)]
	[Category("Expression")]
	[Description("Allows aggregate functions to be used with CalculationExpression.")]
	public virtual CalculationExpressionTypeEnum CalculationExpressionType
	{
		get
		{
			return _CalculationExpressionType;
		}
		set
		{
			_CalculationExpressionType = value;
			if (BindingSource != null)
			{
				BindingSource.RowActivated -= BindingSource_RowActivated_UpdateRunningTotal;
				BindingSource.RowActivated -= BindingSource_RowActivated_UpdateSingle;
				if (_CalculationExpressionType == CalculationExpressionTypeEnum.RunningTotal)
				{
					BindingSource.RowActivated += BindingSource_RowActivated_UpdateRunningTotal;
				}
				if (_CalculationExpressionType == CalculationExpressionTypeEnum.Single)
				{
					BindingSource.RowActivated += BindingSource_RowActivated_UpdateSingle;
				}
			}
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that is used to determine if this field should be readonly. This allows you to make a field readonly based on another value in the same table by accessing the Fields() collection. This will be evaluated and Or'd with the ReadOnlyExpressionUser to determine a value. If the RelatedTableGetAdoRecord method references fields from the parent table, and the parent table BindingSource is available, it will reevaluate this expression on change of those fields.")]
	public virtual string ReadOnlyExpression
	{
		get
		{
			return _ReadOnlyExpression;
		}
		set
		{
			_ReadOnlyExpression = value;
			needToProcessReadOnly = true;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("VBScript expression that is used to determine if this field should be readonly. This allows you to make a field readonly based on another value in the same table by accessing the Fields() collection. This will be evaluated and Or'd with the ReadOnlyExpression to determine a value. If the RelatedTableGetAdoRecord method references fields from the parent table, and the parent table BindingSource is available, it will reevaluate this expression on change of those fields.")]
	public virtual string ReadOnlyExpressionUser
	{
		get
		{
			return _ReadOnlyExpressionUser;
		}
		set
		{
			_ReadOnlyExpressionUser = value;
			needToProcessReadOnly = true;
		}
	}

	[Browsable(false)]
	public bool ReadOnlyResolved { get; private set; }

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("If this VBScript expression returns true or is empty, the foreign key is required. This allows you to turn off the requirement to have a value exist in a foreign table. This is used for requiring parts to exist. This value is evaluated for each row, so can be turned off if a value on the current row changes (such as UseDefaultBin on the PartMaterials table). The user foreign key required expression will override the standard expression if it is set.")]
	public virtual string RelatedTableForeignKeyRequiredExpression
	{
		get
		{
			return _RelatedTableForeignKeyRequiredExpression;
		}
		set
		{
			_RelatedTableForeignKeyRequiredExpression = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Expression")]
	[Description("If this VBScript expression returns true or is empty, the foreign key is required. This allows you to turn off the requirement to have a value exist in a foreign table. This is used for requiring parts to exist. This value is evaluated for each row, so can be turned off if a value on the current row changes (such as UseDefaultBin on the PartMaterials table). The user foreign key required expression will override the standard expression if it is set.")]
	public virtual string RelatedTableForeignKeyRequiredExpressionUser
	{
		get
		{
			return _RelatedTableForeignKeyRequiredExpressionUser;
		}
		set
		{
			_RelatedTableForeignKeyRequiredExpressionUser = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool Custom
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
	[Category("Behavior")]
	[Description("Indicates the module required for using this field. If this module is not available, this field will not be shown.")]
	public string Module
	{
		get
		{
			return _Module;
		}
		set
		{
			_Module = value;
		}
	}

	[Browsable(false)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("This is used by the data viewer.")]
	public string MapFromPart
	{
		get
		{
			return _MapFromPart;
		}
		set
		{
			_MapFromPart = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if this field should always be hidden in grids. This is generally for fields that contain data that do not have an easily readable format.")]
	public bool AlwaysHidden
	{
		get
		{
			return _AlwaysHidden;
		}
		set
		{
			_AlwaysHidden = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Definition")]
	[ParenthesizePropertyName(true)]
	[Description("The number of characters to allow for this field. If the field is a numeric type, this will include the decimal point and the number of decimals, if any.")]
	public virtual int FieldLength
	{
		get
		{
			return _FieldLength;
		}
		set
		{
			_FieldLength = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Category("Definition")]
	[ParenthesizePropertyName(true)]
	[Description("The number of digits after the decimal point allowed for this field. Only available when the field type is numeric.")]
	public virtual byte FieldDecimals
	{
		get
		{
			return _FieldDecimals;
		}
		set
		{
			_FieldDecimals = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates if this field should be shown as a ComboBox in grids. This should only be set to True for ComboBoxes that will have fewer than 75 items (based on the related table query including the RelatedTableForeignFilter condition).")]
	public bool ShowAsDropdown
	{
		get
		{
			return _ShowAsDropdown;
		}
		set
		{
			_ShowAsDropdown = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("The list of values that is used to fill a ComboBox when it is bound to this field. The values should be in the format of value,text (1,Job or \"C\",Closed) with a carriage return between each set of data.")]
	public virtual string ValueList
	{
		get
		{
			return _ValueList;
		}
		set
		{
			_ValueList = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Specifies an input mask to be used by controls that are bound to this field. This is currently only used for GL Account fields.")]
	public string Format
	{
		get
		{
			return _Format;
		}
		set
		{
			_Format = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates if this field allows input of lower case characters (if a string type) or negative numbers (if a number type).")]
	public bool AllowLowerCaseOrNegative
	{
		get
		{
			return _AllowLowerCaseOrNegative;
		}
		set
		{
			_AllowLowerCaseOrNegative = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates the name of a field from the parent table or a foreign key linked table. When set, the BoundParentFieldType controls the direction of the binding. This allows you to either push or pull the value of this field based on a field in another table.")]
	public virtual string BoundParentField
	{
		get
		{
			return _BoundParentField;
		}
		set
		{
			_BoundParentField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(BoundParentFieldTypeEnum.None)]
	[Category("Behavior")]
	[Description("Indicates the binding direction to be used with the associated BoundParentField. FromParent will make the current field value always match the parent field (and will also make this field readonly). ToParent will summarize the current field's values for all child rows to the specified parent field. When the record is deleted, the amount is removed. This is only available on numeric fields. ToParentForeign can be used when the current field is the key into a foreign table. The bound parent field is assumed to be a boolean that will be set to true when this field's value is not empty, and will be set to false when the field's value is cleared or the row is deleted, and there are no other rows in the current table that reference the foreign record. If the current field is not the key into the foreign table, then you must specify the BoundParentProxyField, which is the field from the current table that links to the foreign table. This would generally be used for quantity (numeric) fields that you want to be added/subtracted to the bound foreign parent field.")]
	public virtual BoundParentFieldTypeEnum BoundParentFieldType
	{
		get
		{
			return _BoundParentFieldType;
		}
		set
		{
			_BoundParentFieldType = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("Indicates the field in this table that is a key into a foreign table. This can be used when the BoundParentFieldType is set to ToParentForeign and the current field is not a key into a foreign table.")]
	public virtual string BoundParentFieldProxy
	{
		get
		{
			return _BoundParentFieldProxy;
		}
		set
		{
			_BoundParentFieldProxy = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Behavior")]
	[Description("VBScript expression used to calculate the value of this field when the BoundParentFieldType is set to ToParent. This allows you to modify the value of the current field before it is added to the parent field.")]
	public virtual string BoundParentFieldExpression
	{
		get
		{
			return _BoundParentFieldExpression;
		}
		set
		{
			_BoundParentFieldExpression = value;
			BoundParentFieldExpressionReferencedFields.Clear();
			if (_BoundParentFieldExpression != null && _BoundParentFieldExpression.Length != 0)
			{
				BoundParentFieldExpressionReferencedFields.ParseCodeForFields(_BoundParentFieldExpression);
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(M1CurrencyStyle.None)]
	[Category("Currency")]
	[Description("Indicates if this field contains base or foreign currency values.")]
	public virtual M1CurrencyStyle CurrencyType
	{
		get
		{
			return _CurrencyType;
		}
		set
		{
			_CurrencyType = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Currency")]
	[Description("Indicates the currency field that contains the converted value of the current field. If this field is a base currency field, the related field should be the foreign currency value field. The related field will be updated automatically using the current exchange rate if CurrencyUpdateRelatedField is true.")]
	public virtual string CurrencyRelatedField
	{
		get
		{
			return _CurrencyRelatedField;
		}
		set
		{
			_CurrencyRelatedField = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Currency")]
	[Description("Indicates if the CurencyRelatedField should be updated automatically using the current exchange rate. If set, the related field will be recalculated when the current field changes or the exchange rate changes. Note: The currency properties on the table definition must be set for this to be calculated. This will not function if there is a calculation expression associated with the currency related field.")]
	public virtual bool CurrencyUpdateRelatedField
	{
		get
		{
			return _CurrencyUpdateRelatedField;
		}
		set
		{
			_CurrencyUpdateRelatedField = value;
		}
	}

	[Browsable(false)]
	public TableDefinition Table
	{
		get
		{
			return _Table;
		}
		set
		{
			if (_Table != null)
			{
				_Table.NoAccessChanged -= _Table_NoAccessChanged;
				_Table.ReadOnlyChanged -= _Table_ReadOnlyChanged;
				_Table.ExchangeRateChanged -= Table_ExchangeRateChanged;
			}
			_Table = value;
			if (_Table == null)
			{
				return;
			}
			_Table.NoAccessChanged += _Table_NoAccessChanged;
			_Table.ReadOnlyChanged += _Table_ReadOnlyChanged;
			string[] keyFieldsArray = Table.KeyFieldsArray;
			for (int i = 0; i < keyFieldsArray.Length; i++)
			{
				if (!keyFieldsArray[i].Equals(FieldName, StringComparison.CurrentCultureIgnoreCase))
				{
					continue;
				}
				IsPartOfKey = true;
				int num = Table.KeyFieldsArray.Length - ((Table.KeysAtThisLevel == 0) ? 1 : Table.KeysAtThisLevel);
				for (int j = 0; j < Table.KeyFieldsArray.Length; j++)
				{
					if (Table.KeyFieldsArray[j].Equals(FieldName, StringComparison.CurrentCultureIgnoreCase))
					{
						if (j >= num)
						{
							IsEditableKey = true;
						}
						break;
					}
				}
				break;
			}
		}
	}

	[Browsable(false)]
	public bool IsValid
	{
		get
		{
			return _IsValid;
		}
		private set
		{
			_IsValid = value;
		}
	}

	[Browsable(false)]
	public string ErrorText
	{
		get
		{
			return _ErrorText;
		}
		set
		{
			if (!_ErrorText.Equals(value))
			{
				_ErrorText = value;
				OnErrorTextChanged(EventArgs.Empty);
			}
		}
	}

	[Browsable(false)]
	public M1BindingSource BindingSource
	{
		get
		{
			return _BindingSource;
		}
		set
		{
			if (_BindingSource != null)
			{
				_BindingSource.QueryDatabase -= BindingSource_RowActivated_UpdateRunningTotal;
				_BindingSource.QueryDatabase -= BindingSource_RowActivated_UpdateSingle;
				_BindingSource.EditCancelled -= _BindingSource_EditCancelled;
				_BindingSource.CurrentChanged -= _BindingSource_CurrentChanged;
				_BindingSource.RowUpdateAddBefore -= BindingSource_RowUpdateSave;
				_BindingSource.RowUpdateSaveBefore -= BindingSource_RowUpdateSave;
				_BindingSource.RowUpdateDeleteBefore -= BindingSource_RowUpdateDelete;
			}
			_BindingSource = value;
			if (errorList != null)
			{
				errorList.BindingSource = _BindingSource;
			}
			if (_BindingSource != null)
			{
				_BindingSource.CurrentChanged += _BindingSource_CurrentChanged;
				if (_CalculationExpressionType == CalculationExpressionTypeEnum.RunningTotal)
				{
					_BindingSource.QueryDatabase += BindingSource_RowActivated_UpdateRunningTotal;
					_BindingSource.EditCancelled += _BindingSource_EditCancelled;
				}
				if (_CalculationExpressionType == CalculationExpressionTypeEnum.Single)
				{
					_BindingSource.QueryDatabase += BindingSource_RowActivated_UpdateSingle;
				}
			}
		}
	}

	[DispId(0)]
	[Browsable(false)]
	public object Value
	{
		get
		{
			DataRow dataRow = CurrentDataRow();
			if (dataRow == null)
			{
				return DBNull.Value;
			}
			if (dataRow.RowState == DataRowState.Deleted)
			{
				return getOriginalValueForRow(dataRow);
			}
			return getValueForRow(dataRow);
		}
		set
		{
			if (value == null || value == DBNull.Value)
			{
				if (IsFieldTypeAString(FieldType))
				{
					CurrentDataRow()[FieldName] = string.Empty;
				}
				else if (IsFieldTypeANumber(FieldType))
				{
					CurrentDataRow()[FieldName] = 0;
				}
				else if (FieldType != FieldTypeEnum.Bit)
				{
					CurrentDataRow()[FieldName] = DBNull.Value;
				}
			}
			else if (FieldType == FieldTypeEnum.Bit)
			{
				if (Convert.ToBoolean(value))
				{
					CurrentDataRow()[FieldName] = 1;
				}
				else
				{
					CurrentDataRow()[FieldName] = 0;
				}
			}
			else
			{
				CurrentDataRow().SetField(FieldName, value);
			}
		}
	}

	[Browsable(false)]
	public object OriginalValue
	{
		get
		{
			DataRow dataRow = CurrentDataRow();
			if (dataRow == null)
			{
				return DBNull.Value;
			}
			if (dataRow.HasVersion(DataRowVersion.Original))
			{
				return getOriginalValueForRow(dataRow);
			}
			if (dataRow.RowState == DataRowState.Added)
			{
				return GetDefaultForFieldType();
			}
			if (dataRow[FieldName] == DBNull.Value)
			{
				return DBNull.Value;
			}
			if (IsFieldTypeAString(FieldType))
			{
				return dataRow.Field<string>(FieldName);
			}
			switch (FieldType)
			{
			case FieldTypeEnum.Bit:
				return dataRow.Field<bool>(FieldName);
			case FieldTypeEnum.Int:
				return dataRow.Field<int>(FieldName);
			case FieldTypeEnum.Numeric:
				if (FieldDecimals == 0)
				{
					return (int)dataRow.Field<decimal>(FieldName);
				}
				return (double)dataRow.Field<decimal>(FieldName);
			case FieldTypeEnum.Money:
				return (double)dataRow.Field<decimal>(FieldName);
			case FieldTypeEnum.Date:
			case FieldTypeEnum.DateTime:
				return dataRow.Field<DateTime>(FieldName);
			default:
				return dataRow[FieldName];
			}
		}
		set
		{
		}
	}

	[Browsable(false)]
	public int DefinedSize
	{
		get
		{
			return FieldLength;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			return FieldNameFormatted;
		}
		set
		{
		}
	}

	public event EventHandler CaptionChanged;

	public event EventHandler RequiredChanged;

	public event EventHandler ReadOnlyChanged;

	public event EventHandler NoAccessChanged;

	public event EventHandler ErrorTextChanged;

	[ProcessCodeBindings(true)]
	[Description("Allows you to write VBScript code that runs after the value that has been entered has been verified to see if it exists in the foreign table. All the objects that are available in the field-level valid event are available here (sender, e), but if there are errors in this event, the value will be changed back to its previous value. This event will fire whenever any fields that are referenced in the code using the Fields(\"fieldname\").Value syntax have been changed.")]
	public event EventHandler<ValidEventArgs> ForeignKeyValid;

	[ProcessCodeBindings(true)]
	[Description("Allows you to write VBScript code that will validate this field. This code has access to a sender object (which is the FieldDefinition reference for the current field), and an e object, which is the ValidationInfo object, which allows you to add errors, warnings or messages relating to the current field. This event will fire whenever any fields that are referenced in the code using the Fields(\"fieldname\").Value syntax have been changed.")]
	public event EventHandler<ValidEventArgs> Valid;

	public event EventHandler<ColumnErrorChangedEventArgs> ColumnErrorChanged;

	public event EventHandler<FieldValueChangedEventArgs> IsValidChanged;

	public event EventHandler<FieldValueChangedEventArgs> AfterValueChanged;

	[Description("VBScript code that runs when the field has been changed by user input. If e.Cancel is set to a non-empty string, the text will be shown to the user and the field value set back to it's previous value. This code has access to a sender object (which is the FieldDefinition reference for the current field), and an e object, which has a property for the PreviousValue.")]
	public event EventHandler<FieldValueChangedEventArgs> ValueChanged;

	public event EventHandler Flash;

	public event EventHandler<ForeignKeyInvalidEventArgs> ForeignKeyInvalid;

	public event EventHandler Disposed;

	protected virtual void OnCaptionChanged(EventArgs e)
	{
		this.CaptionChanged?.Invoke(this, e);
	}

	public void RunFormatter(M1Database database, DataRow row)
	{
		if (!string.IsNullOrWhiteSpace(FieldGroup) && database.Formatters.ContainsKey(FieldGroup) && database.Formatters[FieldGroup] is IFormatterOnUserChange)
		{
			((IFormatterOnUserChange)database.Formatters[FieldGroup]).OnUserChange(this, row);
		}
	}

	private void _BindingSource_EditCancelled(object sender, EventArgs e)
	{
		UpdateRunningTotal(BindingSource.Database);
	}

	private void BindingSource_RowActivated_UpdateRunningTotal(object sender, M1BindingSource.QueryDatabaseEventArgs e)
	{
		UpdateRunningTotal(e.Database);
	}

	private void BindingSource_RowActivated_UpdateSingle(object sender, M1BindingSource.QueryDatabaseEventArgs e)
	{
		UpdateSingle(e.Database);
	}

	public bool IsForeignKey()
	{
		if (IsPartOfKey)
		{
			if (RelatedFieldsAndCurrentField.Equals(Table.KeyFields, StringComparison.CurrentCultureIgnoreCase))
			{
				return false;
			}
			return !string.IsNullOrWhiteSpace(RelatedTable);
		}
		return !string.IsNullOrWhiteSpace(RelatedTable);
	}

	public FieldDefinition(AppContext context, M1User user, M1DataDictionary m1DataDictionary, M1Database database)
	{
		Context = context;
		User = user;
		DataDictionary = m1DataDictionary;
		Database = database;
		errorList = new ValidationInfo(null, null, null, this);
	}

	private void _Table_NoAccessChanged(object sender, DbAndRowEventArgs e)
	{
		EvaluateNoAccess(e.Database, e.Row);
	}

	private void _Table_ReadOnlyChanged(object sender, DbAndRowEventArgs e)
	{
		EvaluateReadOnlyExpression(e.Database, e.Row);
	}

	public bool IsMatchingType(DataColumn column)
	{
		if (column.DataType == typeof(string) && IsFieldTypeAString(FieldType))
		{
			return true;
		}
		if (column.DataType == typeof(double) && (FieldType == FieldTypeEnum.Money || FieldType == FieldTypeEnum.Numeric))
		{
			return true;
		}
		if (column.DataType == typeof(DateTime) && (FieldType == FieldTypeEnum.Date || FieldType == FieldTypeEnum.DateTime))
		{
			return true;
		}
		if (column.DataType == typeof(Guid) && FieldType == FieldTypeEnum.UniqueIdentifier)
		{
			return true;
		}
		if (column.DataType == typeof(bool) && FieldType == FieldTypeEnum.Bit)
		{
			return true;
		}
		if (column.DataType == typeof(int) && (FieldType == FieldTypeEnum.Int || (FieldType == FieldTypeEnum.Numeric && FieldDecimals == 0)))
		{
			return true;
		}
		if (column.DataType == typeof(decimal) && FieldType == FieldTypeEnum.Numeric)
		{
			return true;
		}
		return false;
	}

	public SqlDbType GetSqlDbType()
	{
		return GetSqlDbType(FieldType);
	}

	public static SqlDbType GetSqlDbType(Type type)
	{
		if (type == typeof(long))
		{
			return SqlDbType.BigInt;
		}
		if (type == typeof(bool))
		{
			return SqlDbType.Bit;
		}
		if (type == typeof(DateTime))
		{
			return SqlDbType.DateTime;
		}
		if (type == typeof(int))
		{
			return SqlDbType.Int;
		}
		if (type == typeof(decimal))
		{
			return SqlDbType.Decimal;
		}
		if (type == typeof(string))
		{
			return SqlDbType.NVarChar;
		}
		if (type == typeof(short))
		{
			return SqlDbType.SmallInt;
		}
		if (type == typeof(byte))
		{
			return SqlDbType.TinyInt;
		}
		if (type == typeof(Guid))
		{
			return SqlDbType.UniqueIdentifier;
		}
		return SqlDbType.NVarChar;
	}

	public static SqlDbType GetSqlDbType(FieldTypeEnum fieldType)
	{
		return fieldType switch
		{
			FieldTypeEnum.BigInt => SqlDbType.BigInt, 
			FieldTypeEnum.Binary => SqlDbType.Binary, 
			FieldTypeEnum.Bit => SqlDbType.Bit, 
			FieldTypeEnum.Char => SqlDbType.Char, 
			FieldTypeEnum.Date => SqlDbType.DateTime, 
			FieldTypeEnum.DateTime => SqlDbType.DateTime, 
			FieldTypeEnum.Float => SqlDbType.Float, 
			FieldTypeEnum.Identity => SqlDbType.Int, 
			FieldTypeEnum.Image => SqlDbType.Image, 
			FieldTypeEnum.Int => SqlDbType.Int, 
			FieldTypeEnum.Money => SqlDbType.Money, 
			FieldTypeEnum.NChar => SqlDbType.NChar, 
			FieldTypeEnum.NText => SqlDbType.NText, 
			FieldTypeEnum.Numeric => SqlDbType.Decimal, 
			FieldTypeEnum.NVarchar => SqlDbType.NVarChar, 
			FieldTypeEnum.NVarchar_max => SqlDbType.NVarChar, 
			FieldTypeEnum.Real => SqlDbType.Real, 
			FieldTypeEnum.SmallDateTime => SqlDbType.SmallDateTime, 
			FieldTypeEnum.SmallInt => SqlDbType.SmallInt, 
			FieldTypeEnum.SmallMoney => SqlDbType.SmallMoney, 
			FieldTypeEnum.Text => SqlDbType.Text, 
			FieldTypeEnum.TimeStamp => SqlDbType.Timestamp, 
			FieldTypeEnum.TinyInt => SqlDbType.TinyInt, 
			FieldTypeEnum.UniqueIdentifier => SqlDbType.UniqueIdentifier, 
			FieldTypeEnum.VarBinary => SqlDbType.VarBinary, 
			FieldTypeEnum.Varchar => SqlDbType.VarChar, 
			FieldTypeEnum.Varchar_max => SqlDbType.VarChar, 
			_ => SqlDbType.NVarChar, 
		};
	}

	public static FieldTypeEnum charToFieldType(string fieldType)
	{
		return fieldType.ToLower() switch
		{
			"bigint" => FieldTypeEnum.BigInt, 
			"binary" => FieldTypeEnum.Binary, 
			"bit" => FieldTypeEnum.Bit, 
			"char" => FieldTypeEnum.Char, 
			"date" => FieldTypeEnum.Date, 
			"datetime" => FieldTypeEnum.DateTime, 
			"float" => FieldTypeEnum.Float, 
			"identity" => FieldTypeEnum.Identity, 
			"image" => FieldTypeEnum.Image, 
			"int" => FieldTypeEnum.Int, 
			"money" => FieldTypeEnum.Money, 
			"nchar" => FieldTypeEnum.NChar, 
			"ntext" => FieldTypeEnum.NText, 
			"numeric" => FieldTypeEnum.Numeric, 
			"nvarchar" => FieldTypeEnum.NVarchar, 
			"nvarchar(max)" => FieldTypeEnum.NVarchar_max, 
			"real" => FieldTypeEnum.Real, 
			"smalldatetime" => FieldTypeEnum.SmallDateTime, 
			"smallint" => FieldTypeEnum.SmallInt, 
			"smallmoney" => FieldTypeEnum.SmallMoney, 
			"text" => FieldTypeEnum.Text, 
			"timestamp" => FieldTypeEnum.TimeStamp, 
			"tinyint" => FieldTypeEnum.TinyInt, 
			"uniqueidentifier" => FieldTypeEnum.UniqueIdentifier, 
			"varbinary" => FieldTypeEnum.VarBinary, 
			"varchar" => FieldTypeEnum.Varchar, 
			"varchar(max)" => FieldTypeEnum.Varchar_max, 
			_ => FieldTypeEnum.None, 
		};
	}

	public static bool IsFieldTypeAMemo(FieldTypeEnum fieldType)
	{
		if (fieldType != FieldTypeEnum.NText && fieldType != FieldTypeEnum.NVarchar_max && fieldType != FieldTypeEnum.Text)
		{
			return fieldType == FieldTypeEnum.Varchar_max;
		}
		return true;
	}

	public static bool IsFieldTypeANumber(FieldTypeEnum fieldType)
	{
		if (fieldType != FieldTypeEnum.BigInt && fieldType != FieldTypeEnum.Float && fieldType != FieldTypeEnum.Identity && fieldType != FieldTypeEnum.Int && fieldType != FieldTypeEnum.Money && fieldType != FieldTypeEnum.Numeric && fieldType != FieldTypeEnum.Real && fieldType != FieldTypeEnum.SmallInt && fieldType != FieldTypeEnum.SmallMoney)
		{
			return fieldType == FieldTypeEnum.TinyInt;
		}
		return true;
	}

	public static bool IsFieldTypeADecimal(FieldTypeEnum fieldType)
	{
		if (fieldType != FieldTypeEnum.Float && fieldType != FieldTypeEnum.Money && fieldType != FieldTypeEnum.Numeric && fieldType != FieldTypeEnum.Real)
		{
			return fieldType == FieldTypeEnum.SmallMoney;
		}
		return true;
	}

	public static bool IsFieldTypeADate(FieldTypeEnum fieldType)
	{
		if (fieldType != FieldTypeEnum.Date && fieldType != FieldTypeEnum.DateTime)
		{
			return fieldType == FieldTypeEnum.SmallDateTime;
		}
		return true;
	}

	public static bool IsFieldTypeAInteger(FieldTypeEnum fieldType)
	{
		if (fieldType != FieldTypeEnum.BigInt && fieldType != FieldTypeEnum.Identity && fieldType != FieldTypeEnum.Int && fieldType != FieldTypeEnum.SmallInt)
		{
			return fieldType == FieldTypeEnum.TinyInt;
		}
		return true;
	}

	public static bool IsFieldTypeAString(FieldTypeEnum fieldType)
	{
		if (fieldType != FieldTypeEnum.Char && fieldType != FieldTypeEnum.Varchar && fieldType != FieldTypeEnum.Text && fieldType != FieldTypeEnum.NChar && fieldType != FieldTypeEnum.NVarchar && fieldType != FieldTypeEnum.NText && fieldType != FieldTypeEnum.Varchar_max)
		{
			return fieldType == FieldTypeEnum.NVarchar_max;
		}
		return true;
	}

	public static string FieldTypeToChar(FieldTypeEnum fieldType)
	{
		return fieldType switch
		{
			FieldTypeEnum.BigInt => "bigint", 
			FieldTypeEnum.Binary => "binary", 
			FieldTypeEnum.Bit => "bit", 
			FieldTypeEnum.Char => "char", 
			FieldTypeEnum.Date => "date", 
			FieldTypeEnum.DateTime => "datetime", 
			FieldTypeEnum.Float => "float", 
			FieldTypeEnum.Identity => "identity", 
			FieldTypeEnum.Image => "image", 
			FieldTypeEnum.Int => "int", 
			FieldTypeEnum.Money => "money", 
			FieldTypeEnum.NChar => "nchar", 
			FieldTypeEnum.NText => "ntext", 
			FieldTypeEnum.Numeric => "numeric", 
			FieldTypeEnum.NVarchar => "nvarchar", 
			FieldTypeEnum.NVarchar_max => "nvarchar(max)", 
			FieldTypeEnum.Real => "real", 
			FieldTypeEnum.SmallDateTime => "smalldatetime", 
			FieldTypeEnum.SmallInt => "smallint", 
			FieldTypeEnum.SmallMoney => "smallmoney", 
			FieldTypeEnum.Text => "text", 
			FieldTypeEnum.TimeStamp => "timestamp", 
			FieldTypeEnum.TinyInt => "tinyint", 
			FieldTypeEnum.UniqueIdentifier => "uniqueidentifier", 
			FieldTypeEnum.VarBinary => "varbinary", 
			FieldTypeEnum.Varchar => "varchar", 
			FieldTypeEnum.Varchar_max => "varchar(max)", 
			_ => string.Empty, 
		};
	}

	public void Load(DataRow ddFieldsRow, DataRow[] extensionRows, DataRow[] actionRows, bool allowEditing)
	{
		AllowEditing = allowEditing;
		_UniqueID = ddFieldsRow.Field<Guid?>("dfUniqueID");
		_AppExtensionID = ddFieldsRow.Field<string>("dfAppExtensionID");
		FieldName = ddFieldsRow.Field<string>("dfField");
		FieldNameFormatted = ddFieldsRow.Field<string>("dfDisplayName");
		if (FieldNameFormatted.Length == 0)
		{
			FieldNameFormatted = FieldName;
		}
		TableName = ddFieldsRow.Field<string>("dfTable");
		Caption = ddFieldsRow.Field<string>("dfCaption");
		CaptionExpression = ddFieldsRow.Field<string>("dfCaptionExpression");
		CaptionExpressionUser = ddFieldsRow.Field<string>("dfCaptionExpressionUser");
		ProcessCaptionReferences();
		SaveAsExpression = ddFieldsRow.Field<string>("dfSaveAsExpression");
		SaveAsExpressionUser = ddFieldsRow.Field<string>("dfSaveAsExpressionUser");
		Custom = ddFieldsRow.Field<bool>("dfCustom");
		FieldType = charToFieldType(ddFieldsRow.Field<string>("dfDBType"));
		FieldLength = ddFieldsRow.Field<byte>("dfLength");
		FieldDecimals = ddFieldsRow.Field<byte>("dfDecimals");
		AllowLowerCaseOrNegative = ddFieldsRow.Field<bool>("dfLower");
		Format = ddFieldsRow.Field<string>("dfFormat");
		Sequence = ddFieldsRow.Field<short>("dfSequence");
		SequenceUser = ddFieldsRow.Field<short>("dfSequenceUser");
		RequiredExpression = ddFieldsRow.Field<string>("dfRequiredExpression");
		RequiredExpressionUser = ddFieldsRow.Field<string>("dfRequiredExpressionUser");
		ProcessRequiredRefs();
		DefaultExpression = ddFieldsRow.Field<string>("dfDefaultExpression");
		DefaultExpressionUser = ddFieldsRow.Field<string>("dfDefaultExpressionUser");
		DefaultExpressionUserSetting = ddFieldsRow.Field<string>("daDefault");
		DefaultToPrevious = ddFieldsRow.Field<bool>("dfdprv");
		DefaultToPreviousUser = ddFieldsRow.Field<DefaultToPreviousUserEnum>("dfudpr");
		CalculationExpression = ddFieldsRow.Field<string>("dfCalculationExpression");
		BoundParentFieldExpression = ddFieldsRow.Field<string>("dfBoundParentFieldExpression");
		ValidCodeReferencedFields.Clear();
		ForeignKeyValidCodeReferencedFields.Clear();
		RelatedTableForeignKeyRequiredExpression = ddFieldsRow.Field<string>("dfForeignKeyRequiredExpression");
		RelatedTableForeignKeyRequiredExpressionUser = ddFieldsRow.Field<string>("dfForeignKeyRequiredExpressionUser");
		ReadOnlyExpression = ddFieldsRow.Field<string>("dfReadonlyExpression");
		ReadOnlyExpressionUser = ddFieldsRow.Field<string>("dfReadonlyExpressionUser");
		ProcessReadOnlyReferences();
		Module = ddFieldsRow.Field<string>("dfModule");
		VisibleExpression = ddFieldsRow.Field<string>("dfVisibleExpression");
		VisibleExpressionUser = ddFieldsRow.Field<string>("dfVisibleExpressionUser");
		ProcessVisibleReferences();
		RelatedTable = ddFieldsRow.Field<string>("dfRelatedTable");
		RelatedTableCaption = ddFieldsRow.Field<string>("dtCaption");
		RelatedTableKeyFields = ddFieldsRow.Field<string>("dtKeyFields");
		RelatedTableKeyFieldsArray = RelatedTableKeyFields.Split(',');
		RelatedTableLastKeyCanBeEmpty = ddFieldsRow.Field<bool>("dtLastKeyCanBeEmpty");
		RelatedTableQuickSearchFields = ddFieldsRow.Field<string>("dtQuickSearchFields");
		RelatedTableModule = ddFieldsRow.Field<string>("dtModule");
		RelatedTableCurrencyModeLocationRelatedFields = ddFieldsRow.Field<string>("CurrencyModeLocationRelatedFields");
		RelatedTableCurrencyModeLocationField = ddFieldsRow.Field<string>("dtCurrencyModeLocationField");
		RelatedTableCurrencyModeLocationAndRelatedFields = ((RelatedTableCurrencyModeLocationRelatedFields.Length == 0) ? RelatedTableCurrencyModeLocationField : (RelatedTableCurrencyModeLocationRelatedFields + "," + RelatedTableCurrencyModeLocationField));
		RelatedTableCurrencyModeLocationAndRelatedFieldsArray = RelatedTableCurrencyModeLocationAndRelatedFields.Split(',');
		RelatedTableCurrencyRateIdField = ddFieldsRow.Field<string>("dtCurrencyRateIdField");
		RelatedTableCurrencyCustomRateField = ddFieldsRow.Field<string>("dtCurrencyCustomRateField");
		RelatedTableCurrencyExchangeRateField = ddFieldsRow.Field<string>("dtCurrencyExchangeRateField");
		RelatedTableDocumentDateField = ddFieldsRow.Field<string>("dtDocumentDateField");
		RelatedFields = ddFieldsRow.Field<string>("dfRelatedFields");
		RelatedTableRequiredForeignRelation = ddFieldsRow.Field<bool>("dfRequiredForeignRelation");
		RelatedTableForeignFilter = ddFieldsRow.Field<string>("dfffil");
		ValueList = ddFieldsRow.Field<string>("dfValueList");
		ShowAsDropdown = ddFieldsRow.Field<bool>("dfShowAsDropdown");
		RelatedTableSearchGridID = ddFieldsRow.Field<string>("dfRelatedTableSearchGridId");
		RelatedTableReturnField = ddFieldsRow.Field<string>("dfRelatedTableReturnField");
		RelatedTableDescriptionField = ddFieldsRow.Field<string>("dfRelatedTabledescriptionField");
		RelatedTableOrderByField = ddFieldsRow.Field<string>("dfRelatedTableOrderByField");
		RelatedTableFilter = ddFieldsRow.Field<string>("dfRelatedTableFilter");
		RelatedTableMemoDescription = ddFieldsRow.Field<string>("dtMemoDescription");
		RelatedTableShowMemos = ddFieldsRow.Field<bool>("dtShowMemos");
		RelatedTableUniqueIDField = ddFieldsRow.Field<string>("dtUniqueField");
		AlwaysHidden = ddFieldsRow.Field<bool>("dfhide");
		AllowNulls = ddFieldsRow.Field<bool>("dfAllowNulls");
		FieldGroup = ddFieldsRow.Field<string>("dfGroup");
		FieldGroupParameters = ddFieldsRow.Field<string>("dfGroupParameters");
		BoundParentField = ddFieldsRow.Field<string>("dfBoundParentField");
		BoundParentRelatedFields = ddFieldsRow.Field<string>("parentdfRelatedFields");
		BoundParentRelatedAndCurrentFields = ((BoundParentRelatedFields.Length == 0) ? BoundParentField : (BoundParentRelatedFields + "," + BoundParentField));
		BoundParentRelatedAndCurrentFieldsArray = BoundParentRelatedAndCurrentFields.Split(',');
		BoundParentFieldProxy = ddFieldsRow.Field<string>("dfBoundParentFieldProxy");
		BoundParentFieldType = ddFieldsRow.Field<BoundParentFieldTypeEnum>("dfBoundParentFieldType");
		CurrencyType = ddFieldsRow.Field<M1CurrencyStyle>("dfCurrencyType");
		CurrencyRelatedField = ddFieldsRow.Field<string>("dfCurrencyRelatedField");
		CurrencyUpdateRelatedField = ddFieldsRow.Field<bool>("dfCurrencyUpdateRelatedField");
		DataRow[] array;
		if (actionRows != null && actionRows.Length != 0)
		{
			FieldActions = new List<OpenWithDefinition>();
			array = actionRows;
			foreach (DataRow row in array)
			{
				FieldActions.Add(new OpenWithDefinition(row));
			}
		}
		if (!(extensionRows != null && extensionRows.Length != 0 && DataDictionary != null && allowEditing))
		{
			return;
		}
		FieldExtensions = new List<FieldExtension>();
		array = extensionRows;
		foreach (DataRow dataRow in array)
		{
			string text = dataRow.Field<string>("dhClass");
			if (string.IsNullOrEmpty(text))
			{
				continue;
			}
			Type typeFromCodeAssemblies = DataDictionary.AppExtensions.GetTypeFromCodeAssemblies(text);
			if (!(typeFromCodeAssemblies != null))
			{
				continue;
			}
			FieldExtension fieldExtension = (FieldExtension)Activator.CreateInstance(typeFromCodeAssemblies, new object[0]);
			fieldExtension.Load(dataRow, ValidCodeReferencedFields, this, dataRow.Field<string>("dhOpenWithID"), dataRow.Field<string>("dhCaption"));
			FieldExtensions.Add(fieldExtension);
			if (dataRow["dwID"] != DBNull.Value)
			{
				if (FieldActions == null)
				{
					FieldActions = new List<OpenWithDefinition>();
				}
				FieldActions.Add(new OpenWithDefinition(dataRow));
			}
		}
	}

	public void ProcessCodeBindings(string eventName, StringBuilder code)
	{
		if (eventName.Equals("Valid", StringComparison.CurrentCultureIgnoreCase))
		{
			ValidCodeReferencedFields.ParseCodeForFields(code.ToString());
		}
		else
		{
			if (!eventName.Equals("ForeignKeyValid", StringComparison.CurrentCultureIgnoreCase))
			{
				return;
			}
			ForeignKeyValidCodeReferencedFields.ParseCodeForFields(code.ToString());
			if (ForeignKeyValidCodeReferencedFields.Count == 0)
			{
				return;
			}
			foreach (string foreignKeyValidCodeReferencedField in ForeignKeyValidCodeReferencedFields)
			{
				if (!ValidCodeReferencedFields.Contains(foreignKeyValidCodeReferencedField))
				{
					ValidCodeReferencedFields.Add(foreignKeyValidCodeReferencedField);
				}
			}
		}
	}

	protected void ProcessCaptionReferences()
	{
		CaptionExpressionReferencedFields.Clear();
		if (CaptionExpressionUser != null && CaptionExpressionUser.Length != 0)
		{
			CaptionExpressionReferencedFields.ParseCodeForFields(CaptionExpressionUser);
		}
		else if (CaptionExpression != null && CaptionExpression.Length != 0)
		{
			CaptionExpressionReferencedFields.ParseCodeForFields(CaptionExpression);
		}
	}

	private void ProcessRequiredRefs()
	{
		needToProcessRequired = false;
		RequiredExpressionReferencedFields.Clear();
		if (RequiredExpression != null && RequiredExpression.Length != 0)
		{
			RequiredExpressionReferencedFields.ParseCodeForFields(RequiredExpression);
		}
		if (RequiredExpressionUser != null && RequiredExpressionUser.Length != 0)
		{
			RequiredExpressionReferencedFields.ParseCodeForFields(RequiredExpressionUser);
		}
	}

	protected void ProcessReadOnlyReferences()
	{
		needToProcessReadOnly = false;
		ReadOnlyExpressionReferencedFields.Clear();
		ReadOnlyExpressionRelatedTableReferencedFields.Clear();
		if (ReadOnlyExpression != null && ReadOnlyExpression.Length != 0)
		{
			ReadOnlyExpressionReferencedFields.ParseCodeForFields(ReadOnlyExpression);
			ReadOnlyExpressionRelatedTableReferencedFields.ParseCodeForRelatedDataFields(ReadOnlyExpression);
		}
		if (ReadOnlyExpressionUser != null && ReadOnlyExpressionUser.Length != 0)
		{
			ReadOnlyExpressionReferencedFields.ParseCodeForFields(ReadOnlyExpressionUser);
			ReadOnlyExpressionRelatedTableReferencedFields.ParseCodeForRelatedDataFields(ReadOnlyExpressionUser);
		}
	}

	protected void ProcessVisibleReferences()
	{
		VisibleExpressionReferencedFields.Clear();
		if (VisibleExpression != null && VisibleExpression.Length != 0)
		{
			VisibleExpressionReferencedFields.ParseCodeForFields(VisibleExpression);
		}
		if (VisibleExpressionUser != null && VisibleExpressionUser.Length != 0)
		{
			VisibleExpressionReferencedFields.ParseCodeForFields(VisibleExpressionUser);
		}
	}

	public void Load(DataColumn column, bool allowEditing)
	{
		VirtualField = true;
		AllowEditing = allowEditing;
		if (column.DataType == typeof(decimal))
		{
			FieldType = FieldTypeEnum.Numeric;
			FieldLength = 12;
			if (column.Table.Rows.Count > 0)
			{
				try
				{
					string text = column.Table.Rows[0].Field<decimal>(column).ToString();
					int num = text.IndexOf('.');
					if (num != -1)
					{
						text = text.Substring(num + 1);
						FieldDecimals = (byte)text.Length;
					}
				}
				catch (InvalidCastException)
				{
				}
			}
		}
		else if (column.DataType == typeof(Guid))
		{
			FieldType = FieldTypeEnum.UniqueIdentifier;
			FieldLength = 16;
		}
		else if (column.DataType == typeof(DateTime))
		{
			FieldType = FieldTypeEnum.DateTime;
			FieldLength = 14;
		}
		else if (column.DataType == typeof(int))
		{
			FieldType = FieldTypeEnum.Int;
			FieldLength = 4;
		}
		else if (column.DataType == typeof(bool))
		{
			FieldType = FieldTypeEnum.Bit;
			FieldLength = 1;
		}
		else if (column.DataType == typeof(double))
		{
			FieldType = FieldTypeEnum.Numeric;
			FieldLength = 12;
			FieldDecimals = 4;
		}
		else if (column.MaxLength > 512)
		{
			FieldType = FieldTypeEnum.NVarchar_max;
			FieldLength = 50;
		}
		else
		{
			FieldType = FieldTypeEnum.NVarchar;
			FieldLength = column.MaxLength;
		}
		Caption = column.Caption;
		if (Caption.Length == 0)
		{
			Caption = FieldNameFormatted.ToLower();
		}
	}

	public void LoadFieldProperties(string properties)
	{
		if (properties.Length == 0)
		{
			return;
		}
		List<string> list = M1Util.ParseFieldList(properties, ':');
		for (int i = 1; i <= list.Count - 1; i++)
		{
			List<string> list2 = M1Util.ParseFieldList(list[i], '=');
			if (list2.Count < 2)
			{
				continue;
			}
			string text = list2[0].Trim().ToUpper();
			string text2 = list2[1].Trim();
			switch (text)
			{
			case "C":
			case "CAPTION":
				Caption = ConvertPropString(text2);
				break;
			case "VBEXPR":
				CalculationExpression = ConvertPropString(text2);
				break;
			case "VBEXPRTYPE":
				if (text2 == "1")
				{
					CalculationExpressionType = CalculationExpressionTypeEnum.RunningTotal;
				}
				else if (text2 == "2")
				{
					CalculationExpressionType = CalculationExpressionTypeEnum.Single;
				}
				else
				{
					CalculationExpressionType = CalculationExpressionTypeEnum.Standard;
				}
				break;
			case "T":
			{
				if (!text2.StartsWith("numeric(", StringComparison.CurrentCultureIgnoreCase))
				{
					break;
				}
				string text3 = text2.Substring(8).Replace(")", string.Empty);
				int num = text3.IndexOf(',');
				if (num != -1)
				{
					FieldLength = Convert.ToInt16(text3.Substring(0, num));
					FieldDecimals = Convert.ToByte(text3.Substring(num + 1));
					break;
				}
				num = text3.IndexOf('.');
				if (num != -1)
				{
					FieldLength = Convert.ToInt16(text3.Substring(0, num));
					FieldDecimals = Convert.ToByte(text3.Substring(num + 1));
				}
				else
				{
					FieldLength = Convert.ToInt16(text3);
					FieldDecimals = 0;
				}
				break;
			}
			}
		}
	}

	protected string ConvertPropString(string propValue)
	{
		if (propValue.Length > 1)
		{
			propValue = propValue.Substring(1);
		}
		if (propValue.Length >= 1)
		{
			propValue = propValue.Substring(0, propValue.Length - 1);
		}
		return propValue;
	}

	public void LoadDatabase(string databaseName, DataRow row, M1User m1User)
	{
		M1DatabaseFieldSecurity m1DatabaseFieldSecurity = new M1DatabaseFieldSecurity();
		m1DatabaseFieldSecurity.Database = databaseName;
		m1DatabaseFieldSecurity.SetAccessLevels(row, m1User);
		Databases.Add(m1DatabaseFieldSecurity);
	}

	private void checkIsValidForCurrentRow(M1Database database, DataRow row, SqlTransaction transaction, bool isolateInfo)
	{
		bool flag = checkIsValid(database, row, transaction, RequiredResolved, isCurrentRow: true, isolateInfo);
		if (flag != _IsValid)
		{
			_IsValid = flag;
			OnIsValidChanged(new FieldValueChangedEventArgs(database, row, isCurrentRow: true, null, BindingSource.Transaction));
		}
	}

	private void OnErrorTextChanged(EventArgs e)
	{
		this.ErrorTextChanged?.Invoke(this, e);
	}

	public void OnForeignKeyValid(ValidEventArgs e)
	{
		this.ForeignKeyValid?.Invoke(this, e);
	}

	protected void OnValid(ValidEventArgs e)
	{
		this.Valid?.Invoke(this, e);
	}

	private bool checkIsValid(M1Database database, DataRow row, SqlTransaction transaction, bool? required, bool isCurrentRow, bool isolateInfo)
	{
		ValidationInfo info;
		if (isolateInfo)
		{
			info = new ValidationInfo(BindingSource, row, row, this);
		}
		else
		{
			errorList.Clear();
			errorList.Row = row;
			info = errorList;
		}
		info = CheckIsValid(database, row, transaction, required, info, BindingSource.Errors);
		if (isCurrentRow)
		{
			ErrorText = info.ToString();
		}
		return info.ErrorCount == 0;
	}

	public ValidationInfo CheckIsValid(M1Database database, DataRow row, SqlTransaction transaction, bool? required, ValidationInfo info, ErrorItemsList errors)
	{
		bool flag = true;
		if (info == null)
		{
			info = new ValidationInfo(BindingSource, row, row, this);
		}
		if (row != null && row.RowState != DataRowState.Deleted && FieldName.Length != 0)
		{
			if (string.IsNullOrWhiteSpace(BindingSource.AutoRemoveWhereOnSave) || !Table.EvaluateScriptExpressionBool(BindingSource.AutoRemoveWhereOnSave, database, row))
			{
				if (!required.HasValue)
				{
					required = Table.EvaluateScriptExpressionBool(RequiredExpression, RequiredExpressionUser, database, row, transaction);
				}
				if (required == true && (!IsPartOfKey || Table == null || Table.ParentBindingSource == null || !Table.ParentTableName.Equals(Table.ParentBindingSource.PrimaryTable.TableName, StringComparison.CurrentCultureIgnoreCase) || Table.LastKeyField.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase)))
				{
					if (row.IsNull(FieldName))
					{
						flag = false;
					}
					else if (IsFieldTypeAString(FieldType))
					{
						if (string.IsNullOrWhiteSpace(row.Field<string>(FieldName)))
						{
							flag = false;
						}
					}
					else if (IsFieldTypeANumber(FieldType) && Convert.ToDouble(row[FieldName]) == 0.0)
					{
						flag = false;
					}
				}
				bool flag2 = HasFieldOrRelatedFieldValueChanged(row);
				if (!flag2)
				{
					foreach (string foreignKeyValidCodeReferencedField in ForeignKeyValidCodeReferencedFields)
					{
						if (BindingSource.Fields[foreignKeyValidCodeReferencedField].HasValueChanged(row))
						{
							flag2 = true;
							break;
						}
					}
				}
				if (flag2 || ForceToValidate)
				{
					RelatedTableIsForeignKeyValid(database, row, transaction, info);
					ForceToValidate = false;
				}
				if (!flag)
				{
					info.AddError(DataDictionary.Language.GetLanguageText(database, "MISCDATAISREQUIRED", "% is required", new string[1] { Caption }, "ERRORMSG"));
				}
				if (IsEditableKey && Table.LastKeyField.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase) && !RequiredResolved && M1Util.IsNullOrEmpty(row.Field<object>(FieldName)) && !BindingSource.GetKeyState(row) && database.ExecuteScalar("Select 1 as dummy from " + Table.TableName + " Where " + Table.GetFilterForCurrentRow(row), transaction) != null)
				{
					info.AddError("Record already exists for the empty " + Caption);
				}
				OnValid(new ValidEventArgs(info, database, row, transaction));
			}
			errors?.SetRowFieldErrorList(row, this, info);
		}
		return info;
	}

	public bool HasValueChanged(DataRow row)
	{
		if (row.RowState == DataRowState.Added)
		{
			return !M1Util.IsNullOrEmpty(row[FieldName]);
		}
		if (row.HasVersion(DataRowVersion.Original))
		{
			return !row[FieldName, DataRowVersion.Original].Equals(row[FieldName]);
		}
		return false;
	}

	public bool HasFieldOrRelatedFieldValueChanged(DataRow row)
	{
		if (AllowEditing)
		{
			if (IsFieldTypeAMemo(FieldType))
			{
				return HasValueChanged(row);
			}
			string[] relatedFieldsAndCurrentFieldArray = RelatedFieldsAndCurrentFieldArray;
			foreach (string text in relatedFieldsAndCurrentFieldArray)
			{
				try
				{
					if (!text.Equals("''") && BindingSource.Fields[text].HasValueChanged(row))
					{
						return true;
					}
				}
				catch (Exception ex)
				{
					throw new M1Exception($"{ex.Message} - Field {FieldName} has a related field setting of {text}, which does not exist.");
				}
			}
		}
		return false;
	}

	private void OnColumnErrorChanged(ColumnErrorChangedEventArgs e)
	{
		this.ColumnErrorChanged?.Invoke(this, e);
	}

	private void OnIsValidChanged(FieldValueChangedEventArgs e)
	{
		this.IsValidChanged?.Invoke(this, e);
	}

	public void AttachForeignUpdateBinding(string sourceField, string destinationField, bool reverseSign, string enabledExpression)
	{
		if (foreignUpdateHandler == null)
		{
			foreignUpdateHandler = new ForeignUpdateHandler();
		}
		foreignUpdateHandler.AttachFieldBinding(sourceField, destinationField, reverseSign, enabledExpression);
	}

	public void AddRelatedTableLookupField(string field, string parentField)
	{
		if (string.IsNullOrWhiteSpace(parentField))
		{
			if (_RelatedFieldsWrapper == null)
			{
				_RelatedFieldsWrapper = new RelatedTableField(this);
			}
			if (!_RelatedFieldsWrapper.RelatedTableFields.ContainsKey(field))
			{
				_RelatedFieldsWrapper.RelatedTableFields.Add(field, null);
			}
		}
		else
		{
			_RelatedFieldsWrapper.AddRelatedTableLookupField(field, parentField);
		}
	}

	public IRelatedTableField Fields(string name)
	{
		_RelatedFieldsWrapper.Name = name;
		return _RelatedFieldsWrapper;
	}

	public void OnValueChanged(FieldValueChangedEventArgs e)
	{
		if (AllowEditing)
		{
			Validate(e.Database, e.Row, e.SqlTransaction, e.IsCurrentRow);
		}
		if (_RelatedFieldsWrapper != null)
		{
			_RelatedFieldsWrapper.RelatedTableLookupRow = null;
		}
		this.ValueChanged?.Invoke(this, e);
	}

	public void OnAfterValueChanged(FieldValueChangedEventArgs e)
	{
		this.AfterValueChanged?.Invoke(this, e);
	}

	public void BoundParentFieldValueChanged(object sender, FieldValueChangedEventArgs e)
	{
		if (sender is FieldDefinition parentFieldDef)
		{
			BindingSource.ProcessBoundParentValueChange(parentFieldDef, this, e.Database, e.Row);
		}
	}

	public void ProcessBoundParentFieldForRow(DataRow parentRow, M1Database currentDatabase, DataRow currentDataRow)
	{
		if (RelatedFieldsAndCurrentFieldArray.Length < BoundParentRelatedAndCurrentFieldsArray.Length)
		{
			return;
		}
		for (int i = 0; i < BoundParentRelatedAndCurrentFieldsArray.Length; i++)
		{
			if (BoundParentFieldProxy.Length != 0)
			{
				DataRow dataRow = BindingSource.Fields[BoundParentFieldProxy].RelatedTableGetDataRow(BoundParentRelatedAndCurrentFieldsArray[i], currentDatabase, currentDataRow);
				currentDataRow[RelatedFieldsAndCurrentFieldArray[i]] = dataRow[BoundParentRelatedAndCurrentFieldsArray[i]];
			}
			else
			{
				currentDataRow[RelatedFieldsAndCurrentFieldArray[i]] = parentRow[BoundParentRelatedAndCurrentFieldsArray[i]];
			}
		}
	}

	public void RelatedCalculationFieldValueChanged(object sender, FieldValueChangedEventArgs e)
	{
		EvaluateCalculationExpression(e.Database, e.Row, e.SqlTransaction);
	}

	public void RelatedCaptionFieldValueChanged(object sender, FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			EvaluateCaptionExpression(e.Database, e.Row);
		}
	}

	public void RelatedVisibleFieldValueChanged(object sender, FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			EvaluateNoAccess(e.Database, e.Row);
		}
	}

	public void RelatedReadOnlyFieldValueChanged(object sender, FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			EvaluateReadOnlyExpression(e.Database, e.Row);
		}
	}

	public void RelatedRequiredFieldValueChanged(object sender, FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			EvaluateRequiredExpression(e.Database, e.Row, e.SqlTransaction);
		}
		Validate(e.Database, e.Row, e.SqlTransaction, e.IsCurrentRow);
	}

	public void RelatedValidCodeValueChanged(object sender, FieldValueChangedEventArgs e)
	{
		Validate(e.Database, e.Row, e.SqlTransaction, e.IsCurrentRow);
	}

	public void UpdateRunningTotal(M1Database database)
	{
		double num = 0.0;
		foreach (DataRowView item in BindingSource.GetDataView())
		{
			num += Convert.ToDouble(Table.EvaluateScriptExpression(CalculationExpression, database, item.Row));
			item.Row[FieldName] = num;
		}
	}

	public void UpdateSingle(M1Database database)
	{
		foreach (DataRowView item in BindingSource.GetDataView())
		{
			if (!string.IsNullOrEmpty(CalculationExpression))
			{
				object obj = Table.EvaluateScriptExpression(CalculationExpression, database, item.Row);
				if (!string.IsNullOrEmpty(obj.ToString()))
				{
					item.Row[FieldName] = Convert.ToDouble(obj);
				}
			}
		}
	}

	public void EvaluateCalculationExpression(M1Database database, DataRow row, SqlTransaction transaction)
	{
		if (row == null)
		{
			return;
		}
		if (CalculationExpressionType == CalculationExpressionTypeEnum.RunningTotal)
		{
			UpdateRunningTotal(database);
		}
		else if (CalculationExpressionType == CalculationExpressionTypeEnum.Single)
		{
			UpdateSingle(database);
		}
		else
		{
			if (CalculationExpressionReferencedFields.SubFieldReferences != null && CalculationExpressionReferencedFields.SubFieldReferences.Count != 0 && !Table.EvaluateScriptExpressionBool("Fields(\"" + CalculationExpressionReferencedFields.SubFieldReferences.First().Key + "\").Fields(\"" + CalculationExpressionReferencedFields.SubFieldReferences.First().Value.Keys.First() + "\").RowExists", database, row, transaction))
			{
				return;
			}
			object obj = Table.EvaluateScriptExpression(CalculationExpression, database, row, transaction);
			if (FieldType == FieldTypeEnum.Bit)
			{
				if (Convert.ToBoolean(obj).Equals(Convert.ToBoolean(row[FieldName])))
				{
					return;
				}
			}
			else if (IsFieldTypeANumber(FieldType))
			{
				if (Convert.ToDouble(obj).Equals(Convert.ToDouble(row[FieldName])))
				{
					return;
				}
			}
			else if (IsFieldTypeAString(FieldType))
			{
				if (Convert.ToString(obj).Trim().Equals(Convert.ToString(row[FieldName]).Trim()))
				{
					return;
				}
			}
			else if (obj.Equals(row[FieldName]))
			{
				return;
			}
			row[FieldName] = obj;
		}
	}

	protected void EvaluateBoundParentFieldExpression(M1Database database, DataRow row, string fieldToReplace, string valueToUse)
	{
		if (row != null && BoundParentFieldExpression != null)
		{
			string expr = BoundParentFieldExpression.Replace("Fields(\"" + fieldToReplace + "\").Value", valueToUse, caseInsensitive: true);
			double num = Convert.ToDouble(Table.EvaluateScriptExpression(expr, database, row));
			double num2 = Convert.ToDouble(Table.EvaluateScriptExpression(BoundParentFieldExpression, database, row)) - num;
			if (num2 != 0.0 && Table.ParentBindingSource != null)
			{
				DataRow parentDataRow = Table.GetParentDataRow(row);
				double num3 = Convert.ToDouble(parentDataRow[BoundParentField]);
				parentDataRow[BoundParentField] = num3 + num2;
			}
		}
	}

	public void EvaluateRequiredExpression(M1Database database, DataRow row, SqlTransaction transaction)
	{
		bool flag = false;
		if (row != null && row.RowState != DataRowState.Deleted && BindingSource != null)
		{
			flag = Table.EvaluateScriptExpressionBool(RequiredExpression, RequiredExpressionUser, database, row, transaction);
		}
		if (flag != RequiredResolved)
		{
			RequiredResolved = flag;
			checkIsValidForCurrentRow(database, row, transaction, isolateInfo: false);
			OnRequiredChanged(EventArgs.Empty);
		}
	}

	public bool EvaluateForeignKeyRequiredExpression(M1Database database, DataRow row)
	{
		bool flag = false;
		if (BindingSource != null && (RelatedTableForeignKeyRequiredExpression.Length != 0 || RelatedTableForeignKeyRequiredExpressionUser.Length != 0))
		{
			if (RelatedTableForeignKeyRequiredExpressionUser.Length != 0)
			{
				return Table.EvaluateScriptExpressionBool(RelatedTableForeignKeyRequiredExpressionUser, string.Empty, database, row, null);
			}
			return Table.EvaluateScriptExpressionBool(RelatedTableForeignKeyRequiredExpression, string.Empty, database, row, null);
		}
		if (RelatedTable.Length != 0 && !IsPartOfKey)
		{
			return true;
		}
		return false;
	}

	public bool KeyFieldLeaveCheck()
	{
		if (IsEditableKey && !ReadOnlyResolved && BindingSource?.CurrentAsDataRow != null)
		{
			string[] keyFieldsArray = Table.KeyFieldsArray;
			foreach (string text in keyFieldsArray)
			{
				if (BindingSource.Fields[text].RequiredResolved && M1Util.IsNullOrEmpty(BindingSource.CurrentAsDataRow[text]))
				{
					return false;
				}
			}
			if (!Table.EmptyKeyCanBeEdited && Table.LastKeyField.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase) && M1Util.IsNullOrEmpty(Value))
			{
				return false;
			}
			int num = 0;
			bool flag = false;
			for (int j = 0; j < Table.KeyFieldsArray.Length; j++)
			{
				if (flag)
				{
					if (!BindingSource.Fields[Table.KeyFieldsArray[j]].NoAccessResolved)
					{
						num++;
						break;
					}
				}
				else if (Table.KeyFieldsArray[j].Equals(FieldName, StringComparison.CurrentCultureIgnoreCase))
				{
					flag = true;
				}
			}
			if (num != 0)
			{
				return false;
			}
			if (M1Util.IsNullOrEmpty(Value))
			{
				BindingSource.SetKeyState(BindingSource.CurrentAsDataRow, keyIsSet: true);
			}
			else
			{
				BindingSource.SetKeyState(BindingSource.CurrentAsDataRow, keyIsSet: true);
			}
			return true;
		}
		return false;
	}

	public void Validate(M1Database database, DataRow row, SqlTransaction transaction, bool isCurrentRow)
	{
		Validate(database, row, transaction, isCurrentRow, isolateInfo: false);
	}

	public void Validate(M1Database database, DataRow row, SqlTransaction transaction, bool isCurrentRow, bool isolateInfo)
	{
		if (isCurrentRow)
		{
			checkIsValidForCurrentRow(database, row, transaction, isolateInfo);
		}
		else if (Table != null)
		{
			checkIsValid(database, row, transaction, null, isCurrentRow: false, isolateInfo);
		}
	}

	public object EvaluateButtonCode(M1Database database, DataRow row, object forms)
	{
		return null;
	}

	protected virtual void OnRequiredChanged(EventArgs e)
	{
		this.RequiredChanged?.Invoke(this, e);
	}

	private M1DatabaseFieldSecurity getSecurityObject(M1Database database)
	{
		if (Databases.Count > 0 && database != null)
		{
			foreach (M1DatabaseFieldSecurity database2 in Databases)
			{
				if (database2.Database.Equals(database.ID, StringComparison.CurrentCultureIgnoreCase))
				{
					return database2;
				}
			}
		}
		return null;
	}

	public string GetReadOnlyReasons(M1Database database, DataRow row)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (row == null)
		{
			stringBuilder.AppendLine("INVALID: No row is specified.");
		}
		else if (row != null && row.RowState == DataRowState.Deleted)
		{
			stringBuilder.AppendLine("INVALID: The row has been deleted.");
		}
		if (!AllowEditing)
		{
			stringBuilder.AppendLine("CODE: Field " + FieldNameFormatted + " AllowEditing property is false. This is usually set on grids that do not allow editing.");
		}
		if (IsUpdatedFromChildBoundField)
		{
			stringBuilder.AppendLine("DD: Field " + FieldNameFormatted + " is a calculated field (it is updated from a child table).");
		}
		if (BoundParentFieldType == BoundParentFieldTypeEnum.FromParent && BoundParentField.Length != 0)
		{
			stringBuilder.AppendLine("DD: Field " + FieldNameFormatted + " is a calculated field (it is updated from a parent table).");
		}
		M1DatabaseFieldSecurity securityObject = getSecurityObject(database);
		if (securityObject == null)
		{
			stringBuilder.AppendLine("SEC: The security access level for database " + ((database == null) ? string.Empty : database.ID) + " could not be determined, so a default of no access was used.");
		}
		else if ((securityObject.ResolvedAccessLevel & SecurityAccessLevel.None) != SecurityAccessLevel.Default || securityObject.ResolvedAccessLevel == SecurityAccessLevel.View)
		{
			stringBuilder.Append(securityObject.GetReadOnlyReasons(this));
		}
		if (Table != null && Table.ReadOnlyResolved)
		{
			stringBuilder.Append(Table.GetReadOnlyReasons(database, row));
		}
		if (Table != null && BindingSource != null && row != null && IsPartOfKey && BindingSource.GetKeyState(row))
		{
			stringBuilder.AppendLine("DD: The " + FieldNameFormatted + " is part of the primary key, and the last field in the key is not empty.");
		}
		if (BindingSource != null && row != null && Table != null)
		{
			if (Table.EvaluateScriptExpressionBool(ReadOnlyExpression, string.Empty, database, row, null))
			{
				stringBuilder.AppendLine("DD: The " + FieldNameFormatted + " ReadOnlyExpression evaluated to true.");
			}
			if (Table.EvaluateScriptExpressionBool(ReadOnlyExpressionUser, string.Empty, database, row, null))
			{
				stringBuilder.AppendLine("DD: The " + FieldNameFormatted + " ReadOnlyExpressionUser evaluated to true.");
			}
		}
		return stringBuilder.ToString();
	}

	protected virtual void OnNoAccessChanged(EventArgs e)
	{
		this.NoAccessChanged?.Invoke(this, e);
	}

	public void EvaluateNoAccess(M1Database database, DataRow row)
	{
		bool flag = false;
		M1DatabaseFieldSecurity securityObject = getSecurityObject(database);
		if (securityObject == null || (securityObject.ResolvedAccessLevel & SecurityAccessLevel.None) != SecurityAccessLevel.Default)
		{
			flag = true;
		}
		if (!flag && Table != null && Table.NoAccessResolved)
		{
			flag = true;
		}
		if ((!flag && Module.Length != 0 && (!BindingSource.DataDictionary.ProductCode.IsModulePurchased(Module, database) || database.Security.GetModuleAccessLevel(Module) == SecurityAccessLevel.None)) || (RelatedTableModule.Length != 0 && (!BindingSource.DataDictionary.ProductCode.IsModulePurchased(RelatedTableModule, database) || database.Security.GetModuleAccessLevel(RelatedTableModule) == SecurityAccessLevel.None)))
		{
			flag = true;
		}
		if (((!flag && VisibleExpression != null && VisibleExpression.Length != 0) || (VisibleExpressionUser != null && VisibleExpressionUser.Length != 0)) && !GetVisibleExpression(database, row))
		{
			flag = true;
		}
		if (flag != NoAccessResolved)
		{
			NoAccessResolved = flag;
			OnNoAccessChanged(EventArgs.Empty);
		}
	}

	public bool GetVisibleExpression(M1Database database, DataRow row)
	{
		bool result = true;
		if (((BindingSource != null && VisibleExpression != null && VisibleExpression.Length != 0) || (VisibleExpressionUser != null && VisibleExpressionUser.Length != 0)) && (VisibleExpressionReferencedFields.Count == 0 || row != null))
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (VisibleExpression != null && VisibleExpression.Length != 0)
			{
				stringBuilder.Append(VisibleExpression);
			}
			if (VisibleExpressionUser != null && VisibleExpressionUser.Length != 0)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" And ");
				}
				stringBuilder.Append(VisibleExpressionUser);
			}
			result = Table.EvaluateScriptExpressionBool(stringBuilder.ToString(), string.Empty, database, row, null);
		}
		return result;
	}

	public void EvaluateCaptionExpression(M1Database database, DataRow row)
	{
		if (BindingSource != null && database != null && row != null)
		{
			if (CaptionExpressionUser != null && CaptionExpressionUser.Length != 0)
			{
				Caption = (string)Table.EvaluateScriptExpression(CaptionExpressionUser, database, row);
			}
			else if (CaptionExpression != null && CaptionExpression.Length != 0)
			{
				Caption = (string)Table.EvaluateScriptExpression(CaptionExpression, database, row);
			}
		}
	}

	public object GetFieldValueForSaveAs(M1Database database, DataRow row)
	{
		if (BindingSource != null && database != null && row != null)
		{
			if (SaveAsExpressionUser != null && SaveAsExpressionUser.Length != 0)
			{
				return Table.EvaluateScriptExpression(SaveAsExpressionUser, database, row);
			}
			if (SaveAsExpression != null && SaveAsExpression.Length != 0)
			{
				return Table.EvaluateScriptExpression(SaveAsExpression, database, row);
			}
			return row[FieldName];
		}
		return null;
	}

	public string GetRelatedTableFilterExpression(M1Database database, DataRow row)
	{
		if (BindingSource != null && RelatedTableFilter != null && RelatedTableFilter.Length != 0)
		{
			foreach (string relatedTableFilterReferencedField in RelatedTableFilterReferencedFields)
			{
				if (!BindingSource.Fields.Contains(relatedTableFilterReferencedField))
				{
					return string.Empty;
				}
			}
			return Table.EvaluateScriptExpression(RelatedTableFilter, database, row).ToString();
		}
		if (RelatedTableFilter == null)
		{
			return string.Empty;
		}
		return RelatedTableFilter;
	}

	public void EvaluateReadOnlyExpression(M1Database database, DataRow row)
	{
		bool readOnlyExpression = GetReadOnlyExpression(database, row);
		if (readOnlyExpression != ReadOnlyResolved)
		{
			ReadOnlyResolved = readOnlyExpression;
			OnReadOnlyChanged(EventArgs.Empty);
		}
	}

	public bool GetReadOnlyExpression(M1Database database, DataRow row)
	{
		bool flag = false;
		bool flag2 = false;
		if (row == null || (row != null && row.RowState == DataRowState.Deleted) || !AllowEditing || (IsUpdatedFromChildBoundField && string.IsNullOrWhiteSpace(_ReadOnlyExpression)) || (BoundParentFieldType == BoundParentFieldTypeEnum.FromParent && BoundParentField.Length != 0))
		{
			flag = true;
		}
		else
		{
			M1DatabaseFieldSecurity securityObject = getSecurityObject(database);
			if (securityObject == null || (securityObject.ResolvedAccessLevel & SecurityAccessLevel.None) != SecurityAccessLevel.Default || securityObject.ResolvedAccessLevel == SecurityAccessLevel.View)
			{
				flag = true;
			}
			if (!flag && Table != null && (Table.ReadOnlyExpressionResolved || Table.GetHasSecurityExpression(database, row)))
			{
				flag = true;
			}
			if (!flag && Table != null && BindingSource != null && row != null && IsPartOfKey)
			{
				flag = BindingSource.GetKeyState(row) || !IsEditableKey;
			}
			if (BindingSource != null)
			{
				flag2 = Table.EvaluateScriptExpressionBool(ReadOnlyExpression, ReadOnlyExpressionUser, database, row, BindingSource.Transaction);
			}
		}
		return flag || flag2;
	}

	protected virtual void OnReadOnlyChanged(EventArgs e)
	{
		this.ReadOnlyChanged?.Invoke(this, e);
	}

	private void _BindingSource_CurrentChanged(object sender, EventArgs e)
	{
		if (Table == null)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database currentDatabase = BindingSource.CurrentDatabase;
		SqlTransaction transaction = BindingSource.Transaction;
		if (AllowEditing)
		{
			if (currentDatabase != null && currentAsDataRow != null)
			{
				EvaluateNoAccess(currentDatabase, currentAsDataRow);
				EvaluateRequiredExpression(currentDatabase, currentAsDataRow, transaction);
				checkIsValidForCurrentRow(currentDatabase, currentAsDataRow, transaction, isolateInfo: false);
			}
			EvaluateReadOnlyExpression(currentDatabase, currentAsDataRow);
		}
		EvaluateCaptionExpression(currentDatabase, currentAsDataRow);
	}

	public void OnFlash(EventArgs e)
	{
		this.Flash?.Invoke(this, e);
	}

	public string GetGroupCaption(string groupType)
	{
		if (FieldName.Length == 0)
		{
			return "<Nothing>";
		}
		string text = Caption;
		switch (groupType.Trim().ToUpper())
		{
		case "D":
			text += " by day";
			break;
		case "W":
			text += " by week";
			break;
		case "M":
			text += " by month";
			break;
		case "FP":
			text += " by period";
			break;
		case "Q":
			text += " by quarter";
			break;
		case "Y":
			text += " by year";
			break;
		case "FY":
			text += " by fiscal year";
			break;
		}
		return text;
	}

	public override string ToString()
	{
		return Caption;
	}

	public bool EvaluateVisible(M1Database database)
	{
		if (AlwaysHidden)
		{
			return false;
		}
		M1DatabaseFieldSecurity securityObject = getSecurityObject(database);
		if (securityObject != null && (securityObject.ResolvedAccessLevel & SecurityAccessLevel.None) != SecurityAccessLevel.Default)
		{
			return false;
		}
		return true;
	}

	public string GetEmptyForFieldType()
	{
		if (IsFieldTypeAString(FieldType))
		{
			return "''";
		}
		if (IsFieldTypeANumber(FieldType))
		{
			return "0";
		}
		FieldTypeEnum fieldType = FieldType;
		if (fieldType - 5 <= FieldTypeEnum.BigInt || fieldType == FieldTypeEnum.SmallDateTime)
		{
			return "null";
		}
		return string.Empty;
	}

	public object GetDefaultForFieldType()
	{
		if (IsFieldTypeAString(FieldType))
		{
			return string.Empty;
		}
		if (IsFieldTypeANumber(FieldType))
		{
			switch (FieldType)
			{
			case FieldTypeEnum.Int:
				return 0;
			case FieldTypeEnum.Numeric:
				if (FieldDecimals == 0)
				{
					return 0;
				}
				return 0.0;
			case FieldTypeEnum.Money:
				return 0.0;
			default:
				return 0;
			}
		}
		switch (FieldType)
		{
		case FieldTypeEnum.Date:
		case FieldTypeEnum.DateTime:
		case FieldTypeEnum.SmallDateTime:
			return null;
		case FieldTypeEnum.Bit:
			return false;
		default:
			return string.Empty;
		}
	}

	public string GetRelatedFieldDefaultValue(object currentValue, string relatedField, object oldValue)
	{
		string result = string.Empty;
		if (Name.Equals("IMBWAREHOUSEID", StringComparison.CurrentCultureIgnoreCase) && relatedField.Equals("IMBPARTBINID", StringComparison.CurrentCultureIgnoreCase))
		{
			string text = currentValue.ToString();
			if (!string.IsNullOrWhiteSpace(text))
			{
				result = GetDefaultWarehouseBin(Database, text);
			}
		}
		if (Name.Equals("IMRPARTID", StringComparison.CurrentCultureIgnoreCase) && relatedField.Equals("IMRPARTREVISIONID", StringComparison.CurrentCultureIgnoreCase))
		{
			result = (oldValue ?? string.Empty).ToString();
		}
		return result;
	}

	public string RelatedFieldsFormatCaptionAndCurrentValues(DataRow row)
	{
		string text = string.Empty;
		string empty = string.Empty;
		string[] relatedFieldsAndCurrentFieldArray = RelatedFieldsAndCurrentFieldArray;
		foreach (string text2 in relatedFieldsAndCurrentFieldArray)
		{
			if (text.Length != 0)
			{
				text += ", ";
			}
			empty = row[text2].ToString().Trim();
			if (empty.Length == 0)
			{
				empty = "<none>";
			}
			text = text + BindingSource.Fields[text2].Caption.Replace("?", string.Empty) + " " + empty.ToLinq();
		}
		return text;
	}

	private bool RelatedTableRowExists(M1Database database, DataRow row)
	{
		if (RelatedTableLastKeyCanBeEmpty || !M1Util.IsNullOrEmpty(row[FieldName]))
		{
			if (Table != null && Table.ParentTableLinkField != null && Table.ParentTableLinkField.RelatedFieldsAndCurrentFieldArray.Contains(FieldName, StringComparer.CurrentCultureIgnoreCase))
			{
				return true;
			}
			M1BindingSource childBindingSourceForTable = getChildBindingSourceForTable(RelatedTable);
			if (RelatedTable.Length != 0 && childBindingSourceForTable != null && childBindingSourceForTable.Count > 0)
			{
				return true;
			}
			if (database.ExecuteScalar("select 1 as dummy from " + RelatedTable + " Where " + RelatedTableGetWhereClause(zeroRecordsIfFirstFieldIsEmpty: true, includeLastField: true, row), BindingSource.Transaction) != null)
			{
				return true;
			}
		}
		return false;
	}

	private M1BindingSource getChildBindingSourceForTable(string table)
	{
		if (table.Length != 0)
		{
			foreach (M1BindingSource childBindingSource in BindingSource.ChildBindingSources)
			{
				if (childBindingSource.PrimaryTable != null && childBindingSource.PrimaryTable.TableName.Equals(table, StringComparison.CurrentCultureIgnoreCase) && childBindingSource.IsBoundToField(this))
				{
					return childBindingSource;
				}
			}
		}
		return null;
	}

	public void OnForeignKeyInvalid(ForeignKeyInvalidEventArgs e)
	{
		this.ForeignKeyInvalid?.Invoke(this, e);
	}

	public void RelatedTableIsForeignKeyValid(M1Database database, DataRow row, SqlTransaction transaction, ValidationInfo errorInfo)
	{
		if (RelatedTable.Length == 0 || IsPartOfKey || BindingSource.SkipForeignKeyChecks)
		{
			return;
		}
		if (!M1Util.IsNullOrEmpty(row[RelatedFieldsAndCurrentFieldArray[0]]))
		{
			if (!RelatedTableLastKeyCanBeEmpty && M1Util.IsNullOrEmpty(row[FieldName]))
			{
				return;
			}
			bool flag = true;
			if (RelatedTable.StartsWith("DD", StringComparison.CurrentCultureIgnoreCase) || (BindingSource.Fields[RelatedFieldsAndCurrentFieldArray[0]] != this && !BindingSource.Fields[RelatedFieldsAndCurrentFieldArray[0]].RelatedTableRowExists(database, row)))
			{
				flag = false;
			}
			if (!flag)
			{
				return;
			}
			errorInfo.Row = row;
			if (!RelatedTableRowExists(database, row))
			{
				if (EvaluateForeignKeyRequiredExpression(database, row))
				{
					errorInfo.AddError(RelatedFieldsFormatCaptionAndCurrentValues(row) + " does not exist in the " + RelatedTableCaption + " table");
				}
				return;
			}
			OnForeignKeyValid(new ValidEventArgs(errorInfo, database, row, transaction));
			if (RelatedTableForeignFilter.Length != 0 && database.ExecuteScalar("select 1 as dummy from " + RelatedTable + " Where " + RelatedTableGetWhereClause(zeroRecordsIfFirstFieldIsEmpty: true, includeLastField: true, row) + " And (" + RelatedTableForeignFilter + ")", BindingSource.Transaction) == null)
			{
				errorInfo.AddError(RelatedFieldsFormatCaptionAndCurrentValues(row) + " exists in the " + RelatedTableCaption + " table but is not valid for the following reason(s):\r" + formatForeignKeyMessage(RelatedTableForeignFilter));
			}
		}
		else if (RelatedFields.Length != 0 && !M1Util.IsNullOrEmpty(row[FieldName]) && !RelatedTableForeignKeyRequiredExpression.Equals("False", StringComparison.CurrentCultureIgnoreCase))
		{
			errorInfo.AddError(RelatedFieldsFormatCaptionAndCurrentValues(row) + " is not a valid " + Caption + " because it does not exist in the " + RelatedTableCaption + " table");
		}
	}

	private string formatForeignKeyMessage(string filter)
	{
		string text = filter;
		StringBuilder stringBuilder = new StringBuilder();
		while (true)
		{
			int num = text.IndexOf(" and ", StringComparison.CurrentCultureIgnoreCase);
			int num2 = text.IndexOf(" or ", StringComparison.CurrentCultureIgnoreCase);
			string text2;
			if (num != -1 && num2 != -1)
			{
				if (num < num2)
				{
					text2 = text.Substring(0, num);
					text = text.Substring(num + 5);
				}
				else
				{
					text2 = text.Substring(0, num2);
					text = text.Substring(num2 + 4);
				}
			}
			else if (num != -1)
			{
				text2 = text.Substring(0, num);
				text = text.Substring(num + 5);
			}
			else if (num2 != -1)
			{
				text2 = text.Substring(0, num2);
				text = text.Substring(num2 + 4);
			}
			else
			{
				text2 = text;
				text = string.Empty;
			}
			if (text2.Length == 0)
			{
				break;
			}
			string text3 = text2;
			string text4 = string.Empty;
			string text5 = " ";
			int num3 = text3.IndexOf('=');
			if (num3 == -1)
			{
				num3 = text3.IndexOf("<>");
				if (num3 == -1)
				{
					num3 = text3.IndexOf('>');
					if (num3 == -1)
					{
						num3 = text3.IndexOf('<');
						if (num3 == -1)
						{
							num3 = text3.IndexOf(" is ", StringComparison.CurrentCultureIgnoreCase);
							if (num3 == -1)
							{
								text5 = " is ";
								text4 = " is not ";
							}
						}
						else
						{
							text4 = " is not less than ";
						}
					}
					else
					{
						text4 = " is not greater than ";
					}
				}
				else
				{
					text4 = " is ";
					text5 = "<>";
				}
			}
			else
			{
				text4 = " is not ";
			}
			if (num3 != -1)
			{
				string text6 = text3.Substring(num3 + text5.Length).Trim();
				text3 = text3.Substring(0, num3);
				if (text3.StartsWith("IsNull(", StringComparison.CurrentCultureIgnoreCase))
				{
					text3 = text3.Substring(7);
					num3 = text3.IndexOf(',');
					if (num3 != -1)
					{
						text3 = text3.Substring(0, num3);
					}
				}
				SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select dfCaption,dfDBType,dfValueList From DDFields Where dfField = @Field");
				sqlCommand.Parameters.Add(new SqlParameter("@Field", SqlDbType.NVarChar)).Value = text3;
				DataTable dataTable = DataDictionary.GetDataTable(sqlCommand);
				if (dataTable.Rows.Count != 0)
				{
					string text7 = dataTable.Rows[0].Field<string>("dfCaption").Trim().Replace("?", "");
					string text8 = dataTable.Rows[0].Field<string>("dfDBType");
					string text9 = dataTable.Rows[0].Field<string>("dfValueList");
					if (text8.Equals("bit", StringComparison.CurrentCultureIgnoreCase))
					{
						text4 = ((!text4.Equals(" is ")) ? " is " : " is not ");
						text6 = ((!text6.Equals("0")) ? "False" : "True");
					}
					else if (text8.Equals("date", StringComparison.CurrentCultureIgnoreCase) || text8.Equals("datetime", StringComparison.CurrentCultureIgnoreCase))
					{
						if (text6.Equals("null", StringComparison.CurrentCultureIgnoreCase))
						{
							text6 = "Empty";
						}
						else if (text6.Replace(" ", "").Equals("GetDate()", StringComparison.CurrentCultureIgnoreCase))
						{
							text6 = "Today";
						}
					}
					if (!string.IsNullOrWhiteSpace(text9))
					{
						string valueListText = GetValueListText(text6, text9);
						if (!string.IsNullOrWhiteSpace(valueListText))
						{
							text6 = valueListText;
						}
					}
					stringBuilder.Append(text7 + text4 + text6.Trim() + ((text.Length == 0) ? string.Empty : " or "));
				}
				else
				{
					stringBuilder.Append(text2 + ((text.Length == 0) ? string.Empty : " or "));
				}
			}
			else
			{
				stringBuilder.Append(text2 + ((text.Length == 0) ? string.Empty : " or "));
			}
		}
		return stringBuilder.ToString();
	}

	public string RelatedTableGetWhereClause(bool zeroRecordsIfFirstFieldIsEmpty, bool includeLastField)
	{
		return RelatedTableGetWhereClause(zeroRecordsIfFirstFieldIsEmpty, includeLastField, CurrentDataRow());
	}

	public string RelatedTableGetWhereClause(bool zeroRecordsIfFirstFieldIsEmpty, bool includeLastField, DataRow row)
	{
		return RelatedTableGetWhereClause(zeroRecordsIfFirstFieldIsEmpty, includeLastField, row, DataRowVersion.Default, forSql: true);
	}

	public string RelatedTableGetWhereClause(bool zeroRecordsIfFirstFieldIsEmpty, bool includeLastField, DataRow row, DataRowVersion rowVersion, bool forSql)
	{
		try
		{
			if (row == null)
			{
				row = CurrentDataRow();
			}
			if (row != null && row.RowState == DataRowState.Deleted)
			{
				rowVersion = DataRowVersion.Original;
			}
			if (row != null && row.RowState == DataRowState.Detached && !row.HasVersion(rowVersion))
			{
				if (row.HasVersion(DataRowVersion.Original))
				{
					rowVersion = DataRowVersion.Original;
				}
				else if (row.HasVersion(DataRowVersion.Proposed))
				{
					rowVersion = DataRowVersion.Proposed;
				}
			}
			if (row == null)
			{
				if (zeroRecordsIfFirstFieldIsEmpty)
				{
					return "0=1";
				}
				return "0=0";
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (RelatedTable.Length != 0 && RelatedTableKeyFieldsArray.Length == RelatedFieldsAndCurrentFieldArray.Length && RelatedTableKeyFieldsArray[0].Length > 0)
			{
				object obj = null;
				int num = -1;
				string[] relatedFieldsAndCurrentFieldArray = RelatedFieldsAndCurrentFieldArray;
				foreach (string text in relatedFieldsAndCurrentFieldArray)
				{
					num++;
					if (!includeLastField && num == RelatedFieldsAndCurrentFieldArray.Length - 1)
					{
						continue;
					}
					obj = ((text.Length == 0) ? row[FieldName, rowVersion] : (text.Equals("''") ? string.Empty : ((!text.Equals("0")) ? row[text, rowVersion] : ((object)0))));
					if (M1Util.IsNullOrEmpty(obj) && num == 0)
					{
						if (zeroRecordsIfFirstFieldIsEmpty)
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.Append(" And ");
							}
							stringBuilder.Append("0=1");
						}
						else if (RelatedTable == "SerialNumbers" && _FieldName == "kbpSerialNumberID")
						{
							break;
						}
					}
					else if (obj != DBNull.Value && obj != null)
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(" And ");
						}
						if (forSql)
						{
							stringBuilder.Append(RelatedTableKeyFieldsArray[num] + " = " + obj.ToSql());
						}
						else
						{
							stringBuilder.Append(RelatedTableKeyFieldsArray[num] + " = " + obj.ToLinq());
						}
					}
				}
				if (stringBuilder.Length == 0 && zeroRecordsIfFirstFieldIsEmpty)
				{
					stringBuilder.Append("0=1");
				}
			}
			return stringBuilder.ToString();
		}
		catch (RowNotInTableException)
		{
			if (zeroRecordsIfFirstFieldIsEmpty)
			{
				return "0=1";
			}
			return "0=0";
		}
	}

	public DataRow RelatedTableGetDataRow(string fieldsToReturn)
	{
		return RelatedTableGetDataRow(fieldsToReturn, null, null);
	}

	public DataRow RelatedTableGetDataRow(string fieldsToReturn, M1Database databaseToUse, DataRow rowToUse)
	{
		return RelatedTableGetDataRow(fieldsToReturn, databaseToUse, rowToUse, alwaysReturnValidRow: false);
	}

	public DataRow RelatedTableGetDataRow(string fieldsToReturn, M1Database databaseToUse, DataRow rowToUse, bool alwaysReturnValidRow)
	{
		return RelatedTableGetDataRow(fieldsToReturn, databaseToUse, rowToUse, alwaysReturnValidRow, BindingSource.Transaction);
	}

	public DataRow RelatedTableGetDataRow(string fieldsToReturn, M1Database databaseToUse, DataRow rowToUse, bool alwaysReturnValidRow, SqlTransaction sqlTransaction)
	{
		DataRow dataRow = null;
		if (Table != null && IsPartOfKey)
		{
			TableDefinition tableDefinition = Table;
			for (int num = Table.KeyFieldsArray.Length - 1; num >= 0; num--)
			{
				if (Table.KeyFieldsArray[num].Equals(FieldName, StringComparison.CurrentCultureIgnoreCase))
				{
					if (!fieldsToReturn.StartsWith(tableDefinition.FieldPrefix, StringComparison.CurrentCultureIgnoreCase) && (tableDefinition.FieldPrefixUser.Length == 0 || !fieldsToReturn.StartsWith(tableDefinition.FieldPrefixUser, StringComparison.CurrentCultureIgnoreCase)))
					{
						break;
					}
					if (rowToUse != null)
					{
						DataTable dataTable = tableDefinition.BindingSource.GetDataTable();
						if (rowToUse.Table == dataTable)
						{
							dataRow = rowToUse;
						}
						else
						{
							string[] array = ((rowToUse.Table != Table.BindingSource.GetDataTable()) ? Table.ParentBindingSource.Tables[Table.BindingSource.Fields[Table.KeyFieldsArray[Table.KeyFieldsArray.Length - 2]].RelatedTable].KeyFieldsArray : Table.KeyFieldsArray);
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i <= num; i++)
							{
								if (stringBuilder.Length != 0)
								{
									stringBuilder.Append(" And ");
								}
								stringBuilder.Append(RelatedTableKeyFieldsArray[i] + " = " + rowToUse[array[i], (rowToUse.RowState == DataRowState.Detached) ? DataRowVersion.Proposed : ((rowToUse.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current)].ToLinq());
							}
							if (dataTable.Rows.Count != 0)
							{
								if (dataTable.Rows.Count == 1)
								{
									dataRow = dataTable.Rows[0];
								}
								else
								{
									DataRow[] array2 = dataTable.Select(stringBuilder.ToString(), string.Empty, DataViewRowState.ModifiedCurrent);
									if (array2.Length != 0)
									{
										dataRow = array2[0];
									}
								}
							}
						}
					}
					if (dataRow == null)
					{
						dataRow = tableDefinition.GetCurrentDataRowForProcessing();
					}
					if (dataRow != null || rowToUse == null || rowToUse.RowState != DataRowState.Deleted)
					{
						break;
					}
					string filterExpression = RelatedTableGetWhereClause(zeroRecordsIfFirstFieldIsEmpty: true, includeLastField: true, rowToUse, DataRowVersion.Original, forSql: false);
					DataTable changes = tableDefinition.BindingSource.GetDataTable().GetChanges(DataRowState.Deleted);
					if (changes == null || changes.Rows.Count == 0)
					{
						break;
					}
					if (changes.Rows.Count == 1)
					{
						dataRow = changes.Rows[0];
						break;
					}
					DataRow[] array3 = changes.Select(filterExpression, string.Empty, DataViewRowState.Deleted);
					if (array3.Length != 0)
					{
						dataRow = array3[0];
					}
					break;
				}
				if (num > 0)
				{
					if (tableDefinition.ParentBindingSource == null)
					{
						break;
					}
					if (tableDefinition.ParentBindingSource.Tables.Contains(Table.BindingSource.Fields[Table.KeyFieldsArray[num - 1]].RelatedTable))
					{
						tableDefinition = tableDefinition.ParentBindingSource.Tables[Table.BindingSource.Fields[Table.KeyFieldsArray[num - 1]].RelatedTable];
					}
				}
			}
		}
		if (dataRow == null)
		{
			if (databaseToUse == null)
			{
				databaseToUse = Table.BindingSource.CurrentDatabase;
			}
			string text;
			string text2;
			if (IsPartOfKey && rowToUse == null)
			{
				text = getWhereClauseForParentWhenNoCurrentRow();
				text2 = RelatedTable;
			}
			else if (Table.KeyFieldsArray.Length >= 2 && Table.KeyFieldsArray[Table.KeyFieldsArray.Length - 2].Equals(FieldName, StringComparison.CurrentCultureIgnoreCase) && RelatedTable.Length != 0 && Table.ParentTableName.Length != 0 && !RelatedTable.Equals(Table.ParentTableName, StringComparison.CurrentCultureIgnoreCase))
			{
				text = Table.GetPersistentParentWhereClause(rowToUse);
				text2 = Table.ParentTableName;
			}
			else
			{
				text = RelatedTableGetWhereClause(zeroRecordsIfFirstFieldIsEmpty: true, includeLastField: true, rowToUse, (rowToUse != null && rowToUse.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Default, forSql: true);
				text2 = RelatedTable;
			}
			DataTable dataTable2 = databaseToUse.GetDataTable("Select " + fieldsToReturn + " From " + text2 + " Where " + text, sqlTransaction);
			if (dataTable2 != null && dataTable2.Rows.Count != 0)
			{
				dataRow = dataTable2.Rows[0];
			}
			else if (alwaysReturnValidRow)
			{
				dataRow = dataTable2.NewRow();
				dataRow.BlankRow(allowNullForDefaultValue: false);
				dataTable2.Rows.Add(dataRow);
			}
		}
		return dataRow;
	}

	private string getWhereClauseForParentWhenNoCurrentRow()
	{
		TableDefinition tableDefinition = Table;
		int num = Table.KeyFieldsArray.Length - 1;
		while (num >= 0 && tableDefinition.ParentBindingSource != null)
		{
			tableDefinition = tableDefinition.ParentBindingSource.Tables[Table.BindingSource.Fields[Table.KeyFieldsArray[num - 1]].RelatedTable];
			if (tableDefinition.BindingSource.CurrentAsDataRow != null)
			{
				break;
			}
			num--;
		}
		if (tableDefinition.BindingSource.CurrentAsDataRow != null)
		{
			int num2 = 0;
			for (int i = 0; i < Table.KeyFieldsArray.Length; i++)
			{
				if (Table.KeyFieldsArray[i].Equals(FieldName, StringComparison.CurrentCultureIgnoreCase))
				{
					num2 = i;
					break;
				}
			}
			return tableDefinition.BindingSource.Fields[tableDefinition.KeyFieldsArray[num2]].RelatedTableGetWhereClause(zeroRecordsIfFirstFieldIsEmpty: true, includeLastField: true, tableDefinition.BindingSource.CurrentAsDataRow, DataRowVersion.Current, forSql: true);
		}
		return "0=1";
	}

	public DataRow CurrentDataRow()
	{
		if (BindingSource == null || BindingSource.PrimaryTable == null)
		{
			return null;
		}
		return BindingSource.PrimaryTable.GetCurrentDataRowForProcessing();
	}

	private void updateRelatedTableBoundField(RowUpdateEventArgs e, bool doDeleteCheck)
	{
		if (BoundParentField.Length == 0 || BoundParentFieldType != BoundParentFieldTypeEnum.ToForeignParent)
		{
			return;
		}
		FieldDefinition fieldDefinition = ((BoundParentFieldProxy.Length != 0) ? BindingSource.Fields[BoundParentFieldProxy] : this);
		if (fieldDefinition == null || fieldDefinition.RelatedTable.Length == 0)
		{
			return;
		}
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder();
		if (doDeleteCheck)
		{
			for (int i = 0; i < fieldDefinition.RelatedFieldsAndCurrentFieldArray.Length; i++)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" And ");
				}
				stringBuilder.Append(fieldDefinition.RelatedTableKeyFieldsArray[i] + " = " + e.Row[fieldDefinition.RelatedFieldsAndCurrentFieldArray[i], DataRowVersion.Original].ToSql());
				if (!M1Util.IsNullOrEmpty(e.Row[fieldDefinition.RelatedFieldsAndCurrentFieldArray[i], DataRowVersion.Original]))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			string[] keyFieldsArray = fieldDefinition.Table.KeyFieldsArray;
			foreach (string text in keyFieldsArray)
			{
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(" And ");
				}
				stringBuilder2.Append(text + " = " + e.Row[text, DataRowVersion.Original].ToSql());
			}
			StringBuilder stringBuilder3 = new StringBuilder();
			for (int k = 0; k < fieldDefinition.RelatedFieldsAndCurrentFieldArray.Length; k++)
			{
				if (stringBuilder3.Length != 0)
				{
					stringBuilder3.Append(" And ");
				}
				stringBuilder3.Append(fieldDefinition.RelatedFieldsAndCurrentFieldArray[k] + " = " + fieldDefinition.RelatedTableKeyFieldsArray[k]);
			}
			if (fieldDefinition == this)
			{
				Database.ExecuteCommand(string.Format("Update {0} Set {1} = 0  Where {2} And (Select Count(*) From {3} Inner Join {4} On {5} Where {6}{7}", fieldDefinition.RelatedTable, BoundParentField, stringBuilder, fieldDefinition.TableName, fieldDefinition.RelatedTable, stringBuilder3, stringBuilder, " And Not (" + stringBuilder2.ToString() + ")) = 0"), e.SqlTransaction);
				BindingSource.OnTableChanged(fieldDefinition.RelatedTable);
			}
			else
			{
				double num = Convert.ToDouble(e.Row[FieldName, DataRowVersion.Original]);
				if (num != 0.0)
				{
					Database.ExecuteCommand($"Update {fieldDefinition.RelatedTable} Set {BoundParentField} = {BoundParentField} - {num.ToSql()} Where {stringBuilder}", e.SqlTransaction);
					BindingSource.OnTableChanged(fieldDefinition.RelatedTable);
				}
			}
			return;
		}
		for (int l = 0; l < fieldDefinition.RelatedFieldsAndCurrentFieldArray.Length; l++)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" And ");
			}
			stringBuilder.Append(fieldDefinition.RelatedTableKeyFieldsArray[l] + " = " + e.Row[fieldDefinition.RelatedFieldsAndCurrentFieldArray[l]].ToSql());
			if (!M1Util.IsNullOrEmpty(e.Row[fieldDefinition.RelatedFieldsAndCurrentFieldArray[l]]))
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		if (fieldDefinition == this)
		{
			Database.ExecuteCommand("Update " + fieldDefinition.RelatedTable + " Set  " + BoundParentField + "  = -1 Where " + stringBuilder.ToString(), e.SqlTransaction);
			BindingSource.OnTableChanged(fieldDefinition.RelatedTable);
			return;
		}
		double num2 = Convert.ToDouble(e.Row[FieldName]);
		if (num2 != 0.0)
		{
			Database.ExecuteCommand($"Update {fieldDefinition.RelatedTable} Set {BoundParentField} = {BoundParentField} + {num2.ToSql()} Where {stringBuilder}", e.SqlTransaction);
			BindingSource.OnTableChanged(fieldDefinition.RelatedTable);
		}
	}

	private void Table_ParentBindingSourceChanged(object sender, TableDefinition.ParentBindingSourceChangedEventArgs e)
	{
		wireUpReadOnlyRelatedTableFields(e.NewBindingSource);
	}

	private void wireUpReadOnlyRelatedTableFields(M1BindingSource parentBs)
	{
		if (parentBs == null)
		{
			return;
		}
		Table.ParentBindingSourceChanged -= Table_ParentBindingSourceChanged;
		foreach (string readOnlyExpressionRelatedTableReferencedField in ReadOnlyExpressionRelatedTableReferencedFields)
		{
			if (parentBs.Fields.Contains(readOnlyExpressionRelatedTableReferencedField))
			{
				FieldDefinition fieldDefinition = parentBs.Fields[readOnlyExpressionRelatedTableReferencedField];
				if (fieldDefinition != this)
				{
					fieldDefinition.ValueChanged -= RelatedReadOnlyFieldValueChanged;
					fieldDefinition.ValueChanged += RelatedReadOnlyFieldValueChanged;
				}
			}
		}
	}

	public static void ProcessSubFieldReferences(FieldCollection fields, Dictionary<string, Dictionary<string, string>> subFieldReferences)
	{
		if (subFieldReferences == null || subFieldReferences.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<string, Dictionary<string, string>> subFieldReference in subFieldReferences)
		{
			foreach (KeyValuePair<string, string> item in subFieldReference.Value)
			{
				fields[subFieldReference.Key].AddRelatedTableLookupField(item.Key, item.Value);
			}
		}
	}

	public void LoadComplete(FieldCollection fields, bool allowEditing)
	{
		if (allowEditing || VirtualField)
		{
			foreach (ChildReferenceTableLink childReferenceTableLink in Table.ChildReferenceTableLinks)
			{
				if (childReferenceTableLink.BindingType == BoundParentFieldTypeEnum.ToParent && childReferenceTableLink.ParentField.Equals(FieldName, StringComparison.CurrentCultureIgnoreCase))
				{
					IsUpdatedFromChildBoundField = true;
					break;
				}
			}
			try
			{
				foreach (string calculationExpressionReferencedField in CalculationExpressionReferencedFields)
				{
					if (fields.Contains(calculationExpressionReferencedField))
					{
						FieldDefinition fieldDefinition = fields[calculationExpressionReferencedField];
						if (fieldDefinition != this)
						{
							fieldDefinition.ValueChanged -= RelatedCalculationFieldValueChanged;
							fieldDefinition.ValueChanged += RelatedCalculationFieldValueChanged;
						}
					}
				}
				ProcessSubFieldReferences(fields, CalculationExpressionReferencedFields.SubFieldReferences);
			}
			catch (Exception ex)
			{
				throw new M1Exception("Exception '" + ex.Message + "' while processing the CalculationExpression for field " + FieldName + ".", ex);
			}
			try
			{
				if (BoundParentField.Length != 0 && BoundParentFieldType == BoundParentFieldTypeEnum.ToParent && BoundParentFieldExpression != null && BoundParentFieldExpression.Length != 0)
				{
					foreach (string boundParentFieldExpressionReferencedField in BoundParentFieldExpressionReferencedFields)
					{
						if (fields.Contains(boundParentFieldExpressionReferencedField))
						{
							FieldDefinition fieldDefinition = fields[boundParentFieldExpressionReferencedField];
							fieldDefinition.ValueChanged -= RelatedBoundParentExpressionValueChanged;
							fieldDefinition.ValueChanged += RelatedBoundParentExpressionValueChanged;
						}
					}
				}
			}
			catch (Exception ex2)
			{
				throw new M1Exception("Exception '" + ex2.Message + "' while processing the BoundParentFieldExpression for field " + FieldName + ".", ex2);
			}
			try
			{
				foreach (string captionExpressionReferencedField in CaptionExpressionReferencedFields)
				{
					if (fields.Contains(captionExpressionReferencedField))
					{
						FieldDefinition fieldDefinition = fields[captionExpressionReferencedField];
						if (fieldDefinition != this)
						{
							fieldDefinition.ValueChanged -= RelatedCaptionFieldValueChanged;
							fieldDefinition.ValueChanged += RelatedCaptionFieldValueChanged;
						}
					}
				}
			}
			catch (Exception ex3)
			{
				throw new M1Exception("Exception '" + ex3.Message + "' while processing the CaptionExpression for field " + FieldName + ".", ex3);
			}
			try
			{
				foreach (string visibleExpressionReferencedField in VisibleExpressionReferencedFields)
				{
					if (fields.Contains(visibleExpressionReferencedField))
					{
						FieldDefinition fieldDefinition = fields[visibleExpressionReferencedField];
						if (fieldDefinition != this)
						{
							fieldDefinition.ValueChanged -= RelatedVisibleFieldValueChanged;
							fieldDefinition.ValueChanged += RelatedVisibleFieldValueChanged;
						}
					}
				}
			}
			catch (Exception ex4)
			{
				throw new M1Exception("Exception '" + ex4.Message + "' while processing the VisibleExpression for field " + FieldName + ".", ex4);
			}
			try
			{
				if (needToProcessReadOnly)
				{
					ProcessReadOnlyReferences();
				}
				foreach (string readOnlyExpressionReferencedField in ReadOnlyExpressionReferencedFields)
				{
					if (fields.Contains(readOnlyExpressionReferencedField))
					{
						FieldDefinition fieldDefinition = fields[readOnlyExpressionReferencedField];
						if (fieldDefinition != this)
						{
							fieldDefinition.ValueChanged -= RelatedReadOnlyFieldValueChanged;
							fieldDefinition.ValueChanged += RelatedReadOnlyFieldValueChanged;
						}
					}
				}
				ProcessSubFieldReferences(fields, ReadOnlyExpressionReferencedFields.SubFieldReferences);
				if (ReadOnlyExpressionRelatedTableReferencedFields.Count != 0)
				{
					if (Table.ParentBindingSource == null)
					{
						Table.ParentBindingSourceChanged += Table_ParentBindingSourceChanged;
					}
					else
					{
						wireUpReadOnlyRelatedTableFields(Table.ParentBindingSource);
					}
				}
			}
			catch (Exception ex5)
			{
				throw new M1Exception("Exception '" + ex5.Message + "' while processing the ReadOnlyExpression for field " + FieldName + ".", ex5);
			}
			try
			{
				if (needToProcessRequired)
				{
					ProcessRequiredRefs();
				}
				foreach (string requiredExpressionReferencedField in RequiredExpressionReferencedFields)
				{
					if (fields.Contains(requiredExpressionReferencedField))
					{
						FieldDefinition fieldDefinition = fields[requiredExpressionReferencedField];
						if (fieldDefinition != this)
						{
							fieldDefinition.ValueChanged -= RelatedRequiredFieldValueChanged;
							fieldDefinition.ValueChanged += RelatedRequiredFieldValueChanged;
						}
					}
				}
			}
			catch (Exception ex6)
			{
				throw new M1Exception("Exception '" + ex6.Message + "' while processing the RequiredExpression for field " + FieldName + ".", ex6);
			}
			try
			{
				foreach (string validCodeReferencedField in ValidCodeReferencedFields)
				{
					if (fields.Contains(validCodeReferencedField))
					{
						FieldDefinition fieldDefinition = fields[validCodeReferencedField];
						if (fieldDefinition != this)
						{
							fieldDefinition.ValueChanged -= RelatedValidCodeValueChanged;
							fieldDefinition.ValueChanged += RelatedValidCodeValueChanged;
						}
					}
				}
			}
			catch (Exception ex7)
			{
				throw new M1Exception("Exception '" + ex7.Message + "' while processing the Valid code for field " + FieldName + ".", ex7);
			}
			WireUpCurrencyEvents(fields);
			if (BoundParentField.Length != 0 && BoundParentFieldType == BoundParentFieldTypeEnum.ToForeignParent)
			{
				BindingSource.RowUpdateAddBefore -= BindingSource_RowUpdateSave;
				BindingSource.RowUpdateAddBefore += BindingSource_RowUpdateSave;
				BindingSource.RowUpdateSaveBefore -= BindingSource_RowUpdateSave;
				BindingSource.RowUpdateSaveBefore += BindingSource_RowUpdateSave;
				BindingSource.RowUpdateDeleteBefore -= BindingSource_RowUpdateDelete;
				BindingSource.RowUpdateDeleteBefore += BindingSource_RowUpdateDelete;
			}
		}
		if (foreignUpdateHandler != null)
		{
			foreignUpdateHandler.Load(this, allowEditing);
		}
	}

	public void WireUpCurrencyEvents(FieldCollection fields)
	{
		ValueChanged -= FieldDefinition_ValueChanged_UpdateRelatedCurrencyField;
		Table.ExchangeRateChanged -= Table_ExchangeRateChanged;
		if (CurrencyRelatedField.Length != 0 && CurrencyUpdateRelatedField && CurrencyType != M1CurrencyStyle.None)
		{
			List<string> list = new List<string> { "arlSecondTaxAmountBase", "arlTaxAmountBase", "arlTaxAmountForeign", "arlSecondTaxAmountForeign" };
			List<string> list2 = new List<string> { "pmlSecondTaxAmountBase", "pmlTaxAmountBase", "pmlTaxAmountForeign", "pmlSecondTaxAmountForeign", "pmlExtendedCostBase", "pmlExtendedCostForeign" };
			if (fields[CurrencyRelatedField].CalculationExpression.Length == 0 || list.Contains(FieldName) || list2.Contains(FieldName))
			{
				ValueChanged += FieldDefinition_ValueChanged_UpdateRelatedCurrencyField;
				Table.ExchangeRateChanged += Table_ExchangeRateChanged;
			}
		}
	}

	private void RelatedBoundParentExpressionValueChanged(object sender, FieldValueChangedEventArgs e)
	{
		EvaluateBoundParentFieldExpression(e.Database, e.Row, ((FieldDefinition)sender).FieldName, e.PreviousValue.ToString());
	}

	private void Table_ExchangeRateChanged(object sender, TableDefinition.ExchangeRateChangedEventArgs e)
	{
		if ((e.UpdateBaseCurrencyFields && CurrencyType == M1CurrencyStyle.Foreign) || (!e.UpdateBaseCurrencyFields && CurrencyType == M1CurrencyStyle.Base))
		{
			FieldDefinition_ValueChanged_UpdateRelatedCurrencyField(sender, e);
		}
	}

	private void FieldDefinition_ValueChanged_UpdateRelatedCurrencyField(object sender, FieldValueChangedEventArgs e)
	{
		if (CurrencyRelatedField.Length == 0 || !CurrencyUpdateRelatedField)
		{
			return;
		}
		decimal num = Table.GetExchangeRateForRow(e.Database, e.Row, null);
		if (num == 0m)
		{
			num = 1m;
		}
		decimal num2 = default(decimal);
		decimal num3 = default(decimal);
		if (CurrencyType == M1CurrencyStyle.Foreign)
		{
			num2 = M1Math.Round(e.Row.Field<decimal>(FieldName) / num, FieldDecimals);
			num3 = M1Math.Round(num2 * num, FieldDecimals);
		}
		else
		{
			num2 = M1Math.Round(e.Row.Field<decimal>(FieldName) * num, FieldDecimals);
			num3 = M1Math.Round(num2 / num, FieldDecimals);
		}
		if (!e.Row.Field<decimal>(CurrencyRelatedField).Equals(num2))
		{
			e.Row.Field<decimal>(FieldName).Equals(num3);
			bool currencyUpdateRelatedField = BindingSource.Fields[CurrencyRelatedField].CurrencyUpdateRelatedField;
			BindingSource.Fields[CurrencyRelatedField].CurrencyUpdateRelatedField = false;
			try
			{
				e.Row.SetField(CurrencyRelatedField, num2);
			}
			finally
			{
				BindingSource.Fields[CurrencyRelatedField].CurrencyUpdateRelatedField = currencyUpdateRelatedField;
			}
		}
	}

	private void BindingSource_RowUpdateDelete(object sender, RowUpdateEventArgs e)
	{
		updateRelatedTableBoundField(e, doDeleteCheck: true);
	}

	private void BindingSource_RowUpdateSave(object sender, RowUpdateEventArgs e)
	{
		if (e.Row.RowState == DataRowState.Added)
		{
			updateRelatedTableBoundField(e, doDeleteCheck: false);
			return;
		}
		FieldDefinition fieldDefinition = ((BoundParentFieldProxy.Length == 0) ? this : BindingSource.Fields[BoundParentFieldProxy]);
		if (HasValueChanged(e.Row) || fieldDefinition.HasFieldOrRelatedFieldValueChanged(e.Row))
		{
			updateRelatedTableBoundField(e, doDeleteCheck: true);
			updateRelatedTableBoundField(e, doDeleteCheck: false);
		}
	}

	public void ImportFile(M1Database database, DataRow row, string file)
	{
		if (file.Length == 0)
		{
			row[FieldName] = DBNull.Value;
			return;
		}
		FileStream fileStream = new FileStream(file, FileMode.Open);
		try
		{
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, (int)fileStream.Length);
			row[FieldName] = array;
		}
		finally
		{
			fileStream.Close();
		}
	}

	public string GetValueListText(object value)
	{
		if (ValueList != null && ValueList.Length != 0)
		{
			string[] array = ValueList.Replace("|", ",").Replace("\\r", "\r").Replace("\n", string.Empty)
				.Split(new string[1] { "\r" }, StringSplitOptions.None);
			foreach (string text in array)
			{
				int num = text.IndexOf(',');
				if (num != -1)
				{
					string text2 = text.Substring(0, num);
					string text3 = text.Substring(num + 1);
					num = text3.IndexOf('~');
					if (num != -1)
					{
						text3.Substring(num + 1);
						text3 = text3.Substring(0, num);
					}
					else
					{
						_ = string.Empty;
					}
					if (text2.StartsWith("\"") && text2.EndsWith("\""))
					{
						text2 = text2.Substring(1);
						text2 = text2.Substring(0, text2.Length - 1);
					}
					if (object.Equals(text2, value.ToString()))
					{
						return text3;
					}
				}
			}
		}
		return value.ToString();
	}

	public string GetValueListText(object value, string valueList)
	{
		if (!string.IsNullOrWhiteSpace(valueList))
		{
			string[] array = valueList.Replace("|", ",").Replace("\\r", "\r").Replace("\n", string.Empty)
				.Split(new string[1] { "\r" }, StringSplitOptions.None);
			foreach (string text in array)
			{
				int num = text.IndexOf(',');
				if (num != -1)
				{
					string text2 = text.Substring(0, num);
					string text3 = text.Substring(num + 1);
					num = text3.IndexOf('~');
					if (num != -1)
					{
						text3.Substring(num + 1);
						text3 = text3.Substring(0, num);
					}
					else
					{
						_ = string.Empty;
					}
					if (text2.StartsWith("\"") && text2.EndsWith("\""))
					{
						text2 = text2.Substring(1);
						text2 = text2.Substring(0, text2.Length - 1);
					}
					if (object.Equals(text2, value.ToString()))
					{
						return text3;
					}
				}
			}
		}
		return value.ToString();
	}

	public void SetDefaultExpressionForRow(M1Database database, DataRow row)
	{
		if (FieldName.Length != 0)
		{
			if (!string.IsNullOrWhiteSpace(DefaultExpressionUserSetting))
			{
				row[FieldName] = Table.EvaluateScriptExpression(DefaultExpressionUserSetting, database, row);
			}
			else if (!string.IsNullOrWhiteSpace(DefaultExpressionUser))
			{
				row[FieldName] = Table.EvaluateScriptExpression(DefaultExpressionUser, database, row);
			}
			else if (!string.IsNullOrWhiteSpace(DefaultExpression))
			{
				row[FieldName] = Table.EvaluateScriptExpression(DefaultExpression, database, row);
			}
		}
	}

	private string GetDefaultWarehouseBin(M1Database database, string warehouseID)
	{
		object obj = null;
		using (SqlCommand sqlCommand = database.NewSqlCommand("SELECT inbWarehouseBinID FROM  WarehouseBins WHERE (inbWarehouseID = @WarehouseID) AND (inbDefaultBin = 1) AND (inbInactive = 0)"))
		{
			sqlCommand.Parameters.Add(new SqlParameter("@WarehouseID", SqlDbType.NVarChar)).Value = warehouseID;
			obj = database.ExecuteScalar(sqlCommand);
		}
		if (obj != null)
		{
			return obj.ToString();
		}
		return string.Empty;
	}

	private void OnDisposed()
	{
		if (this.Disposed != null)
		{
			this.Disposed(this, EventArgs.Empty);
		}
	}

	public void Dispose()
	{
		OnDisposed();
		this.AfterValueChanged = null;
		this.CaptionChanged = null;
		this.ColumnErrorChanged = null;
		this.ForeignKeyInvalid = null;
		this.ForeignKeyValid = null;
		this.Valid = null;
		this.RequiredChanged = null;
		this.ReadOnlyChanged = null;
		this.NoAccessChanged = null;
		this.ErrorTextChanged = null;
		this.ColumnErrorChanged = null;
		this.IsValidChanged = null;
		this.ValueChanged = null;
		this.Flash = null;
		if (RequiredExpressionReferencedFields != null)
		{
			RequiredExpressionReferencedFields.Clear();
			RequiredExpressionReferencedFields = null;
		}
		if (CalculationExpressionReferencedFields != null)
		{
			CalculationExpressionReferencedFields.Clear();
			CalculationExpressionReferencedFields = null;
		}
		if (BoundParentFieldExpressionReferencedFields != null)
		{
			BoundParentFieldExpressionReferencedFields.Clear();
			BoundParentFieldExpressionReferencedFields = null;
		}
		if (ReadOnlyExpressionReferencedFields != null)
		{
			ReadOnlyExpressionReferencedFields.Clear();
			ReadOnlyExpressionReferencedFields = null;
		}
		if (ReadOnlyExpressionRelatedTableReferencedFields != null)
		{
			ReadOnlyExpressionRelatedTableReferencedFields.Clear();
			ReadOnlyExpressionRelatedTableReferencedFields = null;
		}
		if (ValidCodeReferencedFields != null)
		{
			ValidCodeReferencedFields.Clear();
			ValidCodeReferencedFields = null;
		}
		if (ForeignKeyValidCodeReferencedFields != null)
		{
			ForeignKeyValidCodeReferencedFields.Clear();
			ForeignKeyValidCodeReferencedFields = null;
		}
		if (VisibleExpressionReferencedFields != null)
		{
			VisibleExpressionReferencedFields.Clear();
			VisibleExpressionReferencedFields = null;
		}
		if (CaptionExpressionReferencedFields != null)
		{
			CaptionExpressionReferencedFields.Clear();
			CaptionExpressionReferencedFields = null;
		}
		if (RelatedTableFilterReferencedFields != null)
		{
			RelatedTableFilterReferencedFields.Clear();
			RelatedTableFilterReferencedFields = null;
		}
		if (foreignUpdateHandler != null)
		{
			foreignUpdateHandler.Dispose();
			foreignUpdateHandler = null;
		}
		if (Databases != null)
		{
			Databases.Clear();
			Databases = null;
		}
		if (FieldExtensions != null)
		{
			foreach (FieldExtension fieldExtension in FieldExtensions)
			{
				fieldExtension.Dispose();
			}
			FieldExtensions.Clear();
			FieldExtensions = null;
		}
		if (FieldActions != null)
		{
			FieldActions.Clear();
			FieldActions = null;
		}
		if (Table != null)
		{
			Table.Dispose();
			Table = null;
		}
		errorList = null;
		BindingSource = null;
	}

	private object getValueForRow(DataRow curDataRow)
	{
		if (curDataRow[FieldName] == DBNull.Value)
		{
			if (AllowNulls)
			{
				return DBNull.Value;
			}
			return curDataRow.DefaultValueForType(curDataRow.Table.Columns[FieldName].DataType);
		}
		if (IsFieldTypeAString(FieldType))
		{
			return curDataRow.Field<string>(FieldName);
		}
		switch (FieldType)
		{
		case FieldTypeEnum.Bit:
			return curDataRow.Field<bool>(FieldName);
		case FieldTypeEnum.Int:
			return curDataRow.Field<int>(FieldName);
		case FieldTypeEnum.SmallInt:
			return curDataRow.Field<short>(FieldName);
		case FieldTypeEnum.Numeric:
			if (FieldDecimals == 0)
			{
				return (int)curDataRow.Field<decimal>(FieldName);
			}
			return (double)curDataRow.Field<decimal>(FieldName);
		case FieldTypeEnum.Money:
			return (double)curDataRow.Field<decimal>(FieldName);
		case FieldTypeEnum.Date:
		case FieldTypeEnum.DateTime:
			return curDataRow.Field<DateTime>(FieldName);
		case FieldTypeEnum.UniqueIdentifier:
			return curDataRow.Field<Guid>(FieldName).ToString();
		default:
			if (curDataRow.HasVersion(DataRowVersion.Proposed))
			{
				return curDataRow[FieldName, DataRowVersion.Proposed];
			}
			return curDataRow[FieldName, DataRowVersion.Current];
		}
	}

	private object getOriginalValueForRow(DataRow curDataRow)
	{
		if (curDataRow[FieldName, DataRowVersion.Original] == DBNull.Value)
		{
			return DBNull.Value;
		}
		if (IsFieldTypeAString(FieldType))
		{
			return curDataRow.Field<string>(FieldName, DataRowVersion.Original);
		}
		switch (FieldType)
		{
		case FieldTypeEnum.Bit:
			return curDataRow.Field<bool>(FieldName, DataRowVersion.Original);
		case FieldTypeEnum.Int:
			return curDataRow.Field<int>(FieldName, DataRowVersion.Original);
		case FieldTypeEnum.SmallInt:
			return curDataRow.Field<short>(FieldName, DataRowVersion.Original);
		case FieldTypeEnum.Numeric:
			if (FieldDecimals == 0)
			{
				return (int)curDataRow.Field<decimal>(FieldName, DataRowVersion.Original);
			}
			return (double)curDataRow.Field<decimal>(FieldName, DataRowVersion.Original);
		case FieldTypeEnum.Money:
			return (double)curDataRow.Field<decimal>(FieldName, DataRowVersion.Original);
		case FieldTypeEnum.Date:
		case FieldTypeEnum.DateTime:
			return curDataRow.Field<DateTime>(FieldName, DataRowVersion.Original);
		default:
			return curDataRow[FieldName, DataRowVersion.Original];
		}
	}

	public M1AdoLookupRow GetFields(string fields, object database, object row, object transaction)
	{
		return new M1AdoLookupRow
		{
			Row = RelatedTableGetDataRow(fields, (M1Database)database, (DataRow)row, alwaysReturnValidRow: true, (SqlTransaction)transaction)
		};
	}

	public M1AdoLookupRow RelatedTableGetAdoRecord(string fields)
	{
		M1AdoLookupRow m1AdoLookupRow = new M1AdoLookupRow();
		DbAndRowEventArgs currentDataRowForProcessingQuick = Table.GetCurrentDataRowForProcessingQuick();
		m1AdoLookupRow.Row = RelatedTableGetDataRow(fields, currentDataRowForProcessingQuick.Database, currentDataRowForProcessingQuick.Row, alwaysReturnValidRow: true, currentDataRowForProcessingQuick.SqlTransaction);
		return m1AdoLookupRow;
	}

	public bool RelatedTableRowExists()
	{
		return RelatedTableGetDataRow(RelatedTableKeyFields, null, null, alwaysReturnValidRow: false) != null;
	}
}
