﻿
using System.Diagnostics.CodeAnalysis;

namespace Art.UI;

public class BogusApiManager : IApiManager
{
    public Task<List<Image>> GetRecommendedImages()
    {
        var list = new List<Image>()
        {
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294024368.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
            new(){ FileName = "1711294016761.jpeg", Id = Guid.NewGuid(), CreateAt = DateTimeOffset.UtcNow},
        };

        return Task.FromResult(list);
    }
}
