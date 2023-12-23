namespace OggettoCase.DataContracts.Dtos.Calendars;

public class GoogleCalendarResponse
{
    public string kind { get; set; }
    public string etag { get; set; }
    public string id { get; set; }
    public string summary { get; set; }
    public string description { get; set; }
    public string location { get; set; }
    public string timeZone { get; set; }
    public ConferenceProperties conferenceProperties { get; set; }
}

public class ConferenceProperties
{
    public List<string> allowedConferenceSolutionTypes { get; set; }
}