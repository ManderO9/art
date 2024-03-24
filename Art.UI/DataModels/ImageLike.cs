namespace Art.UI;

public class ImageLike
{
    public required Guid ImageId { get; set; }
    public required DateTimeOffset LikedAt { get; set; }
}
