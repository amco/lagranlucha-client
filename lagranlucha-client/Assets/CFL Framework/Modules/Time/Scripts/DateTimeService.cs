using System;

namespace CFLFramework.Time
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow { get => DateTime.UtcNow; set { } }
    }
}
