namespace Shop.Common.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface IApiService
    {
        Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string tokenType, string accessToken);

        Task<Response> DeleteAsync(string urlBase, string servicePrefix, string controller, int id, string tokenType, string accessToken);

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response> GetUserByEmailAsync(string urlBase, string servicePrefix, string controller, string email, string tokenType, string accessToken);

        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken);

        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, int id, T model, string tokenType, string accessToken);

        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken);

        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, RecoverPasswordRequest recoverPasswordRequest);

        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, NewUserRequest newUserRequest);
    }
}