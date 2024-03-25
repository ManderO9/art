
namespace Art.UI;

public class HistoryService : IHistoryService
{
    #region Private Members

    private readonly string mHistoryKey = "History";
    private readonly ILocalStorage mLocalStorage;

    #endregion


    #region Constructor

    public HistoryService(ILocalStorage localStorage)
    {
        mLocalStorage = localStorage;
    }

    #endregion
    #region Interface Implementation

    public async Task AddImageToHistoryAsync(Guid imageId)
    {
        var history = await mLocalStorage.GetValueAsync<List<ImageInHistory>>(mHistoryKey) ?? [];

        var historyRecord = history.FirstOrDefault(x => x.ImageId == imageId);

        if(historyRecord is null)
        {
            historyRecord = new ImageInHistory()
            {
                AddedAt = DateTimeOffset.UtcNow,
                ImageId = imageId,
            };
            history.Add(historyRecord);
        }
        else
            historyRecord.AddedAt = DateTimeOffset.UtcNow;

        await mLocalStorage.SetValueAsync(mHistoryKey, history);
    }

    public async Task<List<ImageInHistory>> GetHistoryAsync()
        => await mLocalStorage.GetValueAsync<List<ImageInHistory>>(mHistoryKey) ?? [];

    #endregion

}
