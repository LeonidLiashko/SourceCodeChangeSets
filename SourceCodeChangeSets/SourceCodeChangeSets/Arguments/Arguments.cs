namespace SourceCodeChangeSets.Arguments;

public abstract class Arguments
{
    protected static void CheckFilesExists(params string[] paths)
    {
        foreach (var path in paths)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File {path} not found");
            }
        }
    }
}