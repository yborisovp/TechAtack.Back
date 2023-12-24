namespace OggettoCase.DataContracts.Dtos.Statistics;

public class StatisticDto
{
    public long? UserAttended { get; set; }
    public int? QuestionCount { get; set; }
    public long EventMinutesLength { get; set; }
    public bool? IsGoogleMeet { get; set; }
}