using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OpenApi.Services;
using Newtonsoft.Json;
using OggettoCase.DataAccess.Dtos;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.CustomExceptions;
using OggettoCase.DataContracts.Dtos.Authorization;
using OggettoCase.DataContracts.Dtos.Calendars;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OggettoCase.Services;

public class GoogleService: IGoogleService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GoogleService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<UserAuthorizationData> AuthorizeUserAsync(AuthorizeUserDto authorizeUserDto, CancellationToken ct = default)
    {
        const string route = "https://www.googleapis.com/oauth2/v1/userinfo?";
        var httpClient = _httpClientFactory.CreateClient();
        var queryParams = HttpUtility.ParseQueryString("");
        queryParams.Add("alt", "json");
        queryParams.Add("access_token", authorizeUserDto.AccessToken);
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(route + queryParams),
        };

        var response = await httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new GoogleAuthorizationException("Cannot validate auth token!");
        }
        
        var userData = await JsonSerializer.DeserializeAsync<UserAuthorizationData>(await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);
        if (userData is null)
        {
            throw new InvalidCastException("Cannot cast response from Google.");
        }
        
        /*
        const string refresh_route = "https://www.googleapis.com/oauth2/v4/token";
        request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(refresh_route),
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", authorizeUserDto.ClientId },
                { }
            })
        };
        httpClient = _httpClientFactory.CreateClient();
        response = await httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new GoogleAuthorizationException("Cannot get refresh token!");
        }*/
        return userData;
    }

    public async Task<string> CreateEventInGoogleCalendar(CreateCalendarEventDto createCalendarEntityDto, CancellationToken ct = default)
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirstValue("oauthToken") ?? throw new NotImplementedException();
        const string route = "https://www.googleapis.com/calendar/v3/calendars";
        
        var json = JsonConvert.SerializeObject(new
        {
            summary = createCalendarEntityDto.Title,
            description = createCalendarEntityDto.Description ?? string.Empty,
            kind = "calendar#calendar",
            conferenceProperties = new
            {
                allowedConferenceSolutionTypes = new List<string>
                {
                    "hangoutsMeet"
                }
            },
            
        });
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(route),
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", claim);
        var response = await httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new GoogleAuthorizationException("Something whent wrong. Possible vecause of auth token.");
        }
        var calendarData = await JsonSerializer.DeserializeAsync<GoogleCalendarResponse>(await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct) ?? throw new Exception("Parsing response from calendar went wrong/"); 
        return calendarData.id;
    }

    public async Task<GoogleCalendarEvent> CreateGoogleEventAsync(string calendarId, List<User>? users, CreateCalendarEventDto createCalendarEvent, CancellationToken ct = default)
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirstValue("oauthToken") ?? throw new NotImplementedException();
        var route = $"https://www.googleapis.com/calendar/v3/calendars/{calendarId}/events?conferenceDataVersion=1&sendUpdates=all";
        var pool = "abcdefghijklmnopqrstuvwxyz0123456789";
        var meetingUsers = new List<object>();
        if (users != null)
            foreach (var user in users)
            {
                meetingUsers.Add(new
                {
                    email = user.Email,
                    displayName = user.Name + user.Surname
                });
            }

        var json = JsonConvert.SerializeObject(new
        {
            description = createCalendarEvent.Description,
            summary = createCalendarEvent.Title,
            start = new
            {
                dateTime = createCalendarEvent.StartedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            },
            end = new
            {
                dateTime = createCalendarEvent.EndedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            },
            conferenceData = new
            {
                createRequest = new {
                    requestId = string.Join("", Enumerable.Range(0, 9)
                        .Select(x => pool[new Random().Next(0, pool.Length)]))
                }
            },
            attendees = meetingUsers
        });
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(route),
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", claim);
        var response = await httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new GoogleAuthorizationException("Something went wrong. Possible because of auth token.");
        }

        var content = await response.Content.ReadAsStreamAsync(ct);
        var calendarData = await JsonSerializer.DeserializeAsync<GoogleCalendarEvent>(content, cancellationToken: ct) ?? throw new Exception("Parsing response from calendar went wrong/"); 
        
        return calendarData;
    }

    public async Task DeleteEventAsync(string calendarId, string eventId, CancellationToken ct)
    {
       var claim = _httpContextAccessor.HttpContext?.User.FindFirstValue("oauthToken") ?? throw new NotImplementedException();
        var route = $"https://www.googleapis.com/calendar/v3/calendars/{calendarId}/events/{eventId}?sendUpdates=all";
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(route)
        };
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", claim);
        var response = await httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new GoogleAuthorizationException("Something went wrong. Possible because of auth token.");
        }

    }

    public async Task UpdateEventAsync(string externalCalendarId, string eEventId, UpdateCalendarDto dtoToUpdate,
        CancellationToken ct)
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirstValue("oauthToken") ?? throw new NotImplementedException();
        var route = $"https://www.googleapis.com/calendar/v3/calendars/{externalCalendarId}/events/{externalCalendarId}?conferenceDataVersion=1&sendUpdates=all";
        var pool = "abcdefghijklmnopqrstuvwxyz0123456789";
        
        var json = JsonConvert.SerializeObject(new
        {
            description = dtoToUpdate.Description,
            summary = dtoToUpdate.Title,
            start = new
            {
                dateTime = dtoToUpdate.StartedAt?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            },
            end = new
            {
                dateTime = dtoToUpdate.EndedAt?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            }
        });
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Patch,
            RequestUri = new Uri(route),
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", claim);
        var response = await httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new GoogleAuthorizationException("Something went wrong. Possible because of auth token.");
        }
        
    }

    public async Task<GoogleCalendarEvent> GetGoogleEventData(string calendarId, string xternalEventId, CancellationToken ct)
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirstValue("oauthToken") ?? throw new NotImplementedException();
        var route = $"https://www.googleapis.com/calendar/v3/calendars/{calendarId}/events/{xternalEventId}";
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(route),
        };
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", claim);
        var response = await httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new GoogleAuthorizationException("Something went wrong. " + response.ReasonPhrase);
        }

        var content = await response.Content.ReadAsStreamAsync(ct);
        var calendarData = await JsonSerializer.DeserializeAsync<GoogleCalendarEvent>(content, cancellationToken: ct) ?? throw new Exception("Parsing response from calendar went wrong/"); 
        
        return calendarData;
    }

    /*public async Task<bool> AddUserToEventAsync(string calendarId, string eventId, User user)
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirstValue("oauthToken") ?? throw new NotImplementedException();
        var route = $"https://www.googleapis.com/calendar/v3/calendars/{calendarId}/events/{eventId}";
        var json = JsonConvert.SerializeObject(new
        {
            attendees = new List<object>
            {
                new
                {
                    
                }
            }
        });
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(route),
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", claim);
        var response = await httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new GoogleAuthorizationException("Something went wrong. Possible because of auth token.");
        }
        var calendarData = await JsonSerializer.DeserializeAsync<GoogleCalendarEvent>(await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct) ?? throw new Exception("Parsing response from calendar went wrong/"); 
        return calendarData.htmlLink;
    }*/
}