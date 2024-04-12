using System.Net.Http.Json;
using System.Text;

namespace Art.Core;

public class DataAccessService : IDataAccessService
{
    private readonly HttpClient mHttpClient;

    public string FilesUrl => "https://ManderO9.github.io/ai-art-data/";

    public DataAccessService(HttpClient httpClient)
    {
        mHttpClient = httpClient;
    }

    public async Task<TData> ReadFileAsync<TData>(string fileName)
    {
        var path = FilesUrl + fileName;

        return await mHttpClient.GetFromJsonAsync<TData>(path) ?? default!;

    }

    public Task WriteFileAsync(string fileName, byte[] content) => throw new NotImplementedException();

}