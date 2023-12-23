public class ConferenceData
    {
        public CreateRequest createRequest { get; set; }
        public List<EntryPoint> entryPoints { get; set; }
        public ConferenceSolution conferenceSolution { get; set; }
        public string conferenceId { get; set; }
    }

    public class ConferenceSolution
    {
        public Key key { get; set; }
        public string name { get; set; }
        public string iconUri { get; set; }
    }

    public class ConferenceSolutionKey
    {
        public string type { get; set; }
    }

    public class CreateRequest
    {
        public string requestId { get; set; }
        public ConferenceSolutionKey conferenceSolutionKey { get; set; }
        public Status status { get; set; }
    }

    public class Creator
    {
        public string email { get; set; }
        public bool self { get; set; }
    }

    public class End
    {
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class EntryPoint
    {
        public string entryPointType { get; set; }
        public string uri { get; set; }
        public string label { get; set; }
    }

    public class Key
    {
        public string type { get; set; }
    }

    public class Organizer
    {
        public string email { get; set; }
        public bool self { get; set; }
    }

    public class Reminders
    {
        public bool useDefault { get; set; }
    }

    public class GoogleCalendarEvent
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        public string htmlLink { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public Creator creator { get; set; }
        public Organizer organizer { get; set; }
        public Start start { get; set; }
        public End end { get; set; }
        public string iCalUID { get; set; }
        public int sequence { get; set; }
        public string hangoutLink { get; set; }
        public ConferenceData conferenceData { get; set; }
        public Reminders reminders { get; set; }
        public string eventType { get; set; }
    }

    public class Start
    {
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class Status
    {
        public string statusCode { get; set; }
    }