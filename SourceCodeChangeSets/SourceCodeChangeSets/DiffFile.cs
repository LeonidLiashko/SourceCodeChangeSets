using SourceCodeChangeSets.Arguments;

namespace SourceCodeChangeSets;

public static class DiffFile
{
    private const string Replace = "Replace next ";
    private const string With = "With next ";
    private const string Lines = " lines:";
    public static async Task Create(CreateDiffArguments arguments)
    {
        //ReadLines is lazy loading
        var inputLines = File.ReadLines(arguments.TaPath);
        var outputLines = File.ReadLines(arguments.TbPath);
    
        var firstRow = new[] { $"{Replace}{inputLines.Count()}{Lines}" };
        var splitRow = new[] { $"{With}{outputLines.Count()}{Lines}" };
        var diffFileContent = firstRow.Concat(inputLines).Concat(splitRow).Concat(outputLines);

        await File.WriteAllLinesAsync(arguments.ChangeSetPath ,diffFileContent);
    }

    public static async Task Apply(ApplyDiffArguments arguments)
    {
        //ReadLines is lazy loading
        var inputLines = File.ReadLines(arguments.Input);
        var changeSet = File.ReadLines(arguments.Changeset);
        using var enumeratorChangeSet = changeSet.GetEnumerator();
        //First line
        enumeratorChangeSet.MoveNext();
        var linesCount = int.Parse(enumeratorChangeSet.Current[Replace.Length..^Lines.Length]);
        if (linesCount != inputLines.Count())
            RaiseContextNotFound();

        using var enumeratorInput = inputLines.GetEnumerator();
        enumeratorChangeSet.MoveNext();
        foreach (var inputLine in inputLines)
        {
            if (inputLine != enumeratorChangeSet.Current)
                RaiseContextNotFound();
            enumeratorChangeSet.MoveNext();
        }

        //Split line
        var outputLinesCount = int.Parse(enumeratorChangeSet.Current[With.Length..^Lines.Length]);
        var outputLines = YieldCountInArray(enumeratorChangeSet, outputLinesCount);
        await File.WriteAllLinesAsync(arguments.Output, outputLines);
    }
    
    
    private static IEnumerable<string> YieldCountInArray(IEnumerator<string> enumerator, int count)
    {
        for (var i = 0; i < count; i++)
        {
            enumerator.MoveNext();
            yield return enumerator.Current;
        }
    }

    private static void RaiseContextNotFound()
    {
        throw new Exception("Required context not found"); 
    }
}