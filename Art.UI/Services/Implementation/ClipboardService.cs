using Microsoft.JSInterop;

namespace Art.UI;

/// <summary>
/// Responsible for writing and reading text to the clipboard
/// </summary>
public class ClipboardService : IClipboardService, IAsyncDisposable
{
    #region Private Members

    /// <summary>
    /// JS object reference to the module that will contain the JavaScript methods
    /// </summary>
    private Lazy<IJSObjectReference> mAccessorJsRef = new();

    /// <summary>
    /// JS runtime to use to load the accessor module
    /// </summary>
    private readonly IJSRuntime mJsRuntime;

    #endregion

    #region Contructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="jsRuntime"></param>
    public ClipboardService(IJSRuntime jsRuntime)
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



    #region Private Helpers

    /// <summary>
    /// Waits for the JS module which is responsible for clipboard to load if it isn't already
    /// </summary>
    /// <returns></returns>
    private async Task WaitForReference()
    {
        // If the value was not created for the lazy object
        if(mAccessorJsRef.IsValueCreated is false)
            // Create it
            mAccessorJsRef = new(await mJsRuntime.InvokeAsync<IJSObjectReference>("import", "./Assets/Js/Clipboard.js"));
    }

    #endregion

    /// <inheritdoc />
    public async Task Copy(string text)
    {
        // Wait for js module to load if it didn't yet
        await WaitForReference();

        // Invoke the copy method
        await mAccessorJsRef.Value.InvokeVoidAsync("copyToClipboard", text);
    }
}
