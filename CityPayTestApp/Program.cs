using CityPayAPI.Api;
using CityPayAPI.Client;
using CityPayAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace CityPayTestApp
{
    internal class Program
    {
        private static string _clientID;
        private static string _licenceKey;
        private static int _batchId;

        private static void Main(string[] args)
        {
            Console.WriteLine();

            Log("## Start");

            try
            {
                if (args == null || args.Length != 3)
                {
                    throw new ArgumentException("Please provide three arguments: <ClientId> <LicenceKey> <BatchId>");
                }

                _clientID = args[0];
                _licenceKey = args[1];

                if (!int.TryParse(args[2], out _batchId))
                {
                    throw new ArgumentException("BatchId argument is not a valid integer");
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var transactions = new List<BatchTransaction>
                {
                    new BatchTransaction("1992075-2003", 10, $"{_batchId}_CP-MS-R", 12011685)
                };
                Log($"Created BatchTransaction object {transactions[0].Identifier}");

                var processBatchRequest = new ProcessBatchRequest(DateTime.Now, _batchId, _clientID, transactions);
                Log("Created ProcessBatchRequest object");

                Log("> BatchProcessRequest");
                var processBatchResponse = GetBatchProcessingApi().BatchProcessRequest(processBatchRequest);
                Log("< BatchProcessRequest");

                if (processBatchResponse.Valid)
                {
                    Log("Request is valid");

                    var checkBatchStatus = new CheckBatchStatus(new List<int> { _batchId }, _clientID);
                    Log("Created CheckBatchStatus object");

                    Log("> CheckBatchStatusRequest");
                    var checkBatchStatusResponse = GetBatchProcessingApi().CheckBatchStatusRequest(checkBatchStatus);
                    Log("< CheckBatchStatusRequest");

                    var batch = checkBatchStatusResponse.Batches[0];
                    Log($"Batch Status {batch.BatchStatus}");

                    while (batch.BatchStatus != "CANCELLED" &&
                           batch.BatchStatus != "COMPLETE" &&
                           batch.BatchStatus != "ERROR_IN_PROCESSING")
                    {
                        Thread.Sleep(3 * 60 * 1000);

                        Log("> CheckBatchStatusRequest");
                        checkBatchStatusResponse = GetBatchProcessingApi().CheckBatchStatusRequest(checkBatchStatus);
                        Log("< CheckBatchStatusRequest");

                        batch = checkBatchStatusResponse.Batches[0];
                        Log($"Batch Status {batch.BatchStatus}");
                    }

                    if (checkBatchStatusResponse.Batches.First().BatchStatus == "COMPLETE")
                    {
                        var batchReportRequest = new BatchReportRequest(_batchId, _clientID);
                        Log("Created BatchReportRequest object");

                        Log("> BatchReportRequest");
                        var batchReportResponseModel = GetBatchProcessingApi().BatchReportRequest(batchReportRequest);
                        Log("< BatchReportRequest");

                        Log($"Batch Status {batchReportResponseModel.BatchStatus}");
                        Log($"Transaction Message {batchReportResponseModel.Transactions[0].Message}");
                    }
                }
            }
            catch (ApiException ex)
            {
                Log($"ApiException: {ex.Message}");
                Log(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Log($"Exception: {ex.Message}");
                Log(ex.StackTrace);
            }

            Log("## End");

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static BatchProcessingApi GetBatchProcessingApi()
        {
            Log(">> Creating Configuration");
            var configuration = new Configuration { BasePath = "https://api.citypay.com/v6" };

            Log(">> Generating Key");
            configuration.AddApiKey("cp-api-key", new ApiKey(_clientID, _licenceKey).GenerateKey());

            Log(">> Creating Api");
            var api = new BatchProcessingApi(configuration);

            Log(">> Calling Api Method");
            return api;
        }

        private static void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss}: {message}");
        }
    }
}
