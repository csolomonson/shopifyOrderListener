namespace M1.Core;

public interface IImportProcessing
{
	void BeforeUpdate(ImportProcessingParms parm);

	void AfterUpdate(ImportProcessingParms parm);
}
