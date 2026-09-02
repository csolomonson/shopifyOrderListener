using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using M1.Images.Properties;

namespace M1.Core;

[DebuggerDisplay("{Description} - {ID}")]
public class OpenWithDefinition
{
	private string _ID = string.Empty;

	private string _AppExtensionID = string.Empty;

	private string _Table = string.Empty;

	private string _Field = string.Empty;

	private string _Description = string.Empty;

	private string _CaptionExpression = string.Empty;

	private string _CaptionExpressionUser = string.Empty;

	private OpenWithTypeEnum _Type;

	private byte _Sequence;

	private string _Code;

	private string _ObjectID = string.Empty;

	private string _FieldExtension = string.Empty;

	private string _ButtonImage = string.Empty;

	private string _ButtonImageUser = string.Empty;

	private string _ActionName = string.Empty;

	private string _EnabledExpression;

	private string _EnabledExpressionUser;

	[Browsable(false)]
	public ReferencedFieldsList EnabledExpressionReferencedFields = new ReferencedFieldsList();

	private bool _BindReadOnly;

	private bool _SaveBefore;

	private string _PromptField = string.Empty;

	private string _HideExpression;

	private string _HideExpressionUser;

	[Browsable(false)]
	public ReferencedFieldsList HideExpressionReferencedFields = new ReferencedFieldsList();

	private bool _Custom = true;

	public string ID
	{
		get
		{
			return _ID;
		}
		set
		{
			_ID = value;
		}
	}

	public string AppExtensionID
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

	public string Table
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

	public string Field
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

	public string Description
	{
		get
		{
			return _Description;
		}
		set
		{
			_Description = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Definition")]
	[Description("VBScript expression that is used to determine the text of this item. This allows you to change the text based on another value by accessing the Fields() collection. This is only valid where the type is BindingSource.")]
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
	[Description("VBScript expression that is used to determine the text of this item. This allows you to change the text based on another value by accessing the Fields() collection. This overrides the DescriptionExpression property.")]
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

	public OpenWithTypeEnum Type
	{
		get
		{
			return _Type;
		}
		set
		{
			_Type = value;
		}
	}

	public byte Sequence
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

	public string Code
	{
		get
		{
			return _Code;
		}
		set
		{
			_Code = value;
		}
	}

	public string ObjectID
	{
		get
		{
			return _ObjectID;
		}
		set
		{
			_ObjectID = value;
		}
	}

	public string FieldExtension
	{
		get
		{
			return _FieldExtension;
		}
		set
		{
			_FieldExtension = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Image that will be shown on the button associated with the button code for this field. This should be a 16x16 pixel image.")]
	public virtual string ButtonImage
	{
		get
		{
			return _ButtonImage;
		}
		set
		{
			_ButtonImage = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Image that will be shown on the button associated with the button code for this field. This should be a 16x16 pixel image. This overrides the ButtonImage property.")]
	public virtual string ButtonImageUser
	{
		get
		{
			return _ButtonImageUser;
		}
		set
		{
			_ButtonImageUser = value;
		}
	}

	[Description("Specify a class that supports the BindingSourceAction interface (to be defined).")]
	public string ActionName
	{
		get
		{
			return _ActionName;
		}
		set
		{
			_ActionName = value;
		}
	}

	public string EnabledExpression
	{
		get
		{
			return _EnabledExpression;
		}
		set
		{
			_EnabledExpression = value;
		}
	}

	public string EnabledExpressionUser
	{
		get
		{
			return _EnabledExpressionUser;
		}
		set
		{
			_EnabledExpressionUser = value;
		}
	}

	public bool BindReadOnly
	{
		get
		{
			return _BindReadOnly;
		}
		set
		{
			_BindReadOnly = value;
		}
	}

	public bool SaveBefore
	{
		get
		{
			return _SaveBefore;
		}
		set
		{
			_SaveBefore = value;
		}
	}

	public string PromptField
	{
		get
		{
			return _PromptField;
		}
		set
		{
			_PromptField = value;
		}
	}

	public string HideExpression
	{
		get
		{
			return _HideExpression;
		}
		set
		{
			_HideExpression = value;
		}
	}

	public string HideExpressionUser
	{
		get
		{
			return _HideExpressionUser;
		}
		set
		{
			_HideExpressionUser = value;
		}
	}

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

	public OpenWithDefinition(M1DataDictionary dataDictionary, M1Database database, string openWithID)
	{
		if (openWithID.Length != 0)
		{
			SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select dwID,dwAppExtensionID,dwTable,dwField,dwType,dwSequence,dwCode,dwObject,dwButtonImage,dwButtonImageUser,dwExtension,dwActionName,dwEnabledExpression,dwEnabledExpressionUser,dwSaveBefore,dwBindReadOnly,dwPromptField,dwHide,dwUHide,dwCustom,dwCaptionExpression,dwCaptionExpressionUser," + dataDictionary.Language.GetdwDescField(database) + " From DDOpenWiths " + dataDictionary.Language.GetdwDescJoin(database) + " Where dwID = @OpenWithID");
			sqlCommand.Parameters.Add(new SqlParameter("@OpenWithID", SqlDbType.NVarChar)).Value = openWithID;
			DataTable dataTable = dataDictionary.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				LoadRow(dataTable.Rows[0]);
			}
		}
	}

	public OpenWithDefinition(DataRow row)
	{
		if (row != null)
		{
			LoadRow(row);
		}
	}

	public OpenWithDefinition()
	{
	}

	public void LoadRow(DataRow row)
	{
		ID = row.Field<string>("dwID");
		AppExtensionID = row.Field<string>("dwAppExtensionID");
		Table = row.Field<string>("dwTable");
		Field = row.Field<string>("dwField");
		Description = row.Field<string>("dwDesc");
		CaptionExpression = row.Field<string>("dwCaptionExpression");
		CaptionExpressionUser = row.Field<string>("dwCaptionExpressionUser");
		Type = row.Field<OpenWithTypeEnum>("dwType");
		Sequence = row.Field<byte>("dwSequence");
		Code = row.Field<string>("dwCode");
		ObjectID = row.Field<string>("dwObject");
		ButtonImage = row.Field<string>("dwButtonImage");
		ButtonImageUser = row.Field<string>("dwButtonImageUser");
		FieldExtension = row.Field<string>("dwExtension");
		ActionName = row.Field<string>("dwActionName");
		EnabledExpression = row.Field<string>("dwEnabledExpression");
		EnabledExpressionUser = row.Field<string>("dwEnabledExpressionUser");
		SaveBefore = row.Field<bool>("dwSaveBefore");
		BindReadOnly = row.Field<bool>("dwBindReadOnly");
		PromptField = row.Field<string>("dwPromptField");
		HideExpression = row.Field<string>("dwHide");
		HideExpressionUser = row.Field<string>("dwUHide");
		Custom = row.Field<bool>("dwCustom");
		EnabledExpressionReferencedFields.Clear();
		if (EnabledExpression != null && EnabledExpression.Length != 0)
		{
			EnabledExpressionReferencedFields.ParseCodeForFields(EnabledExpression);
		}
		if (EnabledExpressionUser != null && EnabledExpressionUser.Length != 0)
		{
			EnabledExpressionReferencedFields.ParseCodeForFields(EnabledExpressionUser);
		}
		HideExpressionReferencedFields.Clear();
		if (HideExpression != null && HideExpression.Length != 0)
		{
			HideExpressionReferencedFields.ParseCodeForFields(HideExpression);
		}
		if (HideExpressionUser != null && HideExpressionUser.Length != 0)
		{
			HideExpressionReferencedFields.ParseCodeForFields(HideExpressionUser);
		}
	}

	public Image GetImage()
	{
		return Type switch
		{
			OpenWithTypeEnum.Entry => Resources.file16, 
			OpenWithTypeEnum.EntryNew => Resources.addFile16, 
			OpenWithTypeEnum.Report => Resources.print16, 
			OpenWithTypeEnum.Search => Resources.binoculars16, 
			_ => Resources.fantasy16, 
		};
	}

	public Image GetImageLarge()
	{
		return Type switch
		{
			OpenWithTypeEnum.Entry => Resources.file32, 
			OpenWithTypeEnum.EntryNew => Resources.addFile32, 
			OpenWithTypeEnum.Report => Resources.print32, 
			OpenWithTypeEnum.Search => Resources.binoculars32, 
			_ => Resources.fantasy32, 
		};
	}
}
