using System.Text.Json.Serialization;

namespace Art.Core;

public class Image
{
    /// <summary>
    /// The unique identifier of the image
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The name of the image file
    /// </summary>
    public required string FileName { get; set; }

    /// <summary>
    /// The time at which the image will be displayed to the user as created at
    /// </summary>
    public required DateTimeOffset CreateAt { get; set; }

    /// <summary>
    /// Whether the current user liked the image
    /// </summary>
    [JsonIgnore]
    public bool IsLiked { get; set; }
    
    /// <summary>
    /// Whether this image has been shared by the user(i.e, he cliked the share button on it)
    /// </summary>
    [JsonIgnore]
    public bool IsShared { get; set; }

    /// <summary>
    /// The file content to use when uploading images
    /// </summary>
    [JsonIgnore]
    public byte[]? FileContent { get; set; }

    /// <summary>
    /// File Path to use when we are upload the image
    /// </summary>
    [JsonIgnore]
    public string FilePath { get; set; } = string.Empty;
}
