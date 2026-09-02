namespace M1.API.Utilities;

public static class WebAPIConstants
{
	public static class EDIPurposeCodes
	{
		public static readonly string Original = "00";

		public static readonly string Cancellation = "01";

		public static readonly string Change = "04";

		public static readonly string Replace = "05";
	}

	public const byte MAXREQUESTS_PERUSER_PERPERIOD = 10;

	public const byte MAXREQUESTS_PERIOD_IN_MINUTES = 1;

	public const byte SO_DEFAULT_DELIVERY_TYPE = 1;

	public const string EDI_SALESORDER_WARNING_HEADER = "EDI Order Creation Warnings..";

	public const string XML810_SAC_DISCOUNT_CODE = "DISC";

	public const string XML810_SAC_TAX_CODE = "TAX";

	public const string XML810_SAC_TAX_SECOND_CODE = "TAX2";

	public const string XML810_SAC_FREIGHT_CODE = "FRE";

	public const string API_BLANK_REVISION_URL_CHARACTOR = "_";

	public static readonly string[] M1_STANDARD_FACTORS_ARRAY = new string[12]
	{
		"HP", "HC", "HM", "MP", "MC", "MM", "PH", "PM", "TD", "TH",
		"TM", "SP"
	};
}
