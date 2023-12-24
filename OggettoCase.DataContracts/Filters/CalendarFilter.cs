namespace OggettoCase.DataContracts.Filters;

public class CalendarFilter
{
    public string? Title { get; set; }
    public string? OwnerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Category { get; set; }
}