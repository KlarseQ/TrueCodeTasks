using System.Text;

namespace StreamBytesReader;

public class StreamBytesReader
{
    private readonly Stream _stream;
    private readonly byte _delimiter;

    public StreamBytesReader(Stream stream, byte delimiter)
    {
        _stream = stream;
        _delimiter = delimiter;
    }
    
    public IEnumerable<string> ReadStream()
    {
        var outputBuilder = new StringBuilder();
        int currentByte;

        while ((currentByte = _stream.ReadByte()) != -1)
        {
            if (currentByte == _delimiter)
            {
                if (outputBuilder.Length == 0)
                    continue;
                yield return outputBuilder.ToString();
                outputBuilder.Clear();
            }
            else
            {
                outputBuilder.Append((char)currentByte);
            }
        }

        if (outputBuilder.Length > 0)
        {
            yield return outputBuilder.ToString();
        }
    }
}