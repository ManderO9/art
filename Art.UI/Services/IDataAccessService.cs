namespace Art.UI;

public interface IDataAccessService
{
    Task WriteFileAsync( /* Credentials as well*/ string fileName, byte[] content);

    Task<byte[]> ReadFileAsync(string fileName);
}
