using Microsoft.AspNetCore.Mvc;
using src.api.entities;
using src.api;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class ComicLibraryController: ControllerBase
{
    private const string ComicLibraryDirectory = @"/home/matt/Comics/";

    private readonly ILogger<ComicLibraryController> _logger;
    private IFileReader _fileReader;

    public ComicLibraryController(
        ILogger<ComicLibraryController> logger,
        IFileReader fileReader) => (_logger, _fileReader) = (logger, fileReader);

    [HttpGet]
    public IEnumerable<Comic> Get()
    {
      foreach (var comic in _fileReader.ReadAllComics(ComicLibraryDirectory)) {
        yield return comic;
      }
    }
}
