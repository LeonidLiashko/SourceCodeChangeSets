using System.Text;

namespace SourceCodeChangeSetsTests;

public class ProgramTests
{
    private static string CurrDir { get; set; } = null!;

    public ProgramTests()
    {
        CurrDir = Directory.GetCurrentDirectory();
    }

    [Test]
    [NonParallelizable]
    public async Task RegularCreate()
    {
        const string ta = """
                          A
                          B
                          """;
        const string tb = """
                          C
                          D
                          """;
        
        var taPath = Path.Combine(CurrDir, "ta.txt");
        var tbPath = Path.Combine(CurrDir, "tb.txt");
        var changesetPath = Path.Combine(CurrDir, "change.txt");
        
        using var taFile = FileWrapper.CreateFileWrapper(File.WriteAllTextAsync(taPath, ta), taPath);
        using var tbFile = FileWrapper.CreateFileWrapper(File.WriteAllTextAsync(tbPath, tb), tbPath);
        using var changeFile =
            FileWrapper.CreateFileWrapper(SourceCodeChangeSets.Program.Main(new [] {taPath, tbPath, changesetPath}),
                changesetPath);
        
        var expected = new  StringBuilder("Replace next 2 lines:");
        expected.AppendLine();
        expected.AppendLine(ta);
        expected.AppendLine("With next 2 lines:");
        expected.AppendLine(tb);
        var actual = await File.ReadAllTextAsync(changesetPath);
        Assert.That(actual, Is.EqualTo(expected.ToString()));
    }
    
    [Test]
    [NonParallelizable]
    public async Task EmptyCreate()
    {
        const string ta = "";
        const string tb = "";
        
        var taPath = Path.Combine(CurrDir, "ta.txt");
        var tbPath = Path.Combine(CurrDir, "tb.txt");
        var changesetPath = Path.Combine(CurrDir, "change.txt");
        
        using var taFile = FileWrapper.CreateFileWrapper(File.WriteAllTextAsync(taPath, ta), taPath);
        using var tbFile = FileWrapper.CreateFileWrapper(File.WriteAllTextAsync(tbPath, tb), tbPath);
        using var changeFile =
            FileWrapper.CreateFileWrapper(SourceCodeChangeSets.Program.Main(new [] {taPath, tbPath, changesetPath}),
                changesetPath);
        
        var expected = new  StringBuilder("Replace next 0 lines:");
        expected.AppendLine();
        expected.AppendLine("With next 0 lines:");
        var actual = await File.ReadAllTextAsync(changesetPath);
        Assert.That(actual, Is.EqualTo(expected.ToString()));
    }
    
    [Test]
    [NonParallelizable]
    public async Task Apply()
    {
        const string input = """
                          A
                          B
                          """;
        const string change = """
                                  Replace next 2 lines:
                                  A
                                  B
                                  With next 2 lines:
                                  C
                                  D
                                  """;
        
        var inputPath = Path.Combine(CurrDir, "input.txt");
        var outputPath = Path.Combine(CurrDir, "output.txt");
        var changesetPath = Path.Combine(CurrDir, "change.txt");
        
        using var inputFile = FileWrapper.CreateFileWrapper(File.WriteAllTextAsync(inputPath, input), inputPath);
        using var changeFile = FileWrapper.CreateFileWrapper(File.WriteAllTextAsync(changesetPath, change), changesetPath);
        using var outputFile =
            FileWrapper.CreateFileWrapper(SourceCodeChangeSets.Program.Main(new [] {inputPath, outputPath, changesetPath, "/apply"}),
                outputPath);

        var expected = """
                       C
                       D
                       
                       """;
        var actual = await File.ReadAllTextAsync(outputPath);
        Assert.That(actual, Is.EqualTo(expected));
    }
}

