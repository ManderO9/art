namespace Art.UI;

public class Image
{
    public required Guid Id { get; set; }
    public required string FileName { get; set; }
    public required DateTimeOffset CreateAt { get; set; }
    public bool IsLiked { get; set; }
    public bool IsShared { get; set; }
}
