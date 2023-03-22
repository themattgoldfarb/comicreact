using NUnit.Framework;
using src.impl;
using System.Collections.Generic;
using System.IO;

namespace test.impl;

public class FileReaderTest
{
    [Test]
    public void TestRead()
    {
        string dir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "/impl/testdata/";
        FileReader reader = new FileReader();
        IEnumerable<string> result = reader.ReadAllFiles(dir);
        foreach(string line in result)
        {
          Assert.That(line, Is.Not.Null);
          Assert.That(line, Does.EndWith("impl/testdata/TestData.txt"));
        }
    }
}
