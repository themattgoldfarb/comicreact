using NUnit.Framework;
using src.impl;
using System.Collections.Generic;
using System.IO;
using src.api.entities;

namespace test.impl;

public class FileReaderTest
{
    private string _testDataDir;

    [SetUp]
    public void Setup()
    {
        _testDataDir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "/impl/testdata/";

    }

    [Test]
    public void TestReadAllFiles()
    {
        FileReader reader = new FileReader();
        IEnumerable<string> result = reader.ReadAllFiles(_testDataDir);
        Assert.That(result, Has.Some.Matches<string>(
              s => s.EndsWith("impl/testdata/TestData.txt")));
    }

    [Test]
    [TestCase("TestData.txt", "TestData", Comic.ArchiveType.Unknown)]
    [TestCase("TestComic.cbz", "TestComic", Comic.ArchiveType.Zip)]
    [TestCase("TestComic.zip", "TestComic", Comic.ArchiveType.Zip)]

    public void TestGetComic(
        string fileName, string expectedTitle, Comic.ArchiveType expectedArchive) {
        FileReader reader = new FileReader();

        Comic comic = reader.GetComic(_testDataDir + fileName);
        Assert.That(comic, Is.Not.Null);
        Assert.That(comic.Title, Is.EqualTo(expectedTitle));
        Assert.That(comic.Archive, Is.EqualTo(expectedArchive));
    }
}
