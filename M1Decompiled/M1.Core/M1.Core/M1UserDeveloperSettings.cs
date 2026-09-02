using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M1.Core;

public class M1UserDeveloperSettings
{
	public string DesignerPositions = string.Empty;

	public string VisibleDesigners = string.Empty;

	public string DesignerLoadedItems = string.Empty;

	public string DesignerContainerLeftPosition = string.Empty;

	public string DesignerContainerRightPosition = string.Empty;

	public string DesignerActiveItems = string.Empty;

	public string QuickAccessToolbarItems = string.Empty;

	public bool QuickAccessToolbarItemsLoaded;

	public string QuickAccessToolbarLocation = string.Empty;

	public bool MinimizeRibbon = true;

	public int PropertyGridDescriptionPaneHeight = 75;

	public bool LoadCodeSyntaxEditor = true;

	public M1UserCompareWindowSettings CompareWindowSettings = new M1UserCompareWindowSettings();

	public M1UserFieldBindingWindowSettings FieldBindingWindowSettings = new M1UserFieldBindingWindowSettings();

	private void OnPropChanged(EventHandler handler, EventArgs e)
	{
		handler?.Invoke(this, e);
	}

	public string GetUserProperties(M1DataDictionary dataDictionary, string userID)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select duDeveloperProperties from DDUsers Where duUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		return dataDictionary.ExecuteScalar(sqlCommand).ToString();
	}

	public void LoadSettings(string properties)
	{
		LoadDefaults();
		if (properties == null)
		{
			return;
		}
		string[] array = properties.Split('\r');
		foreach (string text in array)
		{
			int num = text.IndexOf("=");
			if (num > 0)
			{
				string text2 = text.Substring(0, num - 1).Trim().ToUpper();
				string value = text.Substring(num + 1).Trim();
				switch (text2)
				{
				case "DESIGNERPOSITIONS":
					DesignerPositions = convertPropToString(value);
					break;
				case "VISIBLEDESIGNERS":
					VisibleDesigners = convertPropToString(value);
					break;
				case "DESIGNERLOADEDITEMS":
					DesignerLoadedItems = convertPropToString(value);
					break;
				case "DESIGNERACTIVEITEMS":
					DesignerActiveItems = convertPropToString(value);
					break;
				case "DESIGNERCONTAINERSELECTEDITEMS":
					DesignerActiveItems = convertPropToString(value);
					break;
				case "DESIGNERCONTAINERLEFTPOSITION":
					DesignerContainerLeftPosition = convertPropToString(value);
					break;
				case "DESIGNERCONTAINERRIGHTPOSITION":
					DesignerContainerRightPosition = convertPropToString(value);
					break;
				case "QUICKACCESSTOOLBARITEMS":
					QuickAccessToolbarItems = convertPropToString(value);
					QuickAccessToolbarItemsLoaded = true;
					break;
				case "QUICKACCESSTOOLBARLOCATION":
					QuickAccessToolbarLocation = convertPropToString(value);
					break;
				case "MINIMIZERIBBON":
					MinimizeRibbon = convertPropToBool(value);
					break;
				case "PROPERTYGRIDDESCRIPTIONPANEHEIGHT":
					PropertyGridDescriptionPaneHeight = convertPropToInt(value);
					break;
				case "LOADCODESYNTAXEDITOR":
					LoadCodeSyntaxEditor = convertPropToBool(value);
					break;
				case "COMPAREWINDOWSETTINGS.SHOWLINENUMBERS":
					CompareWindowSettings.ShowLineNumbers = convertPropToBool(value);
					break;
				case "COMPAREWINDOWSETTINGS.SHOWLINEDETAILS":
					CompareWindowSettings.ShowLineDetails = convertPropToBool(value);
					break;
				case "COMPAREWINDOWSETTINGS.SHOWTHUMBNAILVIEW":
					CompareWindowSettings.ShowThumbnailView = convertPropToBool(value);
					break;
				case "COMPAREWINDOWSETTINGS.SHOWWHITESPACE":
					CompareWindowSettings.ShowWhitespace = convertPropToBool(value);
					break;
				case "COMPAREWINDOWSETTINGS.CURRENTFILTER":
				{
					EnumConverter enumConverter3 = new EnumConverter(typeof(DifferencesFilter));
					CompareWindowSettings.CurrentFilter = (DifferencesFilter)enumConverter3.ConvertFromString(convertPropToString(value));
					break;
				}
				case "FIELDBINDINGWINDOWSETTINGS.FILTERFIELDSONDRAG":
					FieldBindingWindowSettings.FilterFieldsOnDrag = convertPropToBool(value);
					break;
				case "FIELDBINDINGWINDOWSETTINGS.CURRENTFIELDTYPEFILTER":
				{
					EnumConverter enumConverter2 = new EnumConverter(typeof(FieldBindingLinkFieldTypeFilter));
					FieldBindingWindowSettings.CurrentFieldTypeFilter = (FieldBindingLinkFieldTypeFilter)enumConverter2.ConvertFromString(convertPropToString(value));
					break;
				}
				case "FIELDBINDINGWINDOWSETTINGS.CURRENTLINKTYPEFILTER":
				{
					EnumConverter enumConverter = new EnumConverter(typeof(FieldBindingLinkTypeFilter));
					FieldBindingWindowSettings.CurrentLinkTypeFilter = (FieldBindingLinkTypeFilter)enumConverter.ConvertFromString(convertPropToString(value));
					break;
				}
				}
			}
		}
	}

	private bool convertPropToBool(string value)
	{
		return value.Trim().ToUpper() != "FALSE";
	}

	private decimal convertPropToDecimal(string value)
	{
		decimal result = default(decimal);
		if (decimal.TryParse(value, out result))
		{
			return result;
		}
		return 0m;
	}

	private int convertPropToInt(string value)
	{
		int result = 0;
		if (int.TryParse(value, out result))
		{
			return result;
		}
		return 0;
	}

	private string convertPropToString(string value)
	{
		value = value.Trim().Substring(1);
		value = value.Substring(0, value.Length - 1);
		return value;
	}

	private string convertBoolToProp(bool value)
	{
		if (value)
		{
			return "True";
		}
		return "False";
	}

	private string convertDecimalToProp(decimal value)
	{
		return value.ToString("G");
	}

	private string convertStringToProp(string value)
	{
		return "'" + value + "'";
	}

	public void LoadDefaults()
	{
		DesignerPositions = "SolutionExplorer:BottomRight|M1PropertyGrid:BottomRight|Toolbox:BottomLeft|ScriptExplorer:TopLeft|FormCollectionDesigner:BottomLeft|GridDefinitionExplorer:TopLeft|TableDesigner:BottomLeft|";
		VisibleDesigners = "SolutionExplorer,M1PropertyGrid,Toolbox,FormCollectionDesigner,TableDesigner";
		DesignerLoadedItems = "SolutionExplorer:SolutionDefinition\\M1SOLUTION|";
		DesignerActiveItems = "BottomLeft:Toolbox|BottomRight:M1PropertyGrid|";
		DesignerContainerLeftPosition = "20%,50%";
		DesignerContainerRightPosition = "15%,50%";
		QuickAccessToolbarItems = string.Empty;
		QuickAccessToolbarItemsLoaded = false;
		QuickAccessToolbarLocation = "AboveRibbon";
		PropertyGridDescriptionPaneHeight = 75;
		MinimizeRibbon = true;
		LoadCodeSyntaxEditor = true;
		CompareWindowSettings.LoadDefaults();
		FieldBindingWindowSettings.LoadDefaults();
	}

	public void SaveSettings(DataRow userRow)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("DesignerContainerLeftPosition = " + convertStringToProp(DesignerContainerLeftPosition) + "\r");
		stringBuilder.Append("DesignerContainerRightPosition = " + convertStringToProp(DesignerContainerRightPosition) + "\r");
		stringBuilder.Append("DesignerActiveItems = " + convertStringToProp(DesignerActiveItems) + "\r");
		stringBuilder.Append("DesignerLoadedItems = " + convertStringToProp(DesignerLoadedItems) + "\r");
		stringBuilder.Append("DesignerPositions = " + convertStringToProp(DesignerPositions) + "\r");
		stringBuilder.Append("LoadCodeSyntaxEditor = " + convertBoolToProp(LoadCodeSyntaxEditor) + "\r");
		stringBuilder.Append("MinimizeRibbon = " + convertBoolToProp(MinimizeRibbon) + "\r");
		stringBuilder.Append("PropertyGridDescriptionPaneHeight = " + convertDecimalToProp(PropertyGridDescriptionPaneHeight) + "\r");
		stringBuilder.Append("QuickAccessToolbarItems = " + convertStringToProp(QuickAccessToolbarItems) + "\r");
		stringBuilder.Append("QuickAccessToolbarLocation = " + convertStringToProp(QuickAccessToolbarLocation) + "\r");
		stringBuilder.Append("VisibleDesigners = " + convertStringToProp(VisibleDesigners) + "\r");
		stringBuilder.Append("CompareWindowSettings.ShowLineNumbers = " + convertBoolToProp(CompareWindowSettings.ShowLineNumbers) + "\r");
		stringBuilder.Append("CompareWindowSettings.ShowLineDetails = " + convertBoolToProp(CompareWindowSettings.ShowLineDetails) + "\r");
		stringBuilder.Append("CompareWindowSettings.ShowThumbnailView = " + convertBoolToProp(CompareWindowSettings.ShowThumbnailView) + "\r");
		stringBuilder.Append("CompareWindowSettings.ShowWhitespace = " + convertBoolToProp(CompareWindowSettings.ShowWhitespace) + "\r");
		EnumConverter enumConverter = new EnumConverter(typeof(DifferencesFilter));
		stringBuilder.Append("CompareWindowSettings.CurrentFilter = " + convertStringToProp(enumConverter.ConvertToString(CompareWindowSettings.CurrentFilter)) + "\r");
		stringBuilder.Append("FieldBindingWindowSettings.FilterFieldsOnDrag = " + convertBoolToProp(FieldBindingWindowSettings.FilterFieldsOnDrag) + "\r");
		EnumConverter enumConverter2 = new EnumConverter(typeof(FieldBindingLinkFieldTypeFilter));
		stringBuilder.Append("FieldBindingWindowSettings.CurrentFieldTypeFilter = " + convertStringToProp(enumConverter2.ConvertToString(FieldBindingWindowSettings.CurrentFieldTypeFilter)) + "\r");
		EnumConverter enumConverter3 = new EnumConverter(typeof(FieldBindingLinkTypeFilter));
		stringBuilder.Append("FieldBindingWindowSettings.CurrentLinkTypeFilter = " + convertStringToProp(enumConverter3.ConvertToString(FieldBindingWindowSettings.CurrentLinkTypeFilter)) + "\r");
		userRow.SetField("duDeveloperProperties", stringBuilder.ToString());
	}

	public void SaveSettings(IServiceProvider provider, string userID)
	{
		M1DataDictionary obj = provider.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		DataSet dataSet = new DataSet();
		SqlCommand sqlCommand = obj.NewSqlCommand("Select * From DDUsers Where duUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
		sqlDataAdapter.Fill(dataSet, "Users");
		if (dataSet.Tables["Users"].Rows.Count != 0)
		{
			SaveSettings(dataSet.Tables["Users"].Rows[0]);
			new SqlCommandBuilder(sqlDataAdapter);
			sqlDataAdapter.Update(dataSet.Tables["Users"].GetChanges());
		}
	}
}
