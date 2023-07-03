using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB.Application.Abstractions.Interfaces;
using TB.Application.Abstractions.IServices;
using TB.Infrastructure.Implementations.Interfaces;
using TB.Shared.Requests.Logging;
using TB.Shared.Responses.Logging;
using Serilog.Expressions;
using Serilog.Core;
using Serilog.Events;
using TB.Infrastructure.Helpers;
using MathNet.Numerics.Statistics;
using Google.Protobuf.WellKnownTypes;
using System.IO;
using System.Text.Json;
using TB.Shared.Dtos;
using Microsoft.Extensions.Logging.Abstractions;

namespace TB.Infrastructure.Implementations.Services
{
    internal sealed class LoggingService : ILoggingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LoggingService(IUnitOfWork UnitOfWork, IMapper Mapper)
        {
            unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        public Task<List<QueryLogFileResponse>> FindAll(QueryLogFileRequest logFileQueryRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<List<QueryLogFileResponse>> FilterFromFile(QueryLogFileRequest logFileQueryRequest)
        {
            try
            {
                List<QueryLogFileResponse> queryLogFileResponses = new();
                try
                {
                    using (var streamReader = File.OpenText(logFileQueryRequest.LogFile!))
                    {
                        string line;

                        while (!string.IsNullOrEmpty(line = await streamReader.ReadLineAsync()))
                        {
                            var logEvent = JsonSerializer.Deserialize<LogEntryDto>(line);

                            // Filter the log events based on the properties
                            if (logEvent!.Properties!.Method == logFileQueryRequest.Method &&
                                logEvent.Properties.CorrelationId.ToString() == logFileQueryRequest.CorrelationId)
                            {
                                // Access the log properties
                                QueryLogFileResponse queryLogFileResponse = new()
                                {
                                    TimeStamp = logEvent.Timestamp.ToString(),
                                    Level = logEvent.Level!.ToString(),
                                    MessageTemplate = logEvent.MessageTemplate,
                                    Method = logEvent.Properties.Method,
                                    Time = logEvent.Properties.Time,
                                    RequestId = logEvent.Properties.RequestId,
                                    RequestPath = logEvent.Properties.RequestPath,
                                    ConnectionId = logEvent.Properties.ConnectionId,
                                    CorrelationId = logEvent.Properties.CorrelationId.ToString(),
                                    MachineName = logEvent.Properties.MachineName,
                                };

                                // Perform desired operations with the log data
                                queryLogFileResponses.Add(queryLogFileResponse);
                            }
                        }
                    }

                }
                catch (IOException)
                {

                    throw;
                }

                return queryLogFileResponses;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<QueryLogFileResponse>> QueryFromFile(QueryLogFileRequest logFileQueryRequest)
        {
            try
            {
                List<QueryLogFileResponse> queryLogFileResponses = new();
                try
                {
                    using (var streamReader = File.OpenText(logFileQueryRequest.LogFile!))
                    {
                        string line;

                        while (!string.IsNullOrEmpty(line = await streamReader.ReadLineAsync()))
                        {
                            var logEvent = JsonSerializer.Deserialize<LogEntryDto>(line);

                            // Access the log properties
                            QueryLogFileResponse queryLogFileResponse = new()
                            {
                                TimeStamp = logEvent!.Timestamp.ToString(),
                                Level = logEvent.Level!.ToString(),
                                MessageTemplate = logEvent.MessageTemplate,
                                Method = logEvent.Properties!.Method,
                                Time = logEvent.Properties.Time,
                                RequestId = logEvent.Properties.RequestId,
                                RequestPath = logEvent.Properties.RequestPath,
                                ConnectionId = logEvent.Properties.ConnectionId,
                                CorrelationId = logEvent.Properties.CorrelationId.ToString(),
                                MachineName = logEvent.Properties.MachineName,
                            };

                            // Perform desired operations with the log data
                            queryLogFileResponses.Add(queryLogFileResponse);

                        }
                    }

                }
                catch (IOException)
                {

                    throw;
                }

                return queryLogFileResponses;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<QueryLogFileResponse>> QueryLogFile(QueryLogFileRequest logFileQueryRequest)
        {
            try
            {
                List<QueryLogFileResponse> queryLogFileResponses = new();
                var logFileContents = File.ReadAllLines(logFileQueryRequest.LogFile!);

                // Deserialize the log file into a list of log events
                var logEvents = LoggingHelper.DeserializeLogEvents(logFileContents);

                // Filter the log events based on the properties
                var filteredLogs = logEvents.FindAll(log =>
                    log.Properties.TryGetValue("Method", out var methodValue) && methodValue.ToString() == logFileQueryRequest.Method &&
                    log.Properties.TryGetValue("CorrelationId", out var correlationIdValue) && correlationIdValue.ToString() == logFileQueryRequest.CorrelationId
                );

                if (filteredLogs.Count() > 0)
                {
                    // Iterate through the matching logs
                    foreach (var log in filteredLogs)
                    {
                        // Access the log properties
                        QueryLogFileResponse queryLogFileResponse = new()
                        {
                            TimeStamp = log.Timestamp.ToString(),
                            Level = log.Level.ToString(),
                            MessageTemplate = log.MessageTemplate.ToString(),
                            Method = log.Properties["Method"].ToString(),
                            Time = log.Properties["Time"].ToString(),
                            RequestId = log.Properties["RequestId"].ToString(),
                            RequestPath = log.Properties["RequestPath"].ToString(),
                            ConnectionId = log.Properties["ConnectionId"].ToString(),
                            CorrelationId = log.Properties["CorrelationId"].ToString(),
                            MachineName = log.Properties["MachineName"].ToString(),
                        };

                        // Perform desired operations with the log data
                        queryLogFileResponses.Add(queryLogFileResponse);
                    }
                }

                return queryLogFileResponses;
                
            }
            catch (Exception)
            {

                throw;
            }
        }





    }
}
