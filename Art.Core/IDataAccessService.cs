namespace Art.Core;

public interface IDataAccessService
{
    Task WriteFileAsync(string fileName, byte[] content);

    Task<TData> ReadFileAsync<TData>(string fileName);

    /// <summary>
    /// The url to all images in we gonna display, should end with a backslash '/'
    /// </summary>
    public string FilesUrl { get; }

}
