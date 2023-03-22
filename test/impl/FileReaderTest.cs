using NUnit.Framework;

namespace test.impl;

public class FileReaderTest
{
    [Test]
    public void TestRead()
    {
        FileReader reader = new FileReader();
        string result = reader.ReadAllFiles("");
        Assert.AreEqual("Hello World", result);
    }
}
