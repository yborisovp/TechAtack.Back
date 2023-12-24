namespace OggettoCase.DataAccess.Filters;

public class CalendarFilterInternal
{
    public string? Title { get; set; }
    public string? OwnerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Category { get; set; }
}