namespace src.impl;

using src.api;
using src.api.entities;

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

    private Comic.ArchiveType getArchiveType(string filePathName)
    {
        string extension = Path.GetExtension(filePathName);
        switch (extension)
        {
            case ".zip":
            case ".cbz":
                return Comic.ArchiveType.Zip;
            case ".cbr":
            case ".rar":
                return Comic.ArchiveType.Rar;
            case ".tar":
                return Comic.ArchiveType.Tar;
            case ".tar.gz":
                return Comic.ArchiveType.TarGz;
            case ".tar.bz2":
                return Comic.ArchiveType.TarBz2;
            case ".tar.xz":
                return Comic.ArchiveType.TarXz;
            case ".cb7":
            case ".7z":
                return Comic.ArchiveType.SevenZip;
            default:
                return Comic.ArchiveType.Unknown;
        }
    }

    public Comic GetComic(string filePathName)
    {
      return new Comic{
        Title = Path.GetFileNameWithoutExtension(filePathName),
        Archive = getArchiveType(filePathName)
      };
    }
}
