using src.api.entities;

namespace src.api;

// Interface for reading from filesystem.
public interface IFileReader
{
    // Read all files from a directory.
    IEnumerable<string> ReadAllFiles(string directory);

    // Get a Comic from a file.
    //
    // This method will parse the Title and ArchiveType from the file name.
    // Additionally, it will extract the page filenames from the archive.
    Comic GetComic(string filePathName);

    // Returns the image data from a page.
    byte[] GetPage(Comic comic, int pageNumber);
}
