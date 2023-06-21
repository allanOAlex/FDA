using AutoMapper;
using CsvHelper;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Globalization;
using System.Text;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IServices;
using TB.Domain.Models;
using TB.Shared.Requests.Google;
using TB.Shared.Responses.Google;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class GoogleService : IGoogleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        //private readonly SheetsService? sheetsService;

        public GoogleService(IUnitOfWork UnitOfWork, IMapper Mapper)
        {
            unitOfWork = UnitOfWork;
            mapper = Mapper;
            //sheetsService = SheetsService;
        }

        public async Task<List<GoogleSheetResponse>> GetGoogleSheet(GoogleSheetRequest googleSheetRequest)
        {
            try
            {
                SheetsService sheetsService = new();
                StringBuilder sb = new();
                sb.Append(googleSheetRequest.Sheet);
                sb.Append('!');
                sb.Append(googleSheetRequest.Range);

                var spreadsheetId = googleSheetRequest.Id;                
                var range = sb.ToString();

                // Retrieve the values from the specified range in the sheet
                var request = sheetsService.Spreadsheets.Values.Get(spreadsheetId.ToString(), range);
                var response = await request.ExecuteAsync();

                var csvData = ConvertToCsv(response.Values);
                var financialDatas = await ParseCsvData(csvData);
                return financialDatas;


                //var googleSheetResponse = MapResponseToObject(response);
                //return googleSheetResponse;


            }
            catch (Exception)
            {
                throw;
            }
            
        }


        private string ConvertToCsv(IList<IList<object>> values)
        {
            var csvData = string.Join("\n", values.Select(row => string.Join(",", row)));
            return csvData;
        }

        private async Task<List<GoogleSheetResponse>> ParseCsvData(string csvData)
        {
            using var reader = new StreamReader(csvData);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var googleSheetResponse = new List<GoogleSheetResponse>();
            await foreach (var record in csv.GetRecordsAsync<StockPrice>())
            {
                var financialData = new GoogleSheetRequest
                {
                    Date = record.Date,
                    Asset = record.Asset,
                    Open = record.Open,
                    High = record.High,
                    Low = record.Low,
                    Close = record.Close,
                    CloseAdjusted = record.CloseAdjusted,
                    SplitCoefficient = record.SplitCoefficient,

                };

                var mapRequest = new MapperConfiguration(cfg => cfg.CreateMap<GoogleSheetRequest, StockPrice>());
                var mapResponse = new MapperConfiguration(cfg => cfg.CreateMap<StockPrice, GoogleSheetResponse>());

                IMapper requestMap = mapRequest.CreateMapper();
                IMapper responseMap = mapResponse.CreateMapper();

                var destination = requestMap.Map<GoogleSheetRequest, StockPrice>(financialData);
                var itemToCreate = await unitOfWork.FinancialData.Create(destination);
                var result = responseMap.Map<StockPrice, GoogleSheetResponse>(itemToCreate);

                googleSheetResponse.Add(result);
            }

            await unitOfWork.CompleteAsync();
            bool Successful = true;

            return Successful == true ? googleSheetResponse : googleSheetResponse;

        }

        private async Task<List<GoogleSheetResponse>> MapResponseToObject(ValueRange response)
        {
            try
            {
                var googleSheetResponse = new List<GoogleSheetResponse>();

                if (response.Values != null && response.Values.Count > 0)
                {
                    foreach (var row in response.Values)
                    {
                        var sheetResponse = new GoogleSheetRequest
                        {
                            Date = (DateTime)row[0],
                            Asset = row[1].ToString(),
                            Open = (decimal)row[2],
                            High = (decimal)row[3],
                            Low = (decimal)row[4],
                            Close = (decimal)row[5],
                            CloseAdjusted = (decimal)row[6],
                            SplitCoefficient = (decimal)row[7],
                        };

                        var mapRequest = new MapperConfiguration(cfg => cfg.CreateMap<GoogleSheetRequest, StockPrice>());
                        var mapResponse = new MapperConfiguration(cfg => cfg.CreateMap<StockPrice, GoogleSheetResponse>());

                        IMapper requestMap = mapRequest.CreateMapper();
                        IMapper responseMap = mapResponse.CreateMapper();

                        var destination = requestMap.Map<GoogleSheetRequest, StockPrice>(sheetResponse);
                        var itemToCreate = await unitOfWork.FinancialData.Create(destination);
                        var result = responseMap.Map<StockPrice, GoogleSheetResponse>(itemToCreate);

                        googleSheetResponse.Add(result);
                    }

                    await unitOfWork.CompleteAsync();
                    bool Successful = true;

                    return Successful == true ? googleSheetResponse : googleSheetResponse;

                }

                return googleSheetResponse;
            }
            catch (Exception)
            {

                throw;
            }
            
        }


    }
}
