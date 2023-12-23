namespace OggettoCase.DataAccess.Filters;

public class CalendarFilterInternal
{
    public string? Title { get; set; }
    public string? OwnerName { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public string? Category { get; set; }
}