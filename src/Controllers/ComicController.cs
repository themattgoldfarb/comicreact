using Microsoft.AspNetCore.Mvc;
using src.api;
using src.api.entities;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class ComicController: ControllerBase
{
  private const string ComicLibraryDirectory = @"/home/matt/Comics/";
  private IFileReader _fileReader;

  public ComicController(IFileReader fileReader) => _fileReader = fileReader;

  [HttpGet]
  public string Get()
  {
    return "Hello World!";
  }

  [HttpGet("getcomic/{comicName}")]
  public Comic GetComic(string comicName)
  {
    List<Comic> comics = _fileReader.ReadAllFiles(ComicLibraryDirectory)
      .Select(file => _fileReader.GetComic(file)).ToList();
    if (!comics.Any(comic => comic.Title == comicName)) {
      return new Comic();
    }
    return comics.First(comic => comic.Title == comicName);
  }

  [HttpGet("page/{comicName}/{pageNumber}")]
  public FileContentResult GetPage(string comicName, int pageNumber)
  {
    List<Comic> comics = _fileReader.ReadAllFiles(ComicLibraryDirectory)
      .Select(file => _fileReader.GetComic(file)).ToList();
    if (!comics.Any(comic => comic.Title == comicName)) {
      return File(new byte[0], "image/jpeg");
    }
    Comic comic = comics.First(comic => comic.Title == comicName);

    return File(_fileReader.GetPage(comic, pageNumber), "image/jpeg");
  }

  
}
