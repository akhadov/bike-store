using SharedKernel;

namespace Domain.Silver.DimDates;

public sealed class DimDate : Entity
{
    public DateTime DateId { get; set; }
    public int Year { get; set; }
    public int Quarter { get; set; }
    public int Month { get; set; }
    public int Week { get; set; }
    public int Day { get; set; }
    public string DayName { get; set; }
    public string MonthName { get; set; }
    public bool IsWeekend { get; set; }
}
