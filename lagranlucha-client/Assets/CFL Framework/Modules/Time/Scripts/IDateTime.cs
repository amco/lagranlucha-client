using System;

namespace CFLFramework.Time
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; set; }
    }
}
