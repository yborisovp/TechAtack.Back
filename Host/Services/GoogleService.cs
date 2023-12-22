using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using OggettoCase.DataAccess.Models.Users;
using OggettoCase.DataContracts.CustomExceptions;
using OggettoCase.DataContracts.Dtos.Users;
using OggettoCase.DataContracts.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OggettoCase.Services;

public class GoogleService: IGoogleService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GoogleService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    public async Task<UserAuthorizationData> AuthorizeUserAsync(string authorizationToken, CancellationToken ct = default)
    {
        const string route = "https://www.googleapis.com/oauth2/v1/userinfo?";
        var httpClient = _httpClientFactory.CreateClient();
        var queryParams = HttpUtility.ParseQueryString("");
        queryParams.Add("alt", "json");
        queryParams.Add("access_token", authorizationToken);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(route + queryParams),
        };

        var response = await httpClient.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            throw new GoogleAuthorizationException("Cannot get authorize state from service");
        }
        
        var userData = await JsonSerializer.DeserializeAsync<UserAuthorizationData>(await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);
        if (userData is null)
        {
            throw new InvalidCastException("Cannot cast response from Google.");
        }
        return userData;
    }
}