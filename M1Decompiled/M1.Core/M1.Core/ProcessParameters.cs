using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class ProcessParameters : IDisposable
{
	public ProcessValidationCollection Validations = new ProcessValidationCollection();

	public List<object[]> PromptFieldValues = new List<object[]>();

	public string[] PromptFieldNames = new string[0];

	public bool PromptFieldAllowMultiples;

	public List<object[]> KeyValues = new List<object[]>();

	public string[] KeyValueFieldNames = new string[0];

	public string KeyValueTableName = string.Empty;

	public string[] ExtraFieldNames = new string[0];

	public string Description = string.Empty;

	public string GridID = string.Empty;

	public string NotificationGridID = string.Empty;

	public string NotificationMessage = string.Empty;

	public string NotificationZeroMessage = string.Empty;

	public string ContinueMessage = string.Empty;

	public string SecurityRole = string.Empty;

	public string HelpLink = string.Empty;

	public bool ShowRefresh;

	public bool MultipleDestinationRowsCreated;

	public int AutoRefreshInterval;

	public List<PromptFieldValidation> PromptFieldValidations = new List<PromptFieldValidation>();

	public List<AdditionalFilterParameter> AdditionalFilterParameters = new List<AdditionalFilterParameter>();

	public string[] DefaultValueFieldNames = new string[0];

	public Dictionary<string, object> DefaultValueFieldNamesInitialValues;

	public string DefaultValueFilterExpression = string.Empty;

	public Dictionary<string, object> DefaultValues = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

	public Dictionary<string, object> UseCurrentValueFields = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

	public IServiceProvider ServiceProvider;

	protected string[] HeaderSourceFields;

	protected string HeaderSourceTable = string.Empty;

	public string[] HeaderDestinationFields;

	protected bool CheckFixForeign = true;

	protected bool HeaderFixForeign = true;

	public M1BindingSource BindingSource;

	public bool CreatedBindingSource;

	public string CreatedBindingSourceCaption;

	public string BindingSourceTable = string.Empty;

	public int Duration = 5;

	protected Dictionary<DataRow, List<object[]>> _ProcessedHeaderKeys;

	public static List<string> RunTransferProcess(string processType, IServiceProvider provider, List<object[]> promptValues, ProcessCheckValidation checkValidation)
	{
		ProcessParameters processParameters = CreateTransferProcess(processType, provider, null);
		M1Database m1Database = (M1Database)provider.GetService(typeof(M1Database));
		List<string> list = new List<string>();
		if (!string.IsNullOrWhiteSpace(processParameters.SecurityRole) && !m1Database.Security.IsInRole(processParameters.SecurityRole))
		{
			list.Add($"{getTransferCaption(processParameters)} is not available because {processParameters.SecurityRole} has been marked as no access by your administrator.");
		}
		if (!string.IsNullOrWhiteSpace(processParameters.BindingSourceTable) && !m1Database.Security.IsInRoleByTable(processParameters.BindingSourceTable, "Add"))
		{
			list.Add($"{getTransferCaption(processParameters)} is not available because it references the {processParameters.BindingSourceTable} table, which you do not have access to modify.");
		}
		if (list.Count == 0)
		{
			StartProcessEventArgs e = processParameters.Run(promptValues, checkValidation);
			if (e.ValidationMessages != null && e.ValidationMessages.Count != 0)
			{
				foreach (ValidationInfo validationMessage in e.ValidationMessages)
				{
					foreach (ErrorItem error in validationMessage.Errors)
					{
						list.Add(error.ErrorText);
					}
				}
			}
			if (e.Messages != null && e.Messages.Count != 0)
			{
				list.AddRange(e.Messages);
			}
		}
		return list;
	}

	private static string getTransferCaption(ProcessParameters proc)
	{
		if (!string.IsNullOrWhiteSpace(proc.CreatedBindingSourceCaption))
		{
			return proc.CreatedBindingSourceCaption;
		}
		proc.VerifyBindingSource();
		return proc.BindingSource.Query.Description;
	}

	public static ProcessParameters CreateTransferProcess(string processType, IServiceProvider provider, List<object[]> promptValues)
	{
		M1DataDictionary obj = (M1DataDictionary)provider.GetService(typeof(M1DataDictionary));
		_ = (M1Database)provider.GetService(typeof(M1Database));
		ProcessParameters processParameters = (ProcessParameters)Activator.CreateInstance(obj.AppExtensions.GetTypeFromCodeAssemblies(processType), provider);
		if (promptValues != null)
		{
			processParameters.PromptFieldValues = promptValues;
		}
		return processParameters;
	}

	public ProcessParameters(IServiceProvider provider, bool multipleDestinationRowsCreated = false)
	{
		MultipleDestinationRowsCreated = multipleDestinationRowsCreated;
		if (provider is M1BindingSource)
		{
			M1BindingSource m1BindingSource = (BindingSource = (M1BindingSource)provider);
			if (m1BindingSource != null)
			{
				ServiceProvider = m1BindingSource.CurrentDatabase;
			}
			CreatedBindingSource = false;
		}
		else
		{
			CreatedBindingSource = true;
			ServiceProvider = provider;
		}
		OnLoad();
		CheckFieldsForParameters();
		if (CreatedBindingSource || BindingSource == null || DefaultValueFieldNames == null || DefaultValueFieldNames.Length == 0)
		{
			return;
		}
		if (DefaultValueFieldNamesInitialValues == null)
		{
			DefaultValueFieldNamesInitialValues = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
		}
		string[] defaultValueFieldNames = DefaultValueFieldNames;
		foreach (string text in defaultValueFieldNames)
		{
			if (BindingSource.Fields.Contains(text) && !DefaultValueFieldNamesInitialValues.ContainsKey(text))
			{
				DefaultValueFieldNamesInitialValues.Add(text, BindingSource.CurrentAsDataRow[text]);
			}
		}
	}

	public bool ValidateAndSave(M1BindingSource bindingSource, StartProcessEventArgs arg)
	{
		return ValidateAndSave(bindingSource, arg, hideWarnings: false);
	}

	public bool ValidateAndSave(M1BindingSource bindingSource, StartProcessEventArgs arg, bool hideWarnings)
	{
		int num = 0;
		ErrorItemsList errors = bindingSource.GetErrors(changedRowsOnly: false);
		errors.RemoveSkippedErrors(arg.SkippedErrors);
		if (errors.Count != 0)
		{
			if (arg.ValidationMessages == null)
			{
				arg.ValidationMessages = new ErrorItemsList();
			}
			else
			{
				arg.ValidationMessages.Clear();
			}
			foreach (ValidationInfo item in errors)
			{
				if ((item.Field == null || !item.Field.Custom) && (!item.ErrorCount.Equals(0) || (item.ErrorCount.Equals(0) && !hideWarnings)))
				{
					num += item.ErrorCount;
					item.RowDescription = item.GetRowDescription();
					arg.ValidationMessages.Add(item);
				}
			}
		}
		arg.ApplyFilterRegex();
		bool flag = num == 0;
		if (arg.CheckValidationForSave != null)
		{
			if (arg.Messages.Count > 0)
			{
				arg.SelectedItems.RemoveAll((ProcessSelectedItemValues w) => w.DiscardSave);
				arg.Cancel = arg.SelectedItems.Count <= 0;
				if (errors != null && errors.Count > 0)
				{
					if (CreatedBindingSource)
					{
						bindingSource.CancelEdit();
						bindingSource.ClearCache();
						if (!MultipleDestinationRowsCreated)
						{
							BindingSource.AddNew();
						}
					}
					return false;
				}
			}
			flag = arg.CheckValidationForSave(bindingSource.Database, arg.ValidationMessages, arg);
		}
		if (flag)
		{
			bindingSource.SaveData();
			return true;
		}
		arg.Messages.Clear();
		if (CreatedBindingSource)
		{
			bindingSource.CancelEdit();
			bindingSource.ClearCache();
			if (!MultipleDestinationRowsCreated)
			{
				DataRow keyToNextAvailable = BindingSource.AddNew() as DataRow;
				BindingSource.SetKeyToNextAvailable(keyToNextAvailable);
			}
		}
		return false;
	}

	public virtual void ConstructPromptFieldsWhere(object sender, PromptFieldsWhereEventArgs e)
	{
	}

	protected virtual void OnLoad()
	{
	}

	public virtual void RunNegativeQtyOnHandMethod(StartProcessEventArgs arg)
	{
	}

	public StartProcessEventArgs Run(IEnumerable<object[]> promptFieldValues, ProcessCheckValidation checkValidation)
	{
		M1Database m1Database = ServiceProvider.GetService(typeof(M1Database)) as M1Database;
		SqlCommand sqlCommand = m1Database.NewSqlCommand(string.Empty);
		StringBuilder stringBuilder = new StringBuilder();
		string[] keyValueFieldNames = KeyValueFieldNames;
		foreach (string value in keyValueFieldNames)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(value);
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		int num = 0;
		foreach (object[] promptFieldValue in promptFieldValues)
		{
			num++;
			stringBuilder2.Length = 0;
			for (int j = 0; j < promptFieldValue.Length; j++)
			{
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(" And ");
				}
				stringBuilder2.Append(KeyValueFieldNames[j] + " = @" + KeyValueFieldNames[j] + num);
				sqlCommand.Parameters.Add(new SqlParameter("@" + KeyValueFieldNames[j] + num, promptFieldValue[j]));
			}
			if (stringBuilder3.Length != 0)
			{
				stringBuilder3.Append(" Or ");
			}
			stringBuilder3.Append("(" + stringBuilder2.ToString() + ")");
		}
		sqlCommand.CommandText = "Select " + stringBuilder.ToString() + " From " + KeyValueTableName + " Where " + stringBuilder3.ToString();
		DataTable dataTable = m1Database.GetDataTable(sqlCommand);
		List<ProcessSelectedItemValues> list = new List<ProcessSelectedItemValues>();
		List<object> list2 = new List<object>();
		foreach (DataRow row in dataTable.Rows)
		{
			list2.Clear();
			keyValueFieldNames = KeyValueFieldNames;
			foreach (string columnName in keyValueFieldNames)
			{
				list2.Add(row[columnName]);
			}
			list.Add(new ProcessSelectedItemValues
			{
				KeyValues = list2.ToArray()
			});
		}
		StartProcessEventArgs e = new StartProcessEventArgs(null, list, null, checkValidation);
		Run(e);
		return e;
	}

	public void Run(object sender, StartProcessEventArgs e)
	{
		Run(e);
	}

	public void Run(StartProcessEventArgs arg)
	{
		VerifyBindingSource(createWithNextId: false);
		if (BindingSource != null)
		{
			SetDefaultFieldValues(arg, BindingSource.CurrentAsDataRow);
		}
		OnRun(arg);
		if (!arg.Cancel)
		{
			if (!CreatedBindingSource || BindingSource == null || MultipleDestinationRowsCreated)
			{
				return;
			}
			DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
			BindingSource.SetKeyToNextAvailable(currentAsDataRow);
			if (!ValidateAndSave(BindingSource, arg))
			{
				return;
			}
			if (arg.KeysCreated == null || arg.KeysCreated.Count == 0)
			{
				arg.KeysCreated = new List<object[]>();
				List<object> list = new List<object>();
				string[] keyFieldsArray = BindingSource.PrimaryTable.KeyFieldsArray;
				foreach (string columnName in keyFieldsArray)
				{
					list.Add(currentAsDataRow[columnName]);
				}
				arg.KeysCreated.Add(list.ToArray());
				arg.OpenKeysWithObjectID = BindingSource.PrimaryTable.DefaultFormCollectionID;
			}
			arg.BindingSource = BindingSource;
		}
		else if (CreatedBindingSource && BindingSource != null)
		{
			BindingSource.CancelEdit();
			BindingSource.ClearCache();
			if (!MultipleDestinationRowsCreated)
			{
				BindingSource.AddNew();
			}
		}
	}

	protected void SetDefaultFieldValues(StartProcessEventArgs arg, DataRow row)
	{
		if (arg.DefaultFieldValues == null || arg.DefaultFieldValues.Count == 0 || row == null)
		{
			return;
		}
		foreach (KeyValuePair<string, object> defaultFieldValue in arg.DefaultFieldValues)
		{
			if (row.Table.Columns.Contains(defaultFieldValue.Key))
			{
				row[defaultFieldValue.Key] = defaultFieldValue.Value;
			}
		}
	}

	protected virtual void OnRun(StartProcessEventArgs arg)
	{
	}

	public void GetData(object sender, GetDataEventArgs e)
	{
		OnGetData(e);
	}

	protected virtual void OnGetData(GetDataEventArgs arg)
	{
	}

	protected void CheckForHeaderKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		if (_ProcessedHeaderKeys == null)
		{
			_ProcessedHeaderKeys = new Dictionary<DataRow, List<object[]>>();
		}
		string[] array = new string[1] { KeyValueFieldNames[0] };
		object[] array2 = new object[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = sourceHeaderRow[array[i]];
		}
		if (!_ProcessedHeaderKeys.ContainsKey(destinationHeaderRow))
		{
			_ProcessedHeaderKeys.Add(destinationHeaderRow, new List<object[]>());
		}
		List<object[]> list = _ProcessedHeaderKeys[destinationHeaderRow];
		bool flag = false;
		foreach (object[] item in list)
		{
			flag = true;
			for (int j = 0; j < array.Length; j++)
			{
				if (!item[j].Equals(array2[j]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			list.Add(array2);
			if (list.Count > 1)
			{
				TransferHeaderAddCurrencyFieldsOnly(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
			}
			else
			{
				TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
			}
			TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		}
	}

	protected virtual void TransferHeaderAddCurrencyFieldsOnly(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		M1BindingSource bindingSource = parm.BindingSource;
		if (destinationHeaderRow == null)
		{
			destinationHeaderRow = bindingSource.CurrentAsDataRow;
		}
		foreach (KeyValuePair<string, string> field in headerFieldMatches.Fields)
		{
			FieldDefinition fieldDefinition = bindingSource.Fields[field.Value];
			if ((fieldDefinition.CurrencyType == M1CurrencyStyle.Foreign && CheckFixForeign) ? HeaderFixForeign : (fieldDefinition.CurrencyType == M1CurrencyStyle.Base && CheckFixForeign && !HeaderFixForeign))
			{
				if (field.Key.StartsWith("-"))
				{
					destinationHeaderRow[field.Value] = destinationHeaderRow.Field<decimal>(field.Value) - sourceHeaderRow.Field<decimal>(field.Key.Substring(1));
				}
				else
				{
					destinationHeaderRow[field.Value] = destinationHeaderRow.Field<decimal>(field.Value) + sourceHeaderRow.Field<decimal>(field.Key);
				}
			}
		}
	}

	protected virtual void ProcessDefaultFieldValuesForHeaderRow(DataRow headerRow, Dictionary<string, object> defaultFieldValues)
	{
		if (defaultFieldValues == null || defaultFieldValues.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<string, object> defaultFieldValue in defaultFieldValues)
		{
			if (headerRow.Table.Columns.Contains(defaultFieldValue.Key))
			{
				headerRow[defaultFieldValue.Key] = defaultFieldValue.Value;
			}
		}
	}

	protected virtual void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
	}

	protected virtual void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		M1BindingSource bindingSource = parm.BindingSource;
		if (destinationHeaderRow == null)
		{
			destinationHeaderRow = bindingSource.CurrentAsDataRow;
		}
		M1Database databaseForRow = bindingSource.GetDatabaseForRow(destinationHeaderRow);
		HeaderFixForeign = bindingSource.PrimaryTable.ShouldCurrencyRefreshUpdateBase(databaseForRow, destinationHeaderRow, null);
		string currencyCustomRateField = bindingSource.PrimaryTable.CurrencyCustomRateField;
		string currencyExchangeRateField = bindingSource.PrimaryTable.CurrencyExchangeRateField;
		string currencyRateIdField = bindingSource.PrimaryTable.CurrencyRateIdField;
		string currencyModeLocationField = bindingSource.PrimaryTable.CurrencyModeLocationField;
		foreach (KeyValuePair<string, string> field in headerFieldMatches.Fields)
		{
			FieldDefinition fieldDefinition = bindingSource.Fields[field.Value];
			if (!((fieldDefinition.CurrencyType == M1CurrencyStyle.Foreign && CheckFixForeign) ? HeaderFixForeign : (fieldDefinition.CurrencyType != M1CurrencyStyle.Base || !CheckFixForeign || !HeaderFixForeign)))
			{
				continue;
			}
			if (fieldDefinition.CurrencyType != M1CurrencyStyle.None)
			{
				if (field.Key.StartsWith("-"))
				{
					destinationHeaderRow[field.Value] = destinationHeaderRow.Field<decimal>(field.Value) - sourceHeaderRow.Field<decimal>(field.Key.Substring(1));
				}
				else
				{
					destinationHeaderRow[field.Value] = destinationHeaderRow.Field<decimal>(field.Value) + sourceHeaderRow.Field<decimal>(field.Key);
				}
				continue;
			}
			if (field.Key.StartsWith("-"))
			{
				destinationHeaderRow[field.Value] = -sourceHeaderRow.Field<decimal>(field.Key.Substring(1));
				continue;
			}
			if (field.Value.Equals(currencyExchangeRateField, StringComparison.CurrentCultureIgnoreCase))
			{
				if (destinationHeaderRow.Field<bool>(currencyCustomRateField))
				{
					destinationHeaderRow[field.Value] = sourceHeaderRow[field.Key];
				}
				continue;
			}
			parm.SetValueForField(sourceHeaderRow, destinationHeaderRow, field.Key, field.Value);
			if (field.Value.Equals(currencyRateIdField, StringComparison.CurrentCultureIgnoreCase) || field.Value.Equals(currencyModeLocationField, StringComparison.CurrentCultureIgnoreCase))
			{
				HeaderFixForeign = bindingSource.PrimaryTable.ShouldCurrencyRefreshUpdateBase(databaseForRow, destinationHeaderRow, null);
			}
		}
	}

	protected DataRow TransferLineInfo(ProcessParameters parm, DataRow sourceLineRow, M1BindingSource destinationLinesBs, MatchingFieldsInfo lineFieldMatches)
	{
		return TransferLineInfo(parm, sourceLineRow, destinationLinesBs, lineFieldMatches, null);
	}

	protected DataRow TransferLineInfo(ProcessParameters parm, DataRow sourceLineRow, M1BindingSource destinationLinesBs, MatchingFieldsInfo lineFieldMatches, DataRow parentRow)
	{
		return TransferLineInfo(parm, sourceLineRow, destinationLinesBs, lineFieldMatches, parentRow, null);
	}

	protected DataRow TransferLineInfo(ProcessParameters parm, DataRow sourceLineRow, M1BindingSource destinationLinesBs, MatchingFieldsInfo lineFieldMatches, DataRow parentRow, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = destinationLinesBs.AddNew(destinationLinesBs.Database, parentRow, null, null) as DataRow;
		foreach (KeyValuePair<string, string> field in lineFieldMatches.Fields)
		{
			FieldDefinition fieldDefinition = destinationLinesBs.Fields[field.Value];
			if (!((fieldDefinition.CurrencyType == M1CurrencyStyle.Foreign && CheckFixForeign) ? HeaderFixForeign : (fieldDefinition.CurrencyType != M1CurrencyStyle.Base || !CheckFixForeign || !HeaderFixForeign)))
			{
				continue;
			}
			if (field.Key.StartsWith("-"))
			{
				string text = field.Key.Substring(1);
				if (itemValues != null && itemValues.EditableValues != null && itemValues.EditableValues.ContainsKey(text))
				{
					dataRow[field.Value] = -Convert.ToDecimal(itemValues.EditableValues[text]);
				}
				else
				{
					dataRow[field.Value] = -sourceLineRow.Field<decimal>(text);
				}
			}
			else
			{
				string text = CheckSourceFieldForExpression(field.Key, fieldName: true);
				if (itemValues != null && itemValues.EditableValues != null && itemValues.EditableValues.ContainsKey(text))
				{
					dataRow[field.Value] = itemValues.EditableValues[text];
				}
				else
				{
					dataRow[field.Value] = sourceLineRow[text];
				}
			}
		}
		return dataRow;
	}

	protected string GetFieldCaption(string field)
	{
		M1DataDictionary obj = (M1DataDictionary)ServiceProvider.GetService(typeof(M1DataDictionary));
		SqlCommand sqlCommand = obj.NewSqlCommand("Select dfCaption From DDFields Where dfField = @Field");
		sqlCommand.Parameters.Add(new SqlParameter("@Field", SqlDbType.NVarChar)).Value = field;
		return (string)obj.ExecuteScalar(sqlCommand);
	}

	protected void CheckFieldsForParameters()
	{
		if (PromptFieldNames.Length == 0 || MultipleDestinationRowsCreated)
		{
			return;
		}
		string[] headerSourceFields = HeaderSourceFields;
		string[] headerDestinationFields = HeaderDestinationFields;
		if (headerDestinationFields == null || headerDestinationFields.Length == 0)
		{
			return;
		}
		VerifyBindingSource(createWithNextId: false);
		M1BindingSource bindingSource = BindingSource;
		string fieldCaption = GetFieldCaption(PromptFieldNames[0]);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		List<string> list = new List<string>();
		FieldDefinition field;
		for (int i = 0; i < headerDestinationFields.Length; i++)
		{
			field = bindingSource.Fields[headerDestinationFields[i]];
			if (string.IsNullOrWhiteSpace(field.RelatedTable) || field.RelatedFieldsArray.Length == 0)
			{
				continue;
			}
			string[] relatedFieldsArray = field.RelatedFieldsArray;
			foreach (string text in relatedFieldsArray)
			{
				if (!list.Contains(text, StringComparer.CurrentCultureIgnoreCase))
				{
					list.Add(text);
				}
			}
		}
		List<string> list2 = new List<string>(headerDestinationFields);
		for (int k = 0; k < headerDestinationFields.Length; k++)
		{
			field = bindingSource.Fields[headerDestinationFields[k]];
			if (string.IsNullOrWhiteSpace(field.RelatedTable))
			{
				continue;
			}
			if (field.RelatedTable.Equals("Organizations", StringComparison.CurrentCultureIgnoreCase))
			{
				string text2 = currentAsDataRow.Field<string>(field.FieldName).Trim();
				if (text2.Length != 0 && headerSourceFields[k].IndexOf('=') == -1)
				{
					string fieldName = CheckSourceFieldForExpression(headerSourceFields[k], fieldName: true);
					PromptFieldValidations.Add(new PromptFieldValidationString(fieldName, text2, $"{fieldCaption} is not for {field.Caption} {text2}."));
				}
			}
			if (list.Contains(field.FieldName, StringComparer.CurrentCultureIgnoreCase))
			{
				continue;
			}
			List<string> list3 = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			string[] relatedFieldsArray = field.RelatedFieldsAndCurrentFieldArray;
			foreach (string tempDestField in relatedFieldsArray)
			{
				string text3 = CheckSourceFieldForExpression(headerSourceFields[list2.FindIndex((string item) => item.Equals(tempDestField, StringComparison.CurrentCultureIgnoreCase))], fieldName: true);
				list3.Add(text3);
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(text3);
			}
			string sqlFilterExpression = string.Empty;
			if (bindingSource.Fields[field.RelatedFieldsAndCurrentFieldArray[0]].RelatedTable.Equals("Organizations", StringComparison.CurrentCultureIgnoreCase))
			{
				string text4 = currentAsDataRow.Field<string>(field.RelatedFieldsAndCurrentFieldArray[0]).Trim();
				if (text4.Length != 0)
				{
					string text3 = CheckSourceFieldForExpression(headerSourceFields[list2.FindIndex((string item) => item.Equals(field.RelatedFieldsAndCurrentFieldArray[0], StringComparison.CurrentCultureIgnoreCase))], fieldName: false);
					sqlFilterExpression = text3 + " = " + text4.ToSql();
				}
			}
			if (!field.RelatedTable.Equals("CurrencyRates", StringComparison.CurrentCultureIgnoreCase))
			{
				AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue(field.Caption, currentAsDataRow, field.RelatedFieldsAndCurrentFieldArray)
				{
					SqlFilterExpression = sqlFilterExpression,
					AdditionalFields = stringBuilder.ToString(),
					ValueFields = list3.ToArray()
				});
			}
		}
	}

	protected string CheckSourceFieldForExpression(string field, bool fieldName)
	{
		int num = field.IndexOf('=');
		if (num != -1)
		{
			if (fieldName)
			{
				return field.Substring(0, num);
			}
			return field.Substring(num + 1);
		}
		return field;
	}

	protected ProcessSelectedItemValues GetItemValuesFromList(List<ProcessSelectedItemValues> itemList, DataRow sourceRow)
	{
		List<object> list = new List<object>();
		string[] keyValueFieldNames = KeyValueFieldNames;
		foreach (string columnName in keyValueFieldNames)
		{
			list.Add(sourceRow[columnName]);
		}
		return GetItemValuesFromList(itemList, list.ToArray());
	}

	protected ProcessSelectedItemValues GetItemValuesFromList(List<ProcessSelectedItemValues> itemList, object[] keyValues)
	{
		foreach (ProcessSelectedItemValues item in itemList)
		{
			bool flag = true;
			for (int i = 0; i < item.KeyValues.Length; i++)
			{
				if (!item.KeyValues[i].Equals(keyValues[i]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return item;
			}
		}
		return null;
	}

	public void SetValueForField(DataRow sourceRow, DataRow destinationRow, string sourceField, string destinationField)
	{
		sourceField = CheckSourceFieldForExpression(sourceField, fieldName: true);
		if (!MultipleDestinationRowsCreated)
		{
			if (UseCurrentValueFields != null && UseCurrentValueFields.ContainsKey(sourceField))
			{
				if (UseCurrentValueFields[sourceField] != null && !destinationRow[destinationField].Equals(UseCurrentValueFields[sourceField]))
				{
					destinationRow[destinationField] = UseCurrentValueFields[sourceField];
				}
			}
			else if (DefaultValues != null && DefaultValues.ContainsKey(sourceField))
			{
				destinationRow[destinationField] = DefaultValues[sourceField];
			}
			else
			{
				destinationRow[destinationField] = sourceRow[sourceField];
			}
		}
		else
		{
			destinationRow[destinationField] = sourceRow[sourceField];
		}
	}

	public void SetLineValueForField(DataRow sourceRow, DataRow destinationRow, string sourceField, string destinationField, ProcessSelectedItemValues itemValues)
	{
		if (itemValues != null && itemValues.EditableValues != null && itemValues.EditableValues.ContainsKey(sourceField))
		{
			destinationRow[destinationField] = itemValues.EditableValues[sourceField];
		}
		else
		{
			destinationRow[destinationField] = sourceRow[sourceField];
		}
	}

	protected void VerifyBindingSource(bool createWithNextId = true)
	{
		if (BindingSource == null && !string.IsNullOrWhiteSpace(BindingSourceTable))
		{
			BindingSource = new M1BindingSource(ServiceProvider);
			BindingSource.LoadDefinition(string.Empty, BindingSourceTable, null);
			if (!MultipleDestinationRowsCreated)
			{
				BindingSource.AddNew(createWithNextId);
			}
			CreatedBindingSource = true;
		}
	}

	public string ConstructWhereClause(string[] fieldNames, List<ProcessSelectedItemValues> values)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ProcessSelectedItemValues value in values)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" Or ");
			}
			stringBuilder.Append('(');
			for (int i = 0; i < fieldNames.Length; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(" And ");
				}
				stringBuilder.Append(fieldNames[i] + "=" + value.KeyValues[i].ToSql());
			}
			stringBuilder.Append(')');
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Insert(0, '(');
			stringBuilder.Append(')');
		}
		return stringBuilder.ToString();
	}

	public void Dispose()
	{
		if (BindingSource != null)
		{
			BindingSource.Dispose();
			BindingSource = null;
		}
	}
}
