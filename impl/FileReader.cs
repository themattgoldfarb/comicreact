namespace comicreact.impl;

using comicreact.api;

// Implementation of FileReader.
//
// This is a concrete implementation of the FileReader interface.
// It is used to read files from the filesystem.
public class FileReader : IFileReader
{
    // Read all files from a directory.
    public IEnumerable<string> ReadAllFiles(string directory)
    {
        return Directory.EnumerateFiles(directory);
    }

    public Comic GetComic(string path, string fileName)
    {
        throw new NotImplementedException();
    }
}
