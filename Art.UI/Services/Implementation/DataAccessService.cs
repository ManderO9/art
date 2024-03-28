using System.Text;

namespace Art.UI;

public class DataAccessService : IDataAccessService
{
    public async Task<byte[]> ReadFileAsync(string fileName)
    {
        if(fileName == "data.json")
            return Encoding.UTF8.GetBytes("{\"Images\":[]}");

        await Task.Delay(1000);
        return new byte[3];
    }
    public async Task WriteFileAsync(string fileName, byte[] content)
    {
        await Task.Delay(400);
    }
}