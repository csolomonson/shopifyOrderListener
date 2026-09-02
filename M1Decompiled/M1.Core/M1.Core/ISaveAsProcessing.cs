namespace M1.Core;

public interface ISaveAsProcessing
{
	void BeforeUpdate(SaveAsProcessingParms parm);

	void AfterUpdate(SaveAsProcessingParms parm);
}
