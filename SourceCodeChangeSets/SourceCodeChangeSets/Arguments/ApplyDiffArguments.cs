namespace SourceCodeChangeSets.Arguments;

public class ApplyDiffArguments : Arguments
{
    public string Input { get; }
    public string Output { get; }
    public string Changeset { get; }
    
    public ApplyDiffArguments(IReadOnlyList<string> args)
    {
        if (args[3] != "/apply")
            throw new Exception($"Command {args[3]} not found");
        Input = args[0];
        Output = args[1]; 
        Changeset = args[2];
        CheckFilesExists(Input, Changeset);
    }
}