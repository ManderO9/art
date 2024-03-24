namespace Art.UI;

/// <summary>
/// Responsible for writing and reading text to the clipboard
/// </summary>
public interface IClipboardService
{
    /// <summary>
    /// Copies the passed in text to the clipboard
    /// </summary>
    /// <param name="text">The text to copy</param>
    /// <returns></returns>
    Task Copy(string text);
}
