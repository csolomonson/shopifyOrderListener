using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using M1.Core.Script;
using M1.Extensions;

namespace M1.Core;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IComM1BindingSource))]
[DefaultBindingProperty("ParentFieldValue")]
[DebuggerDisplay("{DataSourceTable}{DataSourceGridID}")]
public class M1BindingSource : BindingSource, ISupportInitialize, IComM1BindingSource, IServiceProvider, IBindableComponent, IComponent, IDisposable, IM1SupportFormatting
{
	public enum BindingSourceLinkTypeEnum : byte
	{
		StandaloneComponent = 1,
		CurrencyLink,
		BoundParentField,
		CodeReferencedLink
	}

	public class ValidateRemoveEventArgs : EventArgs
	{
		public bool Cancel;

		public M1Database Database;

		public DataRow Row;

		public ValidationInfo ValidationInfo;

		public bool TopLevel = true;

		public bool RemoveSkip;
	}

	public delegate void ValidateRemoveEventHandler(object sender, ValidateRemoveEventArgs e);

	private class RowKeyState
	{
		public bool KeyIsSet;

		public bool AutoIncremented;

		public RowKeyState(bool keyIsSet, bool autoIncremented)
		{
			KeyIsSet = keyIsSet;
			AutoIncremented = autoIncremented;
		}
	}

	public class QueryDatabaseEventArgs : EventArgs
	{
		public M1Database Database;

		public SqlTransaction Transaction;

		public DataRow TopLevelDataRow;

		public M1BindingSource TopLevelBindingSource;

		public TableCollection TopLevelTables;

		public DataRow ParentDataRow;

		public QueryDatabaseEventArgs(M1Database database)
		{
			Database = database;
		}

		public QueryDatabaseEventArgs(M1Database database, SqlTransaction transaction)
		{
			Database = database;
			Transaction = transaction;
		}

		public QueryDatabaseEventArgs(QueryDatabaseEventArgs args)
		{
			Database = args.Database;
			Transaction = args.Transaction;
			TopLevelDataRow = args.TopLevelDataRow;
			ParentDataRow = args.ParentDataRow;
		}
	}

	public delegate void SaveDataStartedEventHandler(object sender, SaveDataStartedEventArgs e);

	public class ChangedRowsInfo
	{
		public List<DataRow> AddedRows;

		public List<DataRow> ChangedRows;

		public List<DataRow> DeletedRows;

		public ChangedRowsInfo(DataTable table)
		{
			GetChangedRows(table);
		}

		public void GetChangedRows(DataTable table)
		{
			AddedRows = new List<DataRow>();
			ChangedRows = new List<DataRow>();
			DeletedRows = new List<DataRow>();
			if (table == null)
			{
				return;
			}
			foreach (DataRow row in table.Rows)
			{
				switch (row.RowState)
				{
				case DataRowState.Deleted:
					DeletedRows.Add(row);
					break;
				case DataRowState.Added:
					AddedRows.Add(row);
					break;
				case DataRowState.Modified:
					ChangedRows.Add(row);
					break;
				}
			}
		}
	}

	private class TableJoinInfo
	{
		public string JoinClause = string.Empty;

		public string LastParentField = string.Empty;

		public string ChildTable = string.Empty;

		public List<string> ParentFields;

		public List<string> ChildFields;

		public ReferencedFieldsList ChildReferencedFields;

		public TableJoinInfo(string joinClause, TableCollection tables, FieldCollection fields)
		{
			string text = joinClause.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ')
				.TrimStart(' ');
			string empty = string.Empty;
			string empty2 = string.Empty;
			int num = text.IndexOf(' ');
			if (num == -1)
			{
				return;
			}
			ChildTable = text.Substring(0, num);
			text = text.Substring(num + 1).TrimStart(' ');
			if (!tables.Contains(ChildTable))
			{
				return;
			}
			TableDefinition tableDefinition = tables[ChildTable];
			empty = tableDefinition.FieldPrefix;
			empty2 = tableDefinition.FieldPrefixUser;
			num = text.IndexOf("on ");
			if (num != -1)
			{
				text = text.Substring(num + 3).TrimStart(' ');
			}
			ParentFields = new List<string>();
			ChildFields = new List<string>();
			string[] array = text.ToLower().Split(new string[1] { " and " }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text2 in array)
			{
				num = text2.IndexOf('=');
				if (num != -1)
				{
					string text3 = text2.Substring(0, num).Trim();
					string item = text2.Substring(num + 1).Trim();
					if (text3.StartsWith(empty, StringComparison.CurrentCultureIgnoreCase) || (empty2.Length != 0 && text3.StartsWith(empty2, StringComparison.CurrentCultureIgnoreCase)))
					{
						ParentFields.Add(item);
						ChildFields.Add(text3);
					}
					else
					{
						ParentFields.Add(text3);
						ChildFields.Add(item);
					}
				}
			}
			if (ParentFields == null || ParentFields.Count == 0)
			{
				return;
			}
			LastParentField = ParentFields[ParentFields.Count - 1];
			foreach (FieldDefinition field in fields)
			{
				if (field.FieldName.StartsWith(empty, StringComparison.CurrentCultureIgnoreCase) || (empty2.Length != 0 && field.FieldName.StartsWith(empty2, StringComparison.CurrentCultureIgnoreCase)))
				{
					if (ChildReferencedFields == null)
					{
						ChildReferencedFields = new ReferencedFieldsList();
					}
					ChildReferencedFields.Add(field.FieldName);
				}
			}
		}

		public void RefreshChildFieldsInDataRow(M1BindingSource bs, M1Database database, DataRow row, SqlTransaction sqlTransaction)
		{
			if (ChildReferencedFields == null || ChildReferencedFields.Count == 0)
			{
				return;
			}
			DataRow dataRow = bs.Fields[LastParentField].RelatedTableGetDataRow(ChildReferencedFields.FieldList(), database, row, alwaysReturnValidRow: true, sqlTransaction);
			foreach (string childReferencedField in ChildReferencedFields)
			{
				row[childReferencedField] = dataRow[childReferencedField];
			}
		}
	}

	public class FlashRowEventArgs : EventArgs
	{
		public DataRow Row;
	}

	private class M1Binding
	{
		public Binding DefaultBinding;

		public PropertyInfo ReflectedPropertyInfo;

		public EventInfo ReflectedEventInfo;

		private bool inReadValue;

		private bool inWriteValue;

		public void Attach(Binding binding)
		{
			DefaultBinding = binding;
			ReflectedPropertyInfo = binding.BindableComponent.GetType().GetProperty(binding.PropertyName);
			ReflectedEventInfo = binding.BindableComponent.GetType().GetEvent(binding.PropertyName + "Changed");
			if (ReflectedEventInfo != null)
			{
				ReflectedEventInfo.AddEventHandler(binding.BindableComponent, new EventHandler(ChangedHandler));
			}
		}

		private void ChangedHandler(object sender, EventArgs e)
		{
			WriteValue();
		}

		public void ReadValue()
		{
			DataRowView dataRowView = (DataRowView)((M1BindingSource)DefaultBinding.DataSource).Current;
			if (dataRowView != null && !inReadValue)
			{
				inReadValue = true;
				object obj = dataRowView.Row[DefaultBinding.BindingMemberInfo.BindingField];
				if (obj is DBNull)
				{
					ReflectedPropertyInfo.SetValue(DefaultBinding.BindableComponent, null, null);
				}
				else
				{
					ReflectedPropertyInfo.SetValue(DefaultBinding.BindableComponent, obj, null);
				}
				inReadValue = false;
			}
		}

		public void WriteValue()
		{
			if (inReadValue || inWriteValue || ((M1BindingSource)DefaultBinding.DataSource).LoadingData)
			{
				return;
			}
			DataRowView dataRowView = (DataRowView)((M1BindingSource)DefaultBinding.DataSource).Current;
			if (dataRowView != null)
			{
				inWriteValue = true;
				DataRow row = dataRowView.Row;
				object value = ReflectedPropertyInfo.GetValue(DefaultBinding.BindableComponent, null);
				if (ReflectedPropertyInfo.PropertyType == typeof(string))
				{
					row.SetField(DefaultBinding.BindingMemberInfo.BindingField, (string)((value == null) ? DefaultBinding.DataSourceNullValue : value));
				}
				else
				{
					row[DefaultBinding.BindingMemberInfo.BindingField] = ((value == null) ? DefaultBinding.DataSourceNullValue : value);
				}
				inWriteValue = false;
			}
		}

		private void testDelegate(object state)
		{
			DataRowView dataRowView = (DataRowView)((M1BindingSource)DefaultBinding.DataSource).Current;
			if (dataRowView != null)
			{
				inWriteValue = true;
				DataRow row = dataRowView.Row;
				object value = ReflectedPropertyInfo.GetValue(DefaultBinding.BindableComponent, null);
				if (ReflectedPropertyInfo.PropertyType == typeof(string))
				{
					row.SetField(DefaultBinding.BindingMemberInfo.BindingField, (string)((value == null) ? DefaultBinding.DataSourceNullValue : value));
				}
				else
				{
					row[DefaultBinding.BindingMemberInfo.BindingField] = ((value == null) ? DefaultBinding.DataSourceNullValue : value);
				}
				inWriteValue = false;
			}
		}
	}

	public class ValidateArgs : EventArgs
	{
		public ErrorItemsList Errors;
	}

	public class FocusFieldEventArgs : EventArgs
	{
		public string FocusField = string.Empty;

		public DataRow FocusRow;

		public FocusFieldEventArgs(string focusField, DataRow focusRow)
		{
			FocusField = focusField;
			FocusRow = focusRow;
		}
	}

	private AppContext _AppContext;

	private TableDefinition _PrimaryTable;

	public TableCollection Tables = new TableCollection();

	public QueryDefinition Query = new QueryDefinition();

	private FieldCollection _Fields = new FieldCollection();

	public bool SkipForeignKeyChecks;

	public bool RunningFromWeb;

	private string _ViewID = string.Empty;

	private bool isManuallyAddedBs;

	private string _AdditionalWhere = string.Empty;

	private SqlTransaction _Transaction;

	public List<BindingSourceLinkTypeEnum> BindingSourceLinks = new List<BindingSourceLinkTypeEnum>();

	public List<M1BindingSource> ChildBindingSources = new List<M1BindingSource>();

	private bool checkedOnQuery;

	public bool manuallyLoadedDataTable;

	private bool _IsTopLevel = true;

	public SimpleDatabaseCollection Databases = new SimpleDatabaseCollection();

	private bool _IsDefinitionLoaded;

	private object prevCurrent;

	private int prevPosition = -1;

	private bool _InAddNew;

	private bool modifiedLocked;

	private DataRow currentNewRow;

	private string _DataSourceGridID = string.Empty;

	private string _DataSourceTable = string.Empty;

	private string _CurrencyMode = string.Empty;

	private bool _IsCurrencyAvailable;

	private bool inInit;

	private bool inSetDefaultValues;

	private Dictionary<DataRow, RowKeyState> newRowsKeyState;

	private bool _Modified;

	[Browsable(false)]
	public bool LoadingData;

	public QueryDatabaseEventArgs LastQueryEventArgs;

	private List<string> loadedTopLevelQueries = new List<string>();

	public ChangedRowsInfo ChangedRows;

	private DataChangedEventArgs delayedDataChangedArgs;

	private List<TableChangedEventArgs> delayedTableEventArgsList = new List<TableChangedEventArgs>();

	private static bool InEvaluateChangeCode;

	private bool currentColumnChanged;

	private object prevColumnValue;

	private bool settingToPrevious;

	private bool settingRelatedValues;

	private bool inChangeCode;

	private ValidationInfo tempValidationInfo = new ValidationInfo();

	private bool setLastKeyOverride;

	private List<TableJoinInfo> tableJoins;

	private List<M1Binding> M1Bindings = new List<M1Binding>();

	private object[] prevRelatedFieldValues;

	private DataRow prevParentDataRow;

	private int prevRelatedPosition = -1;

	private string _ChildLinkField = string.Empty;

	private object _ParentFieldValue = string.Empty;

	private string _AutoRemoveWhereOnSave = string.Empty;

	private int _NumberOfChildRowsToForce;

	private bool inSetRowCount;

	public ErrorItemsList Errors;

	private object[] _Parameters;

	private BindingContext bindingContext;

	private M1ControlBindingsCollection dataBindings;

	public M1BindingSource PrimaryBindingSource;

	private FieldDefinition boundFieldDefinition;

	private FieldDefinition prevFieldDefinition;

	private string bindingDeleteFilter;

	private bool _DoCascadeRemoveForForeignRelation;

	[Browsable(false)]
	[DefaultValue(null)]
	public M1User User { get; set; }

	[Browsable(false)]
	[DefaultValue(null)]
	public M1Database Database { get; set; }

	[Browsable(false)]
	[DefaultValue(null)]
	public M1DataDictionary DataDictionary { get; set; }

	[Browsable(false)]
	[DefaultValue(null)]
	public AppContext Context
	{
		get
		{
			return _AppContext;
		}
		set
		{
			_AppContext = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TableDefinition PrimaryTable
	{
		get
		{
			return _PrimaryTable;
		}
		set
		{
			if (_PrimaryTable != null)
			{
				_PrimaryTable.DisableAddNewChanged -= PrimaryTable_DisableAddNewChanged;
				_PrimaryTable.DisableDeleteChanged -= PrimaryTable_DisableDeleteChanged;
				_PrimaryTable.ReadOnlyChanged -= PrimaryTable_ReadOnlyChanged;
			}
			_PrimaryTable = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public FieldCollection Fields
	{
		get
		{
			return _Fields;
		}
		set
		{
			_Fields = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	public string AdditionalWhere
	{
		get
		{
			return _AdditionalWhere;
		}
		set
		{
			_AdditionalWhere = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public SqlTransaction Transaction
	{
		get
		{
			if (_Transaction == null && boundFieldDefinition != null && boundFieldDefinition.BindingSource != null)
			{
				return boundFieldDefinition.BindingSource.Transaction;
			}
			return _Transaction;
		}
		set
		{
			_Transaction = value;
		}
	}

	public override ISite Site
	{
		get
		{
			return base.Site;
		}
		set
		{
			if (base.Site != value)
			{
				base.Site = value;
				if (base.Site != null && base.Site.Container is IServiceProvider)
				{
					SetContext(base.Site.Container as IServiceProvider);
				}
			}
		}
	}

	[Browsable(false)]
	public bool IsTopLevel
	{
		get
		{
			return _IsTopLevel;
		}
		private set
		{
			_IsTopLevel = value;
		}
	}

	[Browsable(false)]
	public bool IsDefinitionLoaded => _IsDefinitionLoaded;

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool InAddNew => _InAddNew;

	[Description("Specifies the query definition used to generate the query to be used for this binding source. Note that only one of the DataSourceGridID or DataSourceTable properties should be set, not both.")]
	[DefaultValue("")]
	public virtual string DataSourceGridID
	{
		get
		{
			return _DataSourceGridID;
		}
		set
		{
			_DataSourceGridID = value;
			if (_DataSourceGridID.Length != 0)
			{
				LoadDefinition(_DataSourceGridID);
			}
		}
	}

	[Description("Specifies the name of the table used to generate the query to be used for this binding source. Note that only one of the DataSourceGridID or DataSourceTable properties should be set, not both.")]
	[DefaultValue("")]
	public virtual string DataSourceTable
	{
		get
		{
			return _DataSourceTable;
		}
		set
		{
			_DataSourceTable = value;
			if (_DataSourceTable.Length != 0)
			{
				if (base.DesignMode)
				{
					LoadDefinition(string.Empty, _DataSourceTable, null);
				}
				else
				{
					LoadDefinition(string.Empty, _DataSourceTable, null, true);
				}
			}
			OnDataSourceTableChanged(EventArgs.Empty);
		}
	}

	[Browsable(false)]
	public M1CurrencyStyle CurrencyStyle
	{
		get
		{
			if (!CurrencyMode.Equals("FOREIGN", StringComparison.CurrentCultureIgnoreCase))
			{
				return M1CurrencyStyle.Base;
			}
			return M1CurrencyStyle.Foreign;
		}
	}

	[DefaultValue("")]
	[Browsable(false)]
	public string CurrencyMode
	{
		get
		{
			return _CurrencyMode;
		}
		set
		{
			if (_CurrencyMode != value)
			{
				_CurrencyMode = value;
				OnCurrencyModeChanged(EventArgs.Empty);
			}
		}
	}

	[Browsable(false)]
	public bool IsCurrencyAvailable => _IsCurrencyAvailable;

	[DefaultValue(false)]
	[Browsable(false)]
	public bool Modified
	{
		get
		{
			return _Modified;
		}
		set
		{
			if (_Modified != value)
			{
				_Modified = value;
				OnModifiedChanged(EventArgs.Empty);
				if (_Modified && boundFieldDefinition != null && boundFieldDefinition.BindingSource != null)
				{
					boundFieldDefinition.BindingSource.Modified = true;
				}
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool InSaveData { get; private set; }

	[Browsable(false)]
	public M1Database CurrentDatabase
	{
		get
		{
			DataRow currentAsDataRow = CurrentAsDataRow;
			if (currentAsDataRow != null && currentAsDataRow.Table.Columns.Contains("Dataset"))
			{
				return Databases[currentAsDataRow.Field<string>("Dataset")];
			}
			return Database;
		}
	}

	[Browsable(false)]
	public DataRow CurrentAsDataRow
	{
		get
		{
			if (currentNewRow != null)
			{
				return currentNewRow;
			}
			if (base.Position == -1 || Count == 0)
			{
				return null;
			}
			try
			{
				_ = base.Current;
			}
			catch (Exception)
			{
				return null;
			}
			if (base.Current is DataRow)
			{
				return (DataRow)base.Current;
			}
			if (base.Current is DataRowView)
			{
				return ((DataRowView)base.Current).Row;
			}
			return null;
		}
		private set
		{
		}
	}

	[Bindable(true)]
	[Browsable(true)]
	[DefaultValue("")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Description("Specifies the field from the current table to be used to filter when bound to a parent table.")]
	public string ChildLinkField
	{
		get
		{
			return _ChildLinkField;
		}
		set
		{
			_ChildLinkField = value;
		}
	}

	[Bindable(true)]
	[Browsable(true)]
	[DefaultValue("")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Description("Binding a field will load the binding source and filter the data on the key field(s) of the data matching the bound value.")]
	public object ParentFieldValue
	{
		get
		{
			return _ParentFieldValue;
		}
		set
		{
			if (boundFieldDefinition != null && boundFieldDefinition.BindingSource != null && !isManuallyAddedBs && !boundFieldDefinition.BindingSource.LoadingData)
			{
				DataRow currentAsDataRow = boundFieldDefinition.BindingSource.CurrentAsDataRow;
				M1Database currentDatabase = boundFieldDefinition.BindingSource.CurrentDatabase;
				if (currentAsDataRow == null)
				{
					parentBindingSourceNoRecordEvent();
				}
				else
				{
					PrimaryTable.SetDisableAddNewOverride(value: false, Database, CurrentAsDataRow, Transaction);
					bool flag = false;
					string[] boundRelatedAndCurrentFields = getBoundRelatedAndCurrentFields();
					if (prevRelatedFieldValues == null || prevParentDataRow == null || (prevParentDataRow != null && prevParentDataRow.RowState == DataRowState.Detached))
					{
						flag = true;
						prevRelatedFieldValues = new object[boundRelatedAndCurrentFields.Length];
					}
					else
					{
						for (int i = 0; i < boundRelatedAndCurrentFields.Length; i++)
						{
							if (!prevRelatedFieldValues[i].Equals(prevParentDataRow[boundRelatedAndCurrentFields[i]]))
							{
								flag = true;
								break;
							}
						}
					}
					if (flag || currentAsDataRow != prevParentDataRow)
					{
						if (flag && (currentAsDataRow == prevParentDataRow || prevParentDataRow == null))
						{
							PrimaryTable.ParentLastKeyField_ValueChanged(null, new FieldDefinition.FieldValueChangedEventArgs(currentDatabase, currentAsDataRow, isCurrentRow: true, _ParentFieldValue, Transaction));
						}
						((DataView)base.DataSource).RowFilter = PrimaryTable.GetFilterForParentRowUsingCurrentFieldNames(currentAsDataRow);
						OnAfterChildFilterSet(EventArgs.Empty);
						if (NumberOfChildRowsToForce != 0 && NumberOfChildRowsToForce > Count && NumberOfChildRowsToForce != 0 && NumberOfChildRowsToForce > Count)
						{
							SetRowCount(NumberOfChildRowsToForce, markModified: false);
							ResequenceKeys(markModified: false);
						}
						for (int j = 0; j < boundRelatedAndCurrentFields.Length; j++)
						{
							prevRelatedFieldValues[j] = currentAsDataRow[boundRelatedAndCurrentFields[j]];
						}
						prevParentDataRow = currentAsDataRow;
					}
				}
				PrimaryTable.SetDisableDeleteOverride(base.List.Count == 0, Database, CurrentAsDataRow, Transaction);
				prevRelatedPosition = boundFieldDefinition.BindingSource.Position;
			}
			_ParentFieldValue = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Specifies the expression to use to remove rows from a child binding source before saving. If this causes the number of rows to be less than the NumberOfChildRowsToForce, the rows will be added back after the save.")]
	public string AutoRemoveWhereOnSave
	{
		get
		{
			return _AutoRemoveWhereOnSave;
		}
		set
		{
			_AutoRemoveWhereOnSave = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(0)]
	[Description("Specifies the number of rows that this binding source should have when bound to a parent binding source. This will add any missing rows automatically whenever the parent field value changes.")]
	public int NumberOfChildRowsToForce
	{
		get
		{
			return _NumberOfChildRowsToForce;
		}
		set
		{
			_NumberOfChildRowsToForce = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public M1AdoRecordsetProxy Recordset
	{
		get
		{
			return GetRecordset();
		}
		set
		{
			base.DataSource = value;
		}
	}

	public bool ReadOnly => PrimaryTable.ReadOnlyResolved;

	public override bool AllowEdit
	{
		get
		{
			if (PrimaryTable != null)
			{
				return !PrimaryTable.ReadOnlyResolved;
			}
			return base.AllowEdit;
		}
	}

	[Browsable(false)]
	public override bool AllowNew
	{
		get
		{
			if (PrimaryTable != null)
			{
				return !PrimaryTable.DisableAddNewResolved;
			}
			return base.AllowNew;
		}
		set
		{
			base.AllowNew = value;
		}
	}

	[Browsable(false)]
	public override bool AllowRemove
	{
		get
		{
			if (PrimaryTable != null)
			{
				return !PrimaryTable.DisableDeleteResolved;
			}
			return base.AllowRemove;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object[] Parameters
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object ParametersLength
	{
		get
		{
			if (_Parameters != null)
			{
				return _Parameters.Length;
			}
			return 0;
		}
	}

	[Browsable(false)]
	public BindingContext BindingContext
	{
		get
		{
			if (bindingContext == null)
			{
				bindingContext = new BindingContext();
			}
			return bindingContext;
		}
		set
		{
			bindingContext = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[ParenthesizePropertyName(true)]
	public ControlBindingsCollection DataBindings
	{
		get
		{
			if (dataBindings == null)
			{
				dataBindings = new M1ControlBindingsCollection(this);
			}
			return dataBindings;
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public FieldDefinition BoundFieldDefinition => boundFieldDefinition;

	[DefaultValue(false)]
	[Description("Specifies if the rows in this binding source should be deleted when it is bound to a parent binding source where the join is a foreign key (not a child table).")]
	public bool DoCascadeRemoveForForeignRelation
	{
		get
		{
			return _DoCascadeRemoveForForeignRelation;
		}
		set
		{
			_DoCascadeRemoveForForeignRelation = value;
		}
	}

	public event EventHandler<ActionMessagesEventArgs> ActionMessage;

	public event EventHandler ChildBindingSourcesAddCompleted;

	public event EventHandler RecordChange;

	public event EventHandler LoadDefinitionCompleted;

	public event ValidateRemoveEventHandler ValidateRemove;

	public event EventHandler<RemoveEventArgs> RemoveStarted;

	public event EventHandler<RemoveEventArgs> RemoveCompleted;

	public event EventHandler<AddNewCompletedEventArgs> AddNewCompleted;

	public event EventHandler DataSourceTableChanged;

	public event EventHandler CurrencyModeChanged;

	public event EventHandler ModifiedChanged;

	public event EventHandler<GetNextIDEventArgs> GetNextID;

	public event EventHandler<QueryDatabaseEventArgs> QueryDatabase;

	public event EventHandler<QueryDatabaseEventArgs> RowActivated;

	public event EventHandler NavigateAway;

	public event EventHandler EditCancelled;

	public event EventHandler<SaveDataCompletedEventArgs> SaveDataCompleted;

	public event SaveDataStartedEventHandler SaveDataStarted;

	public event EventHandler<SaveDataStartedEventArgs> ChangedRowsInit;

	public event EventHandler<DataChangedEventArgs> DataChanged;

	public event EventHandler<RowUpdateEventArgs> RowUpdateSaveBefore;

	public event EventHandler<RowUpdateEventArgs> RowUpdateSaveAfter;

	public event EventHandler<RowUpdateEventArgs> RowUpdateAddBefore;

	public event EventHandler<RowUpdateEventArgs> RowUpdateAddAfter;

	public event EventHandler<RowUpdateEventArgs> RowUpdateDeleteBefore;

	public event EventHandler<RowUpdateEventArgs> RowUpdateDeleteAfter;

	public event EventHandler<FieldChangedEventArgs> MemoAlertFieldChanged;

	public event EventHandler<FieldChangedEventArgs> PrimaryContactChanged;

	public event EventHandler LastKeyFieldValueChanged;

	public event EventHandler<FlashRowEventArgs> FlashRow;

	public event EventHandler AfterChildFilterSet;

	public event EventHandler<FieldDefinition.ColumnErrorChangedEventArgs> ColumnErrorChanged;

	public event EventHandler CacheCleared;

	public event EventHandler<ValidateArgs> Validate;

	public event EventHandler ErrorsChanged;

	public event EventHandler AllowEditChanged;

	public event EventHandler AllowNewChanged;

	public event EventHandler AllowRemoveChanged;

	public event EventHandler<FocusFieldEventArgs> FocusField;

	public event EventHandler BoundFieldDefinitionChanged;

	public M1BindingSource(IServiceProvider container)
		: this()
	{
		SetContext(container);
	}

	public M1BindingSource(IServiceProvider container, bool isManuallyAdded)
		: this()
	{
		isManuallyAddedBs = isManuallyAdded;
		SetContext(container);
	}

	public M1BindingSource()
	{
		doInit();
	}

	public M1BindingSource(IServiceProvider container, SqlTransaction transaction)
		: this()
	{
		_Transaction = transaction;
		SetContext(container);
	}

	private void doInit()
	{
		DataSourceGridID = string.Empty;
		PrimaryBindingSource = this;
		CurrencyManager.Bindings.CollectionChanged += Bindings_CollectionChanged;
		BindingSourceLinks.Add(BindingSourceLinkTypeEnum.StandaloneComponent);
	}

	protected void SetContext(IServiceProvider provider)
	{
		if (provider != null)
		{
			User = provider.GetService(typeof(M1User)) as M1User;
			Database = provider.GetService(typeof(M1Database)) as M1Database;
			DataDictionary = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
			Context = provider.GetService(typeof(AppContext)) as AppContext;
		}
	}

	public void ResetProperties()
	{
		this.RowActivated = null;
		this.CacheCleared = null;
		this.EditCancelled = null;
		this.ActionMessage = null;
		this.ChildBindingSourcesAddCompleted = null;
		this.RecordChange = null;
		this.AddNewCompleted = null;
		this.DataSourceTableChanged = null;
		this.GetNextID = null;
		this.NavigateAway = null;
		this.ChangedRowsInit = null;
		this.DataChanged = null;
		this.MemoAlertFieldChanged = null;
		this.PrimaryContactChanged = null;
		this.LastKeyFieldValueChanged = null;
		this.AfterChildFilterSet = null;
		_IsDefinitionLoaded = false;
		this.LoadDefinitionCompleted = null;
		this.ValidateRemove = null;
		this.RemoveStarted = null;
		this.RemoveCompleted = null;
		this.CurrencyModeChanged = null;
		this.ModifiedChanged = null;
		this.QueryDatabase = null;
		LastQueryEventArgs = null;
		this.SaveDataCompleted = null;
		this.SaveDataStarted = null;
		this.RowUpdateSaveBefore = null;
		this.RowUpdateSaveAfter = null;
		this.RowUpdateAddBefore = null;
		this.RowUpdateAddAfter = null;
		this.RowUpdateDeleteBefore = null;
		this.RowUpdateDeleteAfter = null;
		this.FlashRow = null;
		this.ColumnErrorChanged = null;
		this.Validate = null;
		this.ErrorsChanged = null;
		this.AllowEditChanged = null;
		this.AllowNewChanged = null;
		this.AllowRemoveChanged = null;
		this.FocusField = null;
		if (Databases != null)
		{
			Databases.Clear();
		}
		_DataSourceTable = null;
		if (ChildBindingSources != null)
		{
			ChildBindingSources.Clear();
		}
		boundFieldDefinition = null;
		if (dataBindings != null)
		{
			dataBindings.Clear();
		}
		if (Fields != null)
		{
			foreach (FieldDefinition field in Fields)
			{
				field.Dispose();
			}
			Fields.Clear();
		}
		if (M1Bindings != null)
		{
			M1Bindings.Clear();
		}
		if (Tables != null)
		{
			foreach (TableDefinition table in Tables)
			{
				table.Dispose();
			}
			Tables.Clear();
		}
		base.CurrentChanged -= M1BindingSource_CurrentChanged;
		tempValidationInfo = null;
		prevCurrent = null;
		prevPosition = -1;
		_DataSourceGridID = string.Empty;
		_DataSourceTable = string.Empty;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		dataBindings = null;
		if (_Fields != null)
		{
			_Fields.Clear();
			_Fields = null;
		}
		if (Tables != null)
		{
			if (disposing)
			{
				Tables.Clear();
			}
			Tables = null;
		}
		if (ChildBindingSources != null)
		{
			ChildBindingSources.Clear();
			ChildBindingSources = null;
		}
		if (M1Bindings != null)
		{
			M1Bindings.Clear();
			M1Bindings = null;
		}
		if (PrimaryTable != null)
		{
			PrimaryTable.Dispose();
			PrimaryTable = null;
		}
		ResetProperties();
		GC.Collect();
		GC.SuppressFinalize(this);
	}

	private void Bindings_CollectionChanged(object sender, CollectionChangeEventArgs e)
	{
		if (e.Action != CollectionChangeAction.Add || e.Element == null || !(e.Element is Binding))
		{
			return;
		}
		Binding binding = (Binding)e.Element;
		binding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
		if (!(binding.BindableComponent is IM1SupportFormatting) || Fields == null || !Fields.Contains(binding.BindingMemberInfo.BindingField))
		{
			return;
		}
		FieldDefinition fieldDefinition = Fields[binding.BindingMemberInfo.BindingField];
		((IM1SupportFormatting)binding.BindableComponent).BindToFieldDefinition(fieldDefinition, binding.PropertyName);
		if (binding.BindableComponent is M1BindingSource)
		{
			M1BindingSource item = (M1BindingSource)binding.BindableComponent;
			if (!ChildBindingSources.Contains(item))
			{
				ChildBindingSources.Add(item);
				OnChildBindingSourcesAddCompleted(EventArgs.Empty);
			}
		}
	}

	private void OnActionMessage(ActionMessagesEventArgs e)
	{
		this.ActionMessage?.Invoke(this, e);
	}

	public void GenerateActionMessage(string messageID, object parameters, object parametersEx)
	{
		ActionMessagesEventArgs e = new ActionMessagesEventArgs(messageID, (object[])parameters, (object[])parametersEx);
		OnActionMessage(e);
	}

	public void OnChildBindingSourcesAddCompleted(EventArgs e)
	{
		this.ChildBindingSourcesAddCompleted?.Invoke(this, e);
	}

	public void TransferBindings(M1BindingSource bsToUse)
	{
		bsToUse.CurrencyManager.Bindings.CollectionChanged -= ForeignBindings_CollectionChanged;
		bsToUse.CurrencyManager.Bindings.CollectionChanged += ForeignBindings_CollectionChanged;
		for (int num = bsToUse.CurrencyManager.Bindings.Count - 1; num >= 0; num--)
		{
			Binding binding = bsToUse.CurrencyManager.Bindings[num];
			if (binding.DataSource == bsToUse)
			{
				IBindableComponent bindableComponent = binding.BindableComponent;
				binding.BindableComponent.DataBindings.Remove(binding);
				bindableComponent.DataBindings.Add(new Binding(binding.PropertyName, this, binding.BindingMemberInfo.BindingMember, binding.PropertyName.Equals("Image"), binding.DataSourceUpdateMode, binding.NullValue, binding.FormatString, binding.FormatInfo));
			}
		}
	}

	private void ForeignBindings_CollectionChanged(object sender, CollectionChangeEventArgs e)
	{
		if (e.Action == CollectionChangeAction.Add && e.Element != null && e.Element is Binding)
		{
			Binding binding = (Binding)e.Element;
			IBindableComponent bindableComponent = binding.BindableComponent;
			binding.BindableComponent.DataBindings.Remove(binding);
			bindableComponent.DataBindings.Add(new Binding(binding.PropertyName, this, binding.BindingMemberInfo.BindingMember, binding.FormattingEnabled, binding.DataSourceUpdateMode, binding.NullValue, binding.FormatString, binding.FormatInfo));
		}
	}

	private void VerifyRelatedBindingSources(bool overrideForceLoad = false)
	{
		IsTopLevel = boundFieldDefinition == null;
		M1BindingSource m1BindingSource = null;
		if (boundFieldDefinition != null)
		{
			m1BindingSource = boundFieldDefinition.BindingSource;
		}
		foreach (TableDefinition table in Tables)
		{
			if (table == PrimaryTable && (!Query.AllowEditingOverride.HasValue || Query.AllowEditingOverride.Value))
			{
				table.VerifyChildBindingSources(ChildBindingSources);
				table.VerifyParentBindingSource(m1BindingSource, forceParentLoad: false, overrideForceLoad);
				table.CheckBindToParentFields(m1BindingSource);
			}
		}
		if (m1BindingSource != null)
		{
			m1BindingSource.Validate += parentBindingSource_Validate;
			m1BindingSource.ErrorsChanged += parentBindingSource_ErrorsChanged;
		}
		if (!checkedOnQuery && boundFieldDefinition != null && boundFieldDefinition.BindingSource != null && boundFieldDefinition.BindingSource.LastQueryEventArgs != null)
		{
			checkedOnQuery = true;
			CurrencyMode = boundFieldDefinition.BindingSource.CurrencyMode;
			BindingSource_RowActivated(boundFieldDefinition.BindingSource, boundFieldDefinition.BindingSource.LastQueryEventArgs);
		}
	}

	public void CheckQueryDatabaseForCurrencyLink()
	{
		if (boundFieldDefinition == null && prevFieldDefinition != null)
		{
			BindToFieldDefinition(prevFieldDefinition, string.Empty);
			prevFieldDefinition = null;
			CurrencyMode = boundFieldDefinition.BindingSource.CurrencyMode;
			if (boundFieldDefinition.BindingSource.LastQueryEventArgs != null)
			{
				BindingSource_RowActivated(boundFieldDefinition.BindingSource, boundFieldDefinition.BindingSource.LastQueryEventArgs);
			}
		}
	}

	public DataTable GetBaseTable()
	{
		return Query.DataView.Table;
	}

	private void blankOutProperties()
	{
		Tables.Clear();
		Fields.Clear();
		Fields.Add(new FieldDefinition(null, null, null, null)
		{
			Caption = "<All>"
		});
	}

	private void parentBindingSource_ErrorsChanged(object sender, EventArgs e)
	{
		if (sender is M1BindingSource m1BindingSource)
		{
			Errors = m1BindingSource.Errors;
		}
	}

	private void parentBindingSource_Validate(object sender, ValidateArgs e)
	{
		OnValidate(e);
	}

	private void parentFieldDef_DisableAddNewExpression_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		PrimaryTable.EvaluateDisableAddNewExpression(e.Database, e.Row, null);
	}

	public void ProcessBoundParentValueChange(FieldDefinition parentFieldDef, FieldDefinition childFieldDef, M1Database database, DataRow parentRow)
	{
		if (Query.DataView.Table != null)
		{
			DataRow[] array = Query.DataView.Table.Select(PrimaryTable.GetFilterForParentRowUsingCurrentFieldNames(parentRow));
			foreach (DataRow currentDataRow in array)
			{
				childFieldDef.ProcessBoundParentFieldForRow(parentRow, database, currentDataRow);
			}
		}
	}

	private string checkQueryForExpressions(string query)
	{
		for (int num = query.IndexOf("{*"); num != -1; num = query.IndexOf("{*"))
		{
			int num2 = query.IndexOf("*}");
			if (num2 == -1)
			{
				break;
			}
			string expr = query.Substring(num + 2, num2 - num - 2);
			string fieldFromExpr = getFieldFromExpr(expr);
			query = query.Substring(0, num) + fieldFromExpr + query.Substring(num2 + 2);
		}
		return query;
	}

	private string getFieldFromExpr(string expr)
	{
		int num = expr.IndexOf('(');
		if (num != -1)
		{
			int num2 = expr.IndexOf(')');
			if (num2 != -1)
			{
				return expr.Substring(num + 1, num2 - num - 1);
			}
		}
		return expr;
	}

	public bool LoadDefinition(string gridID)
	{
		return LoadDefinition(gridID, string.Empty, null);
	}

	public bool LoadDefinition(string gridID, string table, DataTable dataTableToLoad)
	{
		return LoadDefinition(gridID, table, dataTableToLoad, null, loadDataNow: false);
	}

	public bool LoadDefinition(string gridID, string table, DataTable dataTableToLoad, bool? allowEditing)
	{
		IServiceProvider provider;
		if (Database != null)
		{
			IServiceProvider database = Database;
			provider = database;
		}
		else
		{
			IServiceProvider database = this;
			provider = database;
		}
		QueryDefinition queryDefinition = new QueryDefinition(provider, gridID, table);
		queryDefinition.DataView = new DataView(dataTableToLoad);
		return LoadDefinition(queryDefinition, allowEditing, queryDefinition.LoadGridOnOpen);
	}

	public bool LoadDefinition(string gridID, string table, DataTable dataTableToLoad, bool? allowEditing, bool loadDataNow)
	{
		QueryDefinition queryDefinition = new QueryDefinition(this, gridID, table);
		queryDefinition.DataView = new DataView(dataTableToLoad);
		return LoadDefinition(queryDefinition, allowEditing, loadDataNow);
	}

	public bool LoadDefinition(QueryDefinition queryDef, bool? allowEditing, bool loadDataNow)
	{
		blankOutProperties();
		Query = queryDef;
		Query.AllowEditingOverride = allowEditing;
		bool flag = Query.IsEditable();
		_DataSourceGridID = queryDef.GridID;
		if (queryDef.DataView == null || queryDef.DataView.Table == null)
		{
			DataTable dataTable = new DataTable();
			if (base.DesignMode || Context == null)
			{
				DataTable fieldsTable = getFieldsTable(Query.TableName);
				if (fieldsTable.Rows.Count != 0)
				{
					foreach (DataRow row in fieldsTable.Rows)
					{
						dataTable.Columns.Add(new DataColumn(getFieldName(row.Field<string>("dffield").TrimEnd(), row.Field<string>("dfDisplayName").TrimEnd()), getFieldType(FieldDefinition.charToFieldType(row.Field<string>("dfDBType")))));
					}
				}
				dataTable.TableName = Query.TableName;
			}
			else
			{
				if (Database == null)
				{
					throw new M1Exception("No database has been opened for this binding source.");
				}
				loginToDatabases(flag);
				if (Databases.Count > 1)
				{
					flag = false;
				}
				try
				{
					if (Query.FromClause.Length == 0)
					{
						dataTable = new DataTable();
						if (Query.NoPrimaryTable)
						{
							Query.KeyFields = string.Empty;
							if (Query.Command != null)
							{
								dataTable = Database.GetDataTable(Query.Command, flag, out Query.DataAdapter);
							}
							else
							{
								string additionalFilter = Query.AdditionalFilter;
								dataTable = Database.GetDataTable(Database.PrepareQuery(additionalFilter), flag, out Query.DataAdapter);
							}
							IsTopLevel = false;
						}
					}
					else
					{
						string constructedSqlQuery = Query.GetConstructedSqlQuery(Database, getAdditionalFieldsForQuery(), loadNow: false, string.Empty);
						try
						{
							dataTable = ((!Query.UseDataDictionary) ? Database.GetDataTable(Database.PrepareQuery(constructedSqlQuery), flag, out Query.DataAdapter, Transaction) : DataDictionary.GetDataTable(Database.PrepareQuery(constructedSqlQuery), flag, out Query.DataAdapter));
						}
						catch (SqlException)
						{
							string text = Query.RemoveInvalidFields(Database);
							if (text.Length == 0)
							{
								throw;
							}
							Query.SaveItemToDDGridDetails(DataDictionary, User);
							Database.OnShowError(new ShowErrorEventArgs("The field(s) '" + text + "' don't exist in any of the selected tables and have been removed from the grid."));
							constructedSqlQuery = Query.GetConstructedSqlQuery(Database, getAdditionalFieldsForQuery(), loadNow: false, string.Empty);
							dataTable = ((!Query.UseDataDictionary) ? Database.GetDataTable(Database.PrepareQuery(constructedSqlQuery), flag, out Query.DataAdapter) : DataDictionary.GetDataTable(Database.PrepareQuery(constructedSqlQuery), flag, out Query.DataAdapter));
						}
					}
				}
				catch (Exception ex2)
				{
					ex2.Data.Add("Reset", new M1ExceptionAction("Reset Query Definition", Query.GridID, User, QueryDefinition.ResetToDefault, closeOnAction: false));
					throw;
				}
			}
			LoadDataTable(dataTable, Query.DataAdapter, isManuallyLoaded: false);
		}
		else
		{
			LoadDataTable(queryDef.DataView.Table, Query.DataAdapter, isManuallyLoaded: true);
		}
		Fields.Load(User, Database, DataDictionary, Context, Query.DatabasesResolved, queryDef.DataView.Table, this, flag);
		Tables.Load(User, Database, DataDictionary, Context, Fields, Query.DatabasesResolved, this, flag);
		if (Tables.Contains(Query.TableName))
		{
			PrimaryTable = Tables[Query.TableName];
		}
		else if (Tables.Count > 0)
		{
			PrimaryTable = Tables[0];
		}
		foreach (FieldDefinition field in Fields)
		{
			if (field.Table == null)
			{
				field.VirtualField = true;
				field.Table = PrimaryTable;
				if (field.CalculationExpression.Length != 0)
				{
					Query.DataView.Table.Columns[field.FieldName].ReadOnly = false;
				}
			}
			if (!_IsCurrencyAvailable && ((field.RelatedTableCurrencyExchangeRateField.Length != 0 && field.IsPartOfKey) || field.CurrencyRelatedField.Length != 0))
			{
				_IsCurrencyAvailable = true;
			}
		}
		if (flag && Tables.Count > 1)
		{
			foreach (FieldDefinition field2 in Fields)
			{
				if (field2.Table != PrimaryTable)
				{
					field2.AllowEditing = false;
				}
			}
		}
		if (flag && Databases.Count > 1)
		{
			foreach (FieldDefinition field3 in Fields)
			{
				if (field3.Table == PrimaryTable)
				{
					Query.DataView.Table.Columns[field3.FieldName].ReadOnly = false;
				}
			}
		}
		Query.DataView.Table.Constraints.Clear();
		if (PrimaryTable != null && flag)
		{
			PrimaryTable.DisableAddNewChanged += PrimaryTable_DisableAddNewChanged;
			PrimaryTable.DisableDeleteChanged += PrimaryTable_DisableDeleteChanged;
			PrimaryTable.ReadOnlyChanged += PrimaryTable_ReadOnlyChanged;
			if (PrimaryTable.CurrencyModeLocationField.Length != 0 && Fields.Contains(PrimaryTable.CurrencyModeLocationField))
			{
				Fields[PrimaryTable.CurrencyModeLocationField].ValueChanged += CurrencyModeLocationField_ValueChanged;
			}
			if (PrimaryTable.CurrencyRateIdField.Length != 0 && Fields.Contains(PrimaryTable.CurrencyRateIdField))
			{
				Fields[PrimaryTable.CurrencyRateIdField].ValueChanged += CurrencyRateIdField_ValueChanged;
			}
		}
		foreach (TableDefinition table in Tables)
		{
			table.LoadComplete(Fields, table == PrimaryTable && flag && !base.DesignMode, base.DesignMode);
		}
		_IsDefinitionLoaded = true;
		RefreshDataSource();
		tableJoins = null;
		if (flag && !inInit)
		{
			VerifyRelatedBindingSources();
		}
		if (flag && Tables.Count > 1)
		{
			string[] array = Query.FromClause.ToLower().Split(new string[4] { " left ", " right ", " outer ", " join " }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				TableJoinInfo tableJoinInfo = new TableJoinInfo(array[i], Tables, Fields);
				if (tableJoinInfo.ChildTable.Length != 0 && tableJoinInfo.ParentFields != null && tableJoinInfo.ParentFields.Count != 0)
				{
					if (tableJoins == null)
					{
						tableJoins = new List<TableJoinInfo>();
					}
					tableJoins.Add(tableJoinInfo);
				}
			}
		}
		return true;
	}

	public void ResyncFieldsCollection()
	{
		foreach (FieldDefinition item in Fields.Load(User, Database, DataDictionary, Context, GetDataTable()))
		{
			item.Table = PrimaryTable;
			item.Table.LoadComplete(Fields, item, allowEditing: true);
		}
	}

	private void loginToDatabases(bool allowEditing)
	{
		string[] databasesResolved = Query.DatabasesResolved;
		foreach (string text in databasesResolved)
		{
			if (Context.InstalledDatabases.Contains(text))
			{
				M1Database m1Database = ((Database == null || !Database.ID.Equals(text, StringComparison.CurrentCultureIgnoreCase) || !string.IsNullOrWhiteSpace(Database.LoginCredentials.UserID)) ? User.Databases.LoginUsingPassedCredentials(text, Database.LoginCredentials, !allowEditing).Database : Database);
				if (!Databases.Contains(m1Database.ID))
				{
					Databases.Add(m1Database);
				}
			}
		}
	}

	private string getAllReferencedFields()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (TableDefinition table in Tables)
		{
			stringBuilder.Append(table.GetAllReferencedFields());
			if (stringBuilder.Length != 0 && stringBuilder[stringBuilder.Length] != ',')
			{
				stringBuilder.Append(',');
			}
		}
		return stringBuilder.ToString();
	}

	public void RefreshDataSource(bool forceToUpdate = false)
	{
		LoadingData = true;
		base.DataSource = Query.DataView;
		base.DataMember = string.Empty;
		LoadingData = false;
		ListChangedEventArgs e = (forceToUpdate ? new ListChangedEventArgs(ListChangedType.PropertyDescriptorChanged, 0, 0) : new ListChangedEventArgs(ListChangedType.Reset, -1));
		OnListChanged(e);
		OnLoadDefinitionCompleted(EventArgs.Empty);
	}

	public void LoadDataTable(DataTable newTable, SqlDataAdapter newAdapter, bool isManuallyLoaded)
	{
		base.CurrentChanged -= M1BindingSource_CurrentChanged;
		if (Query.DataView != null && Query.DataView.Table != null)
		{
			Query.DataView.Table.ColumnChanged -= dataTable_ColumnChanged;
			Query.DataView.Table.ColumnChanging -= dataTable_ColumnChanging;
		}
		Query.DataAdapter = newAdapter;
		newTable.TableName = "main";
		if (Query.DataView == null)
		{
			Query.DataView = new DataView(newTable);
		}
		else
		{
			Query.DataView.Table = newTable;
		}
		Query.DataView.Table.ColumnChanged += dataTable_ColumnChanged;
		Query.DataView.Table.ColumnChanging += dataTable_ColumnChanging;
		manuallyLoadedDataTable = isManuallyLoaded;
		base.CurrentChanged += M1BindingSource_CurrentChanged;
	}

	private void M1BindingSource_CurrentChanged(object sender, EventArgs e)
	{
		if (PrimaryTable != null)
		{
			pushAllFieldValuesToBoundControls();
			DataRow currentAsDataRow = CurrentAsDataRow;
			M1Database currentDatabase = CurrentDatabase;
			PrimaryTable.SetDisableDeleteOverride(base.List.Count == 0, currentDatabase, currentAsDataRow, Transaction);
			if (IsTopLevel && base.List.Count == 1 && (!Query.AllowEditingOverride.HasValue || Query.AllowEditingOverride.Value))
			{
				CurrencyMode = PrimaryTable.GetCurrencyModeForRow(currentDatabase, currentAsDataRow, null);
			}
		}
	}

	private void CurrencyModeLocationField_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			CurrencyMode = PrimaryTable.GetDefaultCurrencyModeForRow(e.Database, e.Row, e.SqlTransaction);
		}
	}

	private void CurrencyRateIdField_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		if (e.IsCurrentRow)
		{
			CurrencyMode = PrimaryTable.GetCurrencyModeForRow(e.Database, e.Row, e.SqlTransaction);
		}
	}

	private void dataTable_ColumnChanging(object sender, DataColumnChangeEventArgs e)
	{
		if ((IsBindingSuspendedInternal() || LoadingData) && !inSetDefaultValues)
		{
			return;
		}
		prevColumnValue = e.Row[e.Column];
		currentColumnChanged = isColumnChanged(e.Column, prevColumnValue, e.ProposedValue);
		if (!currentColumnChanged || !Fields.Contains(e.Column.ColumnName))
		{
			return;
		}
		FieldDefinition fieldDefinition = Fields[e.Column.ColumnName];
		if (fieldDefinition.BoundParentField.Length == 0 || fieldDefinition.BoundParentFieldType != FieldDefinition.BoundParentFieldTypeEnum.ToParent || (fieldDefinition.BoundParentFieldExpression != null && fieldDefinition.BoundParentFieldExpression.Length != 0))
		{
			return;
		}
		double num = Convert.ToDouble(e.ProposedValue) - Convert.ToDouble(e.Row[e.Column]);
		if (fieldDefinition.Table.ParentBindingSource != null)
		{
			DataRow parentDataRow = fieldDefinition.Table.GetParentDataRow(e.Row);
			if (parentDataRow != null)
			{
				object obj = prevColumnValue;
				bool flag = currentColumnChanged;
				double num2 = Convert.ToDouble(parentDataRow[fieldDefinition.BoundParentField]);
				parentDataRow[fieldDefinition.BoundParentField] = num2 + num;
				prevColumnValue = obj;
				currentColumnChanged = flag;
			}
		}
	}

	private void OnLoadDefinitionCompleted(EventArgs e)
	{
		this.LoadDefinitionCompleted?.Invoke(this, e);
	}

	private void PrimaryTable_ReadOnlyChanged(object sender, DbAndRowEventArgs e)
	{
		OnAllowEditChanged(e);
	}

	private void PrimaryTable_DisableDeleteChanged(object sender, EventArgs e)
	{
		OnAllowRemoveChanged(e);
	}

	private void PrimaryTable_DisableAddNewChanged(object sender, EventArgs e)
	{
		OnAllowNewChanged(e);
	}

	private void OnValidateRemove(ValidateRemoveEventArgs e)
	{
		if (!IsTopLevel && !string.IsNullOrEmpty(PrimaryTable.LastKeyField) && M1Util.IsNullOrEmpty(e.Row[PrimaryTable.LastKeyField]) && !PrimaryTable.EmptyKeyCanBeEdited)
		{
			return;
		}
		if (PrimaryTable.IsSecurityDisabled(e.Database, e.Row))
		{
			string empty = string.Empty;
			empty = ((PrimaryTable.LastKeyField.Length == 0) ? "Row has a delete security constraint" : $"{Fields[PrimaryTable.LastKeyField].RelatedFieldsFormatCaptionAndCurrentValues(e.Row)} has a delete security constraint");
			e.ValidationInfo.AddError(empty);
		}
		if (e.TopLevel)
		{
			string text = PrimaryTable.ForeignKeyCheck(e.Row, DataDictionary, e.Database, Fields, checkDeleteFilter: true);
			if (text.Equals("Skip", StringComparison.CurrentCultureIgnoreCase))
			{
				e.RemoveSkip = true;
			}
			if (text.Length != 0)
			{
				e.ValidationInfo.AddError(text);
			}
		}
		this.ValidateRemove?.Invoke(this, e);
	}

	private void OnRemoveStarted(RemoveEventArgs e)
	{
		this.RemoveStarted?.Invoke(this, e);
	}

	private void OnRemoveCompleted(RemoveEventArgs e)
	{
		this.RemoveCompleted?.Invoke(this, e);
	}

	public new void RemoveCurrent()
	{
		for (int num = Query.DataView.Table.Rows.Count - 1; num >= 0; num--)
		{
			DataRow row = Query.DataView.Table.Rows[num];
			freeDeletedNextID(CurrentDatabase, row);
		}
		if (base.Current is DataRowView dataRowView)
		{
			Remove(dataRowView.Row);
		}
	}

	public void Remove(DataRow value)
	{
		Remove(GetDatabaseForRow(value), value, isTopLevel: true, skipValidation: false);
	}

	public void Remove(M1Database database, DataRow value)
	{
		Remove(database, value, isTopLevel: true, skipValidation: false);
	}

	public void Remove(M1Database database, DataRow value, bool isTopLevel)
	{
		Remove(database, value, isTopLevel, skipValidation: false);
	}

	public void Remove(M1Database database, DataRow value, bool isTopLevel, bool skipValidation)
	{
		PrimaryTable.VerifyChildBindingSourcesForDelete();
		if (!skipValidation)
		{
			ValidateRemoveEventArgs e = new ValidateRemoveEventArgs();
			e.ValidationInfo = new ValidationInfo(this, value, value, null);
			e.Database = database;
			e.Row = value;
			e.TopLevel = isTopLevel;
			OnValidateRemove(e);
			if (e.RemoveSkip)
			{
				return;
			}
			if (e.ValidationInfo.ErrorCount != 0)
			{
				throw new M1BORemoveException($"{e.ValidationInfo.GetRowDescription()} cannot be deleted for the following reasons:\r\n {e.ValidationInfo.ToString()} ", e.ValidationInfo.GetRowDescription(), e.ValidationInfo.ToString());
			}
		}
		RemoveEventArgs e2 = new RemoveEventArgs(database, value, null);
		e2.Database = database;
		e2.Row = value;
		OnRemoveStarted(e2);
		foreach (FieldDefinition field in Fields)
		{
			if (field.Table != PrimaryTable)
			{
				continue;
			}
			if (field.BoundParentField.Length != 0)
			{
				switch (field.BoundParentFieldType)
				{
				case FieldDefinition.BoundParentFieldTypeEnum.ToParent:
				{
					double num = ((!string.IsNullOrWhiteSpace(field.BoundParentFieldExpression)) ? Convert.ToDouble(field.Table.EvaluateScriptExpression(field.BoundParentFieldExpression, database, value)) : Convert.ToDouble(value[field.FieldName]));
					if (num != 0.0 && field.Table.ParentBindingSource != null)
					{
						DataRow parentDataRow = field.Table.GetParentDataRow(value);
						double num2 = Convert.ToDouble(parentDataRow[field.BoundParentField]);
						bool skipForeignKeyChecks = field.Table.ParentBindingSource.SkipForeignKeyChecks;
						field.Table.ParentBindingSource.SkipForeignKeyChecks = true;
						parentDataRow[field.BoundParentField] = num2 - num;
						field.Table.ParentBindingSource.SkipForeignKeyChecks = skipForeignKeyChecks;
					}
					break;
				}
				}
			}
			if (field.RelatedTableShowMemos && value.RowState != DataRowState.Detached && !M1Util.IsNullOrEmpty(value[field.RelatedFieldsAndCurrentFieldArray[0]]))
			{
				OnMemoAlertFieldChanged(new FieldChangedEventArgs(field.FieldName));
			}
		}
		if (Errors != null)
		{
			Errors.RemoveAllForSource(value);
		}
		value.Delete();
		if (!_Modified)
		{
			Modified = true;
		}
		OnRemoveCompleted(e2);
	}

	public bool DoesKeyExist(object[] keys)
	{
		foreach (DataRowView item in GetDataView())
		{
			bool flag = false;
			for (int i = 0; i < keys.Length; i++)
			{
				object obj = item.Row[PrimaryTable.KeyFieldsArray[i]];
				object obj2 = keys[i];
				if (!string.IsNullOrEmpty(obj as string))
				{
					obj = obj.ToString().Trim();
				}
				if (!string.IsNullOrEmpty(obj2 as string))
				{
					obj2 = obj2.ToString().Trim();
				}
				if (obj.Equals(obj2))
				{
					flag = true;
					continue;
				}
				flag = false;
				break;
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	public void RemoveWhere(string vbExpr, DataRow parentRow, bool skipValidation)
	{
		if (!string.IsNullOrWhiteSpace(vbExpr))
		{
			foreach (DataRowView item in GetDataView(parentRow))
			{
				if (PrimaryTable.EvaluateScriptExpressionBool(vbExpr, GetDatabaseForRow(item.Row), item.Row))
				{
					Remove(GetDatabaseForRow(item.Row), item.Row, isTopLevel: true, skipValidation);
				}
			}
			return;
		}
		foreach (DataRowView item2 in GetDataView(parentRow))
		{
			Remove(GetDatabaseForRow(item2.Row), item2.Row, isTopLevel: true, skipValidation);
		}
	}

	public void RemoveWhere(string vbExpr, DataRow parentRow)
	{
		RemoveWhere(vbExpr, parentRow, skipValidation: false);
	}

	public void RemoveWhere(string vbExpr)
	{
		RemoveWhere(vbExpr, null);
	}

	public override void RemoveAt(int position)
	{
		if (base.List[position] is DataRowView dataRowView)
		{
			Remove(dataRowView.Row);
		}
	}

	public new void MoveNext()
	{
		if (IsTopLevel && !manuallyLoadedDataTable)
		{
			moveToRecord("next");
		}
		else
		{
			base.MoveNext();
		}
	}

	public new void MovePrevious()
	{
		if (IsTopLevel && !manuallyLoadedDataTable)
		{
			moveToRecord("previous");
		}
		else
		{
			base.MovePrevious();
		}
	}

	public new void MoveFirst()
	{
		if (IsTopLevel && !manuallyLoadedDataTable)
		{
			moveToRecord("first");
		}
		else
		{
			base.MoveFirst();
		}
	}

	public new void MoveLast()
	{
		if (IsTopLevel && !manuallyLoadedDataTable)
		{
			moveToRecord("last");
		}
		else
		{
			base.MoveLast();
		}
	}

	private void moveToRecord(string direction)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = ((PrimaryTable.KeyFieldsArray != null && PrimaryTable.KeyFieldsArray.Length != 0) ? PrimaryTable.KeyFieldsArray[0] : string.Empty);
		if (text3.Length == 0)
		{
			ClearCache();
			NavigateTo(Database, string.Empty);
			return;
		}
		string tableName = PrimaryTable.TableName;
		DataRow currentAsDataRow = CurrentAsDataRow;
		switch (direction)
		{
		case "first":
			text = " asc";
			break;
		case "previous":
			if (currentAsDataRow != null)
			{
				text2 = $" where {text3} < {currentAsDataRow.Field<string>(text3).Trim().ToSql()} ";
				text = " desc";
			}
			else
			{
				text = " asc";
			}
			break;
		case "last":
			text = " desc";
			break;
		case "next":
			if (currentAsDataRow != null)
			{
				text2 = $" where {text3} > {currentAsDataRow.Field<string>(text3).Trim().ToSql()} ";
				text = " asc";
			}
			else
			{
				text = " desc";
			}
			break;
		}
		DataTable dataTable = Database.GetDataTable(string.Format("select top 1 {0} from {1} {2} order by {0} {3}", text3, tableName, text2, text));
		if (dataTable.Rows.Count > 0)
		{
			ClearCache();
			NavigateTo(Database, $" {text3} = {dataTable.Rows[0].Field<string>(text3).Trim().ToSql()}");
		}
	}

	protected override void OnCurrentChanged(EventArgs e)
	{
		if (!LoadingData && base.Current != prevCurrent)
		{
			prevCurrent = base.Current;
			if ((IsTopLevel && !isManuallyAddedBs) || (boundFieldDefinition != null && !boundFieldDefinition.IsPartOfKey))
			{
				OnRowActivated(new QueryDatabaseEventArgs(Database)
				{
					TopLevelDataRow = CurrentAsDataRow,
					TopLevelBindingSource = this,
					TopLevelTables = Tables,
					ParentDataRow = CurrentAsDataRow,
					Transaction = Transaction
				});
			}
			else
			{
				OnRowActivated(new QueryDatabaseEventArgs(Database)
				{
					TopLevelDataRow = PrimaryBindingSource.CurrentAsDataRow,
					TopLevelBindingSource = PrimaryBindingSource,
					TopLevelTables = PrimaryBindingSource.Tables,
					ParentDataRow = CurrentAsDataRow,
					Transaction = Transaction
				});
			}
			if (PrimaryTable != null)
			{
				DbAndRowEventArgs currentDataRowForProcessingQuick = PrimaryTable.GetCurrentDataRowForProcessingQuick();
				if (currentDataRowForProcessingQuick != null)
				{
					currentDataRowForProcessingQuick.Row = CurrentAsDataRow;
				}
			}
			base.OnCurrentChanged(e);
			if (base.Position == prevPosition)
			{
				OnPositionChanged(e);
			}
			if (CurrentAsDataRow != null)
			{
				CurrentChangedEventArgs e2 = new CurrentChangedEventArgs(Database, CurrentAsDataRow);
				foreach (TableDefinition table in Tables)
				{
					table.OnCurrentChanged(e2);
				}
			}
		}
		if (!LoadingData && base.Current != null && !base.IsBindingSuspended && this.RecordChange != null)
		{
			this.RecordChange(this, EventArgs.Empty);
		}
	}

	protected override void OnPositionChanged(EventArgs e)
	{
		if (!LoadingData)
		{
			prevPosition = base.Position;
			base.OnPositionChanged(e);
		}
	}

	public M1AdoRecordsetProxy AddNewAsRs()
	{
		DataRow activeRow = AddNew(Database, null, null, null) as DataRow;
		return new M1AdoRecordsetProxy(GetDataView(), activeRow, new M1AdoConnectionProxy
		{
			Database = Database,
			SqlTransaction = Transaction
		}, Query.DataAdapter);
	}

	public override object AddNew()
	{
		return AddNew(Database, null, null, null);
	}

	public object AddNew(bool createWithNextId)
	{
		return AddNew(Database, null, null, null, createWithNextId);
	}

	public object AddNew(M1Database database, DataRow parentDataRow, object[] newKeyValues, DataRow newRow, bool createWithNextId = true)
	{
		if (boundFieldDefinition != null && boundFieldDefinition.BindingSource.Position == -1 && boundFieldDefinition.BindingSource.CurrentAsDataRow == null && parentDataRow == null)
		{
			return null;
		}
		bool inAddNew = _InAddNew;
		_InAddNew = true;
		try
		{
			if (newRow == null)
			{
				newRow = NewRow(database, parentDataRow, newKeyValues, doSetDefaultValues: true, createWithNextId);
			}
			bool flag = inSetDefaultValues;
			inSetDefaultValues = true;
			try
			{
				OnAddingNew(new AddingNewEventArgs
				{
					NewObject = newRow
				});
			}
			finally
			{
				inSetDefaultValues = flag;
			}
			Query.DataView.Table.BeginLoadData();
			Query.DataView.Table.Rows.Add(newRow);
			Query.DataView.Table.EndLoadData();
			if (boundFieldDefinition != null && !boundFieldDefinition.BindingSource.InAddNew && !modifiedLocked && !_Modified)
			{
				Modified = true;
			}
			OnAddNewCompleted(new AddNewCompletedEventArgs(database, newRow, Transaction, string.Empty));
			return newRow;
		}
		finally
		{
			_InAddNew = inAddNew;
		}
	}

	protected void OnAddNewCompleted(AddNewCompletedEventArgs e)
	{
		this.AddNewCompleted?.Invoke(this, e);
		foreach (TableDefinition table in Tables)
		{
			table.OnAddNewCompleted(e);
		}
		if (e.FocusField != null && e.FocusField.Length != 0)
		{
			OnFocusField(new FocusFieldEventArgs(e.FocusField, e.Row));
			e.FocusField = string.Empty;
		}
	}

	public DataRow NewRow(M1Database database, DataRow parentDataRow, object[] newKeyValues, bool doSetDefaultValues, bool createWithNextId = true)
	{
		DataRow dataRow = Query.DataView.Table.NewRow();
		if (doSetDefaultValues)
		{
			DataRow dataRow2 = currentNewRow;
			try
			{
				currentNewRow = dataRow;
				setDefaultValues(database, dataRow, parentDataRow, newKeyValues, createWithNextId);
			}
			finally
			{
				currentNewRow = dataRow2;
			}
		}
		return dataRow;
	}

	public DataTable GetDataTable()
	{
		return Query.DataView.Table;
	}

	public DataView GetDataView()
	{
		return GetDataView(null);
	}

	public DataView GetDataView(DataRow parentRow)
	{
		if (parentRow == null || parentRow == prevParentDataRow)
		{
			return Query.DataView;
		}
		string filterForParentRowUsingCurrentFieldNames = PrimaryTable.GetFilterForParentRowUsingCurrentFieldNames(parentRow);
		return new DataView(GetDataTable(), filterForParentRowUsingCurrentFieldNames, string.Empty, DataViewRowState.CurrentRows);
	}

	private bool ShouldSerializeDataSource()
	{
		return base.DataSource != null;
	}

	private void OnDataSourceTableChanged(EventArgs e)
	{
		this.DataSourceTableChanged?.Invoke(this, e);
	}

	private void OnCurrencyModeChanged(EventArgs e)
	{
		this.CurrencyModeChanged?.Invoke(this, e);
	}

	private string getFieldName(string upperName, string lowerName)
	{
		if (!upperName.Equals(lowerName, StringComparison.CurrentCultureIgnoreCase))
		{
			return upperName;
		}
		return lowerName;
	}

	private Type getFieldType(FieldTypeEnum fieldType)
	{
		if (FieldDefinition.IsFieldTypeAString(fieldType))
		{
			return typeof(string);
		}
		switch (fieldType)
		{
		case FieldTypeEnum.Date:
		case FieldTypeEnum.DateTime:
		case FieldTypeEnum.SmallDateTime:
			return typeof(DateTime);
		case FieldTypeEnum.Bit:
			return typeof(bool);
		case FieldTypeEnum.Float:
		case FieldTypeEnum.Money:
		case FieldTypeEnum.Numeric:
		case FieldTypeEnum.Real:
		case FieldTypeEnum.SmallMoney:
			return typeof(decimal);
		case FieldTypeEnum.BigInt:
			return typeof(long);
		case FieldTypeEnum.Int:
			return typeof(int);
		case FieldTypeEnum.TinyInt:
			return typeof(short);
		case FieldTypeEnum.Identity:
			return typeof(int);
		case FieldTypeEnum.UniqueIdentifier:
			return typeof(Guid);
		case FieldTypeEnum.Image:
			return typeof(Image);
		default:
			return typeof(string);
		}
	}

	private DataTable getFieldsTable(string table)
	{
		if (DataDictionary != null)
		{
			return DataDictionary.GetDataTable("select * from DDFields Where dfTable = " + table.ToSql() + " order by dfSequence");
		}
		return M1.Core.DesignMode.DesignModeGetDataTable("select * from DDFields Where dfTable = " + table.ToSql() + " order by dfSequence");
	}

	void ISupportInitialize.BeginInit()
	{
		inInit = true;
	}

	void ISupportInitialize.EndInit()
	{
		inInit = false;
		EndEdit();
		VerifyRelatedBindingSources();
	}

	private object[] getKeyValues(DataRow row)
	{
		int num = PrimaryTable.KeyFieldsArray.Length - 1;
		if (num > 0)
		{
			object[] array = new object[num];
			for (int i = 0; i < num; i++)
			{
				if (PrimaryTable.KeyFieldsArray[i].Length != 0)
				{
					array[i] = row[PrimaryTable.KeyFieldsArray[i]];
				}
			}
			return array;
		}
		return null;
	}

	private string[] getBoundRelatedAndCurrentFields()
	{
		if (boundFieldDefinition.IsPartOfKey)
		{
			return boundFieldDefinition.Table.KeyFieldsArray;
		}
		return boundFieldDefinition.RelatedFieldsAndCurrentFieldArray;
	}

	private void setDefaultValues(M1Database database, DataRow row, DataRow parentDataRow, object[] newKeyValues, bool createWithNextId = true)
	{
		int num = 0;
		Query.DataView.Table.ColumnChanged -= dataTable_ColumnChanged;
		Query.DataView.Table.ColumnChanging -= dataTable_ColumnChanging;
		row.BeginEdit();
		row.BlankRow();
		bool flag = false;
		if (boundFieldDefinition != null)
		{
			if (parentDataRow == null)
			{
				parentDataRow = boundFieldDefinition.CurrentDataRow();
			}
			string[] childLinkFields = getChildLinkFields();
			string[] boundRelatedAndCurrentFields = getBoundRelatedAndCurrentFields();
			for (int i = 0; i < boundRelatedAndCurrentFields.Length; i++)
			{
				row[childLinkFields[i]] = parentDataRow[boundRelatedAndCurrentFields[i]];
			}
		}
		if (newKeyValues != null && newKeyValues.Length != 0)
		{
			if (newKeyValues.Length >= PrimaryTable.KeyFieldsArray.Length)
			{
				for (int j = 0; j < PrimaryTable.KeyFieldsArray.Length; j++)
				{
					row[PrimaryTable.KeyFieldsArray[j]] = newKeyValues[j];
				}
			}
			else
			{
				for (int k = 0; k < newKeyValues.Length; k++)
				{
					row[PrimaryTable.KeyFieldsArray[k]] = newKeyValues[k];
				}
				if (PrimaryTable.GetAutoIncrement(database) && Fields[PrimaryTable.LastKeyField].FieldType != FieldTypeEnum.Identity && createWithNextId)
				{
					row[PrimaryTable.LastKeyField] = GenerateNextID(row);
					flag = true;
				}
			}
		}
		else
		{
			bool flag2 = createWithNextId && PrimaryTable.GetAutoIncrement(database);
			if (boundFieldDefinition != null)
			{
				if (!flag2)
				{
					DataRow[] array = Query.DataView.Table.Select(PrimaryTable.GetFilterForParentRowUsingCurrentFieldNames(parentDataRow), PrimaryTable.LastKeyField + " asc");
					if (array != null && array.Length != 0 && M1Util.IsNullOrEmpty(array[0][PrimaryTable.LastKeyField]))
					{
						flag2 = true;
					}
				}
				if (flag2 && FieldDefinition.IsFieldTypeANumber(Fields[PrimaryTable.LastKeyField].FieldType))
				{
					long num2 = 0L;
					DataRow[] array2 = Query.DataView.Table.Select(PrimaryTable.GetFilterForParentRowUsingCurrentFieldNames(parentDataRow), PrimaryTable.LastKeyField + " desc");
					foreach (DataRow dataRow in array2)
					{
						if (dataRow.RowState != DataRowState.Deleted)
						{
							long num3 = Convert.ToInt64(dataRow[PrimaryTable.LastKeyField]);
							if (num3 > num2)
							{
								num2 = num3;
							}
						}
					}
					row[PrimaryTable.LastKeyField] = num2 + 1;
					flag = true;
				}
			}
			else
			{
				if (parentDataRow != null && PrimaryTable.KeyFieldsArray.Length > 1)
				{
					FieldDefinition fieldDefinition = Fields[PrimaryTable.KeyFieldsArray[PrimaryTable.KeyFieldsArray.Length - 2]];
					for (int m = 0; m < fieldDefinition.RelatedFieldsAndCurrentFieldArray.Length; m++)
					{
						row[PrimaryTable.KeyFieldsArray[m]] = parentDataRow[fieldDefinition.RelatedTableKeyFieldsArray[m]];
					}
				}
				if (flag2 && PrimaryTable.KeyFieldsArray.Length == 1 && Fields[PrimaryTable.LastKeyField].FieldType != FieldTypeEnum.Identity)
				{
					if (Fields[PrimaryTable.LastKeyField].FieldType == FieldTypeEnum.UniqueIdentifier)
					{
						row[PrimaryTable.LastKeyField] = Guid.NewGuid();
					}
					else
					{
						row[PrimaryTable.LastKeyField] = GenerateNextID(row);
					}
					flag = true;
				}
			}
		}
		row.EndEdit();
		Query.DataView.Table.ColumnChanging += dataTable_ColumnChanging;
		Query.DataView.Table.ColumnChanged += dataTable_ColumnChanged;
		bool flag3 = inSetDefaultValues;
		inSetDefaultValues = true;
		try
		{
			foreach (FieldDefinition field in Fields)
			{
				try
				{
					if (newKeyValues == null || newKeyValues.Length == 0 || !field.IsPartOfKey)
					{
						field.SetDefaultExpressionForRow(database, row);
					}
				}
				catch
				{
					num++;
					if (num > 3)
					{
						if (!RunningFromWeb && MessageBox.Show("Multiple errors have occurred while trying to set the default values for this data row. Do you want to continue loading default values?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
						{
							throw;
						}
						num = 0;
					}
				}
			}
			for (int n = 0; n < PrimaryTable.KeyFieldsArray.Length - 1; n++)
			{
				Fields[PrimaryTable.KeyFieldsArray[n]].OnValueChanged(new FieldDefinition.FieldValueChangedEventArgs(database, row, isCurrentRow: false, row[PrimaryTable.KeyFieldsArray[n]], Transaction));
			}
			if (boundFieldDefinition != null && parentDataRow != null)
			{
				foreach (FieldDefinition field2 in Fields)
				{
					if (field2.BoundParentField.Length != 0 && field2.BoundParentFieldType == FieldDefinition.BoundParentFieldTypeEnum.FromParent && field2.AllowEditing)
					{
						field2.ProcessBoundParentFieldForRow(parentDataRow, database, row);
					}
				}
			}
			DbAndRowEventArgs e = new DbAndRowEventArgs(database, row, Transaction);
			foreach (TableDefinition table in Tables)
			{
				table.OnSetDefaultValues(e);
			}
		}
		finally
		{
			inSetDefaultValues = flag3;
		}
		if (flag)
		{
			SetKeyState(row, keyIsSet: true, autoIncremented: true);
			OnLastKeyFieldValueChanged();
		}
		else if (PrimaryTable.LastKeyField.Length != 0)
		{
			if (!M1Util.IsNullOrEmpty(row[PrimaryTable.LastKeyField]))
			{
				SetKeyState(row, keyIsSet: true, autoIncremented: false);
				OnLastKeyFieldValueChanged();
			}
			else
			{
				SetKeyState(row, keyIsSet: false, autoIncremented: false);
			}
		}
		else
		{
			SetKeyState(row, keyIsSet: true, autoIncremented: false);
		}
	}

	public bool GetKeyState(DataRow row)
	{
		if (row.RowState == DataRowState.Added)
		{
			if (newRowsKeyState != null && newRowsKeyState.ContainsKey(row))
			{
				return newRowsKeyState[row].KeyIsSet;
			}
			return true;
		}
		return true;
	}

	protected void RemoveKeyState(DataRow row)
	{
		if (newRowsKeyState != null && newRowsKeyState.ContainsKey(row))
		{
			newRowsKeyState.Remove(row);
		}
	}

	public void SetKeyState(DataRow row, bool keyIsSet)
	{
		SetKeyState(row, keyIsSet, autoIncremented: false);
		if (keyIsSet)
		{
			currentColumnChanged = true;
			prevColumnValue = row.Field<object>(PrimaryTable.LastKeyField);
			dataTable_ColumnChanged(this, new DataColumnChangeEventArgs(row, row.Table.Columns[PrimaryTable.LastKeyField], prevColumnValue));
		}
	}

	private void SetKeyState(DataRow row, bool keyIsSet, bool autoIncremented)
	{
		if (newRowsKeyState == null)
		{
			newRowsKeyState = new Dictionary<DataRow, RowKeyState>();
		}
		if (newRowsKeyState.ContainsKey(row))
		{
			newRowsKeyState[row].KeyIsSet = keyIsSet;
			newRowsKeyState[row].AutoIncremented = autoIncremented;
		}
		else
		{
			newRowsKeyState.Add(row, new RowKeyState(keyIsSet, autoIncremented));
		}
	}

	private void OnModifiedChanged(EventArgs e)
	{
		this.ModifiedChanged?.Invoke(this, e);
	}

	public void MarkAsChanged(bool changed = true)
	{
		Modified = changed;
	}

	private bool isColumnChanged(DataColumn column, object prevValue, object newValue)
	{
		if (newValue == DBNull.Value)
		{
			newValue = null;
		}
		if (prevValue == DBNull.Value)
		{
			prevValue = null;
		}
		if (column.DataType == typeof(decimal) || column.DataType == typeof(double) || column.DataType == typeof(short) || column.DataType == typeof(int))
		{
			if ((prevValue != null || (newValue != null && Convert.ToDouble(newValue) != 0.0)) && (prevValue == null || newValue == null || Convert.ToDouble(newValue) != Convert.ToDouble(prevValue)))
			{
				return true;
			}
		}
		else if (column.DataType == typeof(bool))
		{
			if ((prevValue != null || Convert.ToBoolean(newValue)) && (prevValue == null || Convert.ToBoolean(newValue) != Convert.ToBoolean(prevValue)))
			{
				return true;
			}
		}
		else if (column.DataType == typeof(string))
		{
			if (newValue != null)
			{
				newValue = newValue.ToString().Trim();
			}
			if (prevValue != null)
			{
				prevValue = prevValue.ToString().Trim();
			}
			if ((newValue == null && prevValue != null) || (newValue != null && prevValue == null) || (newValue != null && prevValue != null && !newValue.Equals(prevValue)))
			{
				return true;
			}
		}
		else if (column.DataType == typeof(byte[]))
		{
			if ((newValue == null && prevValue != null) || (newValue != null && prevValue == null) || (newValue != null && prevValue != null && Math.Abs(((Array)newValue).Length - ((Array)prevValue).Length) > 100))
			{
				return true;
			}
		}
		else if ((newValue == null && prevValue != null) || (newValue != null && prevValue == null) || (newValue != null && prevValue != null && !newValue.Equals(prevValue)))
		{
			return true;
		}
		return false;
	}

	public void SetKeyToNextAvailable()
	{
		SetKeyToNextAvailable(CurrentAsDataRow);
	}

	public void SetKeyToNextAvailable(DataRow row)
	{
		if (!GetKeyState(row) && Fields[PrimaryTable.LastKeyField].FieldType != FieldTypeEnum.Identity)
		{
			object value = GenerateNextID(row);
			if (PrimaryTable.KeysAtThisLevel <= 1)
			{
				SetKeyState(row, keyIsSet: true, autoIncremented: true);
				SetLastKey(row, value);
			}
			else
			{
				SetLastKey(row, value);
				Fields[PrimaryTable.FirstEditableKeyField].KeyFieldLeaveCheck();
			}
		}
	}

	public void SetLastKeyValue(object value)
	{
		DataRow currentAsDataRow = CurrentAsDataRow;
		if (!GetKeyState(currentAsDataRow) && Fields[PrimaryTable.LastKeyField].FieldType != FieldTypeEnum.Identity)
		{
			if (PrimaryTable.KeysAtThisLevel <= 1)
			{
				SetKeyState(currentAsDataRow, keyIsSet: true, autoIncremented: true);
				SetLastKey(currentAsDataRow, value);
			}
			else
			{
				SetLastKey(currentAsDataRow, value);
				Fields[PrimaryTable.FirstEditableKeyField].KeyFieldLeaveCheck();
			}
		}
	}

	protected void OnGetNextID(GetNextIDEventArgs e)
	{
		this.GetNextID?.Invoke(this, e);
	}

	public object GenerateNextID()
	{
		return GenerateNextID(CurrentAsDataRow);
	}

	public object GenerateNextID(DataRow row)
	{
		GetNextIDEventArgs e = new GetNextIDEventArgs(GetDatabaseForRow(row), row, null);
		OnGetNextID(e);
		PrimaryTable.OnGetNextID(e);
		if (e.Value != null)
		{
			return e.Value;
		}
		object[] keyValues = getKeyValues(row);
		if (keyValues != null && keyValues.Length != 0)
		{
			DataTable dataTable = GetDataTable();
			if (dataTable.Rows.Count == 0 || (dataTable.Rows.Count == 1 && dataTable.Rows[0] == row))
			{
				return Database.NextIDs.GetNextIDForTable(PrimaryTable.TableName, keyValues, null, Transaction);
			}
			return Database.NextIDs.GetNextIDForTable(PrimaryTable.TableName, keyValues, dataTable, Transaction);
		}
		return Database.NextIDs.GetNextIDForTable(PrimaryTable.TableName, null, null, Transaction);
	}

	private void OnQueryDatabase(QueryDatabaseEventArgs e)
	{
		this.QueryDatabase?.Invoke(this, e);
	}

	private void OnRowActivated(QueryDatabaseEventArgs e)
	{
		LastQueryEventArgs = e;
		this.RowActivated?.Invoke(this, e);
	}

	private void freeUnusedNextID(M1Database database, DataRow row)
	{
		if (PrimaryTable != null && PrimaryTable.KeyFieldsArray.Length == 1 && newRowsKeyState != null && newRowsKeyState.ContainsKey(row) && newRowsKeyState[row].AutoIncremented)
		{
			string text = row[PrimaryTable.LastKeyField].ToString().Trim();
			if (text.Length != 0 && text != "0")
			{
				database.NextIDs.FreeUnusedNextIDForTable(PrimaryTable.TableName, text);
			}
			newRowsKeyState[row].AutoIncremented = false;
		}
	}

	private void freeDeletedNextID(M1Database database, DataRow row)
	{
		if (PrimaryTable != null && PrimaryTable.KeyFieldsArray.Length == 1)
		{
			string text = row[PrimaryTable.LastKeyField].ToString().Trim();
			if (text.Length != 0 && text != "0")
			{
				CurrentDatabase.NextIDs.FreeDeletedNextIDForTable(PrimaryTable.TableName, text);
			}
		}
	}

	public void UpdateFilter(string localFilter)
	{
		Query.DataView.RowFilter = localFilter;
	}

	private string getAdditionalFieldsForQuery()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Query.KeyFields);
		if (Query.AdditionalFields != null && Query.AdditionalFields.Length != 0)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(Query.AdditionalFields);
		}
		return stringBuilder.ToString();
	}

	public void OnNavigateAway()
	{
		this.NavigateAway?.Invoke(this, EventArgs.Empty);
	}

	public void ClearCache()
	{
		if (Errors != null && IsTopLevel && !isManuallyAddedBs)
		{
			Errors.Clear();
		}
		if (PrimaryTable != null)
		{
			DbAndRowEventArgs currentDataRowForProcessingQuick = PrimaryTable.GetCurrentDataRowForProcessingQuick();
			if (currentDataRowForProcessingQuick != null)
			{
				currentDataRowForProcessingQuick.Row = null;
			}
		}
		if (Query != null && Query.DataView != null && Query.DataView.Table != null)
		{
			Query.DataView.Table.Rows.Clear();
			Query.DataView.RowFilter = string.Empty;
		}
		loadedTopLevelQueries.Clear();
		Modified = false;
		OnCacheCleared();
	}

	public void NavigateToByArray(object[] aKeys)
	{
		string[] keyFieldsArray = PrimaryTable.KeyFieldsArray;
		StringBuilder stringBuilder = new StringBuilder();
		string[] array = (from object x in aKeys
			select x.ToString()).ToArray();
		if (array.Length == 0)
		{
			return;
		}
		for (int num = 0; num < keyFieldsArray.Length; num++)
		{
			if (num > 0)
			{
				stringBuilder.Append(" And ");
			}
			stringBuilder.Append(keyFieldsArray[num] + "=" + M1Util.ConvertToSql(array[num]));
		}
		if (stringBuilder.Length != 0)
		{
			ClearCache();
			NavigateTo(Database, stringBuilder.ToString());
			OnLastKeyFieldValueChanged();
		}
	}

	public void NavigateTo(string queryFilter)
	{
		NavigateTo(Database, queryFilter, string.Empty);
	}

	public void NavigateTo(M1Database database, string queryFilter)
	{
		NavigateTo(database, queryFilter, string.Empty);
	}

	public void NavigateTo(M1Database database, string queryFilter, string localFilter)
	{
		NavigateTo(new QueryDatabaseEventArgs(database), queryFilter, localFilter);
	}

	public void NavigateTo(QueryDatabaseEventArgs queryArgs, string queryFilter, string localFilter)
	{
		if (queryArgs == null || queryArgs.Database == null)
		{
			return;
		}
		if (loadedTopLevelQueries.Contains(queryFilter, StringComparer.CurrentCultureIgnoreCase))
		{
			Query.DataView.RowFilter = localFilter;
			return;
		}
		SqlTransaction transaction = _Transaction;
		if (queryArgs.Transaction != null)
		{
			_Transaction = queryArgs.Transaction;
		}
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(queryFilter);
			if (!string.IsNullOrWhiteSpace(_AdditionalWhere))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Insert(0, "(");
					stringBuilder.Append(")");
					stringBuilder.Append(" And ");
				}
				stringBuilder.Append("(" + _AdditionalWhere + ")");
			}
			string constructedSqlQuery = Query.GetConstructedSqlQuery(queryArgs.Database, getAdditionalFieldsForQuery(), loadNow: true, stringBuilder.ToString());
			LoadingData = true;
			Query.DataView.RowFilter = localFilter;
			if (Query.UseDataDictionary)
			{
				DataDictionary.Fill(Query.DataView.Table, DataDictionary.PrepareQuery(constructedSqlQuery), fillSchema: false, out Query.DataAdapter);
			}
			else
			{
				queryArgs.Database.Fill(Query.DataView.Table, queryArgs.Database.PrepareQuery(constructedSqlQuery), fillSchema: false, out Query.DataAdapter, Transaction);
			}
			loadedTopLevelQueries.Add(queryFilter);
			LoadingData = false;
			DataRow dataRow = null;
			if (base.List.Count != 0)
			{
				dataRow = (base.List[0] as DataRowView).Row;
				if (base.Position >= Count)
				{
					base.Position = Count - 1;
				}
			}
			if (!isManuallyAddedBs)
			{
				QueryDatabaseEventArgs e = new QueryDatabaseEventArgs(queryArgs);
				e.ParentDataRow = dataRow;
				if (IsTopLevel)
				{
					e.TopLevelDataRow = dataRow;
					e.TopLevelTables = Tables;
				}
				OnQueryDatabase(e);
			}
			prevCurrent = -2;
			prevPosition = -2;
			EventHandler recordChange = this.RecordChange;
			try
			{
				this.RecordChange = null;
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
				OnPositionChanged(EventArgs.Empty);
			}
			finally
			{
				this.RecordChange = recordChange;
			}
			OnCurrentChanged(EventArgs.Empty);
			if (Errors != null && IsTopLevel && !isManuallyAddedBs)
			{
				OnValidate(new ValidateArgs
				{
					Errors = Errors
				});
			}
		}
		finally
		{
			_Transaction = transaction;
		}
	}

	protected override void OnListChanged(ListChangedEventArgs e)
	{
		if (!LoadingData || e.ListChangedType == ListChangedType.PropertyDescriptorChanged || e.ListChangedType == ListChangedType.PropertyDescriptorAdded || e.ListChangedType == ListChangedType.PropertyDescriptorDeleted)
		{
			base.OnListChanged(e);
		}
	}

	protected void OnEditCancelled()
	{
		this.EditCancelled?.Invoke(this, EventArgs.Empty);
	}

	public new void CancelEdit()
	{
		for (int num = Query.DataView.Table.Rows.Count - 1; num >= 0; num--)
		{
			DataRow dataRow = Query.DataView.Table.Rows[num];
			switch (dataRow.RowState)
			{
			case DataRowState.Deleted:
				dataRow.RejectChanges();
				break;
			case DataRowState.Added:
				if (Errors != null)
				{
					Errors.RemoveAllForSource(dataRow);
				}
				freeUnusedNextID(CurrentDatabase, dataRow);
				RemoveKeyState(dataRow);
				dataRow.RejectChanges();
				break;
			case DataRowState.Modified:
			{
				DataColumnCollection columns = Query.DataView.Table.Columns;
				bool flag = false;
				foreach (DataColumn item in columns)
				{
					if (DBNull.Value.Equals(dataRow[item.ColumnName, DataRowVersion.Original]) && !item.AllowDBNull)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					dataRow.RejectChanges();
				}
				break;
			}
			}
		}
		base.CancelEdit();
		OnEditCancelled();
		prevCurrent = -2;
		prevPosition = -2;
		OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		OnPositionChanged(EventArgs.Empty);
		OnCurrentChanged(EventArgs.Empty);
		if (IsTopLevel && !isManuallyAddedBs)
		{
			OnValidate(new ValidateArgs
			{
				Errors = Errors
			});
		}
	}

	public void OnSaveDataCompleted(SaveDataCompletedEventArgs e)
	{
		Modified = false;
		this.SaveDataCompleted?.Invoke(this, e);
		if (ChangedRows != null)
		{
			e.TableChanges.Add(new TableChangedEventArgs(PrimaryTable.TableName, ChangedRows.AddedRows, ChangedRows.ChangedRows, ChangedRows.DeletedRows));
			if (ChangedRows.DeletedRows != null)
			{
				foreach (DataRow deletedRow in ChangedRows.DeletedRows)
				{
					if (deletedRow.RowState != DataRowState.Detached)
					{
						deletedRow.AcceptChanges();
					}
				}
			}
			if (ChangedRows.ChangedRows != null && e.UpdateChangedRowsOnly)
			{
				foreach (DataRow changedRow in ChangedRows.ChangedRows)
				{
					if (changedRow.RowState != DataRowState.Detached)
					{
						changedRow.AcceptChanges();
					}
				}
			}
			if (ChangedRows.AddedRows != null && e.UpdateAddedRowsOnly)
			{
				foreach (DataRow addedRow in ChangedRows.AddedRows)
				{
					if (addedRow.RowState != DataRowState.Detached)
					{
						addedRow.AcceptChanges();
					}
					RemoveKeyState(addedRow);
				}
			}
		}
		else
		{
			e.TableChanges.Add(new TableChangedEventArgs(PrimaryTable.TableName, null, null, null));
		}
		if (AutoRemoveWhereOnSave.Length != 0 && NumberOfChildRowsToForce != 0 && NumberOfChildRowsToForce > Count)
		{
			SetRowCount(NumberOfChildRowsToForce, markModified: false);
			ResequenceKeys(markModified: false);
		}
	}

	private void OnSaveDataStarted(SaveDataStartedEventArgs e)
	{
		this.SaveDataStarted?.Invoke(this, e);
	}

	public virtual void SaveData()
	{
		SaveData(updateDeletedRowsOnly: false);
	}

	public virtual void SaveData(bool updateDeletedRowsOnly)
	{
		delayedDataChangedArgs = null;
		delayedTableEventArgsList = null;
		SqlTransaction sqlTransaction = Transaction;
		if (Transaction == null)
		{
			sqlTransaction = Database.BeginTransaction();
		}
		SaveDataStartedEventArgs e = new SaveDataStartedEventArgs
		{
			SqlTransaction = sqlTransaction,
			Database = Database,
			UpdateDeletedRowsOnly = updateDeletedRowsOnly
		};
		try
		{
			SaveData(e);
		}
		finally
		{
			if (Transaction == null)
			{
				if (e.Cancel)
				{
					Database.RollbackTransaction(sqlTransaction);
				}
				else
				{
					Database.CommitTransaction(sqlTransaction);
				}
			}
			sqlTransaction = null;
		}
		if (!e.Cancel)
		{
			SaveDataCompletedEventArgs e2 = new SaveDataCompletedEventArgs
			{
				UpdateAddedRowsOnly = !updateDeletedRowsOnly,
				UpdateChangedRowsOnly = !updateDeletedRowsOnly
			};
			OnSaveDataCompleted(e2);
			if (e2.TableChanges.Count != 0 && Transaction == null)
			{
				foreach (TableChangedEventArgs tableChange in e2.TableChanges)
				{
					Database.OnTableChanged(tableChange);
				}
			}
		}
		if (delayedDataChangedArgs != null)
		{
			OnDataChanged(delayedDataChangedArgs);
			delayedDataChangedArgs = null;
		}
		if (delayedTableEventArgsList != null)
		{
			foreach (TableChangedEventArgs delayedTableEventArgs in delayedTableEventArgsList)
			{
				if (delayedTableEventArgs != null)
				{
					OnTableChanged(delayedTableEventArgs);
				}
			}
		}
		delayedTableEventArgsList = new List<TableChangedEventArgs>();
		ChangedRows = null;
	}

	protected void OnChangedRowsInit(SaveDataStartedEventArgs e)
	{
		this.ChangedRowsInit?.Invoke(this, e);
	}

	public void SaveData(SaveDataStartedEventArgs saveArgs)
	{
		if (saveArgs.Cancel)
		{
			return;
		}
		bool inSaveData = InSaveData;
		InSaveData = true;
		SqlTransaction transaction = _Transaction;
		try
		{
			_Transaction = saveArgs.SqlTransaction;
			if (Query.DataAdapter.InsertCommand == null)
			{
				generateCommands(PrimaryTable, Query.DataAdapter);
			}
			LoadingData = true;
			base.RaiseListChangedEvents = false;
			EndEdit();
			LoadingData = false;
			base.RaiseListChangedEvents = true;
			RowUpdateEventArgs e = new RowUpdateEventArgs(RowUpdateType.Update, Database, null, saveArgs.SqlTransaction);
			CheckRemoveWhereOnSave();
			ChangedRows = GetChangedRows();
			removeDuplicatedRows(ChangedRows);
			if (!IsTopLevel && IsSkipConcurrency())
			{
				UpdateAllChangedRowsRowVersion(saveArgs.SqlTransaction);
			}
			OnChangedRowsInit(saveArgs);
			if (saveArgs.Cancel)
			{
				CancelEdit();
				return;
			}
			if (ChangedRows.DeletedRows.Count != 0)
			{
				foreach (DataRow deletedRow in ChangedRows.DeletedRows)
				{
					e.Row = deletedRow;
					e.UpdateType = RowUpdateType.Delete;
					OnRowUpdateDeleteBefore(e);
					if (e.Cancel)
					{
						saveArgs.Cancel = true;
						CancelEdit();
						return;
					}
				}
				doUpdate(saveArgs.Database, ChangedRows.DeletedRows, saveArgs.SqlTransaction);
				foreach (DataRow deletedRow2 in ChangedRows.DeletedRows)
				{
					e.Row = deletedRow2;
					e.UpdateType = RowUpdateType.Delete;
					OnRowUpdateDeleteAfter(e);
					if (e.Cancel)
					{
						saveArgs.Cancel = true;
						return;
					}
				}
			}
			if (!saveArgs.UpdateDeletedRowsOnly)
			{
				if (ChangedRows?.ChangedRows != null && ChangedRows.ChangedRows.Count != 0)
				{
					if (ConcurrencyHelper.ExtraVerificationTableNames.Contains(PrimaryTable.TableName) && PrimaryTable.ParentBindingSource != null && !string.IsNullOrEmpty(PrimaryTable.TopLevelKeyFields))
					{
						string tableName = PrimaryTable.ParentBindingSource.PrimaryTable.TableName;
						string topLevelKeyFields = PrimaryTable.TopLevelKeyFields;
						string text = PrimaryTable.ParentBindingSource.PrimaryTable.FieldPrefix + "RowVersion";
						DataRow currentAsDataRow = PrimaryTable.ParentBindingSource.CurrentAsDataRow;
						object value = currentAsDataRow[topLevelKeyFields];
						byte[] second = (byte[])currentAsDataRow[text];
						SqlCommand sqlCommand = new SqlCommand("SELECT " + text + " from " + tableName + " where " + topLevelKeyFields + " = @" + topLevelKeyFields);
						sqlCommand.Parameters.AddWithValue("@" + topLevelKeyFields, value);
						if (!Database.GetDataTable(sqlCommand, saveArgs.SqlTransaction).Rows[0].Field<byte[]>(text).SequenceEqual(second))
						{
							saveArgs.Cancel = true;
							BuildConcurrencyError(string.Empty, showMessageBoxReload: true);
							return;
						}
					}
					foreach (DataRow item in ChangedRows?.ChangedRows)
					{
						e.Row = item;
						e.UpdateType = RowUpdateType.Update;
						OnRowUpdateSaveBefore(e);
						if (e.Cancel)
						{
							saveArgs.Cancel = true;
							return;
						}
					}
					doUpdate(saveArgs.Database, ChangedRows.ChangedRows, saveArgs.SqlTransaction);
					foreach (DataRow changedRow in ChangedRows.ChangedRows)
					{
						e.Row = changedRow;
						e.UpdateType = RowUpdateType.Update;
						OnRowUpdateSaveAfter(e);
						if (e.Cancel)
						{
							saveArgs.Cancel = true;
							return;
						}
					}
				}
				if (ChangedRows != null && ChangedRows.AddedRows != null)
				{
					ChangedRowsInfo changedRows = ChangedRows;
					if (changedRows == null || changedRows.AddedRows.Count != 0)
					{
						foreach (DataRow item2 in ChangedRows?.AddedRows)
						{
							e.Row = item2;
							e.UpdateType = RowUpdateType.Insert;
							OnRowUpdateAddBefore(e);
							if (e.Cancel)
							{
								saveArgs.Cancel = true;
								return;
							}
						}
						doUpdate(saveArgs.Database, ChangedRows.AddedRows, saveArgs.SqlTransaction);
						foreach (DataRow addedRow in ChangedRows.AddedRows)
						{
							e.Row = addedRow;
							e.UpdateType = RowUpdateType.Insert;
							OnRowUpdateAddAfter(e);
							if (e.Cancel)
							{
								saveArgs.Cancel = true;
								return;
							}
						}
						UpdateRowVersionWhenMultipleRowsAdded(e);
						UpdateAllChangedRowsRowVersion(e.SqlTransaction);
					}
				}
			}
			if (!IsTopLevel && !manuallyLoadedDataTable && PrimaryTable != null && PrimaryTable.TableName.Length != 0 && (ChangedRows.AddedRows.Count != 0 || ChangedRows.ChangedRows.Count != 0 || ChangedRows.DeletedRows.Count != 0))
			{
				OnDataChanged(new DataChangedEventArgs(PrimaryTable.TableName));
			}
			OnSaveDataStarted(saveArgs);
			List<object> list = new List<object>();
			if (ChangedRows == null)
			{
				return;
			}
			foreach (DataRow deletedRow3 in ChangedRows.DeletedRows)
			{
				list.Clear();
				string[] keyFieldsArray = PrimaryTable.KeyFieldsArray;
				foreach (string columnName in keyFieldsArray)
				{
					list.Add(deletedRow3.Field<object>(columnName, DataRowVersion.Original));
				}
				PrimaryTable.RecursiveDelete(DataDictionary, PrimaryTable.TableName, saveArgs.Database, list.ToArray(), saveArgs.SqlTransaction, this);
			}
		}
		finally
		{
			_Transaction = transaction;
			InSaveData = inSaveData;
		}
	}

	private bool IsSkipConcurrency()
	{
		if (delayedTableEventArgsList != null)
		{
			_ = 1;
		}
		else
			_ = delayedDataChangedArgs != null;
		foreach (FieldDefinition field in Fields)
		{
			if (field.Name.Substring(3).ToUpper() == "CLOSED")
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateAllChangedRowsRowVersion(SqlTransaction transaction)
	{
		foreach (DataRow changedRow in ChangedRows.ChangedRows)
		{
			SetCurrentRowVersion(changedRow, transaction);
		}
	}

	private void doUpdate(M1Database database, List<DataRow> changedRows, SqlTransaction transaction)
	{
		LoadingData = true;
		bool acceptChangesDuringUpdate = Query.DataAdapter.AcceptChangesDuringUpdate;
		Query.DataAdapter.AcceptChangesDuringUpdate = false;
		try
		{
			int num = 0;
			while (num < 5)
			{
				try
				{
					database.UpdateData(changedRows.ToArray(), Query.DataAdapter, transaction, generateCommands: false);
					num++;
					break;
				}
				catch (DBConcurrencyException)
				{
					BuildConcurrencyError("", showMessageBoxReload: true);
					break;
				}
				catch (SqlException ex2)
				{
					if (ex2.Number == 2601)
					{
						LoadingData = false;
						foreach (DataRow changedRow in changedRows)
						{
							if (!string.IsNullOrWhiteSpace(changedRow.RowError) && changedRow.RowError.IndexOf("Cannot insert duplicate key", StringComparison.InvariantCultureIgnoreCase) != -1)
							{
								SetKeyState(changedRow, keyIsSet: false);
								SetKeyToNextAvailable(changedRow);
								OnDuplicateKeyFix(GetDatabaseForRow(changedRow), changedRow);
								VerifyRelatedBindingSources(overrideForceLoad: true);
								ReRunToParentUpdate(changedRow);
							}
						}
						LoadingData = true;
						continue;
					}
					BuildConcurrencyError(ex2.Message, showMessageBoxReload: true);
					break;
				}
			}
		}
		catch (DBConcurrencyException)
		{
			BuildConcurrencyError("", showMessageBoxReload: true);
		}
		catch (Exception ex4)
		{
			BuildConcurrencyError(ex4.Message, showMessageBoxReload: false);
		}
		finally
		{
			Query.DataAdapter.AcceptChangesDuringUpdate = acceptChangesDuringUpdate;
			LoadingData = false;
		}
	}

	private void ReRunToParentUpdate(DataRow currentDataRow)
	{
		if (currentDataRow == null || Fields == null)
		{
			return;
		}
		foreach (FieldDefinition field in Fields)
		{
			if (field.BoundParentField.Length == 0 || field.BoundParentFieldType != FieldDefinition.BoundParentFieldTypeEnum.ToParent)
			{
				continue;
			}
			double num = Convert.ToDouble(string.IsNullOrWhiteSpace(field.BoundParentFieldExpression) ? currentDataRow[field.FieldName] : field.Table.EvaluateScriptExpression(field.BoundParentFieldExpression, field.Database, currentDataRow));
			if (num != 0.0 && field.Table.ParentBindingSource != null)
			{
				DataRow parentDataRow = field.Table.GetParentDataRow(currentDataRow);
				if (parentDataRow != null)
				{
					double num2 = Convert.ToDouble(parentDataRow[field.BoundParentField]);
					parentDataRow[field.BoundParentField] = num2 + num;
				}
			}
		}
	}

	protected virtual void OnDuplicateKeyFix(M1Database database, DataRow row)
	{
	}

	public void ShowInfoMsg(string msg)
	{
		if (!RunningFromWeb)
		{
			MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	public void OnDataChanged(string childTable)
	{
		if (tableJoins == null)
		{
			return;
		}
		foreach (TableJoinInfo tableJoin in tableJoins)
		{
			if (!childTable.Equals(tableJoin.ChildTable, StringComparison.CurrentCultureIgnoreCase))
			{
				continue;
			}
			{
				foreach (DataRow row in GetDataTable().Rows)
				{
					tableJoin.RefreshChildFieldsInDataRow(this, GetDatabaseForRow(row), row, Transaction);
				}
				break;
			}
		}
	}

	public void OnDataChanged(DataChangedEventArgs e)
	{
		if (InSaveData)
		{
			if (delayedDataChangedArgs != null)
			{
				delayedDataChangedArgs.ChangedTables.AddRange(e.ChangedTables);
			}
			else
			{
				delayedDataChangedArgs = e;
			}
		}
		else if (!IsTopLevel)
		{
			boundFieldDefinition.BindingSource.OnDataChanged(e);
		}
		else if (this.DataChanged != null)
		{
			SetCurrentRowVersion(e, Transaction);
			this.DataChanged(this, e);
		}
	}

	public void OnDataChanged(object value)
	{
		if (value is string)
		{
			OnDataChanged(value.ToString());
			return;
		}
		DataChangedFlag flag = DataChangedFlag.None;
		switch (Convert.ToInt16(value))
		{
		case 1:
			flag = DataChangedFlag.CurrentRow;
			break;
		case 2:
			flag = DataChangedFlag.DetailRows;
			break;
		case 3:
			flag = DataChangedFlag.CurrentAndDetailRows;
			break;
		}
		OnDataChanged(new DataChangedEventArgs(flag));
	}

	public void OnTableChanged(TableChangedEventArgs e)
	{
		if (InSaveData)
		{
			if (delayedTableEventArgsList == null)
			{
				delayedTableEventArgsList = new List<TableChangedEventArgs>();
			}
			delayedTableEventArgsList.Add(e);
		}
		else
		{
			Database?.OnTableChanged(e);
		}
	}

	public void OnTableChanged(string tableName)
	{
		if (InSaveData)
		{
			if (delayedTableEventArgsList == null)
			{
				delayedTableEventArgsList = new List<TableChangedEventArgs>();
			}
			delayedTableEventArgsList.Add(new TableChangedEventArgs(tableName, null, null, null));
		}
		else
		{
			Database?.OnTableChanged(new TableChangedEventArgs(tableName, null, null, null));
		}
	}

	public ChangedRowsInfo GetChangedRows()
	{
		return new ChangedRowsInfo(Query.DataView.Table);
	}

	private void removeDuplicatedRows(ChangedRowsInfo changedRows)
	{
		if (changedRows.ChangedRows.Count <= 1)
		{
			return;
		}
		List<DataRow> duplicatedRows = getDuplicatedRows(changedRows.ChangedRows);
		if (duplicatedRows.Count <= 0)
		{
			return;
		}
		foreach (DataRow item in duplicatedRows)
		{
			changedRows.ChangedRows.Remove(item);
		}
	}

	private List<DataRow> getDuplicatedRows(List<DataRow> list)
	{
		List<DataRow> list2 = new List<DataRow>();
		List<DataRow> list3 = new List<DataRow>();
		foreach (DataRow item in list)
		{
			if (!findDataRowInList(item, list2))
			{
				list2.Add(item);
			}
			else
			{
				list3.Add(item);
			}
		}
		return list3;
	}

	private bool findDataRowInList(DataRow row, List<DataRow> list)
	{
		bool flag = false;
		foreach (DataRow item in list)
		{
			flag = true;
			string[] keyFieldsArray = PrimaryTable.KeyFieldsArray;
			foreach (string columnName in keyFieldsArray)
			{
				if (!item[columnName].Equals(row[columnName]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return flag;
			}
		}
		return flag;
	}

	private void generateCommands(TableDefinition tableDef, SqlDataAdapter adapter)
	{
		SqlCommand sqlCommand = new SqlCommand();
		sqlCommand.CommandTimeout = 0;
		SqlCommand sqlCommand2 = new SqlCommand();
		sqlCommand2.CommandTimeout = 0;
		SqlCommand sqlCommand3 = new SqlCommand();
		sqlCommand3.CommandTimeout = 0;
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		string text = string.Empty;
		foreach (FieldDefinition field in Fields)
		{
			if (field.Table == tableDef && !field.VirtualField && field.FieldType != FieldTypeEnum.Identity && field.FieldType != FieldTypeEnum.TimeStamp)
			{
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(',');
					stringBuilder.Append(',');
					stringBuilder3.Append(',');
				}
				stringBuilder2.Append(field.Name);
				stringBuilder.AppendFormat("@P{0}", num.ToString());
				stringBuilder3.AppendFormat("{0}=@P{1}", field.Name, num.ToString());
				sqlCommand.Parameters.Add(new SqlParameter($"@P{num.ToString()}", getDbTypeForField(field.FieldType), 0, field.Name));
				sqlCommand2.Parameters.Add(new SqlParameter($"@P{num.ToString()}", getDbTypeForField(field.FieldType), 0, field.Name));
				num++;
			}
		}
		if (!string.IsNullOrWhiteSpace(tableDef.LastKeyField) && Fields[tableDef.LastKeyField].FieldType == FieldTypeEnum.Identity && !string.IsNullOrWhiteSpace(tableDef.UniqueField))
		{
			text = ";Select " + tableDef.LastKeyField + "," + tableDef.UniqueField + " From " + tableDef.TableName + " Where " + tableDef.LastKeyField + " = Scope_Identity()";
			sqlCommand.UpdatedRowSource = UpdateRowSource.FirstReturnedRecord;
		}
		StringBuilder stringBuilder4 = BuildWhereClause(tableDef, sqlCommand2, sqlCommand3, num);
		sqlCommand3.CommandText = $"Delete From {tableDef.TableName} Where {stringBuilder4.ToString()}";
		adapter.DeleteCommand = sqlCommand3;
		StringBuilder stringBuilder5 = AddConcurrencyCheckToWhereClauseIfNecessary(stringBuilder4, sqlCommand2);
		sqlCommand2.CommandText = $"Update {tableDef.TableName} Set {stringBuilder3.ToString()} Where {stringBuilder5.ToString()}";
		adapter.UpdateCommand = sqlCommand2;
		sqlCommand.CommandText = string.Format("Insert Into {0} ( {1} ) Values( {2} )" + text, tableDef.TableName, stringBuilder2.ToString(), stringBuilder.ToString());
		adapter.InsertCommand = sqlCommand;
	}

	private StringBuilder AddConcurrencyCheckToWhereClauseIfNecessary(StringBuilder whereBuilder, SqlCommand updateCommand)
	{
		return new Concurrency().AddConcurrencyCheckToWhereClauseIfNecessary(whereBuilder, updateCommand, Fields, PrimaryTable);
	}

	public string RowVersionFieldName()
	{
		return new Concurrency().RowVersionFieldName(Fields, PrimaryTable);
	}

	private StringBuilder BuildWhereClause(TableDefinition tableDef, SqlCommand updateCommand, SqlCommand deleteCommand, int parameterCount)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string[] keyFieldsArray = tableDef.KeyFieldsArray;
		foreach (string text in keyFieldsArray)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" And ");
			}
			stringBuilder.AppendFormat("{0}=@P{1}", text, parameterCount.ToString());
			deleteCommand.Parameters.Add(new SqlParameter($"@P{parameterCount.ToString()}", getDbTypeForField(Fields[text].FieldType), 0, text)).SourceVersion = DataRowVersion.Original;
			updateCommand.Parameters.Add(new SqlParameter($"@P{parameterCount.ToString()}", getDbTypeForField(Fields[text].FieldType), 0, text)).SourceVersion = DataRowVersion.Original;
			parameterCount++;
		}
		if (stringBuilder.Length == 0)
		{
			stringBuilder.AppendFormat("{0}=@P{1}", tableDef.UniqueField, parameterCount.ToString());
			deleteCommand.Parameters.Add(new SqlParameter($"@P{parameterCount.ToString()}", getDbTypeForField(Fields[tableDef.UniqueField].FieldType), 0, tableDef.UniqueField));
			updateCommand.Parameters.Add(new SqlParameter($"@P{parameterCount.ToString()}", getDbTypeForField(Fields[tableDef.UniqueField].FieldType), 0, tableDef.UniqueField));
			parameterCount++;
		}
		return stringBuilder;
	}

	private SqlDbType getDbTypeForField(FieldTypeEnum fieldType)
	{
		switch (fieldType)
		{
		case FieldTypeEnum.Bit:
			return SqlDbType.Bit;
		case FieldTypeEnum.Date:
			return SqlDbType.DateTime;
		case FieldTypeEnum.DateTime:
			return SqlDbType.DateTime;
		case FieldTypeEnum.SmallDateTime:
			return SqlDbType.SmallDateTime;
		case FieldTypeEnum.Identity:
			return SqlDbType.Int;
		case FieldTypeEnum.Image:
			return SqlDbType.Image;
		case FieldTypeEnum.Numeric:
			return SqlDbType.Decimal;
		case FieldTypeEnum.Int:
			return SqlDbType.Int;
		case FieldTypeEnum.BigInt:
			return SqlDbType.BigInt;
		case FieldTypeEnum.SmallInt:
			return SqlDbType.SmallInt;
		case FieldTypeEnum.TinyInt:
			return SqlDbType.TinyInt;
		case FieldTypeEnum.Text:
			return SqlDbType.Text;
		case FieldTypeEnum.NText:
			return SqlDbType.NText;
		case FieldTypeEnum.Money:
			return SqlDbType.Money;
		case FieldTypeEnum.UniqueIdentifier:
			return SqlDbType.UniqueIdentifier;
		case FieldTypeEnum.Char:
		case FieldTypeEnum.Varchar:
			return SqlDbType.NVarChar;
		case FieldTypeEnum.NChar:
		case FieldTypeEnum.NVarchar:
			return SqlDbType.NVarChar;
		case FieldTypeEnum.TimeStamp:
			return SqlDbType.Timestamp;
		default:
			return SqlDbType.NVarChar;
		}
	}

	private void OnRowUpdateSaveBefore(RowUpdateEventArgs e)
	{
		this.RowUpdateSaveBefore?.Invoke(this, e);
	}

	private void OnRowUpdateSaveAfter(RowUpdateEventArgs e)
	{
		this.RowUpdateSaveAfter?.Invoke(this, e);
		SetCurrentRowVersion(e.Row, e.SqlTransaction);
	}

	private void SetCurrentRowVersion(DataRow row, SqlTransaction transaction, bool updateFields = true, DataChangedEventArgs eventArguments = null)
	{
		string text = RowVersionFieldName();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		byte[] newRowVersion = GetNewRowVersion(row, transaction, text, eventArguments);
		if (newRowVersion == null)
		{
			return;
		}
		int num = row.Table.Columns.IndexOf(text);
		if (num == -1)
		{
			return;
		}
		bool readOnly = row.Table.Columns[num].ReadOnly;
		try
		{
			UpdateOneRowRowVersion(row, updateFields, num, text, newRowVersion, readOnly);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Concurrency update failed. " + ex.Message, "Concurrency Error", MessageBoxButtons.OK);
		}
		finally
		{
			row.Table.Columns[num].ReadOnly = readOnly;
		}
	}

	private void UpdateOneRowRowVersion(DataRow row, bool updateFields, int rowVersionIndex, string rowVersionFieldName, byte[] newRowVersion, bool previousReadOnlyValue)
	{
		row.Table.Columns[rowVersionIndex].ReadOnly = false;
		row.SetField(rowVersionFieldName, newRowVersion);
		if (updateFields)
		{
			Fields[rowVersionFieldName].OriginalValue = newRowVersion;
		}
		row.Table.Columns[rowVersionIndex].ReadOnly = previousReadOnlyValue;
		DataRowState rowState = row.RowState;
		if (!SkipAcceptChanges(rowState) && (delayedTableEventArgsList == null || delayedTableEventArgsList.Count <= 0) && delayedDataChangedArgs == null)
		{
			row.AcceptChanges();
		}
	}

	private bool SkipAcceptChanges(DataRowState previousRowState)
	{
		if (IsTopLevel)
		{
			return false;
		}
		string tableName = PrimaryTable.TableName;
		if (tableName == "ReceiptLines" || tableName == "ReceiptComponents")
		{
			return true;
		}
		if (previousRowState != DataRowState.Unchanged && !IsTopLevel)
		{
			if (!IsTopLevel)
			{
				return !InPosting();
			}
			return true;
		}
		return false;
	}

	private bool InPosting()
	{
		FieldDefinition fieldDefinition = Fields.FirstOrDefault((FieldDefinition field) => field.Name.Substring(3) == "Posted");
		if (fieldDefinition?.RelatedFieldsAndCurrentField?.Substring(3) == "Posted")
		{
			return fieldDefinition?.OriginalValue != fieldDefinition?.Value;
		}
		return false;
	}

	private void SetCurrentRowVersion(DataChangedEventArgs eventArguments, SqlTransaction transaction)
	{
		ChangedRows = GetChangedRows();
		removeDuplicatedRows(ChangedRows);
		foreach (DataRow changedRow in ChangedRows.ChangedRows)
		{
			SetCurrentRowVersion(changedRow, transaction, updateFields: true, eventArguments);
		}
	}

	private void UpdateRowVersionWhenMultipleRowsAdded(RowUpdateEventArgs updateArgs)
	{
		if (ChangedRows.AddedRows.Count <= 1)
		{
			return;
		}
		foreach (DataRow addedRow in ChangedRows.AddedRows)
		{
			updateArgs.Row = addedRow;
			updateArgs.UpdateType = RowUpdateType.Insert;
			SetCurrentRowVersion(updateArgs.Row, updateArgs.SqlTransaction, updateFields: false);
		}
	}

	public byte[] GetNewRowVersion(DataRow row, SqlTransaction transaction, string rowVersionFieldName, DataChangedEventArgs eventArguments)
	{
		string filterForCurrentRow = PrimaryTable.GetFilterForCurrentRow(row);
		M1Database databaseForRow = GetDatabaseForRow(row);
		string queryString = BuildRowVersionQuery(rowVersionFieldName, filterForCurrentRow);
		DataTable dataTable = databaseForRow.GetDataTable(queryString, transaction);
		if (dataTable.Rows.Count == 0)
		{
			filterForCurrentRow = GetRowVersionFilterFromEvent(eventArguments);
			if (string.IsNullOrWhiteSpace(filterForCurrentRow))
			{
				return null;
			}
			queryString = BuildRowVersionQuery(rowVersionFieldName, filterForCurrentRow);
			dataTable = GetDatabaseForRow(row).GetDataTable(queryString, transaction);
			if (dataTable == null || dataTable.Rows.Count == 0)
			{
				return null;
			}
		}
		return (byte[])dataTable.Rows[0].ItemArray[0];
	}

	private string GetRowVersionFilterFromEvent(DataChangedEventArgs eventArguments)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		if (eventArguments?.NewKeys != null)
		{
			string[] keyFieldsArray = PrimaryTable.KeyFieldsArray;
			foreach (string text in keyFieldsArray)
			{
				if (stringBuilder.Length == 0)
				{
					object o = eventArguments.NewKeys[num];
					stringBuilder.Append(text + " = " + o.ToSql());
				}
				else
				{
					object o2 = eventArguments.NewKeys[num];
					stringBuilder.Append(" AND " + text + " = " + o2.ToSql());
				}
				num++;
			}
		}
		return stringBuilder.ToString();
	}

	private string BuildRowVersionQuery(string rowVersionFieldName, string curFilter)
	{
		string text = "select " + rowVersionFieldName + " as RowVersion from " + PrimaryTable.TableName + " with (nolock)";
		if (!string.IsNullOrEmpty(curFilter))
		{
			text = text + " Where " + curFilter + " ";
		}
		return text;
	}

	private void OnRowUpdateAddBefore(RowUpdateEventArgs e)
	{
		this.RowUpdateAddBefore?.Invoke(this, e);
	}

	private void OnRowUpdateAddAfter(RowUpdateEventArgs e)
	{
		EventHandler<RowUpdateEventArgs> eventHandler = this.RowUpdateAddAfter;
		if (eventHandler != null)
		{
			SetCurrentRowVersion(e.Row, e.SqlTransaction);
			eventHandler(this, e);
		}
	}

	private void OnRowUpdateDeleteBefore(RowUpdateEventArgs e)
	{
		this.RowUpdateDeleteBefore?.Invoke(this, e);
	}

	private void OnRowUpdateDeleteAfter(RowUpdateEventArgs e)
	{
		this.RowUpdateDeleteAfter?.Invoke(this, e);
	}

	public void OnMemoAlertFieldChanged(FieldChangedEventArgs e)
	{
		this.MemoAlertFieldChanged?.Invoke(this, e);
		if (boundFieldDefinition != null && boundFieldDefinition.BindingSource != null)
		{
			boundFieldDefinition.BindingSource.OnMemoAlertFieldChanged(e);
		}
	}

	public void OnPrimaryContactChanged(FieldChangedEventArgs e)
	{
		this.PrimaryContactChanged?.Invoke(this, e);
	}

	protected void OnLastKeyFieldValueChanged()
	{
		this.LastKeyFieldValueChanged?.Invoke(this, EventArgs.Empty);
	}

	protected bool IsBindingSuspendedInternal()
	{
		if (base.IsBindingSuspended)
		{
			return IsTopLevel;
		}
		return false;
	}

	private void dataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
	{
		if ((IsBindingSuspendedInternal() || LoadingData || settingToPrevious) && !inSetDefaultValues && (!IsBindingSuspendedInternal() || !settingRelatedValues))
		{
			return;
		}
		bool flag = currentColumnChanged;
		object obj = prevColumnValue;
		if (!Fields.Contains(e.Column.ColumnName))
		{
			return;
		}
		FieldDefinition fieldDefinition = Fields[e.Column.ColumnName];
		M1Database databaseForRow = GetDatabaseForRow(e.Row);
		DataRowView dataRowView = null;
		if (CurrentAsDataRow != null)
		{
			dataRowView = base.Current as DataRowView;
		}
		bool flag2 = dataRowView != null && dataRowView.Row == e.Row;
		if (flag)
		{
			if (boundFieldDefinition == null && PrimaryTable != null && PrimaryTable.KeyFieldsArray.Length >= 2 && PrimaryTable.KeyFieldsArray.Contains(fieldDefinition.FieldName, StringComparer.CurrentCultureIgnoreCase) && !fieldDefinition.FieldName.Equals(PrimaryTable.LastKeyField, StringComparison.CurrentCultureIgnoreCase))
			{
				foreach (FieldDefinition field in Fields)
				{
					if (field.BoundParentField.Length != 0 && field.BoundParentFieldType == FieldDefinition.BoundParentFieldTypeEnum.FromParent && ((string.IsNullOrWhiteSpace(field.BoundParentFieldProxy) && fieldDefinition.FieldName.Equals(PrimaryTable.KeyFieldsArray[PrimaryTable.KeyFieldsArray.Length - 2], StringComparison.CurrentCultureIgnoreCase)) || field.BoundParentFieldProxy.Equals(fieldDefinition.FieldName, StringComparison.CurrentCultureIgnoreCase)))
					{
						field.ProcessBoundParentFieldForRow(fieldDefinition.RelatedTableGetDataRow(field.BoundParentRelatedAndCurrentFields), databaseForRow, e.Row);
					}
				}
			}
			if (IsTopLevel && !manuallyLoadedDataTable && !setLastKeyOverride && fieldDefinition.IsEditableKey && GetKeyState(e.Row))
			{
				OnFieldValueChanged(fieldDefinition, setLastKey: true, new FieldDefinition.FieldValueChangedEventArgs(databaseForRow, e.Row, flag2, obj, Transaction));
				string filterForCurrentRow = PrimaryTable.GetFilterForCurrentRow(e.Row);
				if (databaseForRow.GetDataTable($"select 1 as dummy from {PrimaryTable.TableName} Where {filterForCurrentRow} ").Rows.Count != 0)
				{
					string rowFilter = Database.Security.GetRowFilter(PrimaryTable.TableName);
					if (!string.IsNullOrEmpty(rowFilter) && Convert.ToInt32(Database.ExecuteScalar("Select Count(*) From " + PrimaryTable.TableName + " Where (" + filterForCurrentRow + ") And " + rowFilter)) == 0)
					{
						Database.OnShowError(new ShowErrorEventArgs("The record you are trying to access has been marked no access by your security administrator."));
						settingToPrevious = true;
						e.Row[e.Column] = fieldDefinition.GetDefaultForFieldType();
						settingToPrevious = false;
						pushFieldValueToBoundControls(e.Row, e.Column.ColumnName, force: true);
					}
					else
					{
						ClearCache();
						NavigateTo(databaseForRow, filterForCurrentRow);
						OnLastKeyFieldValueChanged();
					}
					return;
				}
				string[] keyFieldsArray = PrimaryTable.KeyFieldsArray;
				foreach (string key in keyFieldsArray)
				{
					Fields[key].EvaluateReadOnlyExpression(databaseForRow, e.Row);
				}
				OnLastKeyFieldValueChanged();
			}
			if (!_Modified && !inSetDefaultValues && !modifiedLocked && (boundFieldDefinition == null || !boundFieldDefinition.BindingSource.InAddNew))
			{
				Modified = true;
			}
		}
		if (flag && !fieldDefinition.IsPartOfKey && !settingRelatedValues && !settingToPrevious && flag)
		{
			fieldDefinition.RelatedTableIsForeignKeyValid(databaseForRow, e.Row, Transaction, tempValidationInfo);
			if (tempValidationInfo.ErrorCount != 0)
			{
				ForeignKeyInvalidEventArgs e2 = new ForeignKeyInvalidEventArgs(e.Row, fieldDefinition, databaseForRow, tempValidationInfo);
				databaseForRow.OnForeignKeyInvalid(e2);
				if (e2.RetryValidation)
				{
					tempValidationInfo.Clear();
					fieldDefinition.RelatedTableIsForeignKeyValid(databaseForRow, e.Row, Transaction, tempValidationInfo);
				}
				if (tempValidationInfo.ErrorCount != 0)
				{
					if (!e2.Cancel)
					{
						fieldDefinition.OnForeignKeyInvalid(e2);
					}
					if (e2.Cancel)
					{
						tempValidationInfo.Clear();
						settingToPrevious = true;
						e.Row[e.Column] = obj;
						settingToPrevious = false;
						pushFieldValueToBoundControls(e.Row, e.Column.ColumnName, force: true);
						return;
					}
				}
				if (tempValidationInfo.ErrorCount != 0)
				{
					fieldDefinition.ForceToValidate = true;
					tempValidationInfo.Clear();
				}
				else
				{
					fieldDefinition.ForceToValidate = false;
				}
			}
		}
		if (!fieldDefinition.IsPartOfKey && (flag || settingRelatedValues))
		{
			bool flag3 = settingRelatedValues;
			settingRelatedValues = true;
			foreach (FieldDefinition field2 in Fields)
			{
				if (field2.RelatedFields.Length != 0 && !FieldDefinition.IsFieldTypeAMemo(field2.FieldType) && field2.RelatedFieldsArray[field2.RelatedFieldsArray.Length - 1].Equals(fieldDefinition.FieldName, StringComparison.CurrentCultureIgnoreCase))
				{
					e.Row[field2.FieldName] = e.Row.DefaultValueForType(e.Row.Table.Columns[field2.FieldName].DataType);
				}
			}
			settingRelatedValues = flag3;
		}
		else if (fieldDefinition.IsEditableKey && flag && !fieldDefinition.Table.LastKeyField.Equals(fieldDefinition.FieldName, StringComparison.CurrentCultureIgnoreCase))
		{
			dataTable_ColumnChanged(this, new DataColumnChangeEventArgs(e.Row, e.Row.Table.Columns[fieldDefinition.Table.LastKeyField], e.Row.Field<object>(fieldDefinition.Table.LastKeyField)));
		}
		if (PrimaryTable != null && PrimaryTable.PrimaryContactField.Length != 0 && PrimaryTable.PrimaryContactField.Equals(fieldDefinition.FieldName, StringComparison.CurrentCultureIgnoreCase))
		{
			OnPrimaryContactChanged(new FieldChangedEventArgs(fieldDefinition.FieldName));
		}
		if (!flag && fieldDefinition.RelatedFields.Length != 0 && settingRelatedValues)
		{
			flag = true;
		}
		if (flag && fieldDefinition.RelatedTableShowMemos)
		{
			OnMemoAlertFieldChanged(new FieldChangedEventArgs(fieldDefinition.FieldName));
		}
		if (!flag)
		{
			return;
		}
		if (flag2)
		{
			bool force = setLastKeyOverride || inChangeCode;
			pushFieldValueToBoundControls(e.Row, e.Column.ColumnName, force);
		}
		OnFieldValueChanged(fieldDefinition, setLastKey: false, new FieldDefinition.FieldValueChangedEventArgs(databaseForRow, e.Row, flag2, obj, Transaction));
		if (tableJoins == null)
		{
			return;
		}
		foreach (TableJoinInfo tableJoin in tableJoins)
		{
			if (e.Column.ColumnName.Equals(tableJoin.LastParentField, StringComparison.CurrentCultureIgnoreCase))
			{
				tableJoin.RefreshChildFieldsInDataRow(this, databaseForRow, e.Row, Transaction);
			}
		}
	}

	private bool OnFieldValueChanged(FieldDefinition fieldDef, bool setLastKey, FieldDefinition.FieldValueChangedEventArgs arg)
	{
		bool flag = inChangeCode;
		inChangeCode = true;
		bool inUserChangeEvent = Database != null && Database.InUserChangeEvent;
		bool inEvaluateChangeCode = InEvaluateChangeCode;
		if (setLastKey)
		{
			setLastKeyOverride = true;
		}
		if (!InEvaluateChangeCode)
		{
			if (Database != null)
			{
				Database.InUserChangeEvent = true;
			}
			InEvaluateChangeCode = true;
		}
		try
		{
			fieldDef.OnValueChanged(arg);
			if (fieldDef != null && fieldDef.BindingSource != null && !string.IsNullOrEmpty(fieldDef.BindingSource.DataSourceGridID))
			{
				fieldDef.BindingSource.RefreshDataSource(forceToUpdate: true);
			}
			if (!arg.IsCancelled())
			{
				fieldDef.OnAfterValueChanged(arg);
			}
		}
		finally
		{
			inChangeCode = flag;
			if (Database != null)
			{
				Database.InUserChangeEvent = inUserChangeEvent;
			}
			InEvaluateChangeCode = inEvaluateChangeCode;
			if (setLastKey)
			{
				setLastKeyOverride = false;
			}
		}
		if (arg.IsCancelled())
		{
			if (arg.Cancel is string)
			{
				Database.OnShowError(new ShowErrorEventArgs(arg.Cancel.ToString()));
			}
			settingToPrevious = true;
			arg.Row[fieldDef.FieldName] = arg.PreviousValue;
			settingToPrevious = false;
			pushFieldValueToBoundControls(arg.Row, fieldDef.FieldName, force: true);
			fieldDef.OnValueChanged(arg);
			return true;
		}
		if (arg.FocusField != null && arg.FocusField.Length != 0)
		{
			OnFocusField(new FocusFieldEventArgs(arg.FocusField, arg.Row));
			arg.FocusField = string.Empty;
		}
		return false;
	}

	protected void SetLastKey(DataRow row, object value)
	{
		object previousValue = row[PrimaryTable.FirstEditableKeyField];
		M1Database databaseForRow = GetDatabaseForRow(row);
		bool flag = setLastKeyOverride;
		setLastKeyOverride = true;
		row[PrimaryTable.FirstEditableKeyField] = value;
		setLastKeyOverride = flag;
		OnFieldValueChanged(Fields[PrimaryTable.FirstEditableKeyField], setLastKey: true, new FieldDefinition.FieldValueChangedEventArgs(databaseForRow, row, isCurrentRow: true, previousValue, Transaction));
		string[] keyFieldsArray = PrimaryTable.KeyFieldsArray;
		foreach (string key in keyFieldsArray)
		{
			Fields[key].EvaluateReadOnlyExpression(databaseForRow, row);
		}
		OnLastKeyFieldValueChanged();
	}

	public void SetFields(DataRow row, string[] fieldNames, object[] fieldValues)
	{
		bool flag = settingRelatedValues;
		settingRelatedValues = true;
		row.BeginEdit();
		for (int i = 0; i < fieldNames.Length; i++)
		{
			if (!row[fieldNames[i]].Equals(fieldValues[i]))
			{
				row[fieldNames[i]] = fieldValues[i];
			}
		}
		row.EndEdit();
		settingRelatedValues = flag;
	}

	public M1Database GetDatabaseForRow(DataRow row)
	{
		if (row.Table.Columns.Contains("Dataset"))
		{
			return Databases[row.Field<string>("Dataset")];
		}
		return CurrentDatabase;
	}

	public DataRow GetDataRow(object[] keyValues)
	{
		DataRow dataRow = null;
		if (Query.DataView.Table.PrimaryKey != null && Query.DataView.Table.PrimaryKey.Length != 0)
		{
			dataRow = Query.DataView.Table.Rows.Find(keyValues);
		}
		if (dataRow == null)
		{
			bool flag = true;
			foreach (DataRow row in Query.DataView.Table.Rows)
			{
				flag = true;
				DataRowVersion version = ((row.RowState != DataRowState.Deleted) ? DataRowVersion.Current : DataRowVersion.Original);
				for (int i = 0; i < PrimaryTable.KeyFieldsArray.Length; i++)
				{
					if (!row[PrimaryTable.KeyFieldsArray[i], version].Equals(keyValues[i]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return row;
				}
			}
		}
		return dataRow;
	}

	public void ActivateRowByListIndex(int index)
	{
		if (base.Position != index)
		{
			base.Position = index;
		}
	}

	public void ExecuteCodeForRow(string code, DataRow row)
	{
		M1Database databaseForRow = GetDatabaseForRow(row);
		if (!PrimaryTable.GetReadOnlyExpression(databaseForRow, row, Transaction))
		{
			PrimaryTable.ExecuteScript(code, databaseForRow, row);
		}
	}

	public void ExecuteCodeForRows(string code)
	{
		foreach (DataRowView item in GetDataView())
		{
			ExecuteCodeForRow(code, item.Row);
		}
	}

	public void ExecuteCodeForRows(string code, IEnumerable<DataRow> rows)
	{
		if (code.Length == 0 || rows == null)
		{
			return;
		}
		foreach (DataRow row in rows)
		{
			ExecuteCodeForRow(code, row);
		}
	}

	protected bool IsMatchingRow(DataRow bsRow, DataRow rowToCheck, FieldDefinition field)
	{
		if (bsRow == rowToCheck)
		{
			return true;
		}
		if (field != null && Fields.Contains(field))
		{
			string[] relatedFieldsAndCurrentFieldArray = field.RelatedFieldsAndCurrentFieldArray;
			foreach (string columnName in relatedFieldsAndCurrentFieldArray)
			{
				if (bsRow[columnName] != rowToCheck[columnName])
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public bool ActivateRow(DataRow row, FieldDefinition field, bool doFlash)
	{
		if (base.Position != -1 && IsMatchingRow(((DataRowView)base.Current).Row, row, field))
		{
			if (doFlash)
			{
				if (field != null)
				{
					field.OnFlash(EventArgs.Empty);
				}
				else
				{
					OnFlashRow(new FlashRowEventArgs
					{
						Row = row
					});
				}
			}
			return true;
		}
		if (PrimaryTable != null && PrimaryTable.ParentBindingSource != null && !PrimaryTable.ParentBindingSource.ActivateRow(PrimaryTable.GetParentDataRow(row), null, doFlash: false))
		{
			return false;
		}
		for (int i = 0; i < Query.DataView.Count; i++)
		{
			if (!IsMatchingRow(Query.DataView[i].Row, row, field))
			{
				continue;
			}
			CurrencyManager.Position = i;
			if (doFlash)
			{
				if (field != null)
				{
					field.OnFlash(EventArgs.Empty);
				}
				else
				{
					OnFlashRow(new FlashRowEventArgs
					{
						Row = row
					});
				}
			}
			return true;
		}
		return false;
	}

	public void SetPositionByDataRow(DataRow row)
	{
		for (int i = 0; i < Query.DataView.Count; i++)
		{
			if (Query.DataView[i].Row == row)
			{
				CurrencyManager.Position = i;
				break;
			}
		}
	}

	public void OnFlashRow(FlashRowEventArgs e)
	{
		this.FlashRow?.Invoke(this, e);
	}

	private void pushAllFieldValuesToBoundControls()
	{
		if (base.Position == -1 || base.Current == null)
		{
			return;
		}
		foreach (Binding binding in CurrencyManager.Bindings)
		{
			binding.ReadValue();
		}
	}

	private void pushFieldValueToBoundControls(DataRow row, string fieldName, bool force)
	{
		if (force)
		{
			foreach (Binding binding3 in CurrencyManager.Bindings)
			{
				if (binding3.BindingMemberInfo.BindingField.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
				{
					PropertyInfo property = binding3.BindableComponent.GetType().GetProperty(binding3.PropertyName);
					object obj = row[binding3.BindingMemberInfo.BindingField];
					if (obj is DBNull)
					{
						property.SetValue(binding3.BindableComponent, null, null);
					}
					else if (property.PropertyType == typeof(decimal))
					{
						property.SetValue(binding3.BindableComponent, Convert.ToDecimal(obj), null);
					}
					else if (property.PropertyType == typeof(string))
					{
						property.SetValue(binding3.BindableComponent, obj.ToString().Trim(), null);
					}
					else
					{
						property.SetValue(binding3.BindableComponent, obj, null);
					}
				}
			}
			return;
		}
		foreach (Binding binding4 in CurrencyManager.Bindings)
		{
			if (binding4.BindingMemberInfo.BindingField.Equals(fieldName, StringComparison.OrdinalIgnoreCase))
			{
				binding4.ReadValue();
			}
		}
	}

	protected internal FieldDefinition GetChildLinkField()
	{
		if (_ChildLinkField.Length == 0)
		{
			if (boundFieldDefinition != null && boundFieldDefinition.RelatedFieldsAndCurrentFieldArray.Length != 0)
			{
				return Fields[PrimaryTable.KeyFieldsArray[boundFieldDefinition.RelatedFieldsAndCurrentFieldArray.Length - 1]];
			}
			if (boundFieldDefinition != null && boundFieldDefinition.RelatedTableKeyFieldsArray.Length != 0)
			{
				return Fields[PrimaryTable.KeyFieldsArray[boundFieldDefinition.RelatedTableKeyFieldsArray.Length - 1]];
			}
			return Fields[PrimaryTable.KeyFieldsArray[PrimaryTable.KeyFieldsArray.Length - 2]];
		}
		return Fields[_ChildLinkField];
	}

	private int getIndexInArray(string[] items, string value)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].Equals(value, StringComparison.CurrentCultureIgnoreCase))
			{
				return i;
			}
		}
		return -1;
	}

	private string[] getChildLinkFields()
	{
		if (_ChildLinkField.Length == 0)
		{
			if (boundFieldDefinition != null)
			{
				int indexInArray = getIndexInArray(boundFieldDefinition.Table.KeyFieldsArray, boundFieldDefinition.FieldName);
				if (indexInArray != -1)
				{
					string[] array = new string[indexInArray + 1];
					for (int i = 0; i <= indexInArray; i++)
					{
						array[i] = PrimaryTable.KeyFieldsArray[i];
					}
					return array;
				}
				return PrimaryTable.KeyFieldsArray;
			}
			return PrimaryTable.KeyFieldsArray;
		}
		int indexInArray2 = getIndexInArray(PrimaryTable.KeyFieldsArray, _ChildLinkField);
		if (indexInArray2 != -1)
		{
			string[] array2 = new string[indexInArray2 + 1];
			for (int j = 0; j <= indexInArray2; j++)
			{
				array2[j] = PrimaryTable.KeyFieldsArray[j];
			}
			return array2;
		}
		return Fields[_ChildLinkField].RelatedFieldsAndCurrentFieldArray;
	}

	protected void OnAfterChildFilterSet(EventArgs e)
	{
		this.AfterChildFilterSet?.Invoke(this, e);
	}

	protected void CheckRemoveWhereOnSave()
	{
		if (AutoRemoveWhereOnSave.Length == 0)
		{
			return;
		}
		DataRow[] array = new DataRow[GetDataView().Table.Rows.Count];
		GetDataView().Table.Rows.CopyTo(array, 0);
		DataRow[] array2 = array;
		foreach (DataRow dataRow in array2)
		{
			M1Database databaseForRow = GetDatabaseForRow(dataRow);
			if (PrimaryTable.EvaluateScriptExpressionBool(AutoRemoveWhereOnSave, databaseForRow, dataRow))
			{
				Remove(databaseForRow, dataRow, isTopLevel: true, skipValidation: true);
			}
		}
	}

	protected override void OnBindingComplete(BindingCompleteEventArgs e)
	{
		if (e.BindingCompleteState == BindingCompleteState.Exception)
		{
			_ = e.Exception;
		}
		base.OnBindingComplete(e);
	}

	public void ResequenceKeys()
	{
		ResequenceKeys(markModified: true);
	}

	public void ResequenceKeys(bool markModified)
	{
		bool flag = modifiedLocked;
		modifiedLocked = !markModified;
		try
		{
			DataView dataView = GetDataView();
			if (dataView.Count == 0)
			{
				return;
			}
			_ = PrimaryTable.ParentBindingSource.CurrentAsDataRow;
			object[] parentKeyValuesUsingParentBindingSource = PrimaryTable.GetParentKeyValuesUsingParentBindingSource();
			DataTable dataTable = GetDataTable();
			object[] array = new object[PrimaryTable.KeyFieldsArray.Length];
			if (array.Length <= parentKeyValuesUsingParentBindingSource.Length)
			{
				return;
			}
			parentKeyValuesUsingParentBindingSource.CopyTo(array, 0);
			foreach (DataRowView item in dataView)
			{
				SetLastKey(item.Row, item.Row.DefaultValueForType(item.Row.Table.Columns[PrimaryTable.LastKeyField].DataType));
			}
			foreach (DataRowView item2 in dataView)
			{
				SetLastKey(item2.Row, Database.NextIDs.GetNextIDForTable(PrimaryTable.TableName, parentKeyValuesUsingParentBindingSource, dataTable, Transaction));
			}
		}
		finally
		{
			modifiedLocked = flag;
		}
	}

	public void ApplyRowValuesToAllRows(DataRow row, string fieldsToIgnore)
	{
		if (row == null)
		{
			return;
		}
		ReferencedFieldsList referencedFieldsList = new ReferencedFieldsList();
		if (fieldsToIgnore.Length != 0)
		{
			referencedFieldsList.AddRange(fieldsToIgnore.Split(','));
		}
		if (PrimaryTable != null)
		{
			if (PrimaryTable.KeyFieldsArray != null)
			{
				referencedFieldsList.AddRange(PrimaryTable.KeyFieldsArray);
			}
			if (PrimaryTable.UniqueField.Length != 0)
			{
				referencedFieldsList.Add(PrimaryTable.UniqueField);
			}
		}
		foreach (DataRowView item in GetDataView())
		{
			if (item.Row == row)
			{
				continue;
			}
			foreach (DataColumn column in row.Table.Columns)
			{
				if (!referencedFieldsList.Contains(column.ColumnName, StringComparer.CurrentCultureIgnoreCase) && !Fields[column.ColumnName].GetReadOnlyExpression(GetDatabaseForRow(item.Row), item.Row))
				{
					item.Row[column] = row[column];
				}
			}
		}
	}

	public void AllocateValueToAllRows(string field, object value, int decimals)
	{
		double num = Convert.ToDouble(value);
		int num2 = 0;
		int num3 = 0;
		double num4 = 0.0;
		double num5 = Math.Pow(10.0, decimals);
		foreach (DataRowView item in GetDataView())
		{
			if (!Fields[field].GetReadOnlyExpression(GetDatabaseForRow(item.Row), item.Row))
			{
				num2++;
			}
		}
		double num6 = num / (double)num2;
		num6 = ((!(num6 >= 0.0)) ? (Math.Ceiling(num6 * num5) / num5) : (Math.Floor(num6 * num5) / num5));
		foreach (DataRowView item2 in GetDataView())
		{
			if (!Fields[field].GetReadOnlyExpression(GetDatabaseForRow(item2.Row), item2.Row))
			{
				num3++;
				if (num == 0.0)
				{
					item2.Row[field] = 0;
					continue;
				}
				if (num3 == num2)
				{
					item2.Row[field] = num - num4;
					continue;
				}
				item2.Row[field] = num6;
				num4 += num6;
			}
		}
	}

	public bool IsAnyRowFieldNotEmpty(string field)
	{
		DataView dataView = GetDataView();
		DataColumn dataColumn = dataView.Table.Columns[field];
		foreach (DataRowView item in dataView)
		{
			if (dataColumn.DataType == typeof(DateTime) || dataColumn.DataType == typeof(DateTime?))
			{
				if (item.Row[dataColumn] != DBNull.Value)
				{
					return true;
				}
			}
			else if (dataColumn.DataType == typeof(decimal) || dataColumn.DataType == typeof(int) || dataColumn.DataType == typeof(double) || dataColumn.DataType == typeof(byte) || dataColumn.DataType == typeof(short) || dataColumn.DataType == typeof(int) || dataColumn.DataType == typeof(long))
			{
				if (item.Row[dataColumn] != DBNull.Value && Convert.ToDouble(item.Row[dataColumn]) != 0.0)
				{
					return true;
				}
			}
			else if (dataColumn.DataType == typeof(string))
			{
				if (item.Row[dataColumn] != DBNull.Value && item.Row.Field<string>(dataColumn).Trim().Length != 0)
				{
					return true;
				}
			}
			else if (dataColumn.DataType == typeof(bool) && item.Row[dataColumn] != DBNull.Value && item.Row.Field<bool>(dataColumn))
			{
				return true;
			}
		}
		return false;
	}

	public double GetTotal(string field)
	{
		double num = 0.0;
		foreach (DataRowView item in GetDataView())
		{
			num += Convert.ToDouble(item.Row[field]);
		}
		return num;
	}

	public M1AdoRecordsetProxy GetRecordset(object parentRow = null)
	{
		DataRow parentRow2 = null;
		if (parentRow != null)
		{
			parentRow2 = (DataRow)parentRow;
		}
		DataView dataView = GetDataView(parentRow2);
		if (dataView == null)
		{
			return null;
		}
		return new M1AdoRecordsetProxy(dataView, CurrentAsDataRow, new M1AdoConnectionProxy
		{
			Database = Database,
			SqlTransaction = Transaction
		}, Query.DataAdapter);
	}

	public int SetRowCount(object rowCount)
	{
		return SetRowCount(Convert.ToInt32(rowCount));
	}

	public int SetRowCount(int rowCount)
	{
		return SetRowCount(rowCount, markModified: true);
	}

	public int SetRowCount(int rowCount, bool markModified)
	{
		if (inSetRowCount || Query.DataView.RowFilter.Equals("0=1"))
		{
			return 0;
		}
		bool flag = modifiedLocked;
		modifiedLocked = !markModified;
		int num = 0;
		inSetRowCount = true;
		try
		{
			if (rowCount < 0)
			{
				rowCount = 0;
			}
			if (Count != rowCount)
			{
				if (Count < rowCount)
				{
					DataRow dataRow = PrimaryTable.ParentBindingSource?.CurrentAsDataRow;
					object[] array = PrimaryTable?.GetParentKeyValuesUsingParentBindingSource();
					DataTable dataTable = GetDataTable();
					object[] array2 = new object[PrimaryTable.KeyFieldsArray.Length];
					if (dataRow != null && array != null && dataTable != null)
					{
						array.CopyTo(array2, 0);
						while (Count < rowCount)
						{
							if (array2.Length > array.Length)
							{
								array2[array2.Length - 1] = Database.NextIDs.GetNextIDForTable(PrimaryTable.TableName, array, dataTable, Transaction);
							}
							AddNew(Database, dataRow, array2, null);
							num++;
						}
					}
					if (Errors != null)
					{
						OnValidate(new ValidateArgs
						{
							Errors = Errors
						});
					}
				}
				else if (Query != null && Query.DataView != null)
				{
					while (Count > rowCount)
					{
						Remove(Query.DataView[Count - 1].Row);
						num--;
					}
				}
			}
		}
		finally
		{
			modifiedLocked = flag;
			inSetRowCount = false;
		}
		return num;
	}

	private void parentBindingSourceNoRecordEvent()
	{
		((DataView)base.DataSource).RowFilter = "0=1";
		prevRelatedFieldValues = null;
		prevParentDataRow = null;
		PrimaryTable.SetDisableAddNewOverride(value: true, Database, CurrentAsDataRow, Transaction);
	}

	private bool ShouldSerializeParentFieldValue()
	{
		if (_ParentFieldValue == null || _ParentFieldValue.ToString().Length == 0)
		{
			return false;
		}
		return true;
	}

	private void OnColumnErrorChanged(FieldDefinition.ColumnErrorChangedEventArgs e)
	{
		this.ColumnErrorChanged?.Invoke(this, e);
	}

	private void OnCacheCleared()
	{
		this.CacheCleared?.Invoke(this, EventArgs.Empty);
	}

	public void OnValidate(ValidateArgs e)
	{
		OnValidate(e, null);
	}

	public void OnValidate(ValidateArgs e, DataRow rowToValidate)
	{
		if (Errors != e.Errors)
		{
			Errors = e.Errors;
		}
		EndEdit();
		DataRow currentAsDataRow = CurrentAsDataRow;
		M1Database currentDatabase = CurrentDatabase;
		SqlTransaction transaction = Transaction;
		bool flag = false;
		if (rowToValidate != null)
		{
			if (rowToValidate.RowState != DataRowState.Deleted && shouldValidateRow(currentDatabase, rowToValidate))
			{
				flag = true;
				foreach (TableDefinition table in Tables)
				{
					table.Validate(currentDatabase, rowToValidate, transaction, IsTopLevel, rowToValidate == currentAsDataRow);
				}
			}
		}
		else
		{
			foreach (DataRow item in Query.DataView.Table?.Rows)
			{
				if (item.RowState == DataRowState.Deleted || !shouldValidateRow(currentDatabase, item))
				{
					continue;
				}
				flag = true;
				foreach (TableDefinition table2 in Tables)
				{
					table2.Validate(currentDatabase, item, transaction, IsTopLevel, item == currentAsDataRow);
				}
			}
		}
		if (!flag)
		{
			foreach (TableDefinition table3 in Tables)
			{
				table3.Validate(currentDatabase, null, transaction, IsTopLevel, isCurrentRow: false);
			}
		}
		this.Validate?.Invoke(this, e);
	}

	internal bool shouldValidateRow(M1Database database, DataRow row)
	{
		if (AutoRemoveWhereOnSave.Length != 0 && PrimaryTable.EvaluateScriptExpressionBool(AutoRemoveWhereOnSave, database, row))
		{
			return false;
		}
		return true;
	}

	private void OnErrorsChanged(ValidateArgs e)
	{
		this.ErrorsChanged?.Invoke(this, e);
	}

	public void ClearErrors()
	{
		if (Errors != null)
		{
			Errors.Clear();
		}
		Errors = null;
		OnErrorsChanged(new ValidateArgs
		{
			Errors = Errors
		});
	}

	public ErrorItemsList GetErrors()
	{
		return GetErrors(changedRowsOnly: false);
	}

	public ErrorItemsList GetErrors(bool changedRowsOnly)
	{
		return GetErrors(changedRowsOnly, null);
	}

	public ErrorItemsList GetErrors(bool changedRowsOnly, DataRow rowToValidate)
	{
		if (Errors == null || rowToValidate != null)
		{
			Errors = new ErrorItemsList();
			OnValidate(new ValidateArgs
			{
				Errors = Errors
			}, rowToValidate);
		}
		if (changedRowsOnly)
		{
			ErrorItemsList errorItemsList = new ErrorItemsList();
			{
				foreach (ValidationInfo error in Errors)
				{
					if (error.Row == null || error.Row.RowState == DataRowState.Added || error.Row.RowState == DataRowState.Modified || (error.Row.RowState == DataRowState.Unchanged && error.Row == CurrentAsDataRow && Modified))
					{
						errorItemsList.Add(error);
					}
				}
				return errorItemsList;
			}
		}
		return Errors;
	}

	public void CopyDataFromBindingSource(M1BindingSource sourceBs)
	{
		ClearCache();
		copyRows(sourceBs, this);
		foreach (M1BindingSource childBindingSource in sourceBs.ChildBindingSources)
		{
			PrimaryTable.GetChildBindingSource(childBindingSource.PrimaryTable.TableName).CopyDataFromBindingSource(childBindingSource);
		}
		CurrencyManager.Refresh();
		if (PrimaryBindingSource != this)
		{
			return;
		}
		DataRow currentAsDataRow = CurrentAsDataRow;
		if (currentAsDataRow == null)
		{
			return;
		}
		if (PrimaryTable.LastKeyField.Length != 0)
		{
			if (!M1Util.IsNullOrEmpty(currentAsDataRow[PrimaryTable.LastKeyField]))
			{
				SetKeyState(currentAsDataRow, keyIsSet: true, autoIncremented: false);
				OnLastKeyFieldValueChanged();
			}
			else
			{
				SetKeyState(currentAsDataRow, keyIsSet: false, autoIncremented: false);
			}
		}
		else
		{
			SetKeyState(currentAsDataRow, keyIsSet: true, autoIncremented: false);
		}
	}

	private void copyRows(M1BindingSource sourceBs, M1BindingSource destBs)
	{
		bool loadingData = LoadingData;
		bool isBindingSuspended = base.IsBindingSuspended;
		LoadingData = true;
		if (!isBindingSuspended)
		{
			SuspendBinding();
		}
		try
		{
			DataTable dataTable = sourceBs.GetDataTable();
			DataTable dataTable2 = destBs.GetDataTable();
			List<string> list = new List<string>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (dataTable2.Columns.Contains(column.ColumnName))
				{
					list.Add(column.ColumnName);
				}
			}
			foreach (DataRow row in dataTable.Rows)
			{
				DataRow dataRow2 = dataTable2.NewRow().BlankRow();
				foreach (string item in list)
				{
					dataRow2[item] = row[item];
				}
				dataTable2.Rows.Add(dataRow2);
			}
		}
		finally
		{
			LoadingData = loadingData;
			if (!isBindingSuspended)
			{
				ResumeBinding();
			}
		}
	}

	protected void OnAllowEditChanged(EventArgs e)
	{
		this.AllowEditChanged?.Invoke(this, e);
	}

	protected void OnAllowNewChanged(EventArgs e)
	{
		this.AllowNewChanged?.Invoke(this, e);
	}

	protected void OnAllowRemoveChanged(EventArgs e)
	{
		this.AllowRemoveChanged?.Invoke(this, e);
	}

	public void OnFocusField(FocusFieldEventArgs e)
	{
		this.FocusField?.Invoke(this, e);
	}

	public new object GetService(Type serviceType)
	{
		if (serviceType == typeof(M1Database))
		{
			return Database;
		}
		if (Database != null)
		{
			return Database.GetService(serviceType);
		}
		if (User != null)
		{
			return User.GetService(serviceType);
		}
		return null;
	}

	FieldDefinition IComM1BindingSource.Fields(string name)
	{
		return Fields[name];
	}

	object IComM1BindingSource.Parameters(object index)
	{
		if (_Parameters != null)
		{
			return _Parameters[Convert.ToInt32(index) - 1];
		}
		return null;
	}

	private void BuildConcurrencyError(string errorMessage, bool showMessageBoxReload)
	{
		string text = (string.IsNullOrWhiteSpace(errorMessage) ? string.Empty : (errorMessage + Environment.NewLine));
		if (showMessageBoxReload)
		{
			text = text + "The record in table " + PrimaryTable?.TableName + " has been changed or deleted by another user." + Environment.NewLine + "Review your changes and reload the data. ";
			MessageBox.Show(text, "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		if (Errors == null)
		{
			Errors = new ErrorItemsList();
		}
		ValidationInfo validationInfo = new ValidationInfo
		{
			BindingSource = this
		};
		validationInfo.AddError(text);
		Errors.Add(validationInfo);
	}

	public void BindToFieldDefinition(FieldDefinition fieldDefinition, string propertyName)
	{
		if (boundFieldDefinition != null)
		{
			if (boundFieldDefinition.BindingSource != null)
			{
				boundFieldDefinition.BindingSource.SaveDataStarted -= BindingSource_SaveDataStarted;
				boundFieldDefinition.BindingSource.SaveDataCompleted -= BindingSource_SaveDataCompleted;
				boundFieldDefinition.BindingSource.RemoveStarted -= BindingSource_RemoveStarted;
				boundFieldDefinition.BindingSource.CurrencyModeChanged -= BindingSource_CurrencyModeChanged;
				boundFieldDefinition.BindingSource.RowActivated -= BindingSource_RowActivatedExtraCheck;
				boundFieldDefinition.BindingSource.CacheCleared -= BindingSource_CacheCleared;
				boundFieldDefinition.BindingSource.EditCancelled -= BindingSource_EditCancelled;
				boundFieldDefinition.BindingSource.NavigateAway -= BindingSource_NavigateAway;
			}
			if (!boundFieldDefinition.IsPartOfKey)
			{
				boundFieldDefinition.ValueChanged += boundFieldDefinition_ValueChanged;
			}
			PrimaryBindingSource = this;
		}
		boundFieldDefinition = fieldDefinition;
		OnBoundFieldDefinitionChanged(EventArgs.Empty);
		if (boundFieldDefinition != null)
		{
			if (boundFieldDefinition.BindingSource != null)
			{
				boundFieldDefinition.BindingSource.SaveDataStarted += BindingSource_SaveDataStarted;
				boundFieldDefinition.BindingSource.SaveDataCompleted += BindingSource_SaveDataCompleted;
				boundFieldDefinition.BindingSource.RemoveStarted += BindingSource_RemoveStarted;
				boundFieldDefinition.BindingSource.CurrencyModeChanged += BindingSource_CurrencyModeChanged;
				boundFieldDefinition.BindingSource.RowActivated += BindingSource_RowActivatedExtraCheck;
				boundFieldDefinition.BindingSource.CacheCleared += BindingSource_CacheCleared;
				boundFieldDefinition.BindingSource.EditCancelled += BindingSource_EditCancelled;
				boundFieldDefinition.BindingSource.NavigateAway += BindingSource_NavigateAway;
				PrimaryBindingSource = boundFieldDefinition.BindingSource.PrimaryBindingSource;
			}
			if (!boundFieldDefinition.IsPartOfKey)
			{
				boundFieldDefinition.ValueChanged += boundFieldDefinition_ValueChanged;
			}
			if (!inInit)
			{
				VerifyRelatedBindingSources();
			}
		}
	}

	private void boundFieldDefinition_ValueChanged(object sender, FieldDefinition.FieldValueChangedEventArgs e)
	{
		navigateWhenBsIsForeignRelation(e.Database, e.Row);
	}

	private void BindingSource_NavigateAway(object sender, EventArgs e)
	{
		OnNavigateAway();
	}

	private void BindingSource_EditCancelled(object sender, EventArgs e)
	{
		CancelEdit();
	}

	private void BindingSource_CacheCleared(object sender, EventArgs e)
	{
		ClearCache();
	}

	protected void OnBoundFieldDefinitionChanged(EventArgs e)
	{
		this.BoundFieldDefinitionChanged?.Invoke(this, e);
	}

	private void BindingSource_RowActivatedExtraCheck(object sender, QueryDatabaseEventArgs e)
	{
		if (boundFieldDefinition != null && boundFieldDefinition.BindingSource != null && !isManuallyAddedBs && boundFieldDefinition.BindingSource.CurrentAsDataRow == null)
		{
			parentBindingSourceNoRecordEvent();
		}
		if (BindingSourceLinks.Count == 1 && BindingSourceLinks[0] == BindingSourceLinkTypeEnum.CurrencyLink)
		{
			if (boundFieldDefinition != null)
			{
				boundFieldDefinition.Table.CurrencyChecked = false;
				prevFieldDefinition = boundFieldDefinition;
				BindToFieldDefinition(null, null);
			}
		}
		else
		{
			BindingSource_RowActivated(sender, e);
		}
	}

	public bool IsBoundToField(FieldDefinition testField)
	{
		return testField == boundFieldDefinition;
	}

	private void BindingSource_RowActivated(object sender, QueryDatabaseEventArgs e)
	{
		if (e.TopLevelDataRow != null)
		{
			string empty = string.Empty;
			string text = string.Empty;
			if (e.ParentDataRow != null && boundFieldDefinition != null && !boundFieldDefinition.IsPartOfKey)
			{
				navigateWhenBsIsForeignRelation(e.Database, e.ParentDataRow);
			}
			else
			{
				empty = ((e.ParentDataRow == null) ? "0=1" : PrimaryTable.GetFilterForParentRowUsingCurrentFieldNames(e.ParentDataRow));
				string[] childLinkFields = getChildLinkFields();
				if (childLinkFields != null && childLinkFields.Length != 0 && childLinkFields[0].Length != 0)
				{
					TableDefinition parentTable = e.TopLevelTables.GetParentTable(boundFieldDefinition.Table);
					if (parentTable != null)
					{
						if (e.TopLevelDataRow.RowState != DataRowState.Added)
						{
							for (int i = 0; i < parentTable.KeyFieldsArray.Length; i++)
							{
								text = text + ((text.Length != 0) ? " And " : string.Empty) + childLinkFields[i] + " = " + e.TopLevelDataRow[parentTable.KeyFieldsArray[i]].ToSql();
							}
						}
						else
						{
							text = "0=1";
						}
					}
				}
				NavigateTo(e, text, empty);
			}
		}
		prevRelatedFieldValues = null;
	}

	private void navigateWhenBsIsForeignRelation(M1Database database, DataRow parentRow)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string[] childLinkFields = getChildLinkFields();
		if (childLinkFields != null && childLinkFields.Length != 0 && childLinkFields[0].Length != 0)
		{
			if (M1Util.IsNullOrEmpty(parentRow[boundFieldDefinition.RelatedFieldsAndCurrentFieldArray[0]]))
			{
				text = "0=1";
				text2 = "0=1";
			}
			else
			{
				for (int i = 0; i < boundFieldDefinition.RelatedFieldsAndCurrentFieldArray.Length; i++)
				{
					text = text + ((text.Length != 0) ? " And " : string.Empty) + childLinkFields[i] + " = " + parentRow[boundFieldDefinition.RelatedFieldsAndCurrentFieldArray[i]].ToSql();
					text2 = text2 + ((text2.Length != 0) ? " And " : string.Empty) + childLinkFields[i] + " = " + parentRow[boundFieldDefinition.RelatedFieldsAndCurrentFieldArray[i]].ToLinq();
				}
			}
		}
		NavigateTo(database, text, text2);
	}

	private void BindingSource_CurrencyModeChanged(object sender, EventArgs e)
	{
		CurrencyMode = boundFieldDefinition.BindingSource?.CurrencyMode;
	}

	private void BindingSource_SaveDataCompleted(object sender, SaveDataCompletedEventArgs e)
	{
		OnSaveDataCompleted(e);
		if (delayedTableEventArgsList != null)
		{
			foreach (TableChangedEventArgs delayedTableEventArgs in delayedTableEventArgsList)
			{
				if (delayedTableEventArgs != null)
				{
					OnTableChanged(delayedTableEventArgs);
				}
			}
		}
		delayedTableEventArgsList = new List<TableChangedEventArgs>();
	}

	private void BindingSource_RemoveStarted(object sender, RemoveEventArgs e)
	{
		if (!(sender is M1BindingSource))
		{
			return;
		}
		if (_DoCascadeRemoveForForeignRelation)
		{
			bindingDeleteFilter = string.Empty;
		}
		if ((boundFieldDefinition != null && !boundFieldDefinition.IsPartOfKey && !_DoCascadeRemoveForForeignRelation) || !Query.IsEditable())
		{
			return;
		}
		if (bindingDeleteFilter == null)
		{
			if (boundFieldDefinition != null && boundFieldDefinition.IsPartOfKey && PrimaryTable.ParentTableLinkField != null && PrimaryTable.ParentTableLinkField.IsPartOfKey)
			{
				SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select drDFilter From DDRelations Where drPTable = @PTable And drCTable = @CTable And drPField Like '%'+@PField+'%' And drCField Like '%'+@CField+'%'");
				sqlCommand.Parameters.Add(new SqlParameter("@PTable", SqlDbType.NVarChar)).Value = boundFieldDefinition.Table.TableName;
				sqlCommand.Parameters.Add(new SqlParameter("@CTable", SqlDbType.NVarChar)).Value = PrimaryTable.ParentTableLinkField.Table.TableName;
				sqlCommand.Parameters.Add(new SqlParameter("@PField", SqlDbType.NVarChar)).Value = boundFieldDefinition.FieldName;
				sqlCommand.Parameters.Add(new SqlParameter("@CField", SqlDbType.NVarChar)).Value = PrimaryTable.ParentTableLinkField.FieldName;
				bindingDeleteFilter = Convert.ToString(DataDictionary.ExecuteScalar(sqlCommand));
			}
			if (bindingDeleteFilter == null)
			{
				bindingDeleteFilter = string.Empty;
			}
		}
		if (!bindingDeleteFilter.Equals("0=1"))
		{
			string filterForParentRowUsingCurrentFieldNames = PrimaryTable.GetFilterForParentRowUsingCurrentFieldNames(e.Row);
			{
				foreach (DataRowView item in new DataView(((DataView)base.List).Table, filterForParentRowUsingCurrentFieldNames, string.Empty, DataViewRowState.CurrentRows))
				{
					Remove(e.Database, item.Row, isTopLevel: false);
				}
				return;
			}
		}
		string filterForParentRowUsingCurrentFieldNames2 = PrimaryTable.GetFilterForParentRowUsingCurrentFieldNames(e.Row);
		foreach (DataRowView item2 in new DataView(((DataView)base.List).Table, filterForParentRowUsingCurrentFieldNames2, string.Empty, DataViewRowState.CurrentRows))
		{
			Query.DataView.Table.Rows.Remove(item2.Row);
		}
	}

	private void BindingSource_SaveDataStarted(object sender, SaveDataStartedEventArgs e)
	{
		if (Query.IsEditable())
		{
			SaveData(e);
		}
	}
}
