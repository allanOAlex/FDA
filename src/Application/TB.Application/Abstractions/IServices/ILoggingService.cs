using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Shared.Requests.Logging;
using TB.Shared.Responses.Logging;

namespace TB.Application.Abstractions.IServices
{
    public interface ILoggingService
    {
        Task<List<QueryLogFileResponse>> FindAll(QueryLogFileRequest logFileQueryRequest);

        Task<List<QueryLogFileResponse>> FilterFromFile(QueryLogFileRequest logFileQueryRequest);
        Task<List<QueryLogFileResponse>> QueryFromFile(QueryLogFileRequest logFileQueryRequest);
        Task<List<QueryLogFileResponse>> QueryLogFile(QueryLogFileRequest logFileQueryRequest);
    }
}
