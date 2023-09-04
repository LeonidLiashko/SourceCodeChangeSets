// See https://aka.ms/new-console-template for more information

using SourceCodeChangeSets.Arguments;

namespace SourceCodeChangeSets;

public static class Program
{
    public static async Task Main(string[] args)
    {
        switch (args.Length)
        {
            case 3:
            {
                var arguments = new CreateDiffArguments(args);
                await DiffFile.Create(arguments);
                break;
            }
            case 4:
            {
                var arguments = new ApplyDiffArguments(args);
                await DiffFile.Apply(arguments);
                break;
            }
            default:
                throw new Exception("Wrong argument count");
        }
    }
}