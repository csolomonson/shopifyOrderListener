namespace M1.Core;

public interface IChangeIDProcessing
{
	void PreProcessChangeID(ChangeIDProcessingParms parm);

	void ProcessChangeID(ChangeIDProcessingParms parm);

	void PostProcessChangeID(ChangeIDProcessingParms parm);
}
