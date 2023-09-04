namespace SourceCodeChangeSets.Arguments;

public class CreateDiffArguments : Arguments
{
    public string TaPath { get; }
    public string TbPath { get; }
    public string ChangeSetPath { get; }
    
    public CreateDiffArguments(IReadOnlyList<string> args)
    {
        TaPath = args[0];
        TbPath = args[1];
        ChangeSetPath = args[2];
        CheckFilesExists(TaPath, TbPath);
    }
}