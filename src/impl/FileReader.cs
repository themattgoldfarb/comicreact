using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Archives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using src.api;
using src.api.entities;

namespace src.impl;

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
            if (comic.Archive != Comic.ArchiveType.Unknown)
            {
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
        Comic comic = new Comic
        {
            Title = Path.GetFileNameWithoutExtension(filePathName),
            FileName = Path.GetFileName(filePathName),
            Path = Path.GetDirectoryName(filePathName),
            Archive = getArchiveType(filePathName),
            Description = "This comic doesn't have a description..."

        };

        // If the archive type is unknown, we can't extract the comic.
        if (comic.Archive == Comic.ArchiveType.Unknown)
        {
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
        return thumbnailImage(
            image: getPageRaw(comic, pageNumber),
            strategy: ThumbnailStrategy.HighQuality);
    }

    private byte[] getPageRaw(Comic comic, int pageNumber)
    {
        if (pageNumber > comic.PageCount)
        {
            throw new ArgumentOutOfRangeException("pageNumber");
        }

        if (comic.Path == null)
        {
            throw new ArgumentNullException("comic.Path");
        }

        if (comic.FileName == null)
        {
            throw new ArgumentNullException("comic.FileName");
        }

        using (var archive = ArchiveFactory.Open(
              Path.Combine(comic.Path, comic.FileName)))
        {
            var entry = archive.Entries.Where(
                e => e.Key == comic.Pages?.ElementAt(pageNumber)).First();
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

    enum ThumbnailStrategy
    {
        Fast,
        HighQuality
    }

    private IResampler getResampler(ThumbnailStrategy strategy)
    {
        switch (strategy)
        {
            case ThumbnailStrategy.Fast:
                return KnownResamplers.NearestNeighbor;
            case ThumbnailStrategy.HighQuality:
                return KnownResamplers.Lanczos3;
            default:
                return KnownResamplers.NearestNeighbor;
        }
    }

    private byte[] thumbnailImage(
        byte[] image,
        ThumbnailStrategy strategy = ThumbnailStrategy.HighQuality)
    {

        using (var ms = new MemoryStream(image))
        {
            using (var img = Image.Load(ms))
            {
                img.Mutate(x => x.Resize(200, 300, getResampler(strategy)));
                using (var ms2 = new MemoryStream())
                {
                    img.SaveAsJpeg(ms2);
                    return ms2.ToArray();
                }
            }
        }
    }
}
