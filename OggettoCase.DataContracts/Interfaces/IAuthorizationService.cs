namespace OggettoCase.DataContracts.Interfaces;

public interface IAuthorizationService
{
    /// <summary>
    /// Получить токен доступа
    /// </summary>
    /// <param name="email">Электронная почта</param>
    /// <param name="refreshToken">Токен обновления</param>
    /// <param name="ct">Токен прерывания</param>
    /// <returns>Токен авторизации</returns>
    Task<string> GetAccessTokenAsync(string email, string refreshToken, CancellationToken ct);
    
    /// <summary>
    /// Обновить токен доступа
    /// </summary>
    /// <param name="refreshToken">Токен обновления</param>
    /// <param name="email">Электронная почта</param>
    /// <param name="ct">Токен прерывания</param>
    /// <returns>Новый токен доступа</returns>
    Task<string> RefreshTokenAsync(string refreshToken, string email, CancellationToken ct);
}