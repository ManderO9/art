﻿using System.Net.Http.Json;
using System.Text;

namespace Art.Core;

public class DataAccessService : IDataAccessService
{
    private readonly HttpClient mHttpClient;

    public string FilesUrl { get => "https://firebasestorage.googleapis.com/v0/b/ai-art-f6bde.appspot.com/o/"; }

    public DataAccessService(HttpClient httpClient)
    {
        mHttpClient = httpClient;
    }

    public async Task<TData> ReadFileAsync<TData>(string fileName)
    {
        var path = FilesUrl + fileName + "?alt=media";

        return await mHttpClient.GetFromJsonAsync<TData>(path) ?? default!;

    }

    public Task WriteFileAsync(string fileName, byte[] content) => throw new NotImplementedException();

}