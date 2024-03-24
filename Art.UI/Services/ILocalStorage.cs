namespace Art.UI;

/// <summary>
/// Service responsible for accessing and modifying value on the local storage of the browser
/// </summary>
public interface ILocalStorage
{
    /// <summary>
    /// Remove all the data from the local storage
    /// </summary>
    /// <returns></returns>
    Task Clear();

    /// <summary>
    /// Returns a value for the passed in key
    /// </summary>
    /// <typeparam name="T">The data type to return (must be json serializable)</typeparam>
    /// <param name="key">The key to the data we want to retrieve</param>
    /// <returns></returns>
    Task<T?> GetValueAsync<T>(string key);

    /// <summary>
    /// Deletes a value from local storage using it's key
    /// </summary>
    /// <param name="key">The key to the value we want to delete</param>
    /// <returns></returns>
    Task RemoveAsync(string key);

    /// <summary>
    /// Stores the passed in value in the local storage of the browser using a specified key
    /// </summary>
    /// <typeparam name="T">Json serializable data type</typeparam>
    /// <param name="key">The key to access the stored value</param>
    /// <param name="value">The value that we want to store</param>
    /// <returns></returns>
    Task SetValueAsync<T>(string key, T value);
}
