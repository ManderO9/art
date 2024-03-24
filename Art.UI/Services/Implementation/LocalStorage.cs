using Microsoft.JSInterop;
using System.Text.Json;

namespace Art.UI;

/// <summary>
/// Service responsible for accessing and modifying value on the local storage of the browser
/// </summary>
public class LocalStorage : ILocalStorage, IAsyncDisposable
{
    #region Private Members

    /// <summary>
    /// JS object reference to the module that will contain the local storage access methods
    /// </summary>
    private Lazy<IJSObjectReference> mAccessorJsRef = new();

    /// <summary>
    /// JS runtime to use to load the local storage accessor module
    /// </summary>
    private readonly IJSRuntime mJsRuntime;

    #endregion

    #region Contructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="jsRuntime"></param>
    public LocalStorage(IJSRuntime jsRuntime)
    {
        // Set private members
        mJsRuntime = jsRuntime;
    }

    #endregion

    #region IDsiposable implementation
    
    public async ValueTask DisposeAsync()
    {
        // If the value of the lazy object was created
        if(mAccessorJsRef.IsValueCreated)
            // Dispose of it
            await mAccessorJsRef.Value.DisposeAsync();
    }

    #endregion

    #region Interface Implementation

    /// <inheritdoc />
    public async Task<T?> GetValueAsync<T>(string key)
    {
        // Wait for js module to load if it didn't yet
        await WaitForReference();

        // Get the value for the passed in key from local storage as string
        var stringResult = await mAccessorJsRef.Value.InvokeAsync<string>("get", key);

        // If it is empty
        if(stringResult == null || stringResult == "") 
            // Return the default value
            return default; 

        // Otherwise, deserialize the value from json
        var result = JsonSerializer.Deserialize<T>(stringResult);

        // Return the result
        return result;
    }

    /// <inheritdoc />
    public async Task SetValueAsync<T>(string key, T value)
    {
        // Wait for js module to load if it didn't yet
        await WaitForReference();
        
        // Serialize the passed in data to json
        var jsonData = JsonSerializer.Serialize(value);

        // Save it in the local storage
        await mAccessorJsRef.Value.InvokeVoidAsync("set", key, jsonData);
    }

    /// <inheritdoc />
    public async Task Clear()
    {
        // Wait for js module to load if it didn't yet
        await WaitForReference();

        // Clear the local storage
        await mAccessorJsRef.Value.InvokeVoidAsync("clear");
    }

    /// <inheritdoc />
    public async Task RemoveAsync(string key)
    {
        // Wait for js module to load if it didn't yet
        await WaitForReference();

        // Remove the passed in key
        await mAccessorJsRef.Value.InvokeVoidAsync("remove", key);
    }
    
    #endregion


    #region Private Helpers

    /// <summary>
    /// Waits for the JS module which is responsible for accessing local storage to load if it isn't already
    /// </summary>
    /// <returns></returns>
    private async Task WaitForReference()
    {
        // If the value was not created for the lazy object
        if(mAccessorJsRef.IsValueCreated is false)
            // Create it
            mAccessorJsRef = new(await mJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Assets/Js/LocalStorageAccessor.js"));
    }

    #endregion
}
