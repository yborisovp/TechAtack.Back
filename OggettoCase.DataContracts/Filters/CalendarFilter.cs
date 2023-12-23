namespace OggettoCase.DataContracts.Filters;

public class CalendarFilter
{
    public string? Title { get; set; }
    public string? OwnerName { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string? Category { get; set; }
}