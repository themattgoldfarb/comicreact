namespace src.impl;

using src.api;
using src.api.entities;
using SharpCompress.Archives;
using System.Linq;
using SixLabors.ImageSharp;

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

    public IEnumerable<string> ReadAllDirectories(string directory)
    {
        return Directory.EnumerateDirectories(directory);
    }

    public IEnumerable<Comic> ReadAllComics(string directory)
    {
        List<Comic> comics = new List<Comic>();

        var files = ReadAllFiles(directory);
        foreach (var file in files)
        {
          Comic comic = GetComic(file);
          if (comic.Archive != Comic.ArchiveType.Unknown) {
            comics.Add(comic);
          }
        }

        var directories = ReadAllDirectories(directory);
        foreach (var dir in directories)
        {
          comics.AddRange(ReadAllComics(dir));
        }

        return comics;
    }

    // Helper method to determine the archive type from a file name.
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

    private bool isImage(string filePathName)
    {
      string extension = Path.GetExtension(filePathName);
      switch (extension)
      {
        case ".jpg":
        case ".jpeg":
        case ".png":
        case ".gif":
        case ".bmp":
          return true;
        default:
          return false;
      }
    }

    // Helper method to extract the page filenames from an archive.
    private IEnumerable<string> getPages(string filePathName)
    {
      List<string> pages = new List<string>();
      using (var archive = ArchiveFactory.Open(filePathName))
      {
        foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory && isImage(entry.Key)))
        {
          pages.Add(entry.Key);
        }
      }
      return pages.OrderBy(x => x);
    }

    // Get a Comic from a file.
    //
    // This method will parse the Title and ArchiveType from the file name.
    // Additionally, it will extract the page filenames from the archive.
    public Comic GetComic(string filePathName)
    {
      Comic comic = new Comic{
        Title = Path.GetFileNameWithoutExtension(filePathName),
        FileName = Path.GetFileName(filePathName),
        Path = Path.GetDirectoryName(filePathName),
        Archive = getArchiveType(filePathName),
        Description = "This comic doesn't have a description..."

      };

      // If the archive type is unknown, we can't extract the comic.
      if (comic.Archive == Comic.ArchiveType.Unknown) {
        return comic;
      }

      comic.Pages = getPages(filePathName);
      comic.PageCount = comic.Pages.Count();

      return comic;
    }

    // Returns the image data from a page.
    public byte[] GetPage(Comic comic, int pageNumber)
    {
      return getPageRaw(comic, pageNumber);
    }

    public byte[] GetThumbnail(Comic comic, int pageNumber)
    {
      return thumbnailImage(getPageRaw(comic, pageNumber));
    }

    private byte[] getPageRaw(Comic comic, int pageNumber)
    {
      if (pageNumber > comic.PageCount) {
        throw new ArgumentOutOfRangeException("pageNumber");
      }


      using (var archive = ArchiveFactory.Open(
            Path.Combine(comic.Path, comic.FileName)))
      {
        var entry = archive.Entries.Where(
            e => e.Key == comic.Pages.ElementAt(pageNumber)).First();
        using (var stream = entry.OpenEntryStream())
        {
          using (var ms = new MemoryStream())
          {
            stream.CopyTo(ms);
            return ms.ToArray();
          }
        }
      }
    }

    private byte[] thumbnailImage(byte[] image) {
      using (var ms = new MemoryStream(image))
      {
        using (var img = Image.Load(ms))
        {
          img.Mutate(x => x.Resize(300, 200));
          using (var ms2 = new MemoryStream())
          {
            img.SaveAsJpeg(ms2);
            return ms2.ToArray();
          }
        }
      }
    }



    
}
