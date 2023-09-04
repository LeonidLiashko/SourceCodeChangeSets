namespace SourceCodeChangeSetsTests;

public class FileWrapper : IDisposable
{
    private string PathToFile { get; }
    private FileWrapper(string path)
    {
        PathToFile = path;
    }

    public static async Task<FileWrapper> CreateFileWrapper(Task createFile, string path)
    {
        await createFile;
        return new FileWrapper(path);
    }

    public void Dispose()
    {
        File.Delete(PathToFile);
    }
}