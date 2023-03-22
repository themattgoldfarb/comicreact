namespace src.api.entities;

// A Comic containing a title and description.
public class Comic
{
    // The type of archive the comic is stored in.
    //
    // This is used to determine how to extract the comic from the archive.
    public enum ArchiveType
    {
        Zip, 
        Rar,
        Tar,
        TarGz,
        TarBz2,
        TarXz,
        SevenZip,
        Unknown
    }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public ArchiveType? Archive { get; set; }
}

