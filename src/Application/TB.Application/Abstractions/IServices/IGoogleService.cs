using TB.Shared.Requests.Google;
using TB.Shared.Responses.Google;

namespace TB.Application.Abstractions.IServices
{
    public interface IGoogleService
    {
        Task<List<GoogleSheetResponse>> GetGoogleSheet(GoogleSheetRequest googleSheetRequest);


    }
}
