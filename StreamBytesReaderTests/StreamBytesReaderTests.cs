namespace StreamBytesReaderTests;

[TestFixture]
public class StreamBytesReaderTests
{
    [Test]
    [TestCase("Message1$Message2$Message3$", '$', new[] { "Message1", "Message2", "Message3" })]
    [TestCase("Aa$Bb$Cc$", '$', new[] { "Aa", "Bb", "Cc" })]
    [TestCase("$123$456$789$", '$', new[] { "123", "456", "789" })]
    [TestCase("One,Two,Three,Four", ',', new[] { "One", "Two", "Three", "Four" })]
    [TestCase("TestTestTest", 'T', new[] { "est", "est", "est" })]
    [TestCase("$Message1$Message2$", '$', new[] { "Message1", "Message2" })]
    [TestCase("ThisIsAString", '$', new[] { "ThisIsAString" })]
    [TestCase("ABABABABABABABAB", 'A', new[] { "B", "B", "B", "B", "B", "B", "B", "B" })]
    [TestCase("TrueCodeTestTask", 'e', new[] { "Tru", "Cod", "T", "stTask"})]
    [TestCase("AAABCAACBAAA", 'A', new[] { "BC", "CB"})]

    public void TestReadMessages(string input, char delimiter, string[] expectedMessages)
    {
        var data = Encoding.UTF8.GetBytes(input);
        var delimiterByte = (byte)delimiter;

        using Stream stream = new MemoryStream(data);
        var reader = new StreamBytesReader.StreamBytesReader(stream, delimiterByte);
        var messages = reader.ReadStream().ToList();

        CollectionAssert.AreEqual(expectedMessages, messages);
    }
}