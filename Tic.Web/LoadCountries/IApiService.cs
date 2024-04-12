using Tic.Shared.Responses;

namespace Tic.Web.LoadCountries
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string servicePrefix, string controller);
    }
}
