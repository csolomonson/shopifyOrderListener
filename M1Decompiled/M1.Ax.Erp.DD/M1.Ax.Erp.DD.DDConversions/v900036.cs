using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.00.036", "", "")]
public class v900036
{
	public v900036(DDConversionParms parms)
	{
		parms.DmoDD.ExecuteCommand(parms.DatabaseName, "UPDATE DDExplorer SET dxImageLarge = B.dxImageLarge, dxImageSmall = B.dxImageSmall FROM DDExplorer INNER JOIN DDExplorer B ON DDExplorer.dxExtd = B.dxExtd WHERE DDExplorer.dxUser <> '' AND B.dxUser = '' AND DDExplorer.dxMode = 'SBAR'");
	}
}
