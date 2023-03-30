using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using src.api;
using src.api.entities;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class ComicController : ControllerBase
{
    private const string ComicLibraryDirectory = @"/home/matt/Comics/";
    private IFileReader _fileReader;

    public ComicController(IFileReader fileReader) => _fileReader = fileReader;

    [HttpGet]
    public string Get()
    {
        return "Hello World!";
    }

    private Comic getComicByName(string comicName)
    {
        List<Comic> comics = _fileReader.ReadAllComics(ComicLibraryDirectory).ToList();
        if (!comics.Any(comic => comic.Title == comicName))
        {
            return new Comic();
        }
        return comics.First(comic => comic.Title == comicName);
    }

    [HttpGet("getcomic/{comicName}")]
    public Comic GetComic(string comicName)
    {
        return getComicByName(comicName);
    }

    [HttpGet("page/{comicName}/{pageNumber}")]
    public FileContentResult GetPage(string comicName, int pageNumber)
    {
        Comic comic = getComicByName(comicName);

        return File(_fileReader.GetPage(comic, pageNumber), "image/jpeg");
    }

    [HttpGet("thumb/{comicName}/{pageNumber}")]
    public FileContentResult GetThumbnail(string comicName, int pageNumber)
    {
        Comic comic = getComicByName(comicName);

        return File(_fileReader.GetThumbnail(comic, pageNumber), "image/jpeg");
    }


}
