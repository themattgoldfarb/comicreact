namespace src.api;

// Interface for reading from filesystem.
public interface IFileReader
{
    // Read all files from a directory.
    IEnumerable<string> ReadAllFiles(string directory);
}
