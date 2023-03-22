using Microsoft.AspNetCore.Mvc;
using comicreact.api.entities;
using comicreact.api;

namespace comicreact.Controllers;

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

    private IEnumerable<Comic> GetComicsFromDirectory() {
      return _fileReader.ReadAllFiles(ComicLibraryDirectory).Select(file => new Comic{
          Title = file,
          Description = ""
      });
    }

    [HttpGet]
    public IEnumerable<Comic> Get()
    {
      yield return new Comic{
        Title = "Fish-Man!",
        Description = "A fish-man who is also a man."
      };
      yield return new Comic{
        Title = "Adventures of Adventurefolk",
        Description = "A group of adventurers who are also adventurers."
      };
      yield return new Comic{
        Title = "The Adventures of the Amazing Mr. Awesome",
        Description = "Mr. Awesome is a superhero who is also amazing."
      };
      yield return new Comic{
        Title = "Sandwich man and the forgotten bread crumbs",
        Description = ""
      };

      foreach (var comic in GetComicsFromDirectory()) {
        yield return comic;
      }
    }

}
